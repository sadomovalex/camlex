using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CamlexNET.Impl.Operations.AndAlso;
using CamlexNET.Impl.Operations.Array;
using CamlexNET.Impl.Operations.BeginsWith;
using CamlexNET.Impl.Operations.Contains;
using CamlexNET.Impl.Operations.Eq;
using CamlexNET.Impl.Operations.Geq;
using CamlexNET.Impl.Operations.Gt;
using CamlexNET.Impl.Operations.IsNotNull;
using CamlexNET.Impl.Operations.IsNull;
using CamlexNET.Impl.Operations.Leq;
using CamlexNET.Impl.Operations.Lt;
using CamlexNET.Impl.Operations.Neq;
using CamlexNET.Impl.Operations.OrElse;
using CamlexNET.Interfaces;

namespace CamlexNET.Impl.Factories
{
    public class AnalyzerFactory : IAnalyzerFactory
    {
        private readonly IOperandBuilder operandBuilder;
        private readonly IOperationResultBuilder operationResultBuilder;

        public AnalyzerFactory(IOperandBuilder operandBuilder, IOperationResultBuilder operationResultBuilder)
        {
            this.operandBuilder = operandBuilder;
            this.operationResultBuilder = operationResultBuilder;
        }

        public IAnalyzer Create(LambdaExpression expr)
        {
            ExpressionType exprType = expr.Body.NodeType;

            if (exprType == ExpressionType.AndAlso)
            {
                return new AndAlsoAnalyzer(this.operationResultBuilder, this);
            }
            if (exprType == ExpressionType.OrElse)
            {
                return new OrElseAnalyzer(this.operationResultBuilder, this);
            }
            if (exprType == ExpressionType.NewArrayInit)
            {
                return new ArrayAnalyzer(this.operationResultBuilder, this.operandBuilder);
            }
            if (exprType == ExpressionType.GreaterThanOrEqual)
            {
                return new GeqAnalyzer(this.operationResultBuilder, this.operandBuilder);
            }
            if (exprType == ExpressionType.GreaterThan)
            {
                return new GtAnalyzer(this.operationResultBuilder, this.operandBuilder);
            }
            if (exprType == ExpressionType.LessThanOrEqual)
            {
                return new LeqAnalyzer(this.operationResultBuilder, this.operandBuilder);
            }
            if (exprType == ExpressionType.LessThan)
            {
                return new LtAnalyzer(this.operationResultBuilder, this.operandBuilder);
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
                return new EqAnalyzer(this.operationResultBuilder, this.operandBuilder);
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
                return new NeqAnalyzer(this.operationResultBuilder, this.operandBuilder);
            }

            var beginsWithAnalyzer = new BeginsWithAnalyzer(operationResultBuilder, operandBuilder);
            if (beginsWithAnalyzer.IsValid(expr)) return beginsWithAnalyzer;

            var containsAnalyzer = new ContainsAnalyzer(operationResultBuilder, operandBuilder);
            if (containsAnalyzer.IsValid(expr)) return containsAnalyzer;

            throw new NonSupportedExpressionTypeException(exprType);
        }

        private bool isNullExpression(LambdaExpression expr, out IsNullAnalyzer analyzer)
        {
            // the simplest way to check if this IsNotNull expression - is to reuse IsNotNullAnalyzer
            analyzer = new IsNullAnalyzer(this.operationResultBuilder, this.operandBuilder);
            return analyzer.IsValid(expr);
        }

        private bool isNotNullExpression(LambdaExpression expr, out IsNotNullAnalyzer analyzer)
        {
            // the simplest way to check if this IsNotNull expression - is to reuse IsNotNullAnalyzer
            analyzer = new IsNotNullAnalyzer(this.operationResultBuilder, this.operandBuilder);
            return analyzer.IsValid(expr);
        }
    }
}
