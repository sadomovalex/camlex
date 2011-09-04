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
            var translatorForWhere = this.translatorFactory.CreateForWhere(input);
            var translatorForOrderBy = this.translatorFactory.CreateForOrderBy(input);
            var translatorForGroupBy = this.translatorFactory.CreateForGroupBy(input);
            var translatorForViewFields = this.translatorFactory.CreateForViewFields(input);

            if (translatorForWhere == null && translatorForOrderBy == null &&
                translatorForGroupBy == null && translatorForViewFields == null)
            {
                throw new Exception("All parts are empty (Where, OrderBy, GroupBy, ViewFields). At least one part should be non-empty");
            }
            
            // todo: merge expressions from all translators
            var where = this.getExpression(translatorForWhere);
            var orderBy = this.getExpression(translatorForOrderBy);
            var groupBy = this.getExpression(translatorForGroupBy);
            var viewFields = this.getExpression(translatorForViewFields);

            throw new NotImplementedException();
        }

        private Expression getExpression(IReTranslator translator)
        {
            if (translator == null)
            {
                return null;
            }
            return translator.Translate();
        }
    }
}
