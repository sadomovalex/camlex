using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Camlex.NET.Impl.Eq;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl
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