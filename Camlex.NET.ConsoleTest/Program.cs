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
            Camlex.Where(x => x["Count"] != null);
                  //.OrderBy(x => new [] { x["field1"], x["field2"] as Camlex.Asc, x["field3"] as Camlex.Desc });
            object a = new[] {new XElement("el1"), new XElement("el2"), new XElement("el3")};
            object b = new XElement("b");
            var r = new XElement("root", b);
            string s = r.ToString();
        }
    }
}
