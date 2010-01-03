using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Camlex.NET
{
    public static class ReflectionHelper
    {
        public const string IndexerMethodName = "get_Item";

//        public static IEnumerable<ParameterExpression> GetExpressionParameters(ParameterInfo[] parameterInfos)
//        {
//            var result = new List<ParameterExpression>();
//            Array.ForEach(parameterInfos, pi => result.Add(Expression.Parameter(pi.ParameterType, pi.Name)));
//            return result;
//        }
    }
}
