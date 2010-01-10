using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Camlex.NET.Impl.Operands
{
    public class NoteValueOperand : ValueOperand<string>
    {
        public NoteValueOperand(string value) :
            base(typeof(DataTypes.Note), value)
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
