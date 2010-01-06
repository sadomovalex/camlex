using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using Microsoft.SharePoint;


namespace Camlex.NET
{
    class Program
    {
        static void Main(string[] args)
        {
            Camlex
                .Query()
                    .Where(x => x["Count"] != null)
                    .OrderBy(x => x["field5"] as Camlex.Asc);
        }
    }
}
