using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using CamlexNET.Interfaces;
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

        public Expression Link(LambdaExpression where, LambdaExpression orderBy, LambdaExpression groupBy, LambdaExpression viewFields)
        {
            var list = new List<KeyValuePair<string, LambdaExpression>>();
            list.Add(new KeyValuePair<string, LambdaExpression>(ReflectionHelper.WhereMethodName, where));
            list.Add(new KeyValuePair<string, LambdaExpression>(ReflectionHelper.OrderByMethodName, orderBy));
            list.Add(new KeyValuePair<string, LambdaExpression>(ReflectionHelper.GroupByMethodName, groupBy));
            list.Add(new KeyValuePair<string, LambdaExpression>(ReflectionHelper.ViewFieldsMethodName, viewFields));

            if (list.All(kv => kv.Value == null))
            {
                throw new AtLeastOneCamlPartShouldNotBeEmptyException();
            }


            var queryMi = ReflectionHelper.GetMethodInfo(typeof (Camlex), ReflectionHelper.QueryMethodName);
            var queryCall = Expression.Call(queryMi);

            var expr = queryCall;
            for (int i = 0; i < list.Count; i++)
            {
                var kv = list[i];
                if (kv.Value != null)
                {
                    var mi = this.getMethodInfo(kv.Key);
                    expr = Expression.Call(expr, mi, kv.Value);
                }
            }
            return expr;
        }

        private MethodInfo getMethodInfo(string methodName)
        {
            if (methodName == ReflectionHelper.WhereMethodName)
            {
                return ReflectionHelper.GetMethodInfo(typeof (IQuery), methodName);
            }
            throw new NotImplementedException();
        }
    }
}
