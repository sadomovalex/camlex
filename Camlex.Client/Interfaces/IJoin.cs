using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Linq;
using Microsoft.SharePoint;

namespace CamlexNET.Interfaces
{
    public interface IJoin
    {
        IJoin Left(Expression<Func<SPListItem, object>> expr);
        IJoin Inner(Expression<Func<SPListItem, object>> expr);
        string ToString();
        string ToString(bool includeJoinsTag);
        XElement[] ToCaml(bool includeJoinsTag);
    }
}
