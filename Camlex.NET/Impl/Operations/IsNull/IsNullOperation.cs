using System.Xml.Linq;
using Camlex.NET.Impl.Factories;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Operations.IsNull
{
    public class IsNullOperation : OperationBase
    {
        public IsNullOperation(IOperand fieldRefOperand/*, IOperand valueOperand*/)
            : base(fieldRefOperand, null)
        {
        }

        public override IOperationResult ToResult()
        {
            var result = new XElement(Tags.IsNull,
                             this.fieldRefOperand.ToCaml()/*,
                             this.valueOperand.ToCaml()*/);
            return new OperationResultBuilder().Add(result).ToResult();
        }
    }
}


