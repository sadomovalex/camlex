using System.Xml.Linq;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Operations.Leq
{
    public class LeqOperation : OperationBase
    {
        public LeqOperation(IOperand fieldRefOperand, IOperand valueOperand)
            : base(fieldRefOperand, valueOperand)
        {
        }

        public override XElement ToCaml()
        {
            return
                new XElement(Tags.Leq,
                             this.fieldRefOperand.ToCaml(),
                             this.valueOperand.ToCaml());
        }
    }
}


