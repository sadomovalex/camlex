using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public IReAnalyzer Create(string input)
        {
            throw new NotImplementedException();
        }
    }
}
