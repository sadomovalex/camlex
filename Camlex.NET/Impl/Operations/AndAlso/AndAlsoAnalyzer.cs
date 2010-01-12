using System.Linq.Expressions;
using CamlexNET.Interfaces;

namespace CamlexNET.Impl.Operations.AndAlso
{
    public class AndAlsoAnalyzer : CompositeExpressionBaseAnalyzer
    {
        public AndAlsoAnalyzer(IOperationResultBuilder operationResultBuilder,
            IAnalyzerFactory analyzerFactory) :
            base(operationResultBuilder, analyzerFactory)
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
            var operation = new AndAlsoOperation(this.operationResultBuilder, leftOperation, rightOperation);
            return operation;
        }
    }
}


