using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.SharePoint;

namespace Camlex.NET
{
    public class GenericException : Exception
    {
        public GenericException(string message, params object[] args) :
            base(string.Format(message, args))
        {}        
    }

    public class NonSupportedExpressionException : GenericException
    {
        public NonSupportedExpressionException(LambdaExpression expr) :
            base(ErrorMessages.NON_SUPPORTED_EXPRESSION, expr)
        {
        }
    }

    public class NonSupportedExpressionTypeException : GenericException
    {
        public NonSupportedExpressionTypeException(ExpressionType exprType) :
            base(ErrorMessages.NON_SUPPORTED_EXPRESSION_TYPE, exprType)
        {
        }
    }

    public class NonSupportedOperandTypeException : GenericException
    {
        public NonSupportedOperandTypeException(Type type) :
            base(ErrorMessages.NON_SUPPORTED_OPERAND_TYPE, type)
        {
        }
    }
}
