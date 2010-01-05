using System.Linq.Expressions;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Operations.Gt
{
    public class GtAnalyzer : BinaryExpressionBaseAnalyzer
    {
        public GtAnalyzer(IOperationResultBuilder operationResultBuilder, IOperandBuilder operandBuilder)
            : base(operationResultBuilder, operandBuilder)
        {
        }

        public override bool IsValid(LambdaExpression expr)
        {
            if (!base.IsValid(expr))
            {
                return false;
            }
            return (expr.Body.NodeType == ExpressionType.GreaterThan);
        }

        public override IOperation GetOperation(LambdaExpression expr)
        {
            if (!this.IsValid(expr))
            {
                throw new NonSupportedExpressionException(expr);
            }
            var fieldRefOperand = this.getFieldRefOperand(expr);
            var valueOperand = this.getValueOperand(expr);
            return new GtOperation(this.operationResultBuilder, fieldRefOperand, valueOperand);
        }
    }
}


