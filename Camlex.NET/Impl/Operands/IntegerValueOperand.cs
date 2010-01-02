using System;
using System.Xml.Linq;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Operands
{
    public abstract class ValueOperand : IOperand
    {
        protected DataType type;
//        private object value;

        protected ValueOperand(DataType type/*, object value*/)
        {
            this.type = type;
//            this.value = value;
        }

        public abstract XElement ToCaml();
//        {
//            return
//                new XElement(Tags.Value, new XAttribute(Attributes.Type, this.type),
//                    new XText((string)this.value));
//        }
    }
}


