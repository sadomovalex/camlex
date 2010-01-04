using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Camlex.NET.Interfaces;
using Microsoft.SharePoint;

namespace Camlex.NET.Impl
{
    // Base class for AndAlso and OrElse analyzers
    public abstract class CompositeExpressionBaseAnalyzer : ILogicalAnalyzer
    {
        protected IAnalyzerFactory analyzerFactory;

        protected CompositeExpressionBaseAnalyzer(IAnalyzerFactory analyzerFactory)
        {
            this.analyzerFactory = analyzerFactory;
        }

        public abstract IOperation GetOperation(Expression<Func<SPItem, bool>> expr);

        public virtual bool IsValid(Expression<Func<SPItem, bool>> expr)
        {
            // expression should be binary expresion
            if (!(expr.Body is BinaryExpression))
            {
                return false;
            }
            var body = expr.Body as BinaryExpression;

            // left operand should be binary expression
            if (!(body.Left is BinaryExpression))
            {
                return false;
            }

            // right operand should be binary expression
            if (!(body.Right is BinaryExpression))
            {
                return false;
            }

            var lambdaParam = expr.Parameters[0];
            // check left operand
            if (!this.isExpressionValid(body.Left as BinaryExpression, lambdaParam))
            {
                return false;
            }

            // check right operand
            if (!this.isExpressionValid(body.Right as BinaryExpression, lambdaParam))
            {
                return false;
            }
            return true;
        }

        private bool isExpressionValid(BinaryExpression subExpr, ParameterExpression lambdaParam)
        {
            var subExpressionAnalyzer = this.analyzerFactory.CreateLogicalAnalyzer(subExpr.NodeType);

            // make Expression<Func<SPItem, bool>> lambda expression from BinaryExpression
            Expression<Func<SPItem, bool>> lambda = this.createLambdaFromExpression(subExpr, lambdaParam);
            return subExpressionAnalyzer.IsValid(lambda);
        }

        // For composite expressions like x => (string)x["Email"] == "test@example.com" && (int)x["Count1"] == 1
        // it creates 2 lambdas: x => (string)x["Email"] == "test@example.com" ; x => (int)x["Count1"] == 1
        private Expression<Func<SPItem, bool>> createLambdaFromExpression(BinaryExpression subExpr,
            ParameterExpression lambdaParam)
        {
            return Expression.Lambda<Func<SPItem, bool>>(subExpr, lambdaParam);
        }

        private IOperation createOperationFromExpression(BinaryExpression subExpr, ParameterExpression lambdaParam)
        {
            var subExpressionAnalyzer = this.analyzerFactory.CreateLogicalAnalyzer(subExpr.NodeType);

            // make Expression<Func<SPItem, bool>> lambda expression from BinaryExpression
            Expression<Func<SPItem, bool>> lambda = this.createLambdaFromExpression(subExpr, lambdaParam);
            return subExpressionAnalyzer.GetOperation(lambda);
        }

        protected IOperation getLeftOperation(Expression<Func<SPItem, bool>> expr)
        {
            if (!this.IsValid(expr))
            {
                throw new NonSupportedExpressionException(expr);
            }
            var body = expr.Body as BinaryExpression;
            var lambdaParam = expr.Parameters[0];
            var operation = this.createOperationFromExpression(body.Left as BinaryExpression, lambdaParam);
            return operation;
        }

        protected IOperation getRightOperation(Expression<Func<SPItem, bool>> expr)
        {
            if (!this.IsValid(expr))
            {
                throw new NonSupportedExpressionException(expr);
            }
            var body = expr.Body as BinaryExpression;
            var lambdaParam = expr.Parameters[0];
            var operation = this.createOperationFromExpression(body.Right as BinaryExpression, lambdaParam);
            return operation;
        }
    }
}
