using System.Xml.Linq;
using Camlex.NET.Impl.Factories;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Operations.AndAlso
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


