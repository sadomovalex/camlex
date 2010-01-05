using System.Xml.Linq;
using Camlex.NET.Impl.Factories;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Operations.Eq
{
    public class EqOperation : OperationBase
    {
        public EqOperation(IOperand fieldRefOperand, IOperand valueOperand)
            : base(fieldRefOperand, valueOperand)
        {
        }

        public override IOperationResult ToResult()
        {
            var result = new XElement(Tags.Eq,
                             this.fieldRefOperand.ToCaml(),
                             this.valueOperand.ToCaml());
            return new OperationResultBuilder().Add(result).ToResult();
        }
    }
}


