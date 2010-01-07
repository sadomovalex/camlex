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
            if (expr is UnaryExpression)
            {
                expr = ((UnaryExpression)expr).Operand;
            }
            var fieldName = ((ConstantExpression)((MethodCallExpression)expr).Arguments[0]).Value as string;
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
            return this.createValueOperandFromNonConstantExpression(expr);
        }

        private IOperand createValueOperandFromNonConstantExpression(Expression expr)
        {
            // need to add Expression.Convert(..) in order to define Func<object>
            var lambda = Expression.Lambda<Func<object>>(Expression.Convert(expr, typeof(object)));
            object value = lambda.Compile().Invoke();
            // value can be null
            return this.createValueOperand(value != null ? value.GetType() : null, value);
        }

        private IOperand createValueOperandFromConstantExpression(ConstantExpression expr)
        {
            return this.createValueOperand(expr.Type, expr.Value);
        }

        private IOperand createValueOperand(Type type, object value)
        {
            // it is important to have check on NullValueOperand on 1st place
            if (value == null)
            {
                return new NullValueOperand();
            }
            if (type == typeof(string) || type == typeof(DataTypes.Text))
            {
                return new TextValueOperand((string)value);
            }
            if (type == typeof(int) || type == typeof(DataTypes.Integer))
            {
                return new IntegerValueOperand((int)value);
            }
            throw new NonSupportedOperandTypeException(type);
        }
    }
}
