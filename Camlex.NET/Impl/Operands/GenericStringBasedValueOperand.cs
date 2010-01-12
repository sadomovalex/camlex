using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace CamlexNET.Impl.Operands
{
    public class GenericStringBasedValueOperand : ValueOperand<string>
    {
        public GenericStringBasedValueOperand(Type type, string value) : base(type, value)
        {
        }

        public override XElement ToCaml()
        {
            return
                new XElement(Tags.Value, new XAttribute(Attributes.Type, this.TypeName),
                    new XText(this.Value));
        }
    }
}
