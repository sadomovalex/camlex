using System.Linq.Expressions;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Operations.IsNull
{
    public class IsNullAnalyzer : NullabilityBaseAnalyzer
    {
        public IsNullAnalyzer(IOperandBuilder operandBuilder)
            : base(operandBuilder)
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
            // var valueOperand = this.getValueOperand(expr);
            return new IsNullOperation(fieldRefOperand/*, valueOperand*/);
        }
    }
}


