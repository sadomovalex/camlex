using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Operations.Results
{
    public class OperationResult : IOperationMultipleResult
    {
        private XElement[] values;

        public OperationResult(XElement value)
        {
            this.values = new XElement[1];
            this.values[0] = value;
        }

        public OperationResult(XElement[] values)
        {
            this.values = values;
        }

        public XElement Value
        {
            get { return values[0]; }
        }

        public XElement[] Values
        {
            get { return values; }
        }
    }
}
