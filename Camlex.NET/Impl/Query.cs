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
using CamlexNET.Impl.Helpers;
using CamlexNET.Interfaces;
using CamlexNET.Interfaces.ReverseEngeneering;
using Microsoft.SharePoint;

namespace CamlexNET.Impl
{
    internal class Query : IQueryEx
    {
        private readonly ITranslatorFactory translatorFactory;
        private readonly IReTranslatorFactory reTranslatorFactory;
        private XElement where;
        private XElement orderBy;
        private XElement groupBy;

        public Query(ITranslatorFactory translatorFactory, IReTranslatorFactory reTranslatorFactory)
        {
            this.translatorFactory = translatorFactory;
            this.reTranslatorFactory = reTranslatorFactory;
        }

        public IQuery Where(Expression<Func<SPListItem, bool>> expr)
        {
            expr = this.canonize(expr);
            var translator = translatorFactory.Create(expr);
            this.where = translator.TranslateWhere(expr);
            return this;
        }

        private Expression<Func<SPListItem, bool>> canonize(Expression<Func<SPListItem, bool>> expr)
        {
            if (expr == null)
            {
                throw new NullReferenceException("Expression is null");
            }

            if ((expr.Body.NodeType == ExpressionType.AndAlso || expr.Body.NodeType == ExpressionType.OrElse))
            {
                var left = this.canonizeBody(((BinaryExpression) expr.Body).Left);
                var right = this.canonizeBody(((BinaryExpression)expr.Body).Right);
                return Expression.Lambda<Func<SPListItem, bool>>(expr.Body.NodeType == ExpressionType.AndAlso ? Expression.AndAlso(left, right) : Expression.OrElse(left, right),
                    Expression.Parameter(typeof(SPListItem), ReflectionHelper.CommonParameterName));
            }

            // canonize boolean expression with explicit cast to equal expression in order to reuse existing code
            var exprBody = this.canonizeBody(expr.Body);
            return Expression.Lambda<Func<SPListItem, bool>>(exprBody, Expression.Parameter(typeof(SPListItem), ReflectionHelper.CommonParameterName));
        }

        private Expression canonizeBody(Expression expr)
        {
            if ((expr.NodeType == ExpressionType.AndAlso || expr.NodeType == ExpressionType.OrElse))
            {
                var left = this.canonizeBody(((BinaryExpression)expr).Left);
                var right = this.canonizeBody(((BinaryExpression)expr).Right);
                return (expr.NodeType == ExpressionType.AndAlso ? Expression.AndAlso(left, right) : Expression.OrElse(left, right));
            }

            if (expr.Type == typeof(bool) && expr.NodeType == ExpressionType.Convert)
            {
                expr = Expression.Equal(expr, Expression.Constant(true));
            }
            else if (expr.Type == typeof(bool) && expr.NodeType == ExpressionType.Not)
            {
                expr = Expression.Equal(((UnaryExpression)expr).Operand, Expression.Constant(false));
            }
            return expr;
        }

        public IQuery WhereAll(IEnumerable<Expression<Func<SPListItem, bool>>> expressions)
        {
            var combinedExpression = ExpressionsHelper.CombineAnd(expressions);
            return this.Where(combinedExpression);
        }

        public IQuery WhereAll(string existingWhere, Expression<Func<SPListItem, bool>> expression)
        {
            return this.WhereAll(existingWhere, new List<Expression<Func<SPListItem, bool>>>(new[] { expression }));
        }

        public IQuery WhereAll(string existingWhere, IEnumerable<Expression<Func<SPListItem, bool>>> expressions)
        {
            var where = this.getWhereExpressionFromString(existingWhere);
            var exprs = new List<Expression<Func<SPListItem, bool>>>(expressions);
            exprs.Add(where);
            return this.WhereAll(exprs);
        }

        public IQuery WhereAll(IEnumerable<string> expressions)
        {
            if (expressions == null || !expressions.Any())
            {
                throw new EmptyExpressionsListException();
            }
            var exprs = new List<Expression<Func<SPListItem, bool>>>(expressions.Select(getWhereExpressionFromString));
            return this.WhereAll(exprs);
        }

        private Expression<Func<SPListItem, bool>> getWhereExpressionFromString(string existingWhere)
        {
            existingWhere = this.ensureParentTag(existingWhere, Tags.Query);

            var translator = this.reTranslatorFactory.Create(existingWhere);
            var expr = translator.TranslateWhere() as Expression<Func<SPListItem, bool>>;
            if (expr == null)
            {
                throw new IncorrectCamlException(Tags.Where);
            }
            return expr;
        }

