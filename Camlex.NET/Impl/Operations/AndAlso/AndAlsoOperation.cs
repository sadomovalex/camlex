using System.Xml.Linq;
using CamlexNET.Impl.Factories;
using CamlexNET.Interfaces;

namespace CamlexNET.Impl.Operations.AndAlso
{
    public class AndAlsoOperation : CompositeOperationBase
    {
        public AndAlsoOperation(IOperationResultBuilder operationResultBuilder,
            IOperation leftOperation, IOperation rightOperation)
            : base(operationResultBuilder, leftOperation, rightOperation)
        {
        }

        public override IOperationResult ToResult()
        {
            var result = new XElement(Tags.And,
                             this.leftOperation.ToResult().Value,
                             this.rightOperation.ToResult().Value);
            return this.operationResultBuilder.CreateResult(result);
        }
    }
}


