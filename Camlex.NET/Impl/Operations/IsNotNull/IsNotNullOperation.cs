using System.Xml.Linq;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Operations.IsNotNull
{
    public class IsNotNullOperation : OperationBase
    {
        public IsNotNullOperation(IOperand fieldRefOperand/*, IOperand valueOperand*/)
            : base(fieldRefOperand, null)
        {
        }

        public override XElement ToCaml()
        {
            return
                new XElement(Tags.IsNotNull,
                             this.fieldRefOperand.ToCaml()/*,
                             this.valueOperand.ToCaml()*/);
        }
    }
}