        private string ensureParentTag(string xml, string tag)
        {
            if (!string.IsNullOrEmpty(xml) && !xml.StartsWith(string.Format("<{0}>", tag)))
            {
                // re expects Query tag
                xml = string.Format("<{0}>{1}</{0}>", tag, xml);
            }
            return xml;
        }

        private Expression<Func<SPListItem, object[]>> getOrderByExpressionFromString(string existingOrderBy)
        {
            existingOrderBy = this.ensureParentTag(existingOrderBy, Tags.Query);

            var translator = this.reTranslatorFactory.Create(existingOrderBy);
            var orderByExpr = translator.TranslateOrderBy();
            var expr = orderByExpr as Expression<Func<SPListItem, object[]>>;
            if (expr == null)
            {
                var singleExpr = orderByExpr as Expression<Func<SPListItem, object>>;
                if (singleExpr == null)
                {
                    throw new IncorrectCamlException(Tags.OrderBy);
                }
                expr = this.createArrayExpression(singleExpr);
            }
            return expr;
        }

        private Expression<Func<SPListItem, object[]>> getGroupByExpressionFromString(string existingGroupBy, out bool? collapse, out int? groupLimit)
        {
            existingGroupBy = this.ensureParentTag(existingGroupBy, Tags.Query);

            var translator = this.reTranslatorFactory.Create(existingGroupBy);
            GroupByParams groupByParams;
            var orderByExpr = translator.TranslateGroupBy(out groupByParams);
            collapse = groupByParams.HasCollapse ? (bool?)groupByParams.Collapse : null;
            groupLimit = groupByParams.HasGroupLimit ? (int?) groupByParams.GroupLimit : null;

            var expr = orderByExpr as Expression<Func<SPListItem, object[]>>;
            if (expr == null)
            {
                var singleExpr = orderByExpr as Expression<Func<SPListItem, object>>;
                if (singleExpr == null)
                {
                    throw new IncorrectCamlException(Tags.GroupBy);
                }
                expr = this.ensureArrayExpression(singleExpr);
            }
            return expr;
        }

        private Expression<Func<SPListItem, object[]>> getViewFieldsExpressionFromString(string existingViewFields)
        {
            existingViewFields = this.ensureParentTag(existingViewFields, Tags.ViewFields);
            existingViewFields = this.ensureParentTag(existingViewFields, Tags.Query);

            var translator = this.reTranslatorFactory.Create(existingViewFields);
            var viewFieldsExpr = translator.TranslateViewFields();

            var expr = viewFieldsExpr as Expression<Func<SPListItem, object[]>>;
            if (expr == null)
            {
                var singleExpr = viewFieldsExpr as Expression<Func<SPListItem, object>>;
                if (singleExpr == null)
                {
                    throw new IncorrectCamlException(Tags.ViewFields);
                }
                expr = this.ensureArrayExpression(singleExpr);
            }
            return expr;
        }

        public IQuery WhereAny(IEnumerable<Expression<Func<SPListItem, bool>>> expressions)
        {
            var combinedExpression = ExpressionsHelper.CombineOr(expressions);
            return this.Where(combinedExpression);
        }

        public IQuery WhereAny(string existingWhere, Expression<Func<SPListItem, bool>> expression)
        {
            return this.WhereAny(existingWhere, new List<Expression<Func<SPListItem, bool>>>(new[] { expression }));
        }

        public IQuery WhereAny(string existingWhere, IEnumerable<Expression<Func<SPListItem, bool>>> expressions)
        {
            var whereExpr = this.getWhereExpressionFromString(existingWhere);
            var exprs = new List<Expression<Func<SPListItem, bool>>>(expressions);
            exprs.Add(whereExpr);
            return this.WhereAny(exprs);
        }

        public IQuery WhereAny(IEnumerable<string> expressions)
        {
            if (expressions == null || !expressions.Any())
            {
                throw new EmptyExpressionsListException();
            }
            var exprs = new List<Expression<Func<SPListItem, bool>>>(expressions.Select(getWhereExpressionFromString));
            return this.WhereAny(exprs);
        }

        public IQuery OrderBy(Expression<Func<SPListItem, object>> expr)
        {
            var lambda = createArrayExpression(expr);
            return OrderBy(lambda);
        }

        public IQuery OrderBy(Expression<Func<SPListItem, object[]>> expr)
        {
            var translator = translatorFactory.Create(expr);
            this.orderBy = translator.TranslateOrderBy(expr);
            return this;
        }

        public IQuery OrderBy(IEnumerable<Expression<Func<SPListItem, object>>> expressions)
        {
            if (expressions == null || !expressions.Any())
            {
                throw new EmptyExpressionsListException();
            }

            var lambda = createArrayExpression(expressions);
            return OrderBy(lambda);
        }

