using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Camlex.NET.Impl.Operands;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl
{
    public abstract class UnaryOperationBase : OperationBase
    {
        protected IOperand fieldRefOperand;

        protected UnaryOperationBase(IOperationResultBuilder operationResultBuilder,
            IOperand fieldRefOperand) :
            base(operationResultBuilder)
        {
            this.fieldRefOperand = fieldRefOperand;
        }
    }
}
