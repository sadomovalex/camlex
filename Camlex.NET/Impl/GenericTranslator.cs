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
        private readonly ILogicalAnalyzer logicalAnalyzer;
        private readonly IArrayAnalyzer arrayAnalyzer;

        public GenericTranslator(ILogicalAnalyzer logicalAnalyzer)
        {
            this.logicalAnalyzer = logicalAnalyzer;
        }

        public GenericTranslator(IArrayAnalyzer arrayAnalyzer)
        {
            this.arrayAnalyzer = arrayAnalyzer;
        }

        public string TranslateWhere(Expression<Func<SPItem, bool>> expr)
        {
            if (!this.logicalAnalyzer.IsValid(expr))
            {
                throw new NonSupportedExpressionException(expr);
            }

            var operation = this.logicalAnalyzer.GetOperation(expr);
            var operationCaml = operation.ToCaml();

            var caml = new XElement(Tags.Where, operationCaml);
            return caml.ToString();
        }

        public string TranslateOrderBy(Expression<Func<SPItem, object[]>> expr)
        {
            if (!this.arrayAnalyzer.IsValid(expr))
            {
                throw new NonSupportedExpressionException(expr);
            }

            var operation = this.arrayAnalyzer.GetOperation(expr);
            var operationCaml = operation.ToCaml();

            var caml = new XElement(Tags.OrderBy, operationCaml);
            return caml.ToString();
        }

    }
}
