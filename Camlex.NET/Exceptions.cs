using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.SharePoint;

namespace CamlexNET
{
    internal class GenericException : Exception
    {
        public GenericException(string message, params object[] args) :
            base(string.Format(message, args))
        {}        
    }

    internal class NonSupportedExpressionException : GenericException
    {
        public NonSupportedExpressionException(Expression expr) :
            base(ErrorMessages.NON_SUPPORTED_EXPRESSION, expr)
        {
        }
    }

    internal class NonSupportedExpressionTypeException : GenericException
    {
        public NonSupportedExpressionTypeException(ExpressionType exprType) :
            base(ErrorMessages.NON_SUPPORTED_EXPRESSION_TYPE, exprType)
        {
        }
    }

    internal class NonSupportedOperandTypeException : GenericException
    {
        public NonSupportedOperandTypeException(Type type) :
            base(ErrorMessages.NON_SUPPORTED_OPERAND_TYPE, type)
        {
        }
    }

    internal class NullValueOperandCannotBeTranslatedToCamlException : GenericException
    {
        public NullValueOperandCannotBeTranslatedToCamlException() :
            base(ErrorMessages.NULL_VALUE_OPERAND_CAN_NOT_BE_TRANSLATED_TO_CAML)
        {
        }
    }

    internal class InvalidValueForOperandTypeException : GenericException
    {
        public InvalidValueForOperandTypeException(object value, Type operandType) :
            base(ErrorMessages.INVALID_VALUE_FOR_OPERAND_TYPE, value, operandType)
        {
        }
    }

    internal class InvalidFieldNameForFieldRefException : GenericException
    {
        public InvalidFieldNameForFieldRefException(object value) :
            base(ErrorMessages.INVALID_FIELD_NAME_FOR_FIELD_REF_OPERAND, value)
        {
        }
    }
}
