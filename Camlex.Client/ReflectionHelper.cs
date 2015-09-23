#region Copyright(c) Alexey Sadomov, Vladimir Timashkov, Stef Heyenrath. All Rights Reserved.
// -----------------------------------------------------------------------------
// Copyright(c) 2010 Alexey Sadomov, Vladimir Timashkov, Stef Heyenrath. All Rights Reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//
//   1. No Trademark License - Microsoft Public License (Ms-PL) does not grant you rights to use
//      authors names, logos, or trademarks.
//   2. If you distribute any portion of the software, you must retain all copyright,
//      patent, trademark, and attribution notices that are present in the software.
//   3. If you distribute any portion of the software in source code form, you may do
//      so only under this license by including a complete copy of Microsoft Public License (Ms-PL)
//      with your distribution. If you distribute any portion of the software in compiled
//      or object code form, you may only do so under a license that complies with
//      Microsoft Public License (Ms-PL).
//   4. The names of the authors may not be used to endorse or promote products
//      derived from this software without specific prior written permission.
//
// The software is licensed "as-is." You bear the risk of using it. The authors
// give no express warranties, guarantees or conditions. You may have additional consumer
// rights under your local laws which this license cannot change. To the extent permitted
// under your local laws, the authors exclude the implied warranties of merchantability,
// fitness for a particular purpose and non-infringement.
// -----------------------------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CamlexNET
{
    internal static class ReflectionHelper
    {
        public const string QueryMethodName = "Query";
        public const string WhereMethodName = "Where";
        public const string OrderByMethodName = "OrderBy";
        public const string GroupByMethodName = "GroupBy";
		public const string RowLimitMethodName = "Take";
        public const string ViewFieldsMethodName = "ViewFields";
        public const string IndexerMethodName = "get_Item";
        public const string StartsWithMethodName = "StartsWith";
        public const string ContainsMethodName = "Contains";
        public const string IncludeTimeValue = "IncludeTimeValue";
        public const string DateRangesOverlap = "DateRangesOverlap";
        public const string CommonParameterName = "x";
        public const string Item = "Item";
        public const string ToStringMethodName = "ToString";
        public const string UserID = "UserID";

        public const string GreaterThanMethodName = "op_GreaterThan";
        public const string GreaterThanOrEqualMethodName = "op_GreaterThanOrEqual";
        public const string LessThanMethodName = "op_LessThan";
        public const string LessThanOrEqualMethodName = "op_LessThanOrEqual";

        public const string GreaterThanMethodSymbol = ">";
        public const string GreaterThanOrEqualMethodSymbol = ">=";
        public const string LessThanMethodSymbol = "<";
        public const string LessThanOrEqualMethodSymbol = "<=";

        public const string PrimaryListMethodName = "PrimaryList";
        public const string ForeignListMethodName = "ForeignList";
        public const string ListMethodName = "List";
        public const string ShowFieldMethodName = "ShowField";
        public const string JoinsMethodName = "Joins";
        public const string LeftJoinMethodName = "Left";
        public const string InnerJoinMethodName = "Inner";
        public const string FieldMethodName = "Field";
        public const string ProjectedFieldsMethodName = "ProjectedFields";

//        public static IEnumerable<ParameterExpression> GetExpressionParameters(ParameterInfo[] parameterInfos)
//        {
//            var result = new List<ParameterExpression>();
//            Array.ForEach(parameterInfos, pi => result.Add(Expression.Parameter(pi.ParameterType, pi.Name)));
//            return result;
//        }

        public static IEnumerable<MethodInfo> GetExtensionMethods(Assembly assembly,
            Type extendedType)
        {
            var query = from type in assembly.GetTypes()
                        where type.IsSealed && !type.IsGenericType && !type.IsNested
                        from method in type.GetMethods(BindingFlags.Static
                            | BindingFlags.Public | BindingFlags.NonPublic)
                        where method.IsDefined(typeof(ExtensionAttribute), false)
                        where method.GetParameters()[0].ParameterType == extendedType
                        select method;
            return query;
        }

        public static MethodInfo GetMethodInfo(Type type, string methodName)
        {
            return type.GetMethod(methodName);
        }
    }
}