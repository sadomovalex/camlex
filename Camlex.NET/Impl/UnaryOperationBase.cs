using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using CamlexNET.Impl.Operands;
using CamlexNET.Interfaces;

namespace CamlexNET.Impl
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
