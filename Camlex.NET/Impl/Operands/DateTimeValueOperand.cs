using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace CamlexNET.Impl.Operands
{
    internal class DateTimeValueOperand : ValueOperand<DateTime>
    {
        public bool IncludeTimeValue { get; set; }

        public DateTimeValueOperand(DateTime value, bool includeTimeValue) :
            base(typeof(DataTypes.DateTime), value)
        {
            IncludeTimeValue = includeTimeValue;
        }

        public DateTimeValueOperand(string value, bool includeTimeValue) :
            base(typeof(DataTypes.DateTime), DateTime.MinValue)
        {
            IncludeTimeValue = includeTimeValue;

            if (value == Camlex.Now)
            {
                this.value = DateTime.Now;
            }
            else if (value == Camlex.Today)
            {
                this.value = DateTime.Today;
            }
            else if (!DateTime.TryParse(value, out this.value))
            {
                throw new InvalidValueForOperandTypeException(value, Type);
            }
        }

        public override XElement ToCaml()
        {
            if (IncludeTimeValue)
            {
                return new XElement(Tags.Value,
                                    new XAttribute(Attributes.Type, TypeName),
                                    new XAttribute(Attributes.IncludeTimeValue, true.ToString()),
                                    new XText(Value.ToString("s") + "Z"));
            }
            return new XElement(Tags.Value,
                                new XAttribute(Attributes.Type, TypeName),
                                new XText(Value.ToString("s") + "Z"));
        }
    }
}
