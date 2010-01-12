using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using CamlexNET.Impl.Operands;
using CamlexNET.Interfaces;

namespace CamlexNET.Impl
{
    public abstract class BinaryOperationBase : OperationBase
    {
        protected IOperand fieldRefOperand;
        protected IOperand valueOperand;

        protected BinaryOperationBase(IOperationResultBuilder operationResultBuilder,
            IOperand fieldRefOperand, IOperand valueOperand) :
            base(operationResultBuilder)
        {
            this.fieldRefOperand = fieldRefOperand;
            this.valueOperand = valueOperand;
        }
    }
}
