using System.Xml.Linq;
using CamlexNET.Interfaces;

namespace CamlexNET.Impl.Operations.Contains
{
    internal class ContainsOperation : BinaryOperationBase
    {
        public ContainsOperation(IOperationResultBuilder operationResultBuilder,
            IOperand fieldRefOperand, IOperand valueOperand)
            : base(operationResultBuilder, fieldRefOperand, valueOperand)
        {
        }

        public override IOperationResult ToResult()
        {
            var result = new XElement(Tags.Contains,
                             fieldRefOperand.ToCaml(),
                             valueOperand.ToCaml());
            return operationResultBuilder.CreateResult(result);
        }
    }
}
