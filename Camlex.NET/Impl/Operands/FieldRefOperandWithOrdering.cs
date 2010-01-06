using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Camlex.NET.Impl.Operands
{
    public class FieldRefOperandWithOrdering : FieldRefOperand
    {
        private readonly Camlex.OrderDirection orderDirection;

        public FieldRefOperandWithOrdering(FieldRefOperand fieldRefOperand, Camlex.OrderDirection orderDirection)
            : base(fieldRefOperand.FieldName)
        {
            this.orderDirection = orderDirection;
        }

        public override XElement ToCaml()
        {
            var xmlElement = base.ToCaml();
            if (!orderDirection.IsDefault())
            {
                xmlElement.SetAttributeValue(Attributes.Ascending, orderDirection.ToString());
            }
            return xmlElement;
        }
    }
}
