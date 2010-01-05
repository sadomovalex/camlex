using System.Xml.Linq;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Operations.Gt
{
    public class GtOperation : OperationBase
    {
        public GtOperation(IOperand fieldRefOperand, IOperand valueOperand)
            : base(fieldRefOperand, valueOperand)
        {
        }

        public override XElement ToCaml()
        {
            return
                new XElement(Tags.Gt,
                             this.fieldRefOperand.ToCaml(),
                             this.valueOperand.ToCaml());
        }
    }
}


