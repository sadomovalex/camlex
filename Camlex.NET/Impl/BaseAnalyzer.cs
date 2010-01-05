using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl
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
    }
}
