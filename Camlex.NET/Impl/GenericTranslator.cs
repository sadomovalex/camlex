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
    public class GenericTranslator : ITranslator
    {
        private readonly IAnalyzer analyzer;

        public GenericTranslator(IAnalyzer analyzer)
        {
            this.analyzer = analyzer;
        }

        public XElement TranslateWhere(LambdaExpression expr)
        {
            if (!this.analyzer.IsValid(expr))
            {
                throw new NonSupportedExpressionException(expr);
            }

            var operation = this.analyzer.GetOperation(expr);
            var operationCaml = operation.ToCaml();

            var caml = new XElement(Tags.Where, operationCaml);
            return caml;
        }

        public XElement TranslateOrderBy(LambdaExpression expr)
        {
            if (!this.analyzer.IsValid(expr))
            {
                throw new NonSupportedExpressionException(expr);
            }

            var operation = this.analyzer.GetOperation(expr);
            var operationCaml = operation.ToCaml();

            var caml = new XElement(Tags.OrderBy, operationCaml);
            return caml;
        }

    }
}
