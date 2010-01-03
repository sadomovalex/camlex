using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Camlex.NET.Interfaces
{
    public interface ITranslatorFactory
    {
        ITranslator CreateLogicalTranslator(ExpressionType exprType);
        ITranslator CreateArrayTranslator(ExpressionType exprType);
    }
}