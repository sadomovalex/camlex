using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CamlexNET.Interfaces;
using Microsoft.SharePoint;

namespace CamlexNET.Impl
{
    public class Join : IJoin
    {
        public IJoin Left(Expression<Func<SPListItem, object>> expr, string foreignListAlias)
        {
            throw new NotImplementedException();
        }

        public IJoin Left(Expression<Func<SPListItem, object>> expr, string primaryListAlias, string foreignListAlias)
        {
            throw new NotImplementedException();
        }

        public IJoin Inner(Expression<Func<SPListItem, object>> expr, string foreignListAlias)
        {
            throw new NotImplementedException();
        }

        public IJoin Inner(Expression<Func<SPListItem, object>> expr, string primaryListAlias, string foreignListAlias)
        {
            throw new NotImplementedException();
        }
    }
}
