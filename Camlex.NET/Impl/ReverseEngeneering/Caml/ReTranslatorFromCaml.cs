using System;
using System.Linq.Expressions;
using System.Xml.Linq;
using CamlexNET.Interfaces.ReverseEngeneering;

namespace CamlexNET.Impl.ReverseEngeneering.Caml
{
    internal class ReTranslatorFromCaml : IReTranslator
    {
        private readonly IReAnalyzer analyzerForWhere;
        private readonly IReAnalyzer analyzerForOrderBy;
        private readonly IReAnalyzer analyzerForGroupBy;
        private readonly IReAnalyzer analyzerForViewFields;

        public XElement Where { get { return this.getElement(this.analyzerForWhere); } }
        public XElement OrderBy { get { return this.getElement(this.analyzerForOrderBy); } }
        public XElement GroupBy { get { return this.getElement(this.analyzerForGroupBy); } }
        public XElement ViewFields { get { return this.getElement(this.analyzerForViewFields); } }

        private XElement getElement(IReAnalyzer analyzer)
        {
            return (analyzer == null ? null : analyzer.Element);
        }

        public ReTranslatorFromCaml(IReAnalyzer analyzerForWhere, IReAnalyzer analyzerForOrderBy,
            IReAnalyzer analyzerForGroupBy, IReAnalyzer analyzerForViewFields)
        {
            this.analyzerForWhere = analyzerForWhere;
            this.analyzerForOrderBy = analyzerForOrderBy;
            this.analyzerForGroupBy = analyzerForGroupBy;
            this.analyzerForViewFields = analyzerForViewFields;
        }

        public Expression TranslateWhere()
        {
            return this.translate(this.analyzerForWhere, Tags.Where);
        }

        private Expression translate(IReAnalyzer analyzer, string tag)
        {
            if (analyzer == null)
            {
                return null;
            }
            if (!analyzer.IsValid())
            {
                throw new IncorrectCamlException(tag);
            }
            var operation = analyzer.GetOperation();
            return operation.ToExpression();
        }

        public Expression TranslateOrderBy()
        {
            return this.translate(this.analyzerForOrderBy, Tags.OrderBy);
        }

        public Expression TranslateGroupBy()
        {
            return this.translate(this.analyzerForGroupBy, Tags.GroupBy);
        }

        public Expression TranslateViewFields()
        {
            return this.translate(this.analyzerForViewFields, Tags.ViewFields);
        }
    }
}
