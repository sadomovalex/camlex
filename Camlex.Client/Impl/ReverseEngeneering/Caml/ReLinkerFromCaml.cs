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
using System.Reflection;
using System.Xml.Linq;
using CamlexNET.Interfaces;
using CamlexNET.Interfaces.ReverseEngeneering;
using Microsoft.SharePoint.Client;

namespace CamlexNET.Impl.ReverseEngeneering.Caml
{
	internal class ReLinkerFromCaml : IReLinker
	{
		private readonly XElement where;
		private readonly XElement orderBy;
		private readonly XElement groupBy;
		private readonly XElement rowLimit;
		private readonly XElement viewFields;

		private class MethodInfoWithParams
		{
			public MethodInfo MethodInfo { get; private set; }
			public List<Expression> Params { get; private set; }
			public MethodInfoWithParams(MethodInfo mi, List<Expression> p)
			{
				this.MethodInfo = mi;
				this.Params = p;
			}
		}

		public ReLinkerFromCaml(XElement where, XElement orderBy, XElement groupBy, XElement viewFields, XElement rowLimit)
		{
			this.where = where;
			this.viewFields = viewFields;
			this.groupBy = groupBy;
			this.orderBy = orderBy;
			this.rowLimit = rowLimit;
		}

		public Expression Link(LambdaExpression where, LambdaExpression orderBy, LambdaExpression groupBy,
			LambdaExpression viewFields, GroupByParams groupByParams, LambdaExpression rowLimit = null)
		{
			// list of fluent calls
			var listFluent = new List<KeyValuePair<string, LambdaExpression>>
			{
				new KeyValuePair<string, LambdaExpression>(ReflectionHelper.WhereMethodName, where),
				new KeyValuePair<string, LambdaExpression>(ReflectionHelper.OrderByMethodName, orderBy),
				new KeyValuePair<string, LambdaExpression>(ReflectionHelper.GroupByMethodName, groupBy),
                new KeyValuePair<string, LambdaExpression>(ReflectionHelper.ViewFieldsMethodName, viewFields),
				new KeyValuePair<string, LambdaExpression>(ReflectionHelper.RowLimitMethodName, rowLimit)
			};

			// view fields is not fluent
//			var listViewFields = new List<KeyValuePair<string, LambdaExpression>>
//			{
//				new KeyValuePair<string, LambdaExpression>(ReflectionHelper.ViewFieldsMethodName, viewFields)
//			};

//			if (listFluent.Any(kv => kv.Value != null) && listViewFields.Any(kv => kv.Value != null))
//			{
//				throw new OnlyOnePartOfQueryShouldBeNotNullException();
//			}

			//var list = listFluent.Any(kv => kv.Value != null) ? listFluent : listViewFields;
            if (listFluent.All(kv => kv.Value == null))
			{
				throw new AtLeastOneCamlPartShouldNotBeEmptyException();
			}

			var queryMi = ReflectionHelper.GetMethodInfo(typeof(Camlex), ReflectionHelper.QueryMethodName);
			var queryCall = Expression.Call(queryMi);

			var expr = queryCall;
            for (int i = 0; i < listFluent.Count; i++)
			{
                var kv = listFluent[i];
				if (kv.Value != null)
				{
					var mi = this.getMethodInfo(kv.Key, groupByParams);
					if (mi != null && mi.MethodInfo != null)
					{
						var args = new List<Expression>
						{
							kv.Value // 1st param is always lambda expression
						};

						if (mi.Params != null && mi.Params.Count > 0)
						{
							mi.Params.ForEach(args.Add);
						}
						// as we use fluent interfaces we just pass on next step value which we got from prev step
						expr = Expression.Call(expr, mi.MethodInfo, args);
					}
				}
			}
			return expr;
		}

		private MethodInfoWithParams getMethodInfo(string methodName, GroupByParams groupByParams)
		{
			if (methodName == ReflectionHelper.WhereMethodName)
			{
				var mi = ReflectionHelper.GetMethodInfo(typeof(IQuery), methodName);
				return new MethodInfoWithParams(mi, null);
			}
			if (methodName == ReflectionHelper.OrderByMethodName)
			{
				return this.getOrderByMethodInfo();
			}
			if (methodName == ReflectionHelper.GroupByMethodName)
			{
				return this.getGroupByMethodInfo(groupByParams);
			}
			if (methodName == ReflectionHelper.RowLimitMethodName)
			{
				var mi = ReflectionHelper.GetMethodInfo(typeof(IQuery), methodName);
				return new MethodInfoWithParams(mi, null);
			}
			if (methodName == ReflectionHelper.ViewFieldsMethodName)
			{
				return this.getViewFieldsMethodInfo();
			}
			return null;
		}

