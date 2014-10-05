using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Linq;
using CamlexNET.Impl.Helpers;
using CamlexNET.Interfaces;
using CamlexNET.Interfaces.ReverseEngeneering;
using Microsoft.SharePoint;

namespace CamlexNET.Impl
{
    internal class Join : IJoin
    {
        private readonly ITranslatorFactory translatorFactory;
        private List<XElement> joins;

        public Join(ITranslatorFactory translatorFactory, List<XElement> joins)
        {
            this.translatorFactory = translatorFactory;
            this.joins = joins;
        }

        public IJoin Left(Expression<Func<SPListItem, object>> expr, string foreignListAlias)
        {
            return Left(expr, string.Empty, foreignListAlias);
        }

        public IJoin Left(Expression<Func<SPListItem, object>> expr, string primaryListAlias, string foreignListAlias)
        {
            return join(expr, primaryListAlias, foreignListAlias, JoinType.Left);
        }

        private IJoin join(Expression<Func<SPListItem, object>> expr, string primaryListAlias, string foreignListAlias, JoinType type)
        {
            var translator = translatorFactory.Create(expr);
            var join = translator.TranslateJoin(expr, type, primaryListAlias, foreignListAlias);
            this.joins.Add(join);
            return this;
        }

        public IJoin Inner(Expression<Func<SPListItem, object>> expr, string foreignListAlias)
        {
            return Inner(expr, string.Empty, foreignListAlias);
        }

        public IJoin Inner(Expression<Func<SPListItem, object>> expr, string primaryListAlias, string foreignListAlias)
        {
            return join(expr, primaryListAlias, foreignListAlias, JoinType.Inner);
        }
    }
}
