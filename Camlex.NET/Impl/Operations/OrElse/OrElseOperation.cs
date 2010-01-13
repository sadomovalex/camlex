using System.Xml.Linq;
using CamlexNET.Impl.Factories;
using CamlexNET.Interfaces;

namespace CamlexNET.Impl.Operations.OrElse
{
    internal class OrElseOperation : CompositeOperationBase
    {
        public OrElseOperation(IOperationResultBuilder operationResultBuilder,
            IOperation leftOperation, IOperation rightOperation)
            : base(operationResultBuilder, leftOperation, rightOperation)
        {
        }

        public override IOperationResult ToResult()
        {
            var result = new XElement(Tags.Or,
                             this.leftOperation.ToResult().Value,
                             this.rightOperation.ToResult().Value);
            return this.operationResultBuilder.CreateResult(result);
        }
    }
}


