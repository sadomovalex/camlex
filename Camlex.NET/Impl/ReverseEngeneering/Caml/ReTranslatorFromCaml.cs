using System;
using System.Linq.Expressions;
using CamlexNET.Interfaces.ReverseEngeneering;

namespace CamlexNET.Impl.ReverseEngeneering.Caml
{
    internal class ReTranslatorFromCaml : IReTranslator
    {
        private readonly IReAnalyzer analyzer;

        public ReTranslatorFromCaml(IReAnalyzer analyzer)
        {
            this.analyzer = analyzer;
        }

        public Expression Translate(string input)
        {
            throw new NotImplementedException();
        }
    }
}
