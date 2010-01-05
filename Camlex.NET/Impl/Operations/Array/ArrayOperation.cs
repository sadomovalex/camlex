using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Array
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
