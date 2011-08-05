using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.SharePoint;

namespace CamlexNET.Interfaces
{
    public interface IQueryEx : IQuery
    {
        string ViewFields(Expression<Func<SPListItem, object>> expr);
        string ViewFields(Expression<Func<SPListItem, object>> expr, bool includeViewFieldsTag);
        string ViewFields(Expression<Func<SPListItem, object[]>> expr);
        string ViewFields(Expression<Func<SPListItem, object[]>> expr, bool includeViewFieldsTag);
    }
}
