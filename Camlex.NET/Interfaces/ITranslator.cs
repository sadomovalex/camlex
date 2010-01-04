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
        XElement TranslateWhere(LambdaExpression expr);
        XElement TranslateOrderBy(LambdaExpression expr);
    }
}
