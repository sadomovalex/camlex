using System.Xml.Linq;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Operations.Eq
{
    public class EqOperation : OperationBase
    {
        public EqOperation(IOperand fieldRefOperand, IOperand valueOperand)
            : base(fieldRefOperand, valueOperand)
        {
        }

        public override XElement ToCaml()
        {
            return
                new XElement(Tags.Eq,
                             this.fieldRefOperand.ToCaml(),
                             this.valueOperand.ToCaml());
        }
    }
}


