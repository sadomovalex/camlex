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
                if (!this.tryConvertViaInteger(value, out this.value))
                {
                    throw new InvalidValueForOperandTypeException(value, this.Type);
                }
            }
        }

        private bool tryConvertViaInteger(string value, out bool result)
        {
            result = false;
            try
            {
                int val = Convert.ToInt32(value);
                if (val != 0 && val != 1)
                {
                    return false;
                }
                result = Convert.ToBoolean(val);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public override XElement ToCaml()
        {
            // 0 and 1 should be used instead True and False
            return
                new XElement(Tags.Value, new XAttribute(Attributes.Type, this.TypeName),
                    new XText((Convert.ToInt32(this.Value)).ToString()));
        }
    }
}


