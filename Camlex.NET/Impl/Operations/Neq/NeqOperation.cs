using System.Xml.Linq;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Operations.Neq
{
    public class NeqOperation : OperationBase
    {
        public NeqOperation(IOperand fieldRefOperand, IOperand valueOperand)
            : base(fieldRefOperand, valueOperand)
        {
        }

        public override XElement ToCaml()
        {
            return
                new XElement(Tags.Neq,
                             this.fieldRefOperand.ToCaml(),
                             this.valueOperand.ToCaml());
        }
    }
}


