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

        private IReTranslator create(XElement el)
        {
            var analyzer = this.analyzerFactory.Create(el);
            return new ReTranslatorFromCaml(analyzer);
        }

        private IReTranslator createForTag(string input, string tag)
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

                    var el = doc.Descendants().FirstOrDefault(x => x.Name == tag);
                    if (el == null)
                    {
                        return null;
                    }
                    return this.create(el);
                }
            }
            catch (XmlException)
            {
                throw new XmlNotWellFormedException();
            }
        }

        public IReTranslator CreateForWhere(string input)
        {
            return this.createForTag(input, Tags.Where);
        }

        public IReTranslator CreateForOrderBy(string input)
        {
            return this.createForTag(input, Tags.OrderBy);
        }

        public IReTranslator CreateForGroupBy(string input)
        {
            return this.createForTag(input, Tags.GroupBy);
        }

        public IReTranslator CreateForViewFields(string input)
        {
            return this.createForTag(input, Tags.ViewFields);
        }
    }
}
