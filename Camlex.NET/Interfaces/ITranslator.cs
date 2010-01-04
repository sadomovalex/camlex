using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Linq;
using Microsoft.SharePoint;

namespace Camlex.NET.Interfaces
{
    public interface ITranslator
    {
        XElement TranslateWhere(Expression<Func<SPItem, bool>> expr);
        XElement TranslateOrderBy(Expression<Func<SPItem, object[]>> expr);
    }
}
