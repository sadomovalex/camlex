using System;
using System.Xml.Linq;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Operands
{
    public abstract class ValueOperand<T> : IOperand
    {
        protected DataType type;
        protected T value;

        public DataType Type
        {
            get
            {
                return this.type;
            }
        }

        public T Value
        {
            get
            {
                return this.value;
            }
        }

        protected ValueOperand(DataType type, T value)
        {
            this.type = type;
            this.value = value;
        }

        public abstract XElement ToCaml();
    }
}


