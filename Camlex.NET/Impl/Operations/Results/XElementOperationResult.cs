using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Operations.Results
{
    public class XElementOperationResult : IOperationResult
    {
        private XElement element;

        public XElementOperationResult(XElement element)
        {
            this.element = element;
        }

        public object Value
        {
            get { return this.element; }
        }

        public override string ToString()
        {
            return this.element.ToString();
        }
    }
}
