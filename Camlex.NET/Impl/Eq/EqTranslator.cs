using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Linq;
using Camlex.NET.Interfaces;
using Microsoft.SharePoint;

namespace Camlex.NET.Impl.Eq
{
    public class EqTranslator : ITranslator
    {
        private readonly IAnalyzer analyzer;

        public EqTranslator(IAnalyzer analyzer)
        {
            this.analyzer = analyzer;
        }

        public string Translate(Expression<Func<SPItem, bool>> expr)
        {
            if (!this.analyzer.IsValid(expr))
            {
                throw new NonSupportedExpressionException(expr);
            }

            var leftOperand = this.analyzer.GetLeftOperand(expr);
            var rightOperand = this.analyzer.GetLeftOperand(expr);

            var caml =
                new XElement(Tags.Eq,
                             new XElement(leftOperand.ToCaml()),
                             new XElement(rightOperand.ToCaml()));
            return caml.ToString();
        }
    }
}