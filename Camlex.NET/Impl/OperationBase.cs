using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Camlex.NET.Impl.Operands;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl
{
    public abstract class OperationBase : IOperation
    {
        protected IOperand fieldRefOperand;
        protected IOperand valueOperand;

        protected OperationBase(IOperand fieldRefOperand, IOperand valueOperand)
        {
            this.fieldRefOperand = fieldRefOperand;
            this.valueOperand = valueOperand;
        }

        public abstract XElement ToCaml();
    }
}
