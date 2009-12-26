using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.SharePoint;

namespace Camlex.NET.Interfaces
{
    public class EqTranslator : ITranslator
    {
        public string Translate(Expression<Func<SPItem, bool>> expr)
        {
            throw new NotImplementedException();
        }
    }
}
