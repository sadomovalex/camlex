using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Camlex.NET.Interfaces;
using Microsoft.SharePoint;

namespace Camlex.NET.Impl.Eq
{
    public class EqAnalyzer : BinaryExpressionBaseAnalyzer
    {
        public override bool IsValid(Expression<Func<SPItem, bool>> expr)
        {
            if (expr.Body.NodeType != ExpressionType.Equal)
            {
                return false;
            }
            return base.IsValid(expr);
        }
    }
}
