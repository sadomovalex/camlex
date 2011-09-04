using System;
using System.Linq.Expressions;
using CamlexNET.Interfaces.ReverseEngeneering;

namespace CamlexNET.Impl.ReverseEngeneering.Caml
{
    internal class ReTranslatorFromCaml : IReTranslator
    {
        private readonly IReAnalyzer analyzer;
        private readonly string tag;

        public ReTranslatorFromCaml(IReAnalyzer analyzer, string tag)
        {
            this.analyzer = analyzer;
            this.tag = tag;
        }

        public Expression Translate()
        {
            throw new NotImplementedException();
        }
    }
}
