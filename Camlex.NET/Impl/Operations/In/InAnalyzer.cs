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
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using CamlexNET.Impl.Operands;
using CamlexNET.Interfaces;

namespace CamlexNET.Impl.Operations.In
{
    internal class InAnalyzer : BaseAnalyzer
    {
        protected IOperandBuilder operandBuilder;
        public InAnalyzer(IOperationResultBuilder operationResultBuilder, IOperandBuilder operandBuilder)
            : base(operationResultBuilder)
        {
            this.operandBuilder = operandBuilder;
        }

        public override bool IsValid(LambdaExpression expr)
        {
            if (expr == null)
            {
                return false;
            }
            // body should be MethodCallExpression
            if (!(expr.Body is MethodCallExpression))
            {
                return false;
            }
            var body = expr.Body as MethodCallExpression;

            // left operand for string based syntax should be enumerable
            if (body.Arguments == null)
            {
                return false;
            }

            // there should be 2 arguments in Linq Contains method:
            // public static bool Contains<TSource>(this IEnumerable<TSource> source, TSource value);
            if (body.Arguments.Count != 2)
            {
                return false;
            }

            // 1st arg should be enumerable
            var source = body.Arguments[0];
            if (source == null)
            {
                return false;
            }

            if (!typeof(IEnumerable).IsAssignableFrom(source.Type))
            {
                return false;
            }
            if (!this.isValidEvaluableExpression(source))
            {
                return false;
            }

            var val = body.Arguments[1];
            if (val == null)
            {
                return false;
            }

            // left operand for string based syntax should be convert of indexer
            if (!(val is UnaryExpression) || val.NodeType != ExpressionType.Convert)
            {
                return false;
            }
            var methodCallExpression = ((UnaryExpression)val).Operand;
            if (!(methodCallExpression is MethodCallExpression))
            {
                return false;
            }
            var objectExpression = (MethodCallExpression)methodCallExpression;
            if (objectExpression.Method.Name != ReflectionHelper.IndexerMethodName)
            {
                return false;
            }
            if (objectExpression.Arguments.Count != 1)
            {
                return false;
            }
            if (!this.isValidEvaluableExpression(objectExpression.Arguments[0]))
            {
                return false;
            }

            return true;
        }

        public override IOperation GetOperation(LambdaExpression expr)
        {
            if (!IsValid(expr))
            {
                throw new NonSupportedExpressionException(expr);
            }
            var fieldRefOperand = this.getFieldRefOperand(expr);
            var valueOperand = this.getValueOperand(expr);
            return new InOperation(operationResultBuilder, fieldRefOperand, valueOperand);
        }

        private IOperand getValueOperand(LambdaExpression expr)
        {
            var body = expr.Body as MethodCallExpression;
            return operandBuilder.CreateValuesValueOperand(body.Arguments[0]);
        }

        private IOperand getFieldRefOperand(LambdaExpression expr)
        {
            var body = expr.Body as MethodCallExpression;
            return operandBuilder.CreateFieldRefOperand(body.Arguments[1], null);
        }
    }
}


