using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Camlex.NET.Interfaces
{
    public class TranslatorFactory : ITranslatorFactory
    {
        public ITranslator Create(ExpressionType exprType)
        {
            if (exprType == ExpressionType.Equal)
            {
                return new EqTranslator();
            }
            throw new NotImplementedException();
        }
    }
}