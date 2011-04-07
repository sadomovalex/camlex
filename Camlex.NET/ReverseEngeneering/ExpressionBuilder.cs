using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
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
            throw new NotImplementedException();
        }
    }
}
