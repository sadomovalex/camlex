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
            Camlex.Where(x => (string)x["Email"] == "test@example.com");
        }
    }
}
