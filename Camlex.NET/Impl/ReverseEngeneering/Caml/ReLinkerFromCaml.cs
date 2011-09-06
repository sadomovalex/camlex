using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Linq;
using CamlexNET.Interfaces.ReverseEngeneering;

namespace CamlexNET.Impl.ReverseEngeneering.Caml
{
    internal class ReLinkerFromCaml : IReLinker
    {
        private XElement where;
        private XElement orderBy;
        private XElement groupBy;
        private XElement viewFields;

        public ReLinkerFromCaml(XElement where, XElement orderBy, XElement groupBy, XElement viewFields)
        {
            this.where = where;
            this.viewFields = viewFields;
            this.groupBy = groupBy;
            this.orderBy = orderBy;
        }

        public Expression Link(Expression where, Expression orderBy, Expression groupBy, Expression viewFields)
        {
            string firstMethodName = this.getFirstMethodName(where, orderBy, groupBy, viewFields);
            if (string.IsNullOrEmpty(firstMethodName))
            {
                throw new AtLeastOneCamlPartShouldNotBeEmptyException();
            }
            throw new NotImplementedException();
        }

        private string getFirstMethodName(Expression where, Expression orderBy, Expression groupBy, Expression viewFields)
        {
            if (where != null)
            {
                return ReflectionHelper.WhereMethodName;
            }
            if (orderBy != null)
            {
                return ReflectionHelper.OrderByMethodName;
            }
            if (groupBy != null)
            {
                return ReflectionHelper.GroupByMethodName;
            }
            if (viewFields != null)
            {
                return ReflectionHelper.ViewFieldsMethodName;
            }
            return null;
        }
    }
}
