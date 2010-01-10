using System;
using System.Xml.Linq;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Operands
{
    public class BooleanValueOperand : ValueOperand<bool>
    {
        public BooleanValueOperand(bool value) :
            base(typeof(DataTypes.Boolean), value)
        {
        }

        public BooleanValueOperand(string value) :
            base(typeof(DataTypes.Boolean), false)
        {
            if (!bool.TryParse(value, out this.value))
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


