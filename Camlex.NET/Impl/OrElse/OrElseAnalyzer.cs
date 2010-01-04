using System;
using System.Linq.Expressions;
using Camlex.NET.Impl.AndAlso;
using Camlex.NET.Interfaces;
using Microsoft.SharePoint;

namespace Camlex.NET.Impl.OrElse
{
    public class OrElseAnalyzer : CompositeExpressionBaseAnalyzer
    {
        public OrElseAnalyzer(IAnalyzerFactory analyzerFactory) :
            base(analyzerFactory)
        {
        }

        public override bool IsValid(Expression<Func<SPItem, bool>> expr)
        {
            if (!base.IsValid(expr))
            {
                return false;
            }
            return (expr.Body.NodeType == ExpressionType.OrElse);
        }

        public override IOperation GetOperation(Expression<Func<SPItem, bool>> expr)
        {
            if (!this.IsValid(expr))
            {
                throw new NonSupportedExpressionException(expr);
            }
            var leftOperation = this.getLeftOperation(expr);
            var rightOperation = this.getRightOperation(expr);
            var operation = new OrElseOperation(leftOperation, rightOperation);
            return operation;
        }
    }
}


