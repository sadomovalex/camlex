using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Camlex.NET.Interfaces;
using Microsoft.SharePoint;

namespace Camlex.NET.Impl.Array
{
    public class ArrayAnalyzer : IAnalyzer
    {
        private IOperandBuilder _operandBuilder;

        public ArrayAnalyzer(IOperandBuilder operandBuilder)
        {
            _operandBuilder = operandBuilder;
        }

        public bool IsValid(Expression<Func<SPItem, bool>> expr)
        {
            var body = expr.Body as NewArrayExpression;
            if (body == null) return false;
            //body.Expressions.ToList().ForEach(x =>
            //{
            //    if(x is Expr)
            //});

            return false;
        }

        public IOperation GetOperation(Expression<Func<SPItem, bool>> expr)
        {
            throw new NotImplementedException();
        }

        private IOperand[] getFieldRefOperands(Expression<Func<SPItem, object>> expr)
        {
            return null;
        }
    }
}
