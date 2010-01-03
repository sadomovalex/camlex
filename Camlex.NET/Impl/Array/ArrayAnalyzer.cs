using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Camlex.NET.Interfaces;
using Microsoft.SharePoint;

namespace Camlex.NET.Impl.Array
{
    public class ArrayAnalyzer : IAnalyzer
    {
        private IOperandBuilder _operandBuilder;

        public ArrayAnalyzer(IOperandBuilder operandBuilder)
        {
            _operandBuilder = operandBuilder;
        }

        public bool IsValid(Expression<Func<SPItem, bool>> expr)
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

        public IOperation GetOperation(Expression<Func<SPItem, bool>> expr)
        {
            if (!IsValid(expr))
            {
                throw new NonSupportedExpressionException(expr);
            }
            var operands = GetFieldRefOperandsWithOrdering(expr);
            return new ArrayOperation(operands);
        }

        private IOperand[] GetFieldRefOperandsWithOrdering(Expression<Func<SPItem, bool>> expr)
        {
            var operands = new List<IOperand>();
            ((NewArrayExpression)expr.Body).Expressions.ToList().ForEach(ex =>
            {
                var orderDirection = Camlex.OrderDirection.Default;
                if (ex.NodeType == ExpressionType.TypeAs)
                {
                    ex = ((UnaryExpression)ex).Operand;
                    orderDirection = Camlex.OrderDirection.Convert(ex.Type);
                }
                operands.Add(_operandBuilder.CreateFieldRefOperandWithOrdering(ex, orderDirection));
            });
            return operands.ToArray();
        }
    }
}
