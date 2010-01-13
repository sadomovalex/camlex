using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace CamlexNET.Interfaces
{
    internal interface ITranslatorFactory
    {
        ITranslator Create(LambdaExpression expr);
    }
}