using System.Linq.Expressions;
using CamlexNET.Interfaces;

namespace CamlexNET.Impl.Operations.IsNull
{
    public class IsNullAnalyzer : NullabilityBaseAnalyzer
    {
        public IsNullAnalyzer(IOperationResultBuilder operationResultBuilder, IOperandBuilder operandBuilder)
            : base(operationResultBuilder, operandBuilder)
        {
        }

        public override bool IsValid(LambdaExpression expr)
        {
            if (!base.IsValid(expr))
            {
                return false;
            }

            return (expr.Body.NodeType == ExpressionType.Equal);
        }

        public override IOperation GetOperation(LambdaExpression expr)
        {
            if (!this.IsValid(expr))
            {
                throw new NonSupportedExpressionException(expr);
            }
            var fieldRefOperand = this.getFieldRefOperand(expr);
            return new IsNullOperation(this.operationResultBuilder, fieldRefOperand);
        }
    }
}


