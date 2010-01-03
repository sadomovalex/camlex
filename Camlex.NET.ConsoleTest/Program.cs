using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Camlex.NET
{
    class Program
    {
        static void Main(string[] args)
        {
            Camlex.Where(x => (string) x["Email"] == "test@example.com" &&
                              (int)x["Count1"] == 1 && (int)x["Count1"] == 1)
                  .OrderBy(x => new [] { x["field1"], x["field2"] as Camlex.Asc, x["field3"] as Camlex.Desc });
        }
    }
}
