using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Operations.Array
{
    public class ArrayAnalyzer : BaseAnalyzer
    {
        private IOperandBuilder operandBuilder;

        public ArrayAnalyzer(IOperationResultBuilder operationResultBuilder,
            IOperandBuilder operandBuilder) :
            base(operationResultBuilder)
        {
            this.operandBuilder = operandBuilder;
        }

        public override bool IsValid(LambdaExpression expr)
        {
            var body = expr.Body as NewArrayExpression;
            if (body == null) return false;
            var counter = 0;
            body.Expressions.ToList().ForEach(ex =>
            {
                if(ex.NodeType == ExpressionType.TypeAs)
                {
                    var unary = ex as UnaryExpression;
                    if (unary == null ||
                        (unary.Type != typeof(Camlex.Asc) && unary.Type != typeof(Camlex.Desc))) return;
                    ex = unary.Operand;
                }
                var methodCall = ex as MethodCallExpression;
                if (methodCall == null) return;
                if (methodCall.Method.Name != ReflectionHelper.IndexerMethodName) return;
                if (methodCall.Arguments.Count != 1) return;
                if (!(methodCall.Arguments[0] is ConstantExpression)) return;
                counter++;
            });
            return (body.Expressions.Count == counter);
        }

        public override IOperation GetOperation(LambdaExpression expr)
        {
            if (!IsValid(expr))
            {
                throw new NonSupportedExpressionException(expr);
            }
            var operands = getFieldRefOperandsWithOrdering(expr);
            return new ArrayOperation(this.operationResultBuilder, operands);
        }

        private IOperand[] getFieldRefOperandsWithOrdering(LambdaExpression expr)
        {
            var operands = new List<IOperand>();
            ((NewArrayExpression)expr.Body).Expressions.ToList().ForEach(ex =>
            {
                var orderDirection = Camlex.OrderDirection.Default;
                if (ex.NodeType == ExpressionType.TypeAs)
                {
                    orderDirection = Camlex.OrderDirection.Convert(ex.Type);
                    ex = ((UnaryExpression)ex).Operand;
                }
                operands.Add(this.operandBuilder.CreateFieldRefOperandWithOrdering(ex, orderDirection));
            });
            return operands.ToArray();
        }
    }
}


