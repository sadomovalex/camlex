using System.Xml.Linq;
using CamlexNET.Impl.Factories;
using CamlexNET.Interfaces;

namespace CamlexNET.Impl.Operations.Gt
{
    internal class GtOperation : BinaryOperationBase
    {
        public GtOperation(IOperationResultBuilder operationResultBuilder,
            IOperand fieldRefOperand, IOperand valueOperand)
            : base(operationResultBuilder, fieldRefOperand, valueOperand)
        {
        }

        public override IOperationResult ToResult()
        {
            var result = new XElement(Tags.Gt,
                             this.fieldRefOperand.ToCaml(),
                             this.valueOperand.ToCaml());

            return this.operationResultBuilder.CreateResult(result);
        }
    }
}


