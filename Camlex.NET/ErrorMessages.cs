using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Camlex.NET
{
    public static class ErrorMessages
    {
        public const string NON_SUPPORTED_EXPRESSION =  "Expression '{0}' can not be translated into CAML";
        public static string NON_SUPPORTED_EXPRESSION_TYPE = "Expression type '{0}' is not supported";
        public static string NON_SUPPORTED_OPERAND_TYPE = "Operand type '{0}' is not supported";
    }
}
