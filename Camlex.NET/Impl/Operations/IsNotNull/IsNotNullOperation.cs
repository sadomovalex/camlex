using System.Xml.Linq;
using Camlex.NET.Impl.Factories;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Operations.IsNotNull
{
    public class IsNotNullOperation : UnaryOperationBase
    {
        public IsNotNullOperation(IOperationResultBuilder operationResultBuilder,
            IOperand fieldRefOperand)
            : base(operationResultBuilder, fieldRefOperand)
        {
        }

        public override IOperationResult ToResult()
        {
            var result = new XElement(Tags.IsNotNull,
                             this.fieldRefOperand.ToCaml());
            return this.operationResultBuilder.Add(result).ToResult();
        }
    }
}


