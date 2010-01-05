using System.Linq.Expressions;
using Camlex.NET.Impl.Operands;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Operations.IsNotNull
{
    public class IsNotNullAnalyzer : BinaryExpressionBaseAnalyzer
    {
        public IsNotNullAnalyzer(IOperandBuilder operandBuilder)
            : base(operandBuilder)
        {
        }

        public override bool IsValid(LambdaExpression expr)
        {
            // do not call base.IsValid() here as there is no convert for IsNotNull operation
            // (i.e. x["foo"] != null, instead of (T)x["foo"] != null)            
            if (!this.isValidWithoutConvert(expr))
            {
                return false;
            }

            return (expr.Body.NodeType == ExpressionType.NotEqual);
        }

        private bool isValidWithoutConvert(LambdaExpression expr)
        {
            // body should be BinaryExpression
            if (!(expr.Body is BinaryExpression))
            {
                return false;
            }
            var body = expr.Body as BinaryExpression;

            if (!(body.Left is MethodCallExpression))
            {
                return false;
            }
            var leftOperand = body.Left as MethodCallExpression;
            if (leftOperand.Method.Name != ReflectionHelper.IndexerMethodName)
            {
                return false;
            }

            if (leftOperand.Arguments.Count != 1)
            {
                return false;
            }
            // currently only constants are supported as indexer's argument
            if (!(leftOperand.Arguments[0] is ConstantExpression))
            {
                return false;
            }

            // right expression should be constant, variable or method call
            var rightExpression = body.Right;
            if (!this.isValidRightExpression(rightExpression))
            {
                return false;
            }

            // check that right operand is null
            var valueOperand = this.operandBuilder.CreateValueOperand(rightExpression);
            return (valueOperand is NullValueOperand);
        }

        public override IOperation GetOperation(LambdaExpression expr)
        {
            if (!this.IsValid(expr))
            {
                throw new NonSupportedExpressionException(expr);
            }
            var fieldRefOperand = this.getFieldRefOperand(expr);
            // var valueOperand = this.getValueOperand(expr);
            return new IsNotNullOperation(fieldRefOperand/*, valueOperand*/);
        }
    }
}


