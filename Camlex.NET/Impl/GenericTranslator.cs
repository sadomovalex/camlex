using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Linq;
using CamlexNET.Impl.Operations.Results;
using CamlexNET.Interfaces;
using Microsoft.SharePoint;

namespace CamlexNET.Impl
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
            var operationCaml = operation.ToResult().Value;

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
            var result = (XElementArrayOperationResult)operation.ToResult();

            var caml = new XElement(Tags.OrderBy, result.Value);
            return caml;
        }

        public XElement TranslateGroupBy(LambdaExpression expr, bool? collapse, int? groupLimit)
        {
            if (!this.analyzer.IsValid(expr))
            {
                throw new NonSupportedExpressionException(expr);
            }

            var operation = this.analyzer.GetOperation(expr);
            var result = (XElementArrayOperationResult)operation.ToResult();

            var caml = new XElement(Tags.GroupBy, result.Value);
            if (collapse != null)
            {
                caml.SetAttributeValue(Attributes.Collapse, collapse.Value.ToString());
            }
            if (groupLimit != null)
            {
                caml.SetAttributeValue(Attributes.GroupLimit, groupLimit.Value);
            }

            return caml;
        }
    }
}
