using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Camlex.NET.Impl.Operands;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl
{
    public abstract class OperationBase : IOperation
    {
        protected FieldRefOperand fieldRefOperand;
        protected ValueOperand valueOperand;

        protected OperationBase(FieldRefOperand fieldRefOperand, ValueOperand valueOperand)
        {
            this.fieldRefOperand = fieldRefOperand;
            this.valueOperand = valueOperand;
        }

        public abstract string ToCaml();
    }
}
