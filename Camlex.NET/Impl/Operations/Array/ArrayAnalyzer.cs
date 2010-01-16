#region Copyright(c) Alexey Sadomov, Vladimir Timashkov. All Rights Reserved.
// -----------------------------------------------------------------------------
// Copyright(c) 2007 Alexey Sadomov, Vladimir Timashkov. All Rights Reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//
//   1.  No Trademark License - Microsoft Public License (Ms-PL) does not grant you rights to use
//       authors names, logos, or trademarks.
//   2.  If you distribute any portion of the software, you must retain all copyright,
//       patent, trademark, and attribution notices that are present in the software.
//   3.  If you distribute any portion of the software in source code form, you may do
//       so only under this license by including a complete copy of Microsoft Public License (Ms-PL)
//       with your distribution. If you distribute any portion of the software in compiled
//       or object code form, you may only do so under a license that complies with
//       Microsoft Public License (Ms-PL).
//
// The software is licensed "as-is." You bear the risk of using it. The authors
// give no express warranties, guarantees or conditions. You may have additional consumer
// rights under your local laws which this license cannot change. To the extent permitted
// under your local laws, the authors exclude the implied warranties of merchantability,
// fitness for a particular purpose and non-infringement.
// -----------------------------------------------------------------------------
#endregion
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CamlexNET.Interfaces;

namespace CamlexNET.Impl.Operations.Array
{
    internal class ArrayAnalyzer : BaseAnalyzer
    {
        private IOperandBuilder operandBuilder;

        public ArrayAnalyzer(IOperationResultBuilder operationResultBuilder,
            IOperandBuilder operandBuilder) :
            base(operationResultBuilder)
        {
            this.operandBuilder = operandBuilder;
        }

        public override bool IsValid(LambdaExpression expr)
        {
            var body = expr.Body as NewArrayExpression;
            if (body == null) return false;
            var counter = 0;
            body.Expressions.ToList().ForEach(ex =>
            {
                if(ex.NodeType == ExpressionType.TypeAs)
                {
                    var unary = ex as UnaryExpression;
                    if (unary == null ||
                        (unary.Type != typeof(Camlex.Asc) && unary.Type != typeof(Camlex.Desc))) return;
                    ex = unary.Operand;
                }
                var methodCall = ex as MethodCallExpression;
                if (methodCall == null) return;
                if (methodCall.Method.Name != ReflectionHelper.IndexerMethodName) return;
                if (methodCall.Arguments.Count != 1) return;
                if (!(methodCall.Arguments[0] is ConstantExpression)) return;
                counter++;
            });
            return (body.Expressions.Count == counter);
        }

        public override IOperation GetOperation(LambdaExpression expr)
        {
            if (!IsValid(expr))
            {
                throw new NonSupportedExpressionException(expr);
            }
            var operands = getFieldRefOperandsWithOrdering(expr);
            return new ArrayOperation(this.operationResultBuilder, operands);
        }

        private IOperand[] getFieldRefOperandsWithOrdering(LambdaExpression expr)
        {
            var operands = new List<IOperand>();
            ((NewArrayExpression)expr.Body).Expressions.ToList().ForEach(ex =>
            {
                var orderDirection = Camlex.OrderDirection.Default;
                if (ex.NodeType == ExpressionType.TypeAs)
                {
                    orderDirection = Camlex.OrderDirection.Convert(ex.Type);
                    ex = ((UnaryExpression)ex).Operand;
                }
                operands.Add(this.operandBuilder.CreateFieldRefOperandWithOrdering(ex, orderDirection));
            });
            return operands.ToArray();
        }
    }
}


