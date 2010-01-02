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
        public EqAnalyzer(IOperandBuilder operandBuilder) : base(operandBuilder)
        {
        }

        public override bool IsValid(Expression<Func<SPItem, bool>> expr)
        {
            if (expr.Body.NodeType != ExpressionType.Equal)
            {
                return false;
            }
            return base.IsValid(expr);
        }

        public override IOperation GetOperation(Expression<Func<SPItem, bool>> expr)
        {
            if (!this.IsValid(expr))
            {
                throw new NonSupportedExpressionException(expr);
            }
            var fieldRefOperand = this.getFieldRefOperand(expr);
            var valueOperand = this.getValueOperand(expr);
            return new EqOperation(fieldRefOperand, valueOperand);
        }
    }
}
