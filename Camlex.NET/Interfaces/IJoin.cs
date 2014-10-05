using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.SharePoint;

namespace CamlexNET.Interfaces
{
    public interface IJoin
    {
        IJoin Left(Expression<Func<SPListItem, object>> expr, string foreignListAlias);
        IJoin Left(Expression<Func<SPListItem, object>> expr, string foreignListAlias, string primaryListAlias);
        IJoin Inner(Expression<Func<SPListItem, object>> expr, string foreignListAlias);
        IJoin Inner(Expression<Func<SPListItem, object>> expr, string foreignListAlias, string primaryListAlias);
    }
}
