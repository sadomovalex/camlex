#region Copyright(c) Alexey Sadomov, Vladimir Timashkov. All Rights Reserved.
// -----------------------------------------------------------------------------
// Copyright(c) 2010 Alexey Sadomov, Vladimir Timashkov. All Rights Reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//
//   1. No Trademark License - Microsoft Public License (Ms-PL) does not grant you rights to use
//      authors names, logos, or trademarks.
//   2. If you distribute any portion of the software, you must retain all copyright,
//      patent, trademark, and attribution notices that are present in the software.
//   3. If you distribute any portion of the software in source code form, you may do
//      so only under this license by including a complete copy of Microsoft Public License (Ms-PL)
//      with your distribution. If you distribute any portion of the software in compiled
//      or object code form, you may only do so under a license that complies with
//      Microsoft Public License (Ms-PL).
//   4. The names of the authors may not be used to endorse or promote products
//      derived from this software without specific prior written permission.
//
// The software is licensed "as-is." You bear the risk of using it. The authors
// give no express warranties, guarantees or conditions. You may have additional consumer
// rights under your local laws which this license cannot change. To the extent permitted
// under your local laws, the authors exclude the implied warranties of merchantability,
// fitness for a particular purpose and non-infringement.
// -----------------------------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using CamlexNET.Interfaces;
using CamlexNET.Interfaces.ReverseEngeneering;
using Microsoft.SharePoint;

namespace CamlexNET.Impl.ReverseEngeneering.Caml
{
    internal class ReLinkerFromCaml : IReLinker
    {
        private XElement where;
        private XElement orderBy;
        private XElement groupBy;
        private XElement viewFields;

        private class MethodInfoWithParams
        {
            public MethodInfo MethodInfo { get; set; }
            public List<Expression> Params { get; set; }
            public MethodInfoWithParams(MethodInfo mi, List<Expression> p)
            {
                this.MethodInfo = mi;
                this.Params = p;
            }
        }

        public ReLinkerFromCaml(XElement where, XElement orderBy, XElement groupBy, XElement viewFields)
        {
            this.where = where;
            this.viewFields = viewFields;
            this.groupBy = groupBy;
            this.orderBy = orderBy;
        }

        public Expression Link(LambdaExpression where, LambdaExpression orderBy, LambdaExpression groupBy, LambdaExpression viewFields)
        {
            var listFluent = new List<KeyValuePair<string, LambdaExpression>>();
            listFluent.Add(new KeyValuePair<string, LambdaExpression>(ReflectionHelper.WhereMethodName, where));
            listFluent.Add(new KeyValuePair<string, LambdaExpression>(ReflectionHelper.OrderByMethodName, orderBy));
            listFluent.Add(new KeyValuePair<string, LambdaExpression>(ReflectionHelper.GroupByMethodName, groupBy));

            var listViewFields = new List<KeyValuePair<string, LambdaExpression>>();
            listViewFields.Add(new KeyValuePair<string, LambdaExpression>(ReflectionHelper.ViewFieldsMethodName, viewFields));

            if (listFluent.Any(kv => kv.Value != null) && listViewFields.Any(kv => kv.Value != null))
            {
                throw new OnlyOnePartOfQueryShouldBeNotNullException();
            }

            var list = listFluent.Any(kv => kv.Value != null) ? listFluent : listViewFields;
            if (list.All(kv => kv.Value == null))
            {
                throw new AtLeastOneCamlPartShouldNotBeEmptyException();
            }


            var queryMi = ReflectionHelper.GetMethodInfo(typeof (Camlex), ReflectionHelper.QueryMethodName);
            var queryCall = Expression.Call(queryMi);

            var expr = queryCall;
            for (int i = 0; i < list.Count; i++)
            {
                var kv = list[i];
                if (kv.Value != null)
                {
                    var mi = this.getMethodInfo(kv.Key);
                    if (mi != null && mi.MethodInfo != null)
                    {
                        var args = new List<Expression>();
                        // 1st param is always lambda expression
                        args.Add(kv.Value);
                        if (mi.Params != null && mi.Params.Count > 0)
                        {
                            mi.Params.ForEach(p => args.Add(p));
                        }
                        // as we use fluent interfaces we just pass on next step value which we got from prev step
                        expr = Expression.Call(expr, mi.MethodInfo, args);
                    }
                }
            }
            return expr;
        }

        private MethodInfoWithParams getMethodInfo(string methodName)
        {
            if (methodName == ReflectionHelper.WhereMethodName)
            {
                var mi = ReflectionHelper.GetMethodInfo(typeof (IQuery), methodName);
                return new MethodInfoWithParams(mi, null);
            }
            if (methodName == ReflectionHelper.OrderByMethodName)
            {
                return this.getOrderByMethodInfo();
            }
            if (methodName == ReflectionHelper.GroupByMethodName)
            {
                return this.getGroupByMethodInfo();
            }
            if (methodName == ReflectionHelper.ViewFieldsMethodName)
            {
                return this.getViewFieldsMethodInfo();
            }
            return null;
        }

        private MethodInfoWithParams getViewFieldsMethodInfo()
        {
            var count = this.viewFields.Descendants(Tags.FieldRef).Count();
            if (count == 0)
            {
                return null;
            }
            MethodInfo mi = null;
            if (count == 1)
            {
                mi = typeof(IQueryEx).GetMethod(ReflectionHelper.ViewFieldsMethodName,
                                                new[] { typeof(Expression<Func<SPListItem, object>>), typeof(bool) });
            }
            else
            {
                mi = typeof(IQueryEx).GetMethod(ReflectionHelper.ViewFieldsMethodName,
                                                new[] { typeof(Expression<Func<SPListItem, object[]>>), typeof(bool) });
            }
            var p = new List<Expression>();
            p.Add(Expression.Constant(true));
            return new MethodInfoWithParams(mi, p);
        }

