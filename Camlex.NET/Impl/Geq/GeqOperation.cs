using System.Xml.Linq;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Geq
{
    public class GeqOperation : OperationBase
    {
        public GeqOperation(IOperand fieldRefOperand, IOperand valueOperand)
            : base(fieldRefOperand, valueOperand)
        {
        }

        public override XElement ToCaml()
        {
            return
                new XElement(Tags.Geq,
                             this.fieldRefOperand.ToCaml(),
                             this.valueOperand.ToCaml());
        }
    }
}


