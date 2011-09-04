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
        private readonly ReOperandBuilder operandBuilder;
        private readonly ReOperationResultBuilder operationResultBuilder;

        public ReAnalyzerFromCamlFactory(ReOperandBuilder operandBuilder,
            ReOperationResultBuilder operationResultBuilder)
        {
            this.operandBuilder = operandBuilder;
            this.operationResultBuilder = operationResultBuilder;
        }

        public IReAnalyzer Create(XElement el)
        {
            throw new NotImplementedException();
        }
    }
}
