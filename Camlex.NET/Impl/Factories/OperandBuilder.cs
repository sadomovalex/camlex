using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public IOperand CreateValueOperand(Expression expr)
        {
            throw new NotImplementedException();
        }
    }
}
