﻿using System.Linq.Expressions;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Operations.Contains
{
    public class ContainsAnalyzer : UnaryExpressionBaseAnalyzer
    {
        public ContainsAnalyzer(IOperationResultBuilder operationResultBuilder, IOperandBuilder operandBuilder)
            : base(operationResultBuilder, operandBuilder)
        {
        }

        public override bool IsValid(LambdaExpression expr)
        {
            if (!base.IsValid(expr))
            {
                return false;
            }
            return ((MethodCallExpression)expr.Body).Method.Name == ReflectionHelper.ContainsMethodName;
        }

        public override IOperation GetOperation(LambdaExpression expr)
        {
            if (!IsValid(expr))
            {
                throw new NonSupportedExpressionException(expr);
            }
            var fieldRefOperand = getFieldRefOperand(expr);
            var valueOperand = getValueOperand(expr);
            return new ContainsOperation(operationResultBuilder, fieldRefOperand, valueOperand);
        }
    }
}