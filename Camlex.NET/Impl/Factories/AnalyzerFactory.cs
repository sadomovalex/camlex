using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Camlex.NET.Impl.AndAlso;
using Camlex.NET.Impl.Array;
using Camlex.NET.Impl.Eq;
using Camlex.NET.Impl.Geq;
using Camlex.NET.Impl.OrElse;
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
            if (exprType == ExpressionType.OrElse)
            {
                return new OrElseAnalyzer(this);
            }
            if (exprType == ExpressionType.NewArrayInit)
            {
                return new ArrayAnalyzer(this.operandBuilder);
            }
            if (exprType == ExpressionType.GreaterThanOrEqual)
            {
                return new GeqAnalyzer(this.operandBuilder);
            }
            throw new NonSupportedExpressionTypeException(exprType);
        }
    }
}