        private MethodInfoWithParams getGroupByMethodInfo()
        {
            var count = this.groupBy.Descendants(Tags.FieldRef).Count();
            if (count == 0)
            {
                return null;
            }

            bool hasCollapse = this.groupBy.Attributes(Attributes.Collapse).Count() > 0;
            bool hasGroupLimit = this.groupBy.Attributes(Attributes.GroupLimit).Count() > 0;

            bool collapse = false;
            int groupLimit = 0;

            if (hasCollapse)
            {
                if (!bool.TryParse((string) groupBy.Attribute(Attributes.Collapse), out collapse))
                {
                    throw new CantParseBooleanAttributeException(Attributes.Collapse);
                }
            }
            if (hasGroupLimit)
            {
                if (!int.TryParse((string) groupBy.Attribute(Attributes.GroupLimit), out groupLimit))
                {
                    throw new CantParseIntegerAttributeException(Attributes.GroupLimit);
                }
            }

            var p = this.getGroupByParams(count, hasGroupLimit, hasCollapse, groupLimit, collapse);
            var mi =  this.getGroupByMethodInfo(count, hasCollapse, hasGroupLimit);
            return new MethodInfoWithParams(mi, p);
        }

        private MethodInfo getGroupByMethodInfo(int count, bool hasCollapse, bool hasGroupLimit)
        {
            MethodInfo mi;
            if (count == 1)
            {
                if (hasCollapse && hasGroupLimit)
                {
                    mi = typeof (IQuery).GetMethod(ReflectionHelper.GroupByMethodName,
                                                   new[]
                                                       {
                                                           typeof (Expression<Func<SPListItem, object>>), typeof (bool?), typeof (int?)
                                                       });
                }
                else if (hasCollapse && !hasGroupLimit)
                {
                    mi = typeof (IQuery).GetMethod(ReflectionHelper.GroupByMethodName,
                                                   new[] {typeof (Expression<Func<SPListItem, object>>), typeof (bool?)});
                }
                else if (!hasCollapse && hasGroupLimit)
                {
                    mi = typeof (IQuery).GetMethod(ReflectionHelper.GroupByMethodName,
                                                   new[] {typeof (Expression<Func<SPListItem, object>>), typeof (int?)});
                }
                else
                {
                    mi = typeof (IQuery).GetMethod(ReflectionHelper.GroupByMethodName,
                                                   new[] {typeof (Expression<Func<SPListItem, object>>)});
                }
            }
            else
            {
                mi = typeof (IQuery).GetMethod(ReflectionHelper.GroupByMethodName,
                                               new[]
                                                   {
                                                       typeof (Expression<Func<SPListItem, object[]>>), typeof (bool?), typeof (int?)
                                                   });
            }
            return mi;
        }

        private List<Expression> getGroupByParams(int count, bool hasGroupLimit, bool hasCollapse, int groupLimit, bool collapse)
        {
            List<Expression> p = null;
            if (hasCollapse && hasGroupLimit)
            {
                p = new List<Expression>();
                p.Add(Expression.Convert(Expression.Constant(collapse), typeof(bool?)));
                p.Add(Expression.Convert(Expression.Constant(groupLimit), typeof (int?)));
            }
            else if (hasCollapse && !hasGroupLimit)
            {
                p = new List<Expression>();
                p.Add(Expression.Convert(Expression.Constant(collapse), typeof (bool?)));
                // there is only 1 method which receives several field refs: GroupBy(Expression<Func<SPListItem, object[]>> expr, bool? collapse, int? groupLimit).
                // For this method we need always provide 2 arguments);
                if (count > 1)
                {
                    p.Add(Expression.Convert(Expression.Constant(null), typeof(int?)));
                }
            }
            else if (!hasCollapse && hasGroupLimit)
            {
                // there is only 1 method which receives several field refs: GroupBy(Expression<Func<SPListItem, object[]>> expr, bool? collapse, int? groupLimit).
                // For this method we need always provide 2 arguments
                p = new List<Expression>();
                if (count > 1)
                {
                    p.Add(Expression.Convert(Expression.Constant(null), typeof(bool?)));
                }

                p.Add(Expression.Convert(Expression.Constant(groupLimit), typeof(int?)));
            }
            else if (count > 1)
            {
                // there is only 1 method which receives several field refs: GroupBy(Expression<Func<SPListItem, object[]>> expr, bool? collapse, int? groupLimit).
                // For this method we need always provide 2 arguments
                p = new List<Expression>();
                p.Add(Expression.Convert(Expression.Constant(null), typeof(bool?)));
                p.Add(Expression.Convert(Expression.Constant(null), typeof(int?)));
            }
            return p;
        }

        private MethodInfoWithParams getOrderByMethodInfo()
        {
            var count = this.orderBy.Descendants(Tags.FieldRef).Count();
            MethodInfo mi = null;
            if (count == 0)
            {
                return null;
            }
            if (count == 1)
            {
                mi = typeof(IQuery).GetMethod(ReflectionHelper.OrderByMethodName,
                                                new[] {typeof (Expression<Func<SPListItem, object>>)});
            }
            else
            {
                mi = typeof(IQuery).GetMethod(ReflectionHelper.OrderByMethodName,
                                                new[] { typeof(Expression<Func<SPListItem, object[]>>) });
            }
            return new MethodInfoWithParams(mi, null);
        }
    }
}
