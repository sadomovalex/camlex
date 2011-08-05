using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace CamlexNET.Impl.Operands
{
    internal class LookupIdValueOperand : GenericStringBasedValueOperand
    {
        public LookupIdValueOperand(string value)
            : base(typeof(DataTypes.LookupId), value)
        {
            int id;
            if (!int.TryParse(value, out id))
            {
                throw new InvalidValueForOperandTypeException(value, this.Type);
            }
        }

        public override XElement ToCaml()
        {
            // use Lookup type both for LookupValue operand and LookupId operand
            return
                new XElement(Tags.Value, new XAttribute(Attributes.Type, typeof(DataTypes.Lookup).Name),
                    new XText(this.Value));
        }
    }
}
