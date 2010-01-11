using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Camlex.NET.Impl.Operands
{
    public class DateTimeValueOperand : ValueOperand<DateTime>
    {
        public DateTimeValueOperand(DateTime value) :
            base(typeof(DataTypes.DateTime), value)
        {
        }

        public DateTimeValueOperand(string value) :
            base(typeof(DataTypes.DateTime), DateTime.MinValue)
        {
            if (!DateTime.TryParse(value, out this.value))
            {
                throw new InvalidValueForOperandTypeException(value, Type);
            }
        }

        public override XElement ToCaml()
        {
            return
                new XElement(Tags.Value,
                    new XAttribute(Attributes.Type, TypeName),
                    new XAttribute(Attributes.IncludeTimeValue, true.ToString()),
                    new XText(Value.ToString("s")));
        }
    }
}
