using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Camlex.NET.Interfaces;
using Microsoft.SharePoint;

namespace Camlex.NET.Impl.Eq
{
    public class EqAnalyzer : IAnalyzer
    {
        public bool IsValid(Expression<Func<SPItem, bool>> expr)
        {
            throw new NotImplementedException();
        }

        public ILeftOperand GetLeftOperand(Expression<Func<SPItem, bool>> expr)
        {
            throw new NotImplementedException();
        }

        public IRightOperand GetRightOperand(Expression<Func<SPItem, bool>> expr)
        {
            throw new NotImplementedException();
        }
    }
}
