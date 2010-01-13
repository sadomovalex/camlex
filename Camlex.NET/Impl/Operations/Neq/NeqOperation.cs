using System.Xml.Linq;
using CamlexNET.Impl.Factories;
using CamlexNET.Interfaces;

namespace CamlexNET.Impl.Operations.Neq
{
    internal class NeqOperation : BinaryOperationBase
    {
        public NeqOperation(IOperationResultBuilder operationResultBuilder,
            IOperand fieldRefOperand, IOperand valueOperand)
            : base(operationResultBuilder, fieldRefOperand, valueOperand)
        {
        }

        public override IOperationResult ToResult()
        {
            var result = new XElement(Tags.Neq,
                             this.fieldRefOperand.ToCaml(),
                             this.valueOperand.ToCaml());
            return this.operationResultBuilder.CreateResult(result);
        }
    }
}


