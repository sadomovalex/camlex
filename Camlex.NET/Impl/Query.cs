using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Linq;
using Camlex.NET.Interfaces;
using Microsoft.SharePoint;

namespace Camlex.NET.Impl
{
    public class Query : IQuery
    {
        private readonly ITranslatorFactory translatorFactory;
        private XElement where;
        private XElement orderBy;
        private XElement groupBy;

        public Query(ITranslatorFactory translatorFactory)
        {
            this.translatorFactory = translatorFactory;
        }

        public IQuery Where(Expression<Func<SPItem, bool>> expr)
        {
            var translator = translatorFactory.Create(expr);
            this.where = translator.TranslateWhere(expr);
            return this;
        }

        public IQuery OrderBy(Expression<Func<SPItem, object>> expr)
        {
            var lambda = Expression.Lambda<Func<SPItem, object[]>>(
                Expression.NewArrayInit(typeof(object), expr.Body), expr.Parameters);
            return OrderBy(lambda);
        }

        public IQuery OrderBy(Expression<Func<SPItem, object[]>> expr)
        {
            var translator = translatorFactory.Create(expr);
            this.orderBy = translator.TranslateOrderBy(expr);
            return this;
        }

        public IQuery GroupBy(Expression<Func<SPItem, object>> expr)
        {
            var lambda = Expression.Lambda<Func<SPItem, object[]>>(
                Expression.NewArrayInit(typeof(object), expr.Body), expr.Parameters);
            return GroupBy(lambda, null, null);
        }

        public IQuery GroupBy(Expression<Func<SPItem, object[]>> expr, bool? collapse, int? groupLimit)
        {
            var translator = translatorFactory.Create(expr);
            this.groupBy = translator.TranslateGroupBy(expr, collapse, groupLimit);
            return this;
        }

        public XElement ToCaml()
        {
            return new XElement(Tags.Query,
                this.where, this.orderBy, this.groupBy);
        }

        public override string ToString()
        {
            return this.ToCaml().ToString();
        }

        public static implicit operator string(Query query)
        {
            return query.ToString();
        }
    }
}
