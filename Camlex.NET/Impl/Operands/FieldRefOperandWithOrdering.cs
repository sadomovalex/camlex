using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Camlex.NET.Impl.Operands
{
    public class FieldRefOperandWithOrdering : FieldRefOperand
    {
        private readonly bool ascending;

        public bool Ascending
        {
            get { return ascending; }
        }

        public FieldRefOperandWithOrdering(FieldRefOperand fieldRefOperand, Camlex.OrderDirection orderDirection)
            : base(fieldRefOperand.FieldName)
        {
            ascending = orderDirection is Camlex.Asc;
        }

        public override XElement ToCaml()
        {
            var xmlElement = base.ToCaml();
            xmlElement.SetAttributeValue(Attributes.Ascending, Ascending.ToString());
            return xmlElement;
        }
    }
}
