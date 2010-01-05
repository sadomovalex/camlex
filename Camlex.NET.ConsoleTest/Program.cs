using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Microsoft.SharePoint;


namespace Camlex.NET
{
    class Program
    {
        static void Main(string[] args)
        {
            Camlex.Where(x => x["Count"] != null);
                  //.OrderBy(x => new [] { x["field1"], x["field2"] as Camlex.Asc, x["field3"] as Camlex.Desc });
        }
    }
}
