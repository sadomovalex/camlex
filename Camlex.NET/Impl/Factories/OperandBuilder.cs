using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Camlex.NET.Impl.Operands;

namespace Camlex.NET.Interfaces
{
    public class OperandBuilder : IOperandBuilder
    {
        public IOperand CreateFieldRefOperand(Expression expr)
        {
            var unaryExpression = (UnaryExpression)expr;
            var fieldName = ((ConstantExpression)((MethodCallExpression)unaryExpression.Operand).Arguments[0]).Value as string;
            return new FieldRefOperand(fieldName);
        }

        public IOperand CreateFieldRefOperandWithOrdering(Expression expr, Camlex.OrderDirection orderDirection)
        {
            var fieldRefOperand = (FieldRefOperand)CreateFieldRefOperand(expr);
            return new FieldRefOperandWithOrdering(fieldRefOperand, orderDirection);
        }

        public IOperand CreateValueOperand(Expression expr)
        {
            if (expr is ConstantExpression)
            {
                return this.createValueOperandFromConstantExpression(expr as ConstantExpression);
            }
            if (expr is MemberExpression && ((MemberExpression)expr).Expression is ConstantExpression)
            {
                return this.createValueOperandFromMemberExpression(expr as MemberExpression);
            }
            throw new NonSupportedExpressionTypeException(expr.NodeType);
        }

        // Uses reflection to obtain actual value of member expression
        private IOperand createValueOperandFromMemberExpression(MemberExpression expr)
        {
            var fieldInfo = (FieldInfo)expr.Member;
            var constantExpression = (ConstantExpression)expr.Expression;
            object innerObj = constantExpression.Value;
            object value = fieldInfo.GetValue(innerObj);
            return this.createValueOperand(value.GetType(), value);
        }

        private IOperand createValueOperandFromConstantExpression(ConstantExpression expr)
        {
            return this.createValueOperand(expr.Type, expr.Value);
        }

        private IOperand createValueOperand(Type type, object value)
        {
            if (type == typeof(string))
            {
                return new TextValueOperand((string)value);
            }
            if (type == typeof(int))
            {
                return new IntegerValueOperand((int)value);
            }
            throw new NonSupportedOperandTypeException(type);
        }
    }
}
