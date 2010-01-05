using System.Collections.Generic;
using System.Xml.Linq;
using Camlex.NET.Impl.Factories;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Operations.Array
{
    public class ArrayOperation : OperationBase
    {
        private readonly IOperand[] fieldRefOperands;

        public ArrayOperation(IOperationResultBuilder operationResultBuilder,
            params IOperand[] fieldRefOperands) :
            base(operationResultBuilder)
        {
            this.fieldRefOperands = fieldRefOperands;
        }

        public override IOperationResult ToResult()
        {
            System.Array.ForEach(this.fieldRefOperands, x => this.operationResultBuilder.Add(x.ToCaml()));
            return this.operationResultBuilder.ToResult();
        }
    }
}


