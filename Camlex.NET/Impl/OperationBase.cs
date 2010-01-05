using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl
{
    public abstract class OperationBase : IOperation
    {
        protected IOperationResultBuilder operationResultBuilder;

        protected OperationBase(IOperationResultBuilder operationResultBuilder)
        {
            this.operationResultBuilder = operationResultBuilder;
        }

        public abstract IOperationResult ToResult();
    }
}


