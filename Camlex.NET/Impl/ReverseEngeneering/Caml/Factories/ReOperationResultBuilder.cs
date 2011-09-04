using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CamlexNET.Interfaces.ReverseEngeneering;

namespace CamlexNET.Impl.ReverseEngeneering.Caml.Factories
{
    internal class ReOperationResultBuilder : IReOperationResultBuilder
    {
        public IReOperationResult CreateResult(Expression expr)
        {
            throw new NotImplementedException();
        }

        public IReOperationResult CreateResult(Expression[] exprs)
        {
            throw new NotImplementedException();
        }
    }
}
