using System.Collections.Generic;
using System.Xml.Linq;
using Camlex.NET.Impl.Factories;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Operations.Array
{
    public class ArrayOperation : IOperation
    {
        private readonly IOperand[] fieldRefOperands;

        public ArrayOperation(params IOperand[] fieldRefOperands)
        {
            this.fieldRefOperands = fieldRefOperands;
        }

        public IOperationResult ToResult()
        {
            var builder = new OperationResultBuilder();
            System.Array.ForEach(this.fieldRefOperands, x => builder.Add(x.ToCaml()));
            return builder.ToResult();
        }
    }
}


