﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using Microsoft.SharePoint;


namespace CamlexNET.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //Camlex.Query().Where(x => (DateTime)x["Modified"] == new DateTime(2010, 1, 2, 3, 4, 5));
            //Camlex.Query().Where(x => (DateTime)x["Modified"] == new DateTime(2010, 1, 2, 3, 4, 5).IncludeTimeValue());
            //Camlex.Query().Where(x => x["Modified"] == ((DataTypes.DateTime)"02.01.2010 03:04:05"));
            //Camlex.Query().Where(x => x["Modified"] == ((DataTypes.DateTime)"02.01.2010 03:04:05").IncludeTimeValue());

            //var param = "foo";
            //Camlex.Query().Where(x => ((string)x["Count"]).Contains(param));
            //Camlex.Query().Where(x => ((DataTypes.Text)x["Count"]).Contains("foo"));
            //Camlex.Query().Where(x => ((DataTypes.Note)x["Count"]).Contains(param));
            //Camlex.Query().Where(x => ((string)x["Count"]).StartsWith("foo"));
            //Camlex.Query().Where(x => ((DataTypes.Text)x["Count"]).StartsWith(param));
            //Camlex.Query().Where(x => ((DataTypes.Note)x["Count"]).StartsWith("foo"));

            //Camlex.Query().OrderBy(x => x["field1"] as Camlex.Asc);
            //Camlex.Query().GroupBy(x => new[] { x["field1"], x["field2"] as Camlex.Asc, x["field3"] as Camlex.Desc }, true, 10);
        }
    }
}