using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl
{
    public abstract class UnaryExpressionBaseAnalyzer : BaseAnalyzer
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

            // --- check for object ---

            // left operand for string based syntax should be indexer call
            if (!(body.Object is UnaryExpression))
            {
                return false;
            }
            var methodCallExpression = ((UnaryExpression)body.Object).Operand;
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
            // currently only constants are supported as indexer's argument
            if (!(objectExpression.Arguments[0] is ConstantExpression))
            {
                return false;
            }

            // --- check for function ---

            // right expression should be constant, variable or method call
            if (body.Arguments == null || body.Arguments.Count != 1)
            {
                return false;
            }
            var parameterExpression = body.Arguments[0];
            if (parameterExpression is ConstantExpression)
            {
                return true;
            }
            if (parameterExpression is MemberExpression/* && ((MemberExpression)rightExpression).Expression is ConstantExpression*/)
            {
                return true;
            }
            if (parameterExpression is MethodCallExpression/* && ((MethodCallExpression)rightExpression).Object is ConstantExpression*/)
            {
                return true;
            }
            if (parameterExpression is InvocationExpression)
            {
                return true;
            }
            return false;
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
            return operandBuilder.CreateFieldRefOperand(body.Object);
        }

        protected IOperand getValueOperand(LambdaExpression expr)
        {
            if (!IsValid(expr))
            {
                throw new NonSupportedExpressionException(expr);
            }
            var body = expr.Body as MethodCallExpression;
            var valueType = body.Object.Type;
            var parameterExpression = body.Arguments[0];
            return operandBuilder.CreateValueOperandForNativeSyntax(parameterExpression, valueType);
        }
    }
}
