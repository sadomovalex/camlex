using System.Xml.Linq;
using Camlex.NET.Impl.Factories;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Operations.Eq
{
    public class EqOperation : BinaryOperationBase
    {
        public EqOperation(IOperationResultBuilder operationResultBuilder,
            IOperand fieldRefOperand, IOperand valueOperand)
            : base(operationResultBuilder, fieldRefOperand, valueOperand)
        {
        }

        public override IOperationResult ToResult()
        {
            var result = new XElement(Tags.Eq,
                             this.fieldRefOperand.ToCaml(),
                             this.valueOperand.ToCaml());
            return this.operationResultBuilder.Add(result).ToResult();
        }
    }
}


