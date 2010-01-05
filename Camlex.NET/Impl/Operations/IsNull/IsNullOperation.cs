using System.Xml.Linq;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Operations.IsNull
{
    public class IsNullOperation : OperationBase
    {
        public IsNullOperation(IOperand fieldRefOperand/*, IOperand valueOperand*/)
            : base(fieldRefOperand, null)
        {
        }

        public override XElement ToCaml()
        {
            return
                new XElement(Tags.IsNull,
                             this.fieldRefOperand.ToCaml()/*,
                             this.valueOperand.ToCaml()*/);
        }
    }
}


