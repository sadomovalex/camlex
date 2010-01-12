using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CamlexNET.Interfaces;

namespace CamlexNET.Impl
{
    // Base class for all analyzers
    public abstract class BaseAnalyzer : IAnalyzer
    {
        protected IOperationResultBuilder operationResultBuilder;

        protected BaseAnalyzer(IOperationResultBuilder operationResultBuilder)
        {
            this.operationResultBuilder = operationResultBuilder;
        }

        public abstract bool IsValid(LambdaExpression expr);
        public abstract IOperation GetOperation(LambdaExpression expr);

        protected bool isValidEvaluableExpression(Expression expr)
        {
//            if (expr is ConstantExpression)
//            {
//                return true;
//            }
//            if (expr is MemberExpression/* && ((MemberExpression)rightExpression).Expression is ConstantExpression*/)
//            {
//                return true;
//            }
//            if (expr is MethodCallExpression/* && ((MethodCallExpression)rightExpression).Object is ConstantExpression*/)
//            {
//                return true;
//            }
//            if (expr is InvocationExpression)
//            {
//                return true;
//            }
//            if (expr is NewExpression)
//            {
//                return true;
//            }
//            if (expr is ConditionalExpression)
//            {
//                return true;
//            }
//            return false;
            return (!expr.Type.IsSubclassOf(typeof(BaseFieldType)));
        }
    }
}
