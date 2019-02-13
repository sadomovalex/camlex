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
using System.Linq.Expressions;
using CamlexNET.Interfaces;

namespace CamlexNET.Impl.Operations.NotIncludes
{
    internal class NotIncludesAnalyzer : UnaryExpressionBaseAnalyzer
    {
        public NotIncludesAnalyzer(IOperationResultBuilder operationResultBuilder, IOperandBuilder operandBuilder)
            : base(operationResultBuilder, operandBuilder)
        {
        }

        public override bool IsValid(LambdaExpression expr)
        {
            if (!(expr.Body is UnaryExpression) || expr.Body.NodeType != ExpressionType.Not)
            {
                return false;
            }

            // NotIncludes is the same as Includes - with additional ! operand. So in order to check is it valid - create Includes expression with the same params
            var subExpr = (expr.Body as UnaryExpression).Operand;
            if (!base.IsValid(Expression.Lambda(subExpr, expr.Parameters)))
            {
                return false;
            }
            return ((MethodCallExpression)subExpr).Method.Name == ReflectionHelper.IncludesMethodName;
        }

        public override IOperation GetOperation(LambdaExpression expr)
        {
            if (!IsValid(expr))
            {
                throw new NonSupportedExpressionException(expr);
            }
            var fieldRefOperand = getFieldRefOperand(expr);
            var valueOperand = getValueOperand(expr);
            return new NotIncludesOperation(operationResultBuilder, fieldRefOperand, valueOperand);
        }
    }
}
