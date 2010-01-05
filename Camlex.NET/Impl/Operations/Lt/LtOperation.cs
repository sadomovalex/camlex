using System.Xml.Linq;
using Camlex.NET.Impl.Factories;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Operations.Lt
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
            return this.operationResultBuilder.Add(result).ToResult();
        }
    }
}


