using System.Linq.Expressions;
using Camlex.NET.Impl.Operands;
using Camlex.NET.Impl.Operations.IsNotNull;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl
{
    public abstract class NullabilityBaseAnalyzer : BinaryExpressionBaseAnalyzer
    {
        protected NullabilityBaseAnalyzer(IOperationResultBuilder operationResultBuilder, IOperandBuilder operandBuilder)
            : base(operationResultBuilder, operandBuilder)
        {
        }

        public override bool IsValid(LambdaExpression expr)
        {
            // do not call base.IsValid() here as there is no convert for IsNotNull operation
            // (i.e. x["foo"] != null, instead of (T)x["foo"] != null)            

            // body should be BinaryExpression
            if (!(expr.Body is BinaryExpression))
            {
                return false;
            }
            var body = expr.Body as BinaryExpression;

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

            // if right expression has string based syntax we should not evaluate
            // it for IsNull or IsNotNull
            if (this.isValidRightExpressionWithStringBasedSyntax(rightExpression))
            {
                return false;
            }

            if (!this.isValidRightExpressionWithNativeSyntax(rightExpression))
            {
                return false;
            }

            // check that right operand is null
            var valueOperand = this.operandBuilder.CreateValueOperandForNativeSyntax(rightExpression);
            return (valueOperand is NullValueOperand);
        }
    }
}


