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
using System.Text;
using System.Xml.Linq;
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
            return this.whereAllOrAny(expressions, ExpressionType.AndAlso);
        }

        private IQuery whereAllOrAny(IEnumerable<Expression<Func<SPListItem, bool>>> expressions, ExpressionType type)
        {
            if (expressions == null || expressions.Count() == 0)
            {
                throw new EmptyExpressionsListException();
            }

            // todo: check that all expressions have the same parameter name
            // todo: check that all expressions are valid

            Expression result;
            if (expressions.Count() == 1)
            {
                result = expressions.First();
            }
            else
            {
                result = this.joinExpressions(expressions, type);
            }

            // todo: determined parameter name
            var lambda = Expression.Lambda<Func<SPListItem, bool>>(result,
                Expression.Parameter(typeof(SPListItem), "x"));
            return this.Where(lambda);
        }

        private BinaryExpression joinExpressions(IEnumerable<Expression<Func<SPListItem, bool>>> expressions, ExpressionType type)
        {
            return this.joinExpressions(1, expressions, expressions.ElementAt(0).Body, type);
        }

        // See http://sadomovalex.blogspot.com/2010/02/build-dynamic-expressions-for-caml.html
        private BinaryExpression joinExpressions(int currentIdxToAdd, IEnumerable<Expression<Func<SPListItem, bool>>> expressions,
            Expression prevExpr, ExpressionType type)
        {
            if (currentIdxToAdd >= expressions.Count())
            {
                return (BinaryExpression) prevExpr;
            }

            var currentExpression = expressions.ElementAt(currentIdxToAdd);

            Expression resultExpr;
            if (type == ExpressionType.OrElse)
            {
                resultExpr = Expression.OrElse(prevExpr, currentExpression.Body);
            }
            else if (type == ExpressionType.AndAlso)
            {
                resultExpr = Expression.AndAlso(prevExpr, currentExpression.Body);
            }
            else
            {
                throw new OnlyOrAndBinaryExpressionsAllowedForJoinsExceptions();
            }
            return joinExpressions(currentIdxToAdd + 1, expressions, resultExpr, type);
        }

        public IQuery WhereAny(IEnumerable<Expression<Func<SPListItem, bool>>> expressions)
        {
            return this.whereAllOrAny(expressions, ExpressionType.OrElse);
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
