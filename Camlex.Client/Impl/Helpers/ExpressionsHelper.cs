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
using System.Linq.Expressions;
using Microsoft.SharePoint.Client;

namespace CamlexNET.Impl.Helpers
{
	public static class ExpressionsHelper
	{
		public static Expression<Func<ListItem, bool>> CombineAnd(
			IEnumerable<Expression<Func<ListItem, bool>>> expressions)
		{
			return Combine(expressions, ExpressionType.AndAlso);
		}

		public static Expression<Func<ListItem, bool>> CombineOr(
			IEnumerable<Expression<Func<ListItem, bool>>> expressions)
		{
			return Combine(expressions, ExpressionType.OrElse);
		}

		// ----- Internal methods -----

		private static Expression<Func<ListItem, bool>> Combine(
			IEnumerable<Expression<Func<ListItem, bool>>> expressions, ExpressionType type)
		{
			if (expressions == null || expressions.Count() == 0)
			{
				throw new EmptyExpressionsListException();
			}

			// if (!this.allExpressionsHaveTheSameArgumentName(expressions))
			// {
			//     throw new DifferentArgumentsNamesExceptions();
			// }

			Expression result;
			if (expressions.Count() == 1)
			{
				result = expressions.First().Body;
			}
			else
			{
				result = joinExpressions(expressions, type);
			}

			var lambda = Expression.Lambda<Func<ListItem, bool>>(result,
				Expression.Parameter(typeof(ListItem), ReflectionHelper.CommonParameterName));
			return lambda;
		}

		// private bool allExpressionsHaveTheSameArgumentName(IEnumerable<Expression<Func<ListItem, bool>>> expressions)
		// {
		//     if (expressions.Count() == 0)
		//     {
		//         throw new EmptyExpressionsListException();
		//     }
		//
		//     // there is no need to check number of parameters, as compiler checked it when
		//     // we specified Func<ListItem, bool> func which can't have another number
		//     // of parameters except single param
		//     string argumentName = expressions.First().Parameters[0].Name;
		//     for (int i = 1; i < expressions.Count(); i++)
		//     {
		//         if (expressions.ElementAt(i).Parameters[0].Name != argumentName)
		//         {
		//             return false;
		//         }
		//     }
		//     return true;
		// }

		private static BinaryExpression joinExpressions(
			IEnumerable<Expression<Func<ListItem, bool>>> expressions, ExpressionType type)
		{
			return joinExpressions(1, expressions, expressions.ElementAt(0).Body, type);
		}

		// See http://sadomovalex.blogspot.com/2010/02/build-dynamic-expressions-for-caml.html
		private static BinaryExpression joinExpressions(
			int currentIdxToAdd, IEnumerable<Expression<Func<ListItem, bool>>> expressions,
			Expression prevExpr, ExpressionType type)
		{
			if (currentIdxToAdd >= expressions.Count())
			{
				return (BinaryExpression)prevExpr;
			}

			var currentExpression = expressions.ElementAt(currentIdxToAdd);

			Expression resultExpr;
			if (type == ExpressionType.OrElse)
			{
				resultExpr = Expression.OrElse(prevExpr, currentExpression.Body);
			}
			else if (type == ExpressionType.AndAlso)
			{
				resultExpr = Expression.AndAlso(prevExpr, currentExpression.Body);
			}
			else
			{
				throw new OnlyOrAndBinaryExpressionsAllowedForJoinsExceptions();
			}
			return joinExpressions(currentIdxToAdd + 1, expressions, resultExpr, type);
		}

		// ----------- Helper methods working with DateTime ----------

		internal static bool IncludeTimeValue(Expression expression)
		{
			return (expression is MethodCallExpression) && (((MethodCallExpression)expression).Method.Name == ReflectionHelper.IncludeTimeValue);
		}

		internal static Expression RemoveIncludeTimeValueMethodCallIfAny(Expression expression)
		{
			if (!IncludeTimeValue(expression)) return expression;
			var methodCall = (MethodCallExpression)expression;

			if (methodCall.Object != null) return methodCall.Object;
			if (methodCall.Arguments.Count == 1) return methodCall.Arguments[0];

			throw new NonSupportedExpressionException(expression); // it should not happen - either Object or Arguments  is not NULL
		}

		public static bool IsIntegerForUserId(Expression expr)
		{
			if (expr.Type.FullName == typeof(DataTypes.Integer).FullName)
			{
				if (expr is UnaryExpression && expr.NodeType == ExpressionType.Convert)
				{
					expr = ((UnaryExpression)expr).Operand;
					if (expr is UnaryExpression && expr.NodeType == ExpressionType.Convert)
					{
						expr = ((UnaryExpression)expr).Operand;
						if (expr is MemberExpression &&
							((MemberExpression)expr).Member.DeclaringType.FullName ==
							typeof(Camlex).FullName &&
							((MemberExpression)expr).Member.Name == ReflectionHelper.UserID)
						{
							return true;
						}
					}
				}
			}
			return false;
		}
	}
}