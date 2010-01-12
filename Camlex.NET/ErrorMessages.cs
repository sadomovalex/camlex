using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CamlexNET
{
    public static class ErrorMessages
    {
        public const string NON_SUPPORTED_EXPRESSION =  "Expression '{0}' can not be translated into CAML";
        public const string NON_SUPPORTED_EXPRESSION_TYPE = "Expression type '{0}' is not supported";
        public const string NON_SUPPORTED_OPERAND_TYPE = "Operand type '{0}' is not supported";
        public const string NULL_VALUE_OPERAND_CAN_NOT_BE_TRANSLATED_TO_CAML =
            "Value is null. Null rvalue is allowed only with '==' (IsNull) and '!=' (IsNotNull) operations. " +
            "Also null rvalue should not be casted to DataTypes.*";
        public const string INVALID_VALUE_FOR_OPERAND_TYPE = "Value '{0}' is not valid for operand type '{1}'";
        public const string INVALID_FIELD_NAME_FOR_FIELD_REF_OPERAND = "Value '{0}' is not valid field name for FieldRef operand";
    }
}
