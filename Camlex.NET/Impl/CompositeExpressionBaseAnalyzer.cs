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
    public abstract class CompositeExpressionBaseAnalyzer : IAnalyzer
    {
        protected IAnalyzerFactory analyzerFactory;

        protected CompositeExpressionBaseAnalyzer(IAnalyzerFactory analyzerFactory)
        {
            this.analyzerFactory = analyzerFactory;
        }

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

        public abstract IOperation GetOperation(Expression<Func<SPItem, bool>> expr);

        private bool isExpressionValid(BinaryExpression expr, ParameterExpression lambdaParam)
        {
            var expressionAnalyzer = this.analyzerFactory.Create(expr.NodeType);

            // make Expression<Func<SPItem, bool>> expression from BinaryExpression
            Expression<Func<SPItem, bool>> subExpression = Expression.Lambda<Func<SPItem, bool>>(expr, lambdaParam);
            return expressionAnalyzer.IsValid(subExpression);
        }
    }
}
