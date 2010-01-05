using System.Xml.Linq;
using Camlex.NET.Impl.Factories;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Operations.OrElse
{
    public class OrElseOperation : CompositeOperationBase
    {
        public OrElseOperation(IOperation leftOperation, IOperation rightOperation)
            : base(leftOperation, rightOperation)
        {
        }

        public override IOperationResult ToResult()
        {
            var result = new XElement(Tags.Or,
                             this.leftOperation.ToResult().Value,
                             this.rightOperation.ToResult().Value);
            return new OperationResultBuilder().Add(result).ToResult();
        }
    }
}


