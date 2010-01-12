using System.Xml.Linq;
using CamlexNET.Interfaces;

namespace CamlexNET.Impl.Operations.BeginsWith
{
    public class BeginsWithOperation : BinaryOperationBase
    {
        public BeginsWithOperation(IOperationResultBuilder operationResultBuilder,
            IOperand fieldRefOperand, IOperand valueOperand)
            : base(operationResultBuilder, fieldRefOperand, valueOperand)
        {
        }

        public override IOperationResult ToResult()
        {
            var result = new XElement(Tags.BeginsWith,
                             fieldRefOperand.ToCaml(),
                             valueOperand.ToCaml());
            return operationResultBuilder.CreateResult(result);
        }
    }
}
