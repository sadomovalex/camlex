using System.Xml.Linq;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Operands
{
    public class FieldRefOperand : IOperand
    {
        private string name;

        public FieldRefOperand(string name)
        {
            this.name = name;
        }

        public virtual XElement ToCaml()
        {
            return new XElement(Tags.FieldRef, new XAttribute(Attributes.Name, this.name));
        }
    }
}


