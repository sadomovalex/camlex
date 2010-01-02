using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Camlex.NET.Impl.Eq;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Factories
{
    public class AnalyzerFactory : IAnalyzerFactory
    {
        public IAnalyzer Create(ExpressionType exprType)
        {
            if (exprType == ExpressionType.Equal)
            {
                return new EqAnalyzer();
            }
            throw new NonSupportedExpressionTypeException(exprType);
        }
    }
}
