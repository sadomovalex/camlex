using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.SharePoint;

namespace Camlex.NET.Interfaces
{
    public interface ITranslator
    {
        string TranslateWhere(Expression<Func<SPItem, bool>> expr);
    }
}
