using System.Xml.Linq;
using Camlex.NET.Impl.Factories;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Operations.IsNotNull
{
    public class IsNotNullOperation : OperationBase
    {
        public IsNotNullOperation(IOperand fieldRefOperand/*, IOperand valueOperand*/)
            : base(fieldRefOperand, null)
        {
        }

        public override IOperationResult ToResult()
        {
            var result = new XElement(Tags.IsNotNull,
                             this.fieldRefOperand.ToCaml()/*,
                             this.valueOperand.ToCaml()*/);
            return new OperationResultBuilder().Add(result).ToResult();
        }
    }
}


