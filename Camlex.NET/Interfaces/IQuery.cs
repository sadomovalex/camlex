using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Linq;
using Microsoft.SharePoint;

namespace CamlexNET.Interfaces
{
    public interface IQuery
    {
        IQuery Where(Expression<Func<SPItem, bool>> expr);
        IQuery OrderBy(Expression<Func<SPItem, object>> expr);
        IQuery OrderBy(Expression<Func<SPItem, object[]>> expr);
        IQuery GroupBy(Expression<Func<SPItem, object>> expr);
        IQuery GroupBy(Expression<Func<SPItem, object[]>> expr, bool? collapse, int? groupLimit);
        IQuery GroupBy(Expression<Func<SPItem, object>> expr, bool? collapse, int? groupLimit);
        IQuery GroupBy(Expression<Func<SPItem, object>> expr, int? groupLimit);
        IQuery GroupBy(Expression<Func<SPItem, object>> expr, bool? collapse);
        XElement ToCaml();
        string ToString();
    }
}
