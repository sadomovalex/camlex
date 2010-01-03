using System;
using System.Linq.Expressions;
using Camlex.NET.Impl.Eq;
using Camlex.NET.Interfaces;
using Microsoft.SharePoint;

namespace Camlex.NET.Impl.AndAlso
{
    public class AndAlsoAnalyzer : CompositeExpressionBaseAnalyzer
    {
        public AndAlsoAnalyzer(IAnalyzerFactory analyzerFactory) :
            base(analyzerFactory)
        {
        }

        public override bool IsValid(Expression<Func<SPItem, bool>> expr)
        {
            if (!base.IsValid(expr))
            {
                return false;
            }
            return (expr.Body.NodeType == ExpressionType.AndAlso);
        }

        public override IOperation GetOperation(Expression<Func<SPItem, bool>> expr)
        {
            if (!this.IsValid(expr))
            {
                throw new NonSupportedExpressionException(expr);
            }
            throw new NotImplementedException();
        }
    }
}


