using System.Xml.Linq;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Operations.AndAlso
{
    public class AndAlsoOperation : CompositeOperationBase
    {
        public AndAlsoOperation(IOperation leftOperation, IOperation rightOperation)
            : base(leftOperation, rightOperation)
        {
        }

        public override XElement ToCaml()
        {
            return
                new XElement(Tags.And,
                             this.leftOperation.ToCaml(),
                             this.rightOperation.ToCaml());
        }
    }
}


