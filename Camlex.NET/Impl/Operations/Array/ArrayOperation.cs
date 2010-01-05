using System.Collections.Generic;
using System.Xml.Linq;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Operations.Array
{
    public class ArrayOperation : IOperation
    {
        private readonly IOperand[] fieldRefOperands;

        public ArrayOperation(params IOperand[] fieldRefOperands)
        {
            this.fieldRefOperands = fieldRefOperands;
        }

        public XElement ToCaml()
        {
            var content = new List<XElement>();
            System.Array.ForEach(this.fieldRefOperands, x => content.Add(x.ToCaml()));
            return new XElement(Tags.Eq, content);
        }
    }
}


