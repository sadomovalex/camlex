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
    internal class ProjectedField : IProjectedField
    {
        private readonly ITranslatorFactory translatorFactory;
        private List<XElement> fields;

        public ProjectedField(ITranslatorFactory translatorFactory, List<XElement> fields)
        {
            this.translatorFactory = translatorFactory;
            this.fields = fields;
        }

        public IProjectedField Field(Expression<Func<SPListItem, object>> expr)
        {
            var translator = translatorFactory.Create(expr);
            var field = translator.TranslateProjectedField(expr);
            this.fields.Add(field);
            return this;
        }

        public override string ToString()
        {
            return this.ToString(false);
        }

        public string ToString(bool includeParentTag)
        {
            var elements = this.ToCaml(includeParentTag);
            return ConvertHelper.ConvertToString(elements);
        }

        public XElement[] ToCaml(bool includeParentTag)
        {
            if (includeParentTag)
            {
                return new[] { new XElement(Tags.ProjectedFields, this.fields) };
            }
            return this.fields.ToArray();
        }
    }
}
