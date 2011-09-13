using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace CamlexNET.UnitTests.ReverseEngeneering
{
    public static class XmlHelper
    {
        public static XElement Get(string xml)
        {
            using (var tr = new StringReader(xml))
            {
                var doc = XDocument.Load(tr);
                return doc.Descendants().First();
            }
        }
    }
}
