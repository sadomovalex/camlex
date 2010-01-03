using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.SharePoint;

namespace Camlex.NET.Interfaces
{
    public interface IArrayAnalyzer
    {
        bool IsValid(Expression<Func<SPItem, object[]>> expr);
        IOperation GetOperation(Expression<Func<SPItem, object[]>> expr);
    }
}
