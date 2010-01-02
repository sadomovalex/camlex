using System.Xml.Linq;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Operands
{
    public class FieldRefOperand : IOperand
    {
        private readonly string fieldName;

        public string FieldName
        {
            get
            {
                return this.fieldName;
            }
        }

        public FieldRefOperand(string fieldName)
        {
            this.fieldName = fieldName;
        }

        public virtual XElement ToCaml()
        {
            return new XElement(Tags.FieldRef, new XAttribute(Attributes.Name, this.fieldName));
        }
    }
}


