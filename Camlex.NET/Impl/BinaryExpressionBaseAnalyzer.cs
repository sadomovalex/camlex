using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Camlex.NET.Impl.Operands;
using Camlex.NET.Interfaces;
using Microsoft.SharePoint;

namespace Camlex.NET.Impl
{
    // Base class for all analyzers
    public abstract class BinaryExpressionBaseAnalyzer : IAnalyzer
    {
        private IOperandBuilder operandBuilder;

        protected BinaryExpressionBaseAnalyzer(IOperandBuilder operandBuilder)
        {
            this.operandBuilder = operandBuilder;
        }

        public virtual bool IsValid(LambdaExpression expr)
        {
            // body should be BinaryExpression
            if (!(expr.Body is BinaryExpression))
            {
                return false;
            }
            var body = expr.Body as BinaryExpression;
            
            // left operand should be unary expression (Convert of indexer - like (string)x["foo"])
            if (!(body.Left is UnaryExpression))
            {
                return false;
            }
            var left = body.Left as UnaryExpression;
            
            if (left.NodeType != ExpressionType.Convert)
            {
                return false;
            }

            if (!(left.Operand is MethodCallExpression))
            {
                return false;
            }
            var leftOperand = left.Operand as MethodCallExpression;
            if (leftOperand.Method.Name != ReflectionHelper.IndexerMethodName)
            {
                return false;
            }

            if (leftOperand.Arguments.Count != 1)
            {
                return false;
            }
            // currently only constants are supported as indexer's argument
            if (!(leftOperand.Arguments[0] is ConstantExpression))
            {
                return false;
            }

            // right expression should be constant, variable or method call
            var rightExpression = body.Right;
            if (!this.isValidRightExpression(rightExpression))
            {
                return false;
            }

            return true;
        }

        // Right expression should be constant, variable or method call
        private bool isValidRightExpression(Expression rightExpression)
        {
            if (rightExpression is ConstantExpression)
            {
                return true;
            }
            if (rightExpression is MemberExpression/* && ((MemberExpression)rightExpression).Expression is ConstantExpression*/)
            {
                return true;
            }
            if (rightExpression is MethodCallExpression/* && ((MethodCallExpression)rightExpression).Object is ConstantExpression*/)
            {
                return true;
            }
            if (rightExpression is InvocationExpression)
            {
                return true;
            }
            return false;
        }

        public abstract IOperation GetOperation(LambdaExpression expr);

        protected IOperand getFieldRefOperand(LambdaExpression expr)
        {
            if (!this.IsValid(expr))
            {
                throw new NonSupportedExpressionException(expr);
            }
            var body = expr.Body as BinaryExpression;
            return this.operandBuilder.CreateFieldRefOperand(body.Left);
        }

        protected IOperand getValueOperand(LambdaExpression expr)
        {
            if (!this.IsValid(expr))
            {
                throw new NonSupportedExpressionException(expr);
            }
            var body = expr.Body as BinaryExpression;
            return this.operandBuilder.CreateValueOperand(body.Right);
        }
    }
}
