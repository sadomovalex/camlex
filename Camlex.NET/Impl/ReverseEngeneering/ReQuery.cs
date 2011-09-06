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
        private IReTranslatorFactory translatorFactory;
        private IReLinkerFactory linkerFactory;
        private string input;

        public ReQuery(IReTranslatorFactory translatorFactory, IReLinkerFactory linkerFactory, string input)
        {
            this.translatorFactory = translatorFactory;
            this.linkerFactory = linkerFactory;
            this.input = input;
        }

        public Expression ToExpression()
        {
            var translator = this.translatorFactory.Create(input);
            var where = translator.TranslateWhere();
            var orderBy = translator.TranslateOrderBy();
            var groupBy = translator.TranslateGroupBy();
            var viewFields = translator.TranslateViewFields();

            var linker = this.linkerFactory.Create(translator);
            return linker.Link(where, orderBy, groupBy, viewFields);
        }
    }

}
