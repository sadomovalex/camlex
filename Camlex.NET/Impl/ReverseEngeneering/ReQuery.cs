using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CamlexNET.Interfaces.ReverseEngeneering;

namespace CamlexNET.Impl.ReverseEngeneering
{
    internal class ReQuery : IReQuery
    {
        private readonly IReTranslatorFactory translatorFactory;
        private readonly string input;

        public ReQuery(IReTranslatorFactory translatorFactory, string input)
        {
            this.translatorFactory = translatorFactory;
            this.input = input;
        }

        public Expression ToExpression()
        {
            var translator = this.translatorFactory.Create(input);
            var where = translator.TranslateWhere();
            var orderBy = translator.TranslateOrderBy();
            var groupBy = translator.TranslateGroupBy();
            var viewFields = translator.TranslateViewFields();

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
