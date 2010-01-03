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

        public ITranslator CreateLogicalTranslator(ExpressionType exprType)
        {
            var analyzer = this.analyzerFactory.CreateLogicalAnalyzer(exprType);
            return new GenericTranslator(analyzer);
        }

        public ITranslator CreateArrayTranslator(ExpressionType exprType)
        {
            var analyzer = this.analyzerFactory.CreateArrayAnalyzer(exprType);
            return new GenericTranslator(analyzer);
        }
    }
}