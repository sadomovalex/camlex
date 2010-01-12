using System.Collections.Generic;
using System.Xml.Linq;
using CamlexNET.Impl.Factories;
using CamlexNET.Interfaces;

namespace CamlexNET.Impl.Operations.Array
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
            var results = new List<XElement>();
            System.Array.ForEach(this.fieldRefOperands, x => results.Add(x.ToCaml()));
            return this.operationResultBuilder.CreateResult(results.ToArray());
        }
    }
}


