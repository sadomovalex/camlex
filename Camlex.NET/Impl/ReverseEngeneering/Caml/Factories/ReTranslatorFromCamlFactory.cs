using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using CamlexNET.Interfaces.ReverseEngeneering;

namespace CamlexNET.Impl.ReverseEngeneering.Caml.Factories
{
    internal class ReTranslatorFromCamlFactory : IReTranslatorFactory
    {
        private readonly IReAnalyzerFactory analyzerFactory;

        public ReTranslatorFromCamlFactory(IReAnalyzerFactory analyzerFactory)
        {
            this.analyzerFactory = analyzerFactory;
        }

        public IReTranslator Create(string input)
        {
            var where = this.createForTag(input, Tags.Where);
            var orderBy = this.createForTag(input, Tags.OrderBy);
            var groupBy = this.createForTag(input, Tags.GroupBy);
            var viewFields = this.createForTag(input, Tags.ViewFields);

            var analyzerForWhere = this.analyzerFactory.Create(where);
            var analyzerForOrderBy = this.analyzerFactory.Create(orderBy);
            var analyzerForGroupBy = this.analyzerFactory.Create(groupBy);
            var analyzerForViewFields = this.analyzerFactory.Create(viewFields);

            return new ReTranslatorFromCaml(analyzerForWhere, analyzerForOrderBy, analyzerForGroupBy, analyzerForViewFields);
        }

        private XElement createForTag(string input, string tag)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(input.Trim()))
            {
                throw new ArgumentNullException(
                    "Input string is null or empty. In order to get expression from string it should be non-empty");
            }

            if (string.IsNullOrEmpty(tag))
            {
                throw new ArgumentNullException("Tag name is empty");
            }

            try
            {
                using (var tr = new StringReader(input))
                {
                    var doc = XDocument.Load(tr);

                    if (doc.Elements().Count() != 1 || doc.Elements().First().Name != Tags.Query)
                    {
                        throw new XmlNotWellFormedException();
                    }

                    return doc.Descendants().FirstOrDefault(x => x.Name == tag);
                }
            }
            catch (XmlException)
            {
                throw new XmlNotWellFormedException();
            }
        }
    }
}
