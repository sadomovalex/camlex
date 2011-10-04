using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using ExpressionToCodeLib;

namespace CamlexNET.ExpressionToCodeAdapter
{
    public static class Adapter
    {
        public static string ToCode(Expression expr)
        {
            if (expr == null)
            {
                return string.Empty;
            }
            string code = ExpressionToCode.ToCode(expr);
            if (string.IsNullOrEmpty(code))
            {
                return string.Empty;
            }
            return code;
        }
    }
}
