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
using CamlexNET.Impl.Helpers;
using CamlexNET.Impl.Operands;
using CamlexNET.Interfaces;
using Microsoft.SharePoint;

namespace CamlexNET.Impl
{
    // Base class for all binary analyzers
    internal abstract class BinaryExpressionBaseAnalyzer : BaseAnalyzer
    {
        protected IOperandBuilder operandBuilder;

        protected BinaryExpressionBaseAnalyzer(IOperationResultBuilder operationResultBuilder,
            IOperandBuilder operandBuilder) :
            base(operationResultBuilder)
        {
            this.operandBuilder = operandBuilder;
        }

        public override bool IsValid(LambdaExpression expr)
        {
            // body should be BinaryExpression
            if (!(expr.Body is BinaryExpression))
            {
                return false;
            }
            var body = expr.Body as BinaryExpression;

            if (this.isExpressionWithStringBasedSyntax(body.Right))
            {
                // operands are of types - subclasses of BaseFieldType
                if (!this.isValidExpressionWithStringBasedSyntax(body))
                {
                    return false;
                }
            }
            else
            {
                // operands are of native types like int, bool, DateTime
                if (!this.isValidExpressionWithNativeSyntax(body))
                {
                    return false;
                }
            }

            return true;
        }

        private bool isValidExpressionWithStringBasedSyntax(BinaryExpression body)
        {
            // left operand for string based syntax should be indexer call
            var leftExpression = body.Left;
            if (!this.isValidLeftExpressionWithStringBasedSyntax(leftExpression))
            {
                return false;
            }

            // right expression should be constant, variable or method call
            var rightExpression = body.Right;
            if (!this.isValidRightExpressionWithStringBasedSyntax(rightExpression))
            {
                return false;
            }
            return true;
        }

        protected bool isValidLeftExpressionWithStringBasedSyntax(Expression leftExpression)
        {
            if (!(leftExpression is MethodCallExpression))
            {
                return false;
            }
            var leftOperand = leftExpression as MethodCallExpression;
            if (leftOperand.Method.Name != ReflectionHelper.IndexerMethodName)
            {
                return false;
            }

            if (leftOperand.Arguments.Count != 1)
            {
                return false;
            }

            // parameter of indexer can be constant, variable or method call
            var argumentExpression = leftOperand.Arguments[0];
            if (!this.isValidEvaluableExpression(argumentExpression))
            {
                return false;
            }

            // type of argument expression should be string or guid
            return (argumentExpression.Type == typeof(string) || argumentExpression.Type == typeof(Guid));
        }

        private bool isExpressionWithStringBasedSyntax(Expression rightExpression)
        {
            // it is for case when right expression is a method call to IncludeTimeValue method
            rightExpression = ExpressionsHelper.RemoveIncludeTimeValueMethodCallIfAny(rightExpression);

            return (rightExpression.NodeType == ExpressionType.Convert &&
                rightExpression.Type.IsSubclassOf(typeof(BaseFieldType)));
        }

        private bool isValidExpressionWithNativeSyntax(BinaryExpression body)
        {
            // left operand should be unary expression (Convert of indexer - like (string)x["foo"])
            var leftExpression = body.Left;
            if (!this.isValidLeftExpressionWithNativeSyntax(leftExpression))
            {
                return false;
            }

            // right expression can be constant, variable or method call
            var rightExpression = body.Right;
            if (!this.isValidRightExpressionWithNativeSyntax(rightExpression))
            {
                return false;
            }
            return true;
        }

        protected bool isValidLeftExpressionWithNativeSyntax(Expression leftExpression)
        {
            if (!(leftExpression is UnaryExpression))
            {
                return false;
            }
            var left = leftExpression as UnaryExpression;
            
            if (left.NodeType != ExpressionType.Convert)
            {
                return false;
            }

//            if (!(left.Operand is MethodCallExpression))
//            {
//                return false;
//            }
//            var leftOperand = left.Operand as MethodCallExpression;
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
            return this.isValidLeftExpressionWithStringBasedSyntax(left.Operand);
        }

        // Right expression for native syntax should be constant, variable or method call
        protected bool isValidRightExpressionWithNativeSyntax(Expression rightExpression)
        {
            return this.isValidEvaluableExpression(rightExpression);
        }

        // Right expression for string based syntax should be constant, variable or method call
        protected bool isValidRightExpressionWithStringBasedSyntax(Expression rightExpression)
        {
            // it is for case when right expression is a method call to IncludeTimeValue method
            rightExpression = ExpressionsHelper.RemoveIncludeTimeValueMethodCallIfAny(rightExpression);

            // 1st convertion is conversion to specific subclass of BaseFieldType
            if (!(rightExpression is UnaryExpression))
            {
                return false;
            }
            if (rightExpression.NodeType != ExpressionType.Convert)
            {
                return false;
            }
            if (!rightExpression.Type.IsSubclassOf(typeof(BaseFieldType)))
            {
                return false;
            }

            // 2nd convertion is conversion to BaseFieldType
            var operandExpression = ((UnaryExpression)rightExpression).Operand;
            if (!(operandExpression is UnaryExpression))
            {
                return false;
            }
            if (operandExpression.NodeType != ExpressionType.Convert)
            {
                return false;
            }
            if (operandExpression.Type != typeof(BaseFieldType))
            {
                return false;
            }

            var expr = ((UnaryExpression)operandExpression).Operand;

            // operand should be valid native expression
            if (!this.isValidRightExpressionWithNativeSyntax(expr))
            {
                return false;
            }

            // type of casted expression should be string (althoug compiler will not
            // allow to cast to subclass of BaseFieldType from anything except string - because
            // BaseFieldType has explicit conversion operator only for string, we need to do this
            // because there is possibility to cast from BaseFieldType to any subclass)
            return (expr.Type == typeof (string));
        }

        // Some info from value operand can be required for properly initialization of field ref operand
        // (e.g. if value operand is lookup id we need to add LookupId="True" attribute to field ref operand)
        protected IOperand getFieldRefOperand(LambdaExpression expr, IOperand valueOperand)
        {
            if (!this.IsValid(expr))
            {
                throw new NonSupportedExpressionException(expr);
            }
            var body = expr.Body as BinaryExpression;
            return this.operandBuilder.CreateFieldRefOperand(body.Left, valueOperand);
        }

        protected IOperand getValueOperand(LambdaExpression expr)
        {
            if (!this.IsValid(expr))
            {
                throw new NonSupportedExpressionException(expr);
            }
            var body = expr.Body as BinaryExpression;
            if (this.isValidRightExpressionWithNativeSyntax(body.Right))
            {
                return this.operandBuilder.CreateValueOperandForNativeSyntax(body.Right);
            }
            if (this.isValidRightExpressionWithStringBasedSyntax(body.Right))
            {
                return this.operandBuilder.CreateValueOperandForStringBasedSyntax(body.Right);
            }
            throw new NonSupportedExpressionException(body.Right);
        }

        protected IOperation getOperation<T>(LambdaExpression expr,
            Func<IOperationResultBuilder, IOperand, IOperand, T> creator)
            where T : IOperation
        {
            if (!this.IsValid(expr))
            {
                throw new NonSupportedExpressionException(expr);
            }
            var valueOperand = this.getValueOperand(expr);
            var fieldRefOperand = this.getFieldRefOperand(expr, valueOperand);
            return creator(this.operationResultBuilder, fieldRefOperand, valueOperand);
        }
    }
}
