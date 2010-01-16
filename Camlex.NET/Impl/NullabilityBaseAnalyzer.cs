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
using System.Linq.Expressions;
using CamlexNET.Impl.Operands;
using CamlexNET.Impl.Operations.IsNotNull;
using CamlexNET.Interfaces;

namespace CamlexNET.Impl
{
    internal abstract class NullabilityBaseAnalyzer : BinaryExpressionBaseAnalyzer
    {
        protected NullabilityBaseAnalyzer(IOperationResultBuilder operationResultBuilder, IOperandBuilder operandBuilder)
            : base(operationResultBuilder, operandBuilder)
        {
        }

        public override bool IsValid(LambdaExpression expr)
        {
            // do not call base.IsValid() here as convert is not required for IsNull/IsNotNull operations
            // (i.e. x["foo"] == null, instead of (T)x["foo"] == null). Convert on lvalue is optional here

            // body should be BinaryExpression
            if (!(expr.Body is BinaryExpression))
            {
                return false;
            }
            var body = expr.Body as BinaryExpression;

            // check right expression first here

            // if right expression has string based syntax we should not evaluate
            // it for IsNull or IsNotNull
            var rightExpression = body.Right;
            if (this.isValidRightExpressionWithStringBasedSyntax(rightExpression))
            {
                return false;
            }

            if (!this.isValidRightExpressionWithNativeSyntax(rightExpression))
            {
                return false;
            }

            // IsNull/IsNotNull expression may have and may not have convert on lvalue
            // (i.e. x["foo"] == null, instead of (T)x["foo"] == null). Convert on lvalue is optional here.
            // So both syntaxes are valid for left expression here: string based (without convert) and
            // native (with convert)
            if (!this.isValidLeftExpressionWithStringBasedSyntax(body.Left) &&
                !this.isValidLeftExpressionWithNativeSyntax(body.Left))
            {
                return false;
            }

            // check that right operand is null
            var valueOperand = this.operandBuilder.CreateValueOperandForNativeSyntax(rightExpression);
            return (valueOperand is NullValueOperand);
        }

//        private bool isValidLeftExpressionWithoutCast(BinaryExpression body)
//        {
//            if (!(body.Left is MethodCallExpression))
//            {
//                return false;
//            }
//            var leftOperand = body.Left as MethodCallExpression;
//            if (leftOperand.Method.Name != ReflectionHelper.IndexerMethodName)
//            {
//                return false;
//            }
//
//            if (leftOperand.Arguments.Count != 1)
//            {
//                return false;
//            }
            // currently only constants are supported as indexer's argument
//            if (!(leftOperand.Arguments[0] is ConstantExpression))
//            {
//                return false;
//            }
//            return true;
//        }
    }
}


