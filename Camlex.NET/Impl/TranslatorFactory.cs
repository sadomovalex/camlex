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
        public ITranslator Create(ExpressionType exprType)
        {
            if (exprType == ExpressionType.Equal)
            {
                var analyzer = new EqAnalyzer();
                return new EqTranslator(analyzer);
            }
            throw new NotImplementedException();
        }
    }
}