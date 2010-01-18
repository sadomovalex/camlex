#region Copyright(c) Alexey Sadomov, Vladimir Timashkov. All Rights Reserved.
// -----------------------------------------------------------------------------
// Copyright(c) 2010 Alexey Sadomov, Vladimir Timashkov. All Rights Reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//
//   1. No Trademark License - Microsoft Public License (Ms-PL) does not grant you rights to use
//      authors names, logos, or trademarks.
//   2. If you distribute any portion of the software, you must retain all copyright,
//      patent, trademark, and attribution notices that are present in the software.
//   3. If you distribute any portion of the software in source code form, you may do
//      so only under this license by including a complete copy of Microsoft Public License (Ms-PL)
//      with your distribution. If you distribute any portion of the software in compiled
//      or object code form, you may only do so under a license that complies with
//      Microsoft Public License (Ms-PL).
//   4. The names of the authors may not be used to endorse or promote products
//      derived from this software without specific prior written permission.
//
// The software is licensed "as-is." You bear the risk of using it. The authors
// give no express warranties, guarantees or conditions. You may have additional consumer
// rights under your local laws which this license cannot change. To the extent permitted
// under your local laws, the authors exclude the implied warranties of merchantability,
// fitness for a particular purpose and non-infringement.
// -----------------------------------------------------------------------------
#endregion
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
