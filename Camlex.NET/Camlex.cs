using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Linq;
using Camlex.NET.Impl;
using Camlex.NET.Impl.Factories;
using Camlex.NET.Interfaces;
using Microsoft.SharePoint;

namespace Camlex.NET
{
    public class Camlex
    {
        #region OrderBy functionality
        /// <summary>Marker class representing ASC order direction for "OrderBy" functionality</summary>
        public class OrderDirection
        {
            public static OrderDirection Default { get { return new Asc(); } }
            public static OrderDirection Convert(Type type)
            {
                return type == typeof (Asc) ? (OrderDirection) new Asc() : new Desc();
            }
        }
        /// <summary>Marker class representing ASC order direction for "OrderBy" functionality</summary>
        public class Asc : OrderDirection { }
        /// <summary>Marker class representing DESC order direction for "OrderBy" functionality</summary>
        public class Desc : OrderDirection { }
        #endregion

        private static ITranslatorFactory translatorFactory;
        private XElement where;
        private XElement orderBy;
        private XElement groupBy;

        static Camlex()
        {
            // factories setup
            var operandBuilder = new OperandBuilder();
            var analyzerFactory = new AnalyzerFactory(operandBuilder);
            translatorFactory = new TranslatorFactory(analyzerFactory);
        }

        private Camlex(XElement where)
        {
            this.where = where;
        }

        public static Camlex Where(Expression<Func<SPItem, bool>> expr)
        {
            var translator = translatorFactory.Create(expr);
            var where = translator.TranslateWhere(expr);
            return new Camlex(where);
        }

        public Camlex OrderBy(Expression<Func<SPItem, object[]>> expr)
        {
            var translator = translatorFactory.Create(expr);
            var orderBy = translator.TranslateOrderBy(expr);
            this.orderBy = orderBy;
            return this;
        }

        public Camlex GroupBy(Expression<Func<SPItem, object[]>> expr)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return string.Format("{0}{1}{2}", this.where, this.orderBy, this.groupBy);
        }

        public static implicit operator string(Camlex camlex)
        {
            return camlex.ToString();
        }
    }
}
