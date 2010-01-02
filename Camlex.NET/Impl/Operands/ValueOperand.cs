using System;
using System.Xml.Linq;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Operands
{
    public class IntegerValueOperand : ValueOperand
    {
        private readonly int value;

        public IntegerValueOperand(DataType type, int value):
            base(type)
        {
            this.type = type;
            this.value = value;
        }

        public override XElement ToCaml()
        {
            return
                new XElement(Tags.Value, new XAttribute(Attributes.Type, this.type),
                    new XText(this.value.ToString()));
        }
    }
}


