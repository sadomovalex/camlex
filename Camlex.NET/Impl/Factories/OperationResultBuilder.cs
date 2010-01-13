using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using CamlexNET.Impl.Operations.Results;
using CamlexNET.Interfaces;

namespace CamlexNET.Impl.Factories
{
    internal class OperationResultBuilder : IOperationResultBuilder
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
