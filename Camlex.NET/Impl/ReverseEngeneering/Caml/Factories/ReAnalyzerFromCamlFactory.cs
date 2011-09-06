using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using CamlexNET.Interfaces.ReverseEngeneering;

namespace CamlexNET.Impl.ReverseEngeneering.Caml.Factories
{
    internal class ReAnalyzerFromCamlFactory : IReAnalyzerFactory
    {
        private readonly IReOperandBuilder operandBuilder;
        //private readonly ReOperationResultBuilder operationResultBuilder;

        public ReAnalyzerFromCamlFactory(IReOperandBuilder operandBuilder)
        {
            this.operandBuilder = operandBuilder;
        }

        public IReAnalyzer Create(XElement el)
        {
            throw new NotImplementedException();
        }
    }
}
