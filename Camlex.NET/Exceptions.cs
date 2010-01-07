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
        public NonSupportedExpressionException(Expression expr) :
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

    public class NullValueOperandCannotBeTranslatedToCamlException : GenericException
    {
        public NullValueOperandCannotBeTranslatedToCamlException() :
            base(ErrorMessages.NULL_VALUE_OPERAND_CAN_NOT_BE_TRANSLATED_TO_CAML)
        {
        }
    }

    public class InvalidValueForOperandTypeException : GenericException
    {
        public InvalidValueForOperandTypeException(object value, Type operandType) :
            base(ErrorMessages.INVALID_VALUE_FOR_OPERAND_TYPE, value, operandType)
        {
        }
    }
}
