using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Camlex.NET.Impl.Operands;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Eq
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
