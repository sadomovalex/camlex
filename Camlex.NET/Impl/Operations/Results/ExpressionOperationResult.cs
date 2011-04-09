using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CamlexNET.Interfaces;

namespace CamlexNET.Impl.Operations.Results
{
    public class ExpressionOperationResult : IOperationResult
    {
        protected Expression expression;

        public ExpressionOperationResult()
        {}

        public ExpressionOperationResult(Expression expression)
        {
            this.expression = expression;
        }

        public object Value
        {
            get { return this.expression; }
        }
    }
}
