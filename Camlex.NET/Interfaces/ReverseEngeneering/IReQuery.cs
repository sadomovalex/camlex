using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace CamlexNET.Interfaces.ReverseEngeneering
{
    public interface IReQuery
    {
        Expression ToExpression();
    }
}
