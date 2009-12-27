using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.SharePoint;

namespace Camlex.NET.Interfaces
{
    public interface IAnalyzer
    {
        bool IsValid(Expression<Func<SPItem, bool>> expr);
        ILeftOperand GetLeftOperand(Expression<Func<SPItem, bool>> expr);
        IRightOperand GetRightOperand(Expression<Func<SPItem, bool>> expr);
    }
}
