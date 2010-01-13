using System.Xml.Linq;
using CamlexNET.Interfaces;

namespace CamlexNET.Impl.Operands
{
    internal class FieldRefOperand : IOperand
    {
        protected readonly string fieldName;

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


