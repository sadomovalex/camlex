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

namespace CamlexNET.Impl
{
    internal abstract class UnaryExpressionBaseAnalyzer : BaseAnalyzer
    {
        protected IOperandBuilder operandBuilder;

        protected UnaryExpressionBaseAnalyzer(IOperationResultBuilder operationResultBuilder, IOperandBuilder operandBuilder)
            : base(operationResultBuilder)
        {
            this.operandBuilder = operandBuilder;
        }

        public override bool IsValid(LambdaExpression expr)
        {
            // body should be MethodCallExpression
            if (!(expr.Body is MethodCallExpression))
            {
                return false;
            }
            var body = expr.Body as MethodCallExpression;

            if (body.NodeType != ExpressionType.Call)
            {
                return false;
            }

            Expression obj = null;
            if (body.Object != null)
            {
                obj = body.Object;
            }
            else
            {
                // "Includes" extension method
                if (body.Method.Name != ReflectionHelper.IncludesMethodName)
                {
                    return false;
                }
                if (body.Arguments.Count != 2 && body.Arguments.Count != 3)
                {
                    return false;
                }

                obj = body.Arguments[0];
            }

            // --- check for object ---
            // left operand for string based syntax should be convert of indexer
            if (!(obj is UnaryExpression) || obj.NodeType != ExpressionType.Convert)
            {
                return false;
            }

            var methodCallExpression = ((UnaryExpression) obj).Operand;
            if (!(methodCallExpression is MethodCallExpression))
            {
                return false;
            }

            var objectExpression = (MethodCallExpression) methodCallExpression;
            if (objectExpression.Method.Name != ReflectionHelper.IndexerMethodName)
            {
                return false;
            }

            if (objectExpression.Arguments.Count != 1)
            {
                return false;
            }

            // currently only constants are supported as indexer's argument
//            if (!(objectExpression.Arguments[0] is ConstantExpression))
//            {
//                return false;
//            }
            if (!this.isValidEvaluableExpression(objectExpression.Arguments[0]))
            {
                return false;
            }

            // --- check for function ---

            // right expression should be constant, variable or method call
            Expression parameterExpression = null;
            if (body.Object != null)
            {
                if (body.Arguments.Count != 1)
                {
                    return false;
                }

                parameterExpression = body.Arguments[0];
            }
            else
            {
                // "Includes" extension method
                if (body.Method.Name != ReflectionHelper.IncludesMethodName)
                {
                    return false;
                }
                if (body.Arguments.Count != 2 && body.Arguments.Count != 3)
                {
                    return false;
                }

                parameterExpression = body.Arguments[1];
            }

//            if (parameterExpression is ConstantExpression)
//            {
//                return true;
//            }
//            if (parameterExpression is MemberExpression/* && ((MemberExpression)rightExpression).Expression is ConstantExpression*/)
//            {
//                return true;
//            }
//            if (parameterExpression is MethodCallExpression/* && ((MethodCallExpression)rightExpression).Object is ConstantExpression*/)
//            {
//                return true;
//            }
//            if (parameterExpression is InvocationExpression)
//            {
//                return true;
//            }
//            return false;
            if (!this.isValidEvaluableExpression(parameterExpression))
            {
                return false;
            }

            return true;
        }

        //private bool isExpressionBasedOnCustomTypes(Expression objectExpression)
        //{
        //    return (objectExpression.NodeType == ExpressionType.Convert &&
        //        objectExpression.Type.IsSubclassOf(typeof(BaseFieldType)));
        //}

        //-----------------------------------

        protected IOperand getFieldRefOperand(LambdaExpression expr)
        {
            if (!IsValid(expr))
            {
                throw new NonSupportedExpressionException(expr);
            }
            var body = expr.Body as MethodCallExpression;
            Expression obj = null;
            if (body.Object != null)
            {
                obj = body.Object;
            }
            else
            {
                obj = body.Arguments[0];
            }
            return operandBuilder.CreateFieldRefOperand(obj, null);
        }

        protected IOperand getValueOperand(LambdaExpression expr)
        {
            if (!IsValid(expr))
            {
                throw new NonSupportedExpressionException(expr);
            }
            var body = expr.Body as MethodCallExpression;
            Expression obj = null;
            if (body.Object != null)
            {
                obj = body.Object;
            }
            else
            {
                obj = body.Arguments[0];
            }
            var valueType = obj.Type;
            var parameterExpression = body.Arguments[0];
            return operandBuilder.CreateValueOperandForNativeSyntax(parameterExpression, valueType);
        }
    }
}
