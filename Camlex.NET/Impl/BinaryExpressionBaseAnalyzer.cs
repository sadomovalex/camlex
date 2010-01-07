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
    // Base class for all binary analyzers
    public abstract class BinaryExpressionBaseAnalyzer : BaseAnalyzer
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
            if (!(body.Left is MethodCallExpression))
            {
                return false;
            }
            var leftOperand = body.Left as MethodCallExpression;
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
            if (!this.isValidRightExpressionWithStringBasedSyntax(rightExpression))
            {
                return false;
            }
            return true;
        }

        private bool isExpressionWithStringBasedSyntax(Expression rightExpression)
        {
            return (rightExpression.NodeType == ExpressionType.Convert &&
                rightExpression.Type.IsSubclassOf(typeof(BaseFieldType)));
        }

        private bool isValidExpressionWithNativeSyntax(BinaryExpression body)
        {
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
            if (!this.isValidRightExpressionWithNativeSyntax(rightExpression))
            {
                return false;
            }
            return true;
        }

        // Right expression for native syntax should be constant, variable or method call
        protected bool isValidRightExpressionWithNativeSyntax(Expression rightExpression)
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

        // Right expression for string based syntax should be constant, variable or method call
        protected bool isValidRightExpressionWithStringBasedSyntax(Expression rightExpression)
        {
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
