using System.Xml.Linq;
using Camlex.NET.Impl.Factories;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Operations.Geq
{
    public class GeqOperation : OperationBase
    {
        public GeqOperation(IOperand fieldRefOperand, IOperand valueOperand)
            : base(fieldRefOperand, valueOperand)
        {
        }

        public override IOperationResult ToResult()
        {
            var result = new XElement(Tags.Geq,
                             this.fieldRefOperand.ToCaml(),
                             this.valueOperand.ToCaml());
            return new OperationResultBuilder().Add(result).ToResult();
        }
    }
}


