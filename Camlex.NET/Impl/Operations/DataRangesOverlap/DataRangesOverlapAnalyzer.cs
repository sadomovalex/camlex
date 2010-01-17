using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CamlexNET.Interfaces;

namespace CamlexNET.Impl.Operations.DataRangesOverlap
{
    internal class DataRangesOverlapAnalyzer : BinaryExpressionBaseAnalyzer
    {
        public DataRangesOverlapAnalyzer(IOperationResultBuilder operationResultBuilder, IOperandBuilder operandBuilder)
            : base(operationResultBuilder, operandBuilder)
        {
        }

        public override bool IsValid(LambdaExpression expr)
        {
            var methodCall = expr.Body as MethodCallExpression;
            if (methodCall == null) return false;
            if (methodCall.Method.Name != ReflectionHelper.DateRangesOverlap) return false;
            if (methodCall.Arguments.Count != 4) return false;
            {
                if (!isValidLeftExpressionWithStringBasedSyntax(methodCall.Arguments[0])) return false;
                if (!isValidLeftExpressionWithStringBasedSyntax(methodCall.Arguments[1])) return false;
                if (!isValidLeftExpressionWithStringBasedSyntax(methodCall.Arguments[2])) return false;
                return true;
            }
        }

        public override IOperation GetOperation(LambdaExpression expr)
        {
            var methodCall = expr.Body as MethodCallExpression;

            var startFieldRef = operandBuilder.CreateFieldRefOperand(methodCall.Arguments[0]);
            var stopFieldRef = operandBuilder.CreateFieldRefOperand(methodCall.Arguments[1]);
            var recurrenceFieldRef = operandBuilder.CreateFieldRefOperand(methodCall.Arguments[2]);
            var dateTimeValue = operandBuilder.CreateValueOperandForNativeSyntax(methodCall.Arguments[3]);
            var operation = new DataRangesOverlapOperation(
                operationResultBuilder, startFieldRef, stopFieldRef, recurrenceFieldRef, dateTimeValue);

            return operation;
        }
    }
}
