using System.Linq.Expressions;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Operations.AndAlso
{
    public class AndAlsoAnalyzer : CompositeExpressionBaseAnalyzer
    {
        public AndAlsoAnalyzer(IAnalyzerFactory analyzerFactory) :
            base(analyzerFactory)
        {
        }

        public override bool IsValid(LambdaExpression expr)
        {
            if (!base.IsValid(expr))
            {
                return false;
            }
            return (expr.Body.NodeType == ExpressionType.AndAlso);
        }

        public override IOperation GetOperation(LambdaExpression expr)
        {
            if (!this.IsValid(expr))
            {
                throw new NonSupportedExpressionException(expr);
            }
            var leftOperation = this.getLeftOperation(expr);
            var rightOperation = this.getRightOperation(expr);
            var operation = new AndAlsoOperation(leftOperation, rightOperation);
            return operation;
        }
    }
}


