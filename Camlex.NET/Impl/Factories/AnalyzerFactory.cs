using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Camlex.NET.Impl.Operations.AndAlso;
using Camlex.NET.Impl.Operations.Array;
using Camlex.NET.Impl.Operations.Eq;
using Camlex.NET.Impl.Operations.Geq;
using Camlex.NET.Impl.Operations.Gt;
using Camlex.NET.Impl.Operations.IsNotNull;
using Camlex.NET.Impl.Operations.IsNull;
using Camlex.NET.Impl.Operations.Leq;
using Camlex.NET.Impl.Operations.Lt;
using Camlex.NET.Impl.Operations.Neq;
using Camlex.NET.Impl.Operations.OrElse;
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

        public IAnalyzer Create(LambdaExpression expr)
        {
            ExpressionType exprType = expr.Body.NodeType;

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
            if (exprType == ExpressionType.GreaterThan)
            {
                return new GtAnalyzer(this.operandBuilder);
            }
            if (exprType == ExpressionType.LessThanOrEqual)
            {
                return new LeqAnalyzer(this.operandBuilder);
            }
            if (exprType == ExpressionType.LessThan)
            {
                return new LtAnalyzer(this.operandBuilder);
            }

            // it is not enough to check ExpressionType for IsNull operation.
            // We need also to check that right operand is null
            IsNullAnalyzer isNullAnalyzer;
            if (this.isNullExpression(expr, out isNullAnalyzer))
            {
                return isNullAnalyzer;
            }
            // note that it is important to have check on IsNull before check on ExpressionType.Equal.
            // Because x["foo"] == null is also ExpressionType.Equal, but it should be translated
            // into <IsNull> instead of <Eq>
            if (exprType == ExpressionType.Equal)
            {
                return new EqAnalyzer(this.operandBuilder);
            }

            // it is not enough to check ExpressionType for IsNotNull operation.
            // We need also to check that right operand is null
            IsNotNullAnalyzer isNotNullAnalyzer;
            if (this.isNotNullExpression(expr, out isNotNullAnalyzer))
            {
                return isNotNullAnalyzer;
            }
            // note that it is important to have check on IsNotNull before check on ExpressionType.NotEqual.
            // Because x["foo"] != null is also ExpressionType.NotEqual, but it should be translated
            // into <IsNotNull> instead of <Neq>
            if (exprType == ExpressionType.NotEqual)
            {
                return new NeqAnalyzer(this.operandBuilder);
            }

            throw new NonSupportedExpressionTypeException(exprType);
        }

        private bool isNullExpression(LambdaExpression expr, out IsNullAnalyzer analyzer)
        {
            // the simplest way to check if this IsNotNull expression - is to reuse IsNotNullAnalyzer
            analyzer = new IsNullAnalyzer(this.operandBuilder);
            return analyzer.IsValid(expr);
        }

        private bool isNotNullExpression(LambdaExpression expr, out IsNotNullAnalyzer analyzer)
        {
            // the simplest way to check if this IsNotNull expression - is to reuse IsNotNullAnalyzer
            analyzer = new IsNotNullAnalyzer(this.operandBuilder);
            return analyzer.IsValid(expr);
        }
    }
}
