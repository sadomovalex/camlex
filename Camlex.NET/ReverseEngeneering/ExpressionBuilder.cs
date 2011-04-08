using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Microsoft.SharePoint;

namespace CamlexNET.ReverseEngeneering
{
    public static class ExpressionBuilder
    {
        public static Expression<Func<SPListItem, bool>> BuildFromXml(string xml)
        {
            if (string.IsNullOrEmpty(xml) || string.IsNullOrEmpty(xml.Trim()))
            {
                throw new ArgumentNullException("xml");
            }
            try
            {
                using (var tr = new StringReader(xml))
                {
                    var doc = XDocument.Load(tr);
                }
            }
            catch (XmlException)
            {
                throw new XmlNotWellFormedException();
            }
            throw new NotImplementedException();
        }
    }
}
