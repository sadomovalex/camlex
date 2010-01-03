using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Camlex.NET.Impl.AndAlso;
using Camlex.NET.Impl.Eq;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Factories
{
    public class AnalyzerFactory : IAnalyzerFactory
    {
        private readonly IOperandBuilder operandBuilder;

        public AnalyzerFactory(IOperandBuilder operandBuilder)
        {
            this.operandBuilder = operandBuilder;
        }

        public IAnalyzer Create(ExpressionType exprType)
        {
            if (exprType == ExpressionType.Equal)
            {
                return new EqAnalyzer(this.operandBuilder);
            }
            if (exprType == ExpressionType.AndAlso)
            {
                return new AndAlsoAnalyzer(this);
            }
            throw new NonSupportedExpressionTypeException(exprType);
        }
    }
}
