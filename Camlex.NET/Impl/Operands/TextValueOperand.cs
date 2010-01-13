using System;
using System.Xml.Linq;
using CamlexNET.Interfaces;

namespace CamlexNET.Impl.Operands
{
    internal class TextValueOperand : ValueOperand<string>
    {
        public TextValueOperand(string value) :
            base(typeof(DataTypes.Text), value)
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


