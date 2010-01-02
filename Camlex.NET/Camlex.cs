using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Camlex.NET.Impl;
using Camlex.NET.Impl.Factories;
using Camlex.NET.Interfaces;
using Microsoft.SharePoint;


namespace Camlex.NET
{
    public class Camlex
    {
        private static ITranslatorFactory translatorFactory;
        private string where;
        private string orderBy;
        private string groupBy;

        static Camlex()
        {
            // factories setup
            var operandBuilder = new OperandBuilder();
            var analyzerFactory = new AnalyzerFactory(operandBuilder);
            translatorFactory = new TranslatorFactory(analyzerFactory);
        }

        private Camlex(string where)
        {
            this.where = where;
        }

        public static Camlex Where(Expression<Func<SPItem, bool>> expr)
        {
            var translator = translatorFactory.Create(expr.Body.NodeType);
            string where = translator.TranslateWhere(expr);
            return new Camlex(where);
        }

        public Camlex OrderBy(Expression<Func<SPItem, object>> expr)
        {
            throw new NotImplementedException();
        }

        public Camlex GroupBy(Expression<Func<SPItem, object>> expr)
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
