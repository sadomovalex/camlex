using System.Linq.Expressions;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Factories
{
    public class TranslatorFactory : ITranslatorFactory
    {
        private readonly IAnalyzerFactory analyzerFactory;

        public TranslatorFactory(IAnalyzerFactory analyzerFactory)
        {
            this.analyzerFactory = analyzerFactory;
        }

        public ITranslator Create(ExpressionType exprType)
        {
            var analyzer = this.analyzerFactory.Create(exprType);
            return new GenericTranslator(analyzer);
        }
    }
}