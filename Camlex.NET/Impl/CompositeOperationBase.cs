using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl
{
    public abstract class CompositeOperationBase : OperationBase
    {
        protected IOperation leftOperation;
        protected IOperation rightOperation;

        protected CompositeOperationBase(IOperationResultBuilder operationResultBuilder,
            IOperation leftOperation, IOperation rightOperation) :
            base(operationResultBuilder)
        {
            this.leftOperation = leftOperation;
            this.rightOperation = rightOperation;
        }
    }
}
