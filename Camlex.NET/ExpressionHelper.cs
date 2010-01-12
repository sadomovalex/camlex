using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Camlex.NET
{
    public static class ExpressionHelper
    {
        public static bool IncludeTimeValue(Expression expression)
        {
            return (expression is MethodCallExpression) && (((MethodCallExpression)expression).Method.Name == ReflectionHelper.IncludeTimeValue);
        }

        public static Expression RemoveIncludeTimeValueMethodCallIfAny(Expression expression)
        {
            if (!IncludeTimeValue(expression)) return expression;
            var methodCall = (MethodCallExpression) expression;

            if (methodCall.Object != null) return methodCall.Object;
            if (methodCall.Arguments.Count == 1) return methodCall.Arguments[0];

            throw new NonSupportedExpressionException(expression); // it should not happen - either Object or Arguments  is not NULL
        }
    }
}
