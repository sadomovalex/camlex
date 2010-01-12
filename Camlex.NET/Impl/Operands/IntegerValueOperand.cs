using System;
using System.Xml.Linq;
using CamlexNET.Interfaces;

namespace CamlexNET.Impl.Operands
{
    public class IntegerValueOperand : ValueOperand<int>
    {
        public IntegerValueOperand(int value) :
            base(typeof(DataTypes.Integer), value)
        {
        }

        public IntegerValueOperand(string value) :
            base(typeof(DataTypes.Integer), 0)
        {
            if (!int.TryParse(value, out this.value))
            {
                throw new InvalidValueForOperandTypeException(value, this.Type);
            }
        }

        public override XElement ToCaml()
        {
            return
                new XElement(Tags.Value, new XAttribute(Attributes.Type, this.TypeName),
                    new XText(this.Value.ToString()));
        }
    }
}


