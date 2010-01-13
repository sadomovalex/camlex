using System;
using System.Xml.Linq;
using CamlexNET.Interfaces;

namespace CamlexNET.Impl.Operands
{
    internal abstract class ValueOperand<T> : IOperand
    {
        protected Type type;
        protected T value;

        public Type Type
        {
            get
            {
                return this.type;
            }
        }

        public string TypeName
        {
            get
            {
                return this.type.Name;
            }
        }

        public T Value
        {
            get
            {
                return this.value;
            }
        }

        protected ValueOperand(Type type, T value)
        {
            this.type = type;
            this.value = value;
        }

        public abstract XElement ToCaml();
    }
}


