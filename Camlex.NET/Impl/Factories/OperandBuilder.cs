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

        public IOperand CreateValueOperandForNativeSyntax(Expression expr)
        {
            // determine operand type from expression result (specify null as explicitOperandType)
            return createValueOperandForNativeSyntax(expr, null);
        }

        private IOperand createValueOperandForNativeSyntax(Expression expr, Type explicitOperandType)
        {
            if (expr is ConstantExpression)
            {
                return this.createValueOperandFromConstantExpression(expr as ConstantExpression, explicitOperandType);
            }
            return this.createValueOperandFromNonConstantExpression(expr, explicitOperandType);
        }

        public IOperand CreateValueOperandForStringBasedSyntax(Expression expr)
        {
            // retrieve internal native expression from string based syntax
            var internalExpression = ((UnaryExpression)((UnaryExpression)expr).Operand).Operand;
            // use conversion type as operand type (subclass of BaseFieldType should be used here)
            // because conversion operand has always string type for string based syntax
            return this.createValueOperandForNativeSyntax(internalExpression, expr.Type);
        }

        private IOperand createValueOperandFromNonConstantExpression(Expression expr, Type explicitOperandType)
        {
            // need to add Expression.Convert(..) in order to define Func<object>
            var lambda = Expression.Lambda<Func<object>>(Expression.Convert(expr, typeof(object)));
            object value = lambda.Compile().Invoke();
            
            // if operand type is not specified explivitly try to determine operand type from expression result
            var operandType = explicitOperandType;
            if (operandType == null)
            {
                // value can be null
                operandType = value != null ? value.GetType() : null;
            }
            return this.createValueOperand(operandType, value);
        }

        private IOperand createValueOperandFromConstantExpression(ConstantExpression expr, Type explicitOperandType)
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
                if (value.GetType() == typeof(int))
                {
                    return new IntegerValueOperand((int) value);
                }
                if (value.GetType() == typeof(string))
                {
                    return new IntegerValueOperand((string)value);
                }
            }
            throw new NonSupportedOperandTypeException(type);
        }
    }
}
