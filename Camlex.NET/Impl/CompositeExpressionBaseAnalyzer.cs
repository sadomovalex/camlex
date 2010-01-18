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
using Microsoft.SharePoint;

namespace CamlexNET.Impl
{
    // Base class for AndAlso and OrElse analyzers
    internal abstract class CompositeExpressionBaseAnalyzer : BaseAnalyzer
    {
        protected IAnalyzerFactory analyzerFactory;

        protected CompositeExpressionBaseAnalyzer(IOperationResultBuilder operationResultBuilder,
            IAnalyzerFactory analyzerFactory) :
            base(operationResultBuilder)
        {
            this.analyzerFactory = analyzerFactory;
        }

        public override bool IsValid(LambdaExpression expr)
        {
            // expression should be binary expresion
            if (!(expr.Body is BinaryExpression))
            {
                return false;
            }
            var body = expr.Body as BinaryExpression;

//            // left operand should be binary expression
//            if (!(body.Left is BinaryExpression))
//            {
//                return false;
//            }
//
//            // right operand should be binary expression
//            if (!(body.Right is BinaryExpression))
//            {
//                return false;
//            }

            var lambdaParam = expr.Parameters[0];
            // check left operand
            if (!this.isExpressionValid(body.Left, lambdaParam))
            {
                return false;
            }

            // check right operand
            if (!this.isExpressionValid(body.Right, lambdaParam))
            {
                return false;
            }
            return true;
        }

        private bool isExpressionValid(Expression subExpr, ParameterExpression lambdaParam)
        {
            // make Expression<Func<SPItem, bool>> lambda expression from BinaryExpression
            var lambda = this.createLambdaFromExpression(subExpr, lambdaParam);
            var subExpressionAnalyzer = this.analyzerFactory.Create(lambda);
            return subExpressionAnalyzer.IsValid(lambda);
        }

        // For composite expressions like x => (string)x["Email"] == "test@example.com" && (int)x["Count1"] == 1
        // it creates 2 lambdas: x => (string)x["Email"] == "test@example.com" ; x => (int)x["Count1"] == 1
        private Expression<Func<SPItem, bool>> createLambdaFromExpression(Expression subExpr,
            ParameterExpression lambdaParam)
        {
            return Expression.Lambda<Func<SPItem, bool>>(subExpr, lambdaParam);
        }

        private IOperation createOperationFromExpression(Expression subExpr, ParameterExpression lambdaParam)
        {
            // make Expression<Func<SPItem, bool>> lambda expression from BinaryExpression
            var lambda = this.createLambdaFromExpression(subExpr, lambdaParam);
            var subExpressionAnalyzer = this.analyzerFactory.Create(lambda);
            return subExpressionAnalyzer.GetOperation(lambda);
        }

        protected IOperation getLeftOperation(LambdaExpression expr)
        {
            if (!this.IsValid(expr))
            {
                throw new NonSupportedExpressionException(expr);
            }
            var body = expr.Body as BinaryExpression;
            var lambdaParam = expr.Parameters[0];
            var operation = this.createOperationFromExpression(body.Left, lambdaParam);
            return operation;
        }

        protected IOperation getRightOperation(LambdaExpression expr)
        {
            if (!this.IsValid(expr))
            {
                throw new NonSupportedExpressionException(expr);
            }
            var body = expr.Body as BinaryExpression;
            var lambdaParam = expr.Parameters[0];
            var operation = this.createOperationFromExpression(body.Right, lambdaParam);
            return operation;
        }
    }
}
