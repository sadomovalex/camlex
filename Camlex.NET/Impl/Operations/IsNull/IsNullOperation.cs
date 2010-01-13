using System.Xml.Linq;
using CamlexNET.Impl.Factories;
using CamlexNET.Interfaces;

namespace CamlexNET.Impl.Operations.IsNull
{
    internal class IsNullOperation : UnaryOperationBase
    {
        public IsNullOperation(IOperationResultBuilder operationResultBuilder,
            IOperand fieldRefOperand)
            : base(operationResultBuilder, fieldRefOperand)
        {
        }

        public override IOperationResult ToResult()
        {
            var result = new XElement(Tags.IsNull,
                             this.fieldRefOperand.ToCaml());
            return this.operationResultBuilder.CreateResult(result);
        }
    }
}


