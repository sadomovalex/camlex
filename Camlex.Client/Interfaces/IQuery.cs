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
using System.Linq.Expressions;
using System.Xml.Linq;
using Microsoft.SharePoint.Client;

namespace CamlexNET.Interfaces
{
	public interface IQuery
	{
		IQuery Where(Expression<Func<ListItem, bool>> expr);
		IQuery WhereAll(IEnumerable<Expression<Func<ListItem, bool>>> expressions);
		IQuery WhereAll(string existingWhere, Expression<Func<ListItem, bool>> expression);
		IQuery WhereAll(string existingWhere, IEnumerable<Expression<Func<ListItem, bool>>> expressions);
                IQuery WhereAll(IEnumerable<string> expressions);
		IQuery WhereAny(IEnumerable<Expression<Func<ListItem, bool>>> expressions);
		IQuery WhereAny(string existingWhere, Expression<Func<ListItem, bool>> expression);
		IQuery WhereAny(string existingWhere, IEnumerable<Expression<Func<ListItem, bool>>> expressions);
		IQuery WhereAny(IEnumerable<string> expression);
		IQuery OrderBy(Expression<Func<ListItem, object>> expr);
		IQuery OrderBy(Expression<Func<ListItem, object[]>> expr);
		IQuery OrderBy(IEnumerable<Expression<Func<ListItem, object>>> expressions);
		IQuery OrderBy(string existingOrderBy, Expression<Func<ListItem, object>> expr);
		IQuery OrderBy(string existingOrderBy, Expression<Func<ListItem, object[]>> expr);
		IQuery OrderBy(string existingOrderBy, IEnumerable<Expression<Func<ListItem, object>>> expressions);
		IQuery GroupBy(Expression<Func<ListItem, object>> expr);
		IQuery GroupBy(Expression<Func<ListItem, object[]>> expr, bool? collapse, int? groupLimit);
		IQuery GroupBy(Expression<Func<ListItem, object>> expr, bool? collapse, int? groupLimit);
		IQuery GroupBy(Expression<Func<ListItem, object>> expr, int? groupLimit);
		IQuery GroupBy(Expression<Func<ListItem, object>> expr, bool? collapse);
		IQuery GroupBy(string existingGroupBy, Expression<Func<ListItem, object>> expr);
		IQuery GroupBy(string existingGroupBy, Expression<Func<ListItem, object[]>> expr);
		IQuery Take(int count);

        IQuery ViewFields(Expression<Func<ListItem, object>> expr);
//        IQuery ViewFields(Expression<Func<ListItem, object>> expr, bool includeViewFieldsTag);
        IQuery ViewFields(Expression<Func<ListItem, object[]>> expr);
//        IQuery ViewFields(Expression<Func<ListItem, object[]>> expr, bool includeViewFieldsTag);

        IQuery ViewFields(string existingViewFields, Expression<Func<ListItem, object>> expr);
//        IQuery ViewFields(string existingViewFields, Expression<Func<ListItem, object>> expr, bool includeViewFieldsTag);
        IQuery ViewFields(string existingViewFields, Expression<Func<ListItem, object[]>> expr);
//        IQuery ViewFields(string existingViewFields, Expression<Func<ListItem, object[]>> expr, bool includeViewFieldsTag);

        IQuery ViewFields(IEnumerable<string> titles);
//        IQuery ViewFields(IEnumerable<string> titles, bool includeViewFieldsTag);
//        IQuery ViewFields(IEnumerable<Guid> ids);
//        IQuery ViewFields(IEnumerable<Guid> ids, bool includeViewFieldsTag);

        IQuery ViewFields(string existingViewFields, IEnumerable<string> titles);
//        IQuery ViewFields(string existingViewFields, IEnumerable<string> titles, bool includeViewFieldsTag);
//        IQuery ViewFields(string existingViewFields, IEnumerable<Guid> ids);
//        IQuery ViewFields(string existingViewFields, IEnumerable<Guid> ids, bool includeViewFieldsTag);

	    IQuery Scope(ViewScope scope);

	    XElement[] ToCaml(bool includeViewTag);
		string ToString();
		string ToString(bool includeViewTag);
		CamlQuery ToCamlQuery();
	}
}
