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
        public IOperationResult CreateResult(XElement value)
        {
            return new XElementOperationResult(value);
        }

        public IOperationResult CreateResult(XElement[] values)
        {
            return new XElementArrayOperationResult(values);
        }
    }
}
