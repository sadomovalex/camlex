using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace CamlexNET.Impl.Operands
{
    public class DateTimeValueOperand : ValueOperand<DateTime>
    {
        private bool includeTimeValue;

        public DateTimeValueOperand(DateTime value, bool includeTimeValue) :
            base(typeof(DataTypes.DateTime), value)
        {
            this.includeTimeValue = includeTimeValue;
        }

        public DateTimeValueOperand(string value, bool includeTimeValue) :
            base(typeof(DataTypes.DateTime), DateTime.MinValue)
        {
            this.includeTimeValue = includeTimeValue;
            if (!DateTime.TryParse(value, out this.value))
            {
                throw new InvalidValueForOperandTypeException(value, Type);
            }
        }

        public override XElement ToCaml()
        {
            if (includeTimeValue)
            {
                return new XElement(Tags.Value,
                                    new XAttribute(Attributes.Type, TypeName),
                                    new XAttribute(Attributes.IncludeTimeValue, true.ToString()),
                                    new XText(Value.ToString("s")));
            }
            return new XElement(Tags.Value,
                                new XAttribute(Attributes.Type, TypeName),
                                new XText(Value.ToString("s")));
        }
    }
}
