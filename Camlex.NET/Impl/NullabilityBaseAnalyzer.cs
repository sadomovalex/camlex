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


