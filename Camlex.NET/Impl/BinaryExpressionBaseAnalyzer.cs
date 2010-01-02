using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Camlex.NET.Interfaces;
using Microsoft.SharePoint;

namespace Camlex.NET.Impl
{
    public abstract class BinaryExpressionBaseAnalyzer : IAnalyzer
    {
        public virtual bool IsValid(Expression<Func<SPItem, bool>> expr)
        {
            if (!(expr.Body is BinaryExpression))
            {
                return false;
            }
            var body = expr.Body as BinaryExpression;
            
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

        public IOperation GetOperation(Expression<Func<SPItem, bool>> expr)
        {
            throw new NotImplementedException();
        }

//        public IOperand GetLeftOperand(Expression<Func<SPItem, bool>> expr)
//        {
//            if (!this.IsValid(expr))
//            {
//                throw new NonSupportedExpressionException(expr);
//            }
//            var leftExpression = (UnaryExpression)((BinaryExpression)expr.Body).Left;
//            var fieldName = ((ConstantExpression)((MethodCallExpression)leftExpression.Operand).Arguments[0]).Value as string;
//            return new IndexerWithConstantParameterOperand(fieldName);
//        }
//
//        public IOperand GetRightOperand(Expression<Func<SPItem, bool>> expr)
//        {
//            if (!this.IsValid(expr))
//            {
//                throw new NonSupportedExpressionException(expr);
//            }
//            var rightExpression = (ConstantExpression)((BinaryExpression)expr.Body).Right;
//            return new ConstantOperand(null, (string)rightExpression.Value);
//        }
    }
}
