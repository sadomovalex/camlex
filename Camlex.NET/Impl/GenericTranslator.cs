using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
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

        public string Translate(Expression<Func<SPItem, bool>> expr)
        {
            if (!this.analyzer.IsValid(expr))
            {
                throw new NonSupportedExpressionException(expr);
            }

            var operation = this.analyzer.GetOperation(expr);
            var caml = operation.ToCaml();
            return caml.ToString();
        }
    }
}
