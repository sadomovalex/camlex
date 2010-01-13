using System.Linq.Expressions;
using CamlexNET.Interfaces;

namespace CamlexNET.Impl.Factories
{
    internal class TranslatorFactory : ITranslatorFactory
    {
        private readonly IAnalyzerFactory analyzerFactory;

        public TranslatorFactory(IAnalyzerFactory analyzerFactory)
        {
            this.analyzerFactory = analyzerFactory;
        }

        public ITranslator Create(LambdaExpression expr)
        {
            var analyzer = this.analyzerFactory.Create(expr);
            return new GenericTranslator(analyzer);
        }
    }
}