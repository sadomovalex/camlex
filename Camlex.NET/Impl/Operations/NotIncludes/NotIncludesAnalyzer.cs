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
using CamlexNET.Impl.Operations.Includes;
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
            // because of some reason sometimes !x[].Includes() is interpreted as x[].Includes() == False and sometimes as Not(x[])
            if (!((expr.Body is UnaryExpression && expr.Body.NodeType == ExpressionType.Not) ||
                (expr.Body is BinaryExpression && expr.Body.NodeType == ExpressionType.Equal && ((BinaryExpression)expr.Body).Left is MethodCallExpression &&
                ((MethodCallExpression)((BinaryExpression)expr.Body).Left).Method.Name == ReflectionHelper.IncludesMethodName &&
                ((BinaryExpression)expr.Body).Right is ConstantExpression && (bool)((ConstantExpression)((BinaryExpression)expr.Body).Right).Value == false)))
            {
                return false;
            }

            var subExpr = this.getSubExpr(expr);
            if (subExpr == null)
            {
                return false;
            }

            // NotIncludes is the same as Includes but with !. So create Includes expression and validate it
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

//            var fieldRefOperand = getFieldRefOperand(expr);
//            var valueOperand = getValueOperand(expr);
//            return new NotIncludesOperation(operationResultBuilder, fieldRefOperand, valueOperand);
            var subExpr = this.getSubExpr(expr);
            var includesAnalyzer = new IncludesAnalyzer(this.operationResultBuilder, this.operandBuilder);
            var includesOperation = (IncludesOperation)includesAnalyzer.GetOperation(Expression.Lambda(subExpr, expr.Parameters));
            return new NotIncludesOperation(operationResultBuilder, includesOperation.FieldRefOperand, includesOperation.ValueOperand);
        }

//        protected override IOperand getFieldRefOperand(LambdaExpression expr)
//        {
//            if (!IsValid(expr))
//            {
//                throw new NonSupportedExpressionException(expr);
//            }
//            var subExpr = this.getSubExpr(expr);
//            return base.getFieldRefOperand(Expression.Lambda(subExpr, expr.Parameters));
//        }
//
//        protected override IOperand getValueOperand(LambdaExpression expr)
//        {
//            if (!IsValid(expr))
//            {
//                throw new NonSupportedExpressionException(expr);
//            }
//            var subExpr = (expr.Body as UnaryExpression).Operand;
//            return base.getValueOperand(Expression.Lambda(subExpr, expr.Parameters));
//        }

        private Expression getSubExpr(LambdaExpression expr)
        {
            Expression subExpr = null;
            if (expr.Body is UnaryExpression && expr.Body.NodeType == ExpressionType.Not)
            {
                // NotIncludes is the same as Includes - with additional ! operand. So create Includes expression with the same params and reuse base code
                subExpr = (expr.Body as UnaryExpression).Operand;
            }
            else
            {
                subExpr = ((BinaryExpression)expr.Body).Left;
            }

            return subExpr;
        }
    }
}
