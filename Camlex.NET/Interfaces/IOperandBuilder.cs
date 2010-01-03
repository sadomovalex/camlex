using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Camlex.NET.Interfaces
{
    public interface IOperandBuilder
    {
        IOperand CreateFieldRefOperand(Expression expr);
        IOperand CreateFieldRefOperandWithOrdering(Expression expr, Camlex.OrderDirection orderDirection);
        IOperand CreateValueOperand(Expression expr);
    }
}
