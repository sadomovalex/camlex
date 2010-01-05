using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Camlex.NET.Impl.Operations.Results;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Factories
{
    public class OperationResultBuilder : IOperationResultBuilder
    {
        private List<XElement> values = new List<XElement>();

        public IOperationResultBuilder Add(XElement value)
        {
            values.Add(value);
            return this;
        }

        public IOperationResult ToResult()
        {
            if (values.Count == 1)
            {
                return new XElementOperationResult(values[0]);
            }
            else
            {
                return new XElementArrayOperationResult(values.ToArray());
            }
        }
    }
}
