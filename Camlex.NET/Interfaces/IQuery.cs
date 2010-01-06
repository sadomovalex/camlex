using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Linq;
using Microsoft.SharePoint;

namespace Camlex.NET.Interfaces
{
    public interface IQuery
    {
        IQuery Where(Expression<Func<SPItem, bool>> expr);
        IQuery OrderBy(Expression<Func<SPItem, object>> expr);
        IQuery OrderBy(Expression<Func<SPItem, object[]>> expr);
        IQuery GroupBy(Expression<Func<SPItem, object>> expr);
        IQuery GroupBy(Expression<Func<SPItem, object[]>> expr, bool? collapse, int? groupLimit);
        XElement ToCaml();
        string ToString();
    }
}
