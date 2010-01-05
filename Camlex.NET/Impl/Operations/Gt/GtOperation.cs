using System.Xml.Linq;
using Camlex.NET.Impl.Factories;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Operations.Gt
{
    public class GtOperation : BinaryOperationBase
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