        public IQuery OrderBy(string existingOrderBy, Expression<Func<SPListItem, object>> expr)
        {
            var newExpr = this.createArrayExpression(expr);
            return this.OrderBy(existingOrderBy, newExpr);
        }

        public IQuery OrderBy(string existingOrderBy, Expression<Func<SPListItem, object[]>> expr)
        {
            var existingExpr = this.getOrderByExpressionFromString(existingOrderBy);
            var exprs = new List<Expression<Func<SPListItem, object[]>>>(new[] { existingExpr, expr });
            var resultExpr = this.createArrayExpression(exprs);
            return OrderBy(resultExpr);
        }

        public IQuery OrderBy(string existingOrderBy, IEnumerable<Expression<Func<SPListItem, object>>> expressions)
        {
            var newExpr = this.createArrayExpression(expressions);
            return this.OrderBy(existingOrderBy, newExpr);
        }

        private Expression<Func<SPListItem, object[]>> createArrayExpression(IEnumerable<Expression<Func<SPListItem, object>>> expressions)
        {
            var expr = expressions.FirstOrDefault();
            if (expr == null)
            {
                throw new EmptyExpressionsListException();
            }

            return Expression.Lambda<Func<SPListItem, object[]>>(
                Expression.NewArrayInit(typeof(object), expressions.Select(e => e.Body)), expr.Parameters);
        }

        private Expression<Func<SPListItem, object[]>> createArrayExpression(IEnumerable<Expression<Func<SPListItem, object[]>>> expressions)
        {
            var expr = expressions.FirstOrDefault();
            if (expr == null)
            {
                throw new EmptyExpressionsListException();
            }

            var list = expressions.SelectMany(e => ((NewArrayExpression)e.Body).Expressions);

            return Expression.Lambda<Func<SPListItem, object[]>>(
                Expression.NewArrayInit(typeof(object), list), expr.Parameters);
        }

        private Expression<Func<SPListItem, object[]>> ensureArrayExpression(Expression<Func<SPListItem, object>> expr)
        {
            Expression<Func<SPListItem, object[]>> lambda = null;
            if (expr.Body.Type != typeof(object[]))
            {
                lambda = this.createArrayExpression(expr);
            }
            else
            {
                lambda = Expression.Lambda<Func<SPListItem, object[]>>(Expression.NewArrayInit(typeof(object), ((NewArrayExpression)expr.Body).Expressions), expr.Parameters);
            }
            return lambda;
        }

        private Expression<Func<SPListItem, object[]>> createArrayExpression(Expression<Func<SPListItem, object>> expr)
        {
            return Expression.Lambda<Func<SPListItem, object[]>>(Expression.NewArrayInit(typeof(object), expr.Body), expr.Parameters);
        }

        public IQuery GroupBy(Expression<Func<SPListItem, object>> expr)
        {
            var lambda = this.createArrayExpression(expr);
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
            var lambda = this.createArrayExpression(expr);
            return GroupBy(lambda, collapse, groupLimit);
        }

        public IQuery GroupBy(Expression<Func<SPListItem, object>> expr, bool? collapse)
        {
            var lambda = this.createArrayExpression(expr);
            return GroupBy(lambda, collapse, null);
        }

        public IQuery GroupBy(Expression<Func<SPListItem, object>> expr, int? groupLimit)
        {
            var lambda = this.createArrayExpression(expr);
            return GroupBy(lambda, null, groupLimit);
        }

        public IQuery GroupBy(string existingGroupBy, Expression<Func<SPListItem, object>> expr)
        {
            var lambda = this.ensureArrayExpression(expr);
            return GroupBy(existingGroupBy, lambda);
        }

        public IQuery GroupBy(string existingGroupBy, Expression<Func<SPListItem, object[]>> expr)
        {
            bool? existingCollapse;
            int? existingGroupLimit;
            var existingExpr = this.getGroupByExpressionFromString(existingGroupBy, out existingCollapse, out existingGroupLimit);
            var exprs = new List<Expression<Func<SPListItem, object[]>>>(new[] { existingExpr, expr });
            var resultExpr = this.createArrayExpression(exprs);
            return this.GroupBy(resultExpr, existingCollapse, existingGroupLimit);
        }

        public string ViewFields(Expression<Func<SPListItem, object>> expr)
        {
            return ViewFields(expr, false);
        }

        public string ViewFields(Expression<Func<SPListItem, object>> expr, bool includeViewFieldsTag)
        {
            var lambda = this.createArrayExpression(expr);
            return ViewFields(lambda, includeViewFieldsTag);
        }

