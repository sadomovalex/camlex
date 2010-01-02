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
//        IOperand GetLeftOperand(Expression<Func<SPItem, bool>> expr);
//        IOperand GetRightOperand(Expression<Func<SPItem, bool>> expr);
        IOperation GetOperation(Expression<Func<SPItem, bool>> expr);
    }
}
