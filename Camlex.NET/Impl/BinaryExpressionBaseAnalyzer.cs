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

        public virtual bool IsValid(Expression<Func<SPItem, bool>> expr)
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

            // currently only constants are supported as right operand
            if (!(body.Right is ConstantExpression))
            {
                return false;
            }

            return true;
        }

        public abstract IOperation GetOperation(Expression<Func<SPItem, bool>> expr);

        protected IOperand getFieldRefOperand(Expression<Func<SPItem, bool>> expr)
        {
            if (!this.IsValid(expr))
            {
                throw new NonSupportedExpressionException(expr);
            }
            var body = expr.Body as BinaryExpression;
            return this.operandBuilder.CreateFieldRefOperand(body.Left);
        }

        protected IOperand getValueOperand(Expression<Func<SPItem, bool>> expr)
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
