using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;


namespace Camlex.NET
{
    // todo: temp
    public class SPItem
    {
        public object this[string fieldName]
        {
            get { return ""; }
            set {}
        }
    }

    public static class Camlex
    {
        public static string Where(Expression<Func<SPItem, bool>> expression)
        {
            return "";
        }
    }
}
