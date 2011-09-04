using System;
using CamlexNET.Interfaces.ReverseEngeneering;

namespace CamlexNET.Impl.ReverseEngeneering.Caml.Factories
{
    internal class ReTranslatorFromCamlFactory : IReTranslatorFactory
    {
        private readonly IReAnalyzerFactory analyzerFactory;

        public ReTranslatorFromCamlFactory(IReAnalyzerFactory analyzerFactory)
        {
            this.analyzerFactory = analyzerFactory;
        }

        public IReTranslator Create(string input)
        {
            var analyzer = this.analyzerFactory.Create(input);
            return new ReTranslatorFromCaml(analyzer);
        }
    }
}
