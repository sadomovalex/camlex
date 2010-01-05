using System.Xml.Linq;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Operations.OrElse
{
    public class OrElseOperation : CompositeOperationBase
    {
        public OrElseOperation(IOperation leftOperation, IOperation rightOperation)
            : base(leftOperation, rightOperation)
        {
        }

        public override XElement ToCaml()
        {
            return
                new XElement(Tags.Or,
                             this.leftOperation.ToCaml(),
                             this.rightOperation.ToCaml());
        }
    }
}


