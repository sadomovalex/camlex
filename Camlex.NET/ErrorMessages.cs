using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Camlex.NET
{
    public static class ErrorMessages
    {
        public const string NON_SUPPORTED_EXPRESSION =  "C0001: Expression '{0}' can not be translated into CAML";
        public static string NON_SUPPORTED_EXPRESSION_TYPE = "C0002: Expression type '{0}' is not supported";
        public static string NON_SUPPORTED_OPERAND_TYPE = "C0003: Operand type '{0}' is not supported";
//        public const string NON_BINARY_EXPRESSION =     "C0002: Expression '{0}' is not binary expression";
    }
}
