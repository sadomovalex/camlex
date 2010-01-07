using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Camlex.NET
{
    public static class ErrorMessages
    {
        public const string NON_SUPPORTED_EXPRESSION =  "Expression '{0}' can not be translated into CAML";
        public const string NON_SUPPORTED_EXPRESSION_TYPE = "Expression type '{0}' is not supported";
        public const string NON_SUPPORTED_OPERAND_TYPE = "Operand type '{0}' is not supported";
        public const string NULL_VALUE_OPERAND_CAN_NOT_BE_TRANSLATED_TO_CAML = "Value is null. Null is allowed only with '==' (IsNull) and '!=' (IsNotNull) operations";
        public const string INVALID_VALUE_FOR_OPERAND_TYPE = "Value '{0}' is not valid for operand type '{1}'";
    }
}
