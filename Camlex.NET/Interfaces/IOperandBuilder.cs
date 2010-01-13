using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace CamlexNET.Interfaces
{
    internal interface IOperandBuilder
    {
        IOperand CreateFieldRefOperand(Expression expr);
        IOperand CreateFieldRefOperandWithOrdering(Expression expr, Camlex.OrderDirection orderDirection);
        IOperand CreateValueOperandForNativeSyntax(Expression expr);
        IOperand CreateValueOperandForNativeSyntax(Expression expr, Type explicitOperandType);
        IOperand CreateValueOperandForStringBasedSyntax(Expression expr);
    }
}
