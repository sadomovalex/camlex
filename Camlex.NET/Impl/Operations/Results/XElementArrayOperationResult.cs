using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using CamlexNET.Interfaces;

namespace CamlexNET.Impl.Operations.Results
{
    public class XElementArrayOperationResult : IOperationResult
    {
        private XElement[] elements;

        public XElementArrayOperationResult(XElement[] elements)
        {
            this.elements = elements;
        }

        public object Value
        {
            get { return this.elements; }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            System.Array.ForEach(this.elements, e => sb.Append(e.ToString()));
            return sb.ToString();
        }
    }
}