		private MethodInfoWithParams getViewFieldsMethodInfo()
		{
			var count = this.viewFields.Descendants(Tags.FieldRef).Count();
			if (count == 0)
			{
				return null;
			}

			var type = count == 1 ? typeof(Expression<Func<ListItem, object>>) : typeof(Expression<Func<ListItem, object[]>>);
			var methodInfo = typeof(IQuery).GetMethod(ReflectionHelper.ViewFieldsMethodName, new[] { type });

			//var p = new List<Expression>(new[] { Expression.Constant(true) });
			return new MethodInfoWithParams(methodInfo, null);
		}

		private MethodInfoWithParams getGroupByMethodInfo(GroupByParams groupByParams)
		{
			var count = this.groupBy.Descendants(Tags.FieldRef).Count();
			if (count == 0)
			{
				return null;
			}

			var p = this.getGroupByParams(count, groupByParams.HasGroupLimit, groupByParams.HasCollapse,
				groupByParams.GroupLimit, groupByParams.Collapse);
			var mi = this.getGroupByMethodInfo(count, groupByParams.HasCollapse, groupByParams.HasGroupLimit);
			return new MethodInfoWithParams(mi, p);
		}

		private MethodInfo getGroupByMethodInfo(int count, bool hasCollapse, bool hasGroupLimit)
		{
			MethodInfo mi;
			if (count == 1)
			{
				if (hasCollapse && hasGroupLimit)
				{
					mi = typeof(IQuery).GetMethod(ReflectionHelper.GroupByMethodName,
												   new[]
													   {
														   typeof (Expression<Func<ListItem, object>>), typeof (bool?), typeof (int?)
													   });
				}
				else if (hasCollapse && !hasGroupLimit)
				{
					mi = typeof(IQuery).GetMethod(ReflectionHelper.GroupByMethodName,
												   new[] { typeof(Expression<Func<ListItem, object>>), typeof(bool?) });
				}
				else if (!hasCollapse && hasGroupLimit)
				{
					mi = typeof(IQuery).GetMethod(ReflectionHelper.GroupByMethodName,
												   new[] { typeof(Expression<Func<ListItem, object>>), typeof(int?) });
				}
				else
				{
					mi = typeof(IQuery).GetMethod(ReflectionHelper.GroupByMethodName,
												   new[] { typeof(Expression<Func<ListItem, object>>) });
				}
			}
			else
			{
				mi = typeof(IQuery).GetMethod(ReflectionHelper.GroupByMethodName,
											   new[]
												   {
													   typeof (Expression<Func<ListItem, object[]>>), typeof (bool?), typeof (int?)
												   });
			}
			return mi;
		}

		private List<Expression> getGroupByParams(int count, bool hasGroupLimit, bool hasCollapse, int groupLimit, bool collapse)
		{
			List<Expression> p = null;
			if (hasCollapse && hasGroupLimit)
			{
				p = new List<Expression>();
				p.Add(Expression.Constant(collapse, typeof(bool?)));
				p.Add(Expression.Constant(groupLimit, typeof(int?)));
			}
			else if (hasCollapse && !hasGroupLimit)
			{
				p = new List<Expression>();
				p.Add(Expression.Constant(collapse, typeof(bool?)));
				// there is only 1 method which receives several field refs: GroupBy(Expression<Func<ListItem, object[]>> expr, bool? collapse, int? groupLimit).
				// For this method we need always provide 2 arguments);
				if (count > 1)
				{
					p.Add(Expression.Constant(null, typeof(int?)));
				}
			}
			else if (!hasCollapse && hasGroupLimit)
			{
				// there is only 1 method which receives several field refs: GroupBy(Expression<Func<ListItem, object[]>> expr, bool? collapse, int? groupLimit).
				// For this method we need always provide 2 arguments
				p = new List<Expression>();
				if (count > 1)
				{
					p.Add(Expression.Constant(null, typeof(bool?)));
				}

				p.Add(Expression.Constant(groupLimit, typeof(int?)));
			}
			else if (count > 1)
			{
				// there is only 1 method which receives several field refs: GroupBy(Expression<Func<ListItem, object[]>> expr, bool? collapse, int? groupLimit).
				// For this method we need always provide 2 arguments
				p = new List<Expression>();
				p.Add(Expression.Constant(null, typeof(bool?)));
				p.Add(Expression.Constant(null, typeof(int?)));
			}
			return p;
		}

		private MethodInfoWithParams getOrderByMethodInfo()
		{
			var count = this.orderBy.Descendants(Tags.FieldRef).Count();
			MethodInfo mi = null;
			if (count == 0)
			{
				return null;
			}
			if (count == 1)
			{
				mi = typeof(IQuery).GetMethod(ReflectionHelper.OrderByMethodName,
												new[] { typeof(Expression<Func<ListItem, object>>) });
			}
			else
			{
				mi = typeof(IQuery).GetMethod(ReflectionHelper.OrderByMethodName,
												new[] { typeof(Expression<Func<ListItem, object[]>>) });
			}
			return new MethodInfoWithParams(mi, null);
		}
	}
}