﻿using System.Linq.Expressions;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Operations.Geq
{
    public class GeqAnalyzer : BinaryExpressionBaseAnalyzer
    {
        public GeqAnalyzer(IOperandBuilder operandBuilder)
            : base(operandBuilder)
        {
        }

        public override bool IsValid(LambdaExpression expr)
        {
            if (!base.IsValid(expr))
            {
                return false;
            }
            return (expr.Body.NodeType == ExpressionType.GreaterThanOrEqual);
        }

        public override IOperation GetOperation(LambdaExpression expr)
        {
            if (!this.IsValid(expr))
            {
                throw new NonSupportedExpressionException(expr);
            }
            var fieldRefOperand = this.getFieldRefOperand(expr);
            var valueOperand = this.getValueOperand(expr);
            return new GeqOperation(fieldRefOperand, valueOperand);
        }
    }
}

