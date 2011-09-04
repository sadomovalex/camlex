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
            return translator.Translate(input);
        }
    }
}
