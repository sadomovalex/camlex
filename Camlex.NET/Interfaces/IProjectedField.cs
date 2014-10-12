using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Linq;
using Microsoft.SharePoint;

namespace CamlexNET.Interfaces
{
    public interface IProjectedField
    {
        IProjectedField Field(Expression<Func<SPListItem, object>> expr);
        string ToString();
        string ToString(bool includeParentTag);
        XElement[] ToCaml(bool includeParentTag);
    }
}
