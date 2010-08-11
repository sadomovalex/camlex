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
using System.Linq.Expressions;
using System.Text;
using System.Xml.Linq;
using CamlexNET.Impl.Helpers;
using CamlexNET.Interfaces;
using Microsoft.SharePoint;

namespace CamlexNET.Impl
{
    internal class Query : IQuery
    {
        private readonly ITranslatorFactory translatorFactory;
        private XElement where;
        private XElement orderBy;
        private XElement groupBy;

        public Query(ITranslatorFactory translatorFactory)
        {
            this.translatorFactory = translatorFactory;
        }

        public IQuery Where(Expression<Func<SPListItem, bool>> expr)
        {
            var translator = translatorFactory.Create(expr);
            this.where = translator.TranslateWhere(expr);
            return this;
        }

        public IQuery WhereAll(IEnumerable<Expression<Func<SPListItem, bool>>> expressions)
        {
            var combinedExpression = ExpressionsHelper.CombineAnd(expressions);
            return this.Where(combinedExpression);
        }

        public IQuery WhereAny(IEnumerable<Expression<Func<SPListItem, bool>>> expressions)
        {
            var combinedExpression = ExpressionsHelper.CombineOr(expressions);
            return this.Where(combinedExpression);
        }

        public IQuery OrderBy(Expression<Func<SPListItem, object>> expr)
        {
            var lambda = Expression.Lambda<Func<SPListItem, object[]>>(
                Expression.NewArrayInit(typeof(object), expr.Body), expr.Parameters);
            return OrderBy(lambda);
        }

        public IQuery OrderBy(Expression<Func<SPListItem, object[]>> expr)
        {
            var translator = translatorFactory.Create(expr);
            this.orderBy = translator.TranslateOrderBy(expr);
            return this;
        }

        public IQuery GroupBy(Expression<Func<SPListItem, object>> expr)
        {
            var lambda = Expression.Lambda<Func<SPListItem, object[]>>(
                Expression.NewArrayInit(typeof(object), expr.Body), expr.Parameters);
            return GroupBy(lambda, null, null);
        }

        public IQuery GroupBy(Expression<Func<SPListItem, object[]>> expr, bool? collapse, int? groupLimit)
        {
            var translator = translatorFactory.Create(expr);
            this.groupBy = translator.TranslateGroupBy(expr, collapse, groupLimit);
            return this;
        }

        public IQuery GroupBy(Expression<Func<SPListItem, object>> expr, bool? collapse, int? groupLimit)
        {
            var lambda = Expression.Lambda<Func<SPListItem, object[]>>(
                Expression.NewArrayInit(typeof(object), expr.Body), expr.Parameters);
            return GroupBy(lambda, collapse, groupLimit);
        }

        public IQuery GroupBy(Expression<Func<SPListItem, object>> expr, bool? collapse)
        {
            var lambda = Expression.Lambda<Func<SPListItem, object[]>>(
                Expression.NewArrayInit(typeof(object), expr.Body), expr.Parameters);
            return GroupBy(lambda, collapse, null);
        }

        public IQuery GroupBy(Expression<Func<SPListItem, object>> expr, int? groupLimit)
        {
            var lambda = Expression.Lambda<Func<SPListItem, object[]>>(
                Expression.NewArrayInit(typeof(object), expr.Body), expr.Parameters);
            return GroupBy(lambda, null, groupLimit);
        }

        public XElement[] ToCaml(bool includeQueryTag)
        {
            if (includeQueryTag)
            {
                return new[]
                           {
                               new XElement(Tags.Query,
                                            this.where, this.orderBy, this.groupBy)
                           };
            }
            else
            {
                var elements = new List<XElement>();
                if (this.where != null)
                {
                    elements.Add(this.where);
                }
                if (this.orderBy != null)
                {
                    elements.Add(this.orderBy);
                }
                if (this.groupBy != null)
                {
                    elements.Add(this.groupBy);
                }
                return elements.ToArray();
            }
        }

        public override string ToString()
        {
            return this.ToString(false);
        }

        public string ToString(bool includeQueryTag)
        {
            var sb = new StringBuilder();
            var elements = this.ToCaml(includeQueryTag);
            Array.ForEach(elements, e => sb.Append(e.ToString()));
            return sb.ToString();
        }

        public static implicit operator string(Query query)
        {
            return query.ToString();
        }
    }
}
