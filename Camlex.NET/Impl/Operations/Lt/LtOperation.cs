using System.Xml.Linq;
using CamlexNET.Impl.Factories;
using CamlexNET.Interfaces;

namespace CamlexNET.Impl.Operations.Lt
{
    public class LtOperation : BinaryOperationBase
    {
        public LtOperation(IOperationResultBuilder operationResultBuilder,
            IOperand fieldRefOperand, IOperand valueOperand)
            : base(operationResultBuilder, fieldRefOperand, valueOperand)
        {
        }

        public override IOperationResult ToResult()
        {
            var result = new XElement(Tags.Lt,
                             this.fieldRefOperand.ToCaml(),
                             this.valueOperand.ToCaml());
            return this.operationResultBuilder.CreateResult(result);
        }
    }
}