        public string ViewFields(Expression<Func<SPListItem, object[]>> expr)
        {
            return ViewFields(expr, false);
        }

        public string ViewFields(Expression<Func<SPListItem, object[]>> expr, bool includeViewFieldsTag)
        {
            var translator = translatorFactory.Create(expr);
            var viewFields = translator.TranslateViewFields(expr);

            if (!includeViewFieldsTag)
            {
                var elements = viewFields.Elements();
                if (elements == null || !elements.Any())
                {
                    return string.Empty;
                }
                return this.convertToString(elements.ToArray());
            }
            return viewFields.ToString();
        }

        public string ViewFields(string existingViewFields, Expression<Func<SPListItem, object>> expr)
        {
            return ViewFields(existingViewFields, expr, false);
        }

        public string ViewFields(string existingViewFields, Expression<Func<SPListItem, object>> expr, bool includeViewFieldsTag)
        {
            var lambda = this.createArrayExpression(expr);
            return ViewFields(existingViewFields, lambda, includeViewFieldsTag);
        }

        public string ViewFields(string existingViewFields, Expression<Func<SPListItem, object[]>> expr)
        {
            return ViewFields(existingViewFields, expr, false);
        }

        public string ViewFields(string existingViewFields, Expression<Func<SPListItem, object[]>> expr, bool includeViewFieldsTag)
        {
            var existingExpr = this.getViewFieldsExpressionFromString(existingViewFields);
            var exprs = new List<Expression<Func<SPListItem, object[]>>>(new[] { existingExpr, expr });
            var resultExpr = this.createArrayExpression(exprs);
            return this.ViewFields(resultExpr, includeViewFieldsTag);
        }

        public string ViewFields(IEnumerable<string> titles)
        {
            return this.ViewFields(titles, false);
        }

        public string ViewFields(IEnumerable<string> titles, bool includeViewFieldsTag)
        {
            if (titles == null || titles.Any(t => t == null))
            {
                throw new ArgumentNullException();
            }

            return this.ViewFields(this.createExpressionFromArray(titles), includeViewFieldsTag);
        }

        public string ViewFields(IEnumerable<Guid> ids)
        {
            return this.ViewFields(ids, false);
        }

        public string ViewFields(IEnumerable<Guid> ids, bool includeViewFieldsTag)
        {
            if (ids == null)
            {
                throw new ArgumentNullException();
            }

            return this.ViewFields(this.createExpressionFromArray(ids), includeViewFieldsTag);
        }

        public string ViewFields(string existingViewFields, IEnumerable<string> titles)
        {
            return this.ViewFields(existingViewFields, titles, false);
        }

        public string ViewFields(string existingViewFields, IEnumerable<string> titles, bool includeViewFieldsTag)
        {
            if (titles == null || titles.Any(t => t == null))
            {
                throw new ArgumentNullException();
            }

            return this.ViewFields(existingViewFields, this.createExpressionFromArray(titles), includeViewFieldsTag);
        }

        public string ViewFields(string existingViewFields, IEnumerable<Guid> ids)
        {
            return this.ViewFields(existingViewFields, ids, false);
        }

        public string ViewFields(string existingViewFields, IEnumerable<Guid> ids, bool includeViewFieldsTag)
        {
            if (ids == null)
            {
                throw new ArgumentNullException();
            }
            return this.ViewFields(existingViewFields, this.createExpressionFromArray(ids), includeViewFieldsTag);
        }

        private Expression<Func<SPListItem, object[]>> createExpressionFromArray<T>(IEnumerable<T> items)
        {
            return Expression.Lambda<Func<SPListItem, object[]>>(
                Expression.NewArrayInit(
                    typeof(object),
                    (IEnumerable<Expression>)items.Select(
                        t => Expression.Call(Expression.Parameter(typeof(SPListItem), ReflectionHelper.CommonParameterName),
                                             typeof(SPListItem).GetMethod(ReflectionHelper.IndexerMethodName, new[] { typeof(T) }),
                                             new[] { Expression.Constant(t) })).ToArray()),
                Expression.Parameter(typeof(SPListItem), ReflectionHelper.CommonParameterName));
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
            var elements = this.ToCaml(includeQueryTag);
            return convertToString(elements);
        }

        public SPQuery ToSPQuery()
        {
            var query = new SPQuery();
            query.Query = this.ToString(false);
            return query;
        }

        private string convertToString(XElement[] elements)
        {
            var sb = new StringBuilder();
            Array.ForEach(elements, e => sb.Append(e.ToString()));
            return sb.ToString();
        }

        public static implicit operator string(Query query)
        {
            return query.ToString();
        }
    }
}
