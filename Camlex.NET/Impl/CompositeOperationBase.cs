using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using CamlexNET.Interfaces;

namespace CamlexNET.Impl
{
    internal abstract class CompositeOperationBase : OperationBase
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
