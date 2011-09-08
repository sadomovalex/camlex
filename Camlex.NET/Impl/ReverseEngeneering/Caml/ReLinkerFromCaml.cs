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

        public ReLinkerFromCaml(XElement where, XElement orderBy, XElement groupBy, XElement viewFields)
        {
            this.where = where;
            this.viewFields = viewFields;
            this.groupBy = groupBy;
            this.orderBy = orderBy;
        }

        public Expression Link(LambdaExpression where, LambdaExpression orderBy, LambdaExpression groupBy, LambdaExpression viewFields)
        {
            var list = new List<KeyValuePair<string, LambdaExpression>>();
            list.Add(new KeyValuePair<string, LambdaExpression>(ReflectionHelper.WhereMethodName, where));
            list.Add(new KeyValuePair<string, LambdaExpression>(ReflectionHelper.OrderByMethodName, orderBy));
            list.Add(new KeyValuePair<string, LambdaExpression>(ReflectionHelper.GroupByMethodName, groupBy));
            list.Add(new KeyValuePair<string, LambdaExpression>(ReflectionHelper.ViewFieldsMethodName, viewFields));

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
                    if (mi != null)
                    {
                        expr = Expression.Call(expr, mi, kv.Value);
                    }
                }
            }
            return expr;
        }

        private MethodInfo getMethodInfo(string methodName)
        {
            if (methodName == ReflectionHelper.WhereMethodName)
            {
                return ReflectionHelper.GetMethodInfo(typeof (IQuery), methodName);
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
            throw new NotImplementedException();
        }

        private MethodInfo getViewFieldsMethodInfo()
        {
            var count = this.viewFields.Descendants(Tags.FieldRef).Count();
            if (count == 0)
            {
                return null;
            }
            if (count == 1)
            {
                return typeof(IQueryEx).GetMethod(ReflectionHelper.ViewFieldsMethodName,
                                                new[] { typeof(Expression<Func<SPListItem, object>>), typeof(bool) });
            }
            else return typeof(IQueryEx).GetMethod(ReflectionHelper.OrderByMethodName,
                                                new[] { typeof(Expression<Func<SPListItem, object[]>>), typeof(bool) });
        }

        private MethodInfo getGroupByMethodInfo()
        {
            var count = this.groupBy.Descendants(Tags.FieldRef).Count();
            if (count == 0)
            {
                return null;
            }
            if (count == 1)
            {
                bool hasCollapse = this.groupBy.Attributes(Attributes.Collapse).Count() > 0;
                bool hasGroupLimit = this.groupBy.Attributes(Attributes.GroupLimit).Count() > 0;

                if (hasCollapse && hasGroupLimit)
                {
//                    bool collapse;
//                    if (!bool.TryParse((string)groupBy.Attribute(Attributes.Collapse), out collapse))
//                    {
//                        throw new CantParseBooleanAttributeException(Attributes.Collapse);
//                    }
//                    int groupLimit;
//                    if (!int.TryParse((string)groupBy.Attribute(Attributes.GroupLimit), out groupLimit))
//                    {
//                        throw new CantParseIntegerAttributeException(Attributes.GroupLimit);
//                    }
                    return typeof(IQuery).GetMethod(ReflectionHelper.GroupByMethodName,
                                                    new[] { typeof(Expression<Func<SPListItem, object>>), typeof(bool?), typeof(int?) });
                }
                if (hasCollapse && !hasGroupLimit)
                {
//                    bool collapse;
//                    if (!bool.TryParse((string)groupBy.Attribute(Attributes.Collapse), out collapse))
//                    {
//                        throw new CantParseBooleanAttributeException(Attributes.Collapse);
//                    }
                    return typeof(IQuery).GetMethod(ReflectionHelper.GroupByMethodName,
                                                    new[] { typeof(Expression<Func<SPListItem, object>>), typeof(bool?) });
                }
                if (!hasCollapse && hasGroupLimit)
                {
//                    int groupLimit;
//                    if (!int.TryParse((string)groupBy.Attribute(Attributes.GroupLimit), out groupLimit))
//                    {
//                        throw new CantParseIntegerAttributeException(Attributes.GroupLimit);
//                    }
                    return typeof(IQuery).GetMethod(ReflectionHelper.GroupByMethodName,
                                                    new[] { typeof(Expression<Func<SPListItem, object>>), typeof(int?) });
                }
                return typeof(IQuery).GetMethod(ReflectionHelper.GroupByMethodName,
                                                new[] { typeof(Expression<Func<SPListItem, object>>) });
            }
            else return typeof(IQuery).GetMethod(ReflectionHelper.GroupByMethodName,
                                                new[] { typeof(Expression<Func<SPListItem, object[]>>), typeof(bool?), typeof(int?) });
        }

        private MethodInfo getOrderByMethodInfo()
        {
            var count = this.orderBy.Descendants(Tags.FieldRef).Count();
            if (count == 0)
            {
                return null;
            }
            if (count == 1)
            {
                return typeof(IQuery).GetMethod(ReflectionHelper.OrderByMethodName,
                                                new[] {typeof (Expression<Func<SPListItem, object>>)});
            }
            else return typeof(IQuery).GetMethod(ReflectionHelper.OrderByMethodName,
                                                new[] { typeof(Expression<Func<SPListItem, object[]>>) });
        }
    }
}
