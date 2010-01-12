using System.Linq.Expressions;
using CamlexNET.Interfaces;

namespace CamlexNET.Impl.Operations.Lt
{
    public class LtAnalyzer : BinaryExpressionBaseAnalyzer
    {
        public LtAnalyzer(IOperationResultBuilder operationResultBuilder, IOperandBuilder operandBuilder)
            : base(operationResultBuilder, operandBuilder)
        {
        }

        public override bool IsValid(LambdaExpression expr)
        {
            if (!base.IsValid(expr))
            {
                return false;
            }
            return (expr.Body.NodeType == ExpressionType.LessThan);
        }

        public override IOperation GetOperation(LambdaExpression expr)
        {
            if (!this.IsValid(expr))
            {
                throw new NonSupportedExpressionException(expr);
            }
            var fieldRefOperand = this.getFieldRefOperand(expr);
            var valueOperand = this.getValueOperand(expr);
            return new LtOperation(this.operationResultBuilder, fieldRefOperand, valueOperand);
        }
    }
}


