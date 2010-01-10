using System.Linq.Expressions;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Operations.BeginsWith
{
    public class BeginsWithAnalyzer : UnaryExpressionBaseAnalyzer
    {
        public BeginsWithAnalyzer(IOperationResultBuilder operationResultBuilder, IOperandBuilder operandBuilder)
            : base(operationResultBuilder, operandBuilder)
        {
        }

        public override bool IsValid(LambdaExpression expr)
        {
            if (!base.IsValid(expr))
            {
                return false;
            }
            return ((MethodCallExpression)expr.Body).Method.Name == ReflectionHelper.StartsWithMethodName;
        }

        public override IOperation GetOperation(LambdaExpression expr)
        {
            if (!IsValid(expr))
            {
                throw new NonSupportedExpressionException(expr);
            }
            var fieldRefOperand = getFieldRefOperand(expr);
            var valueOperand = getValueOperand(expr);
            return new BeginsWithOperation(operationResultBuilder, fieldRefOperand, valueOperand);
        }
    }
}