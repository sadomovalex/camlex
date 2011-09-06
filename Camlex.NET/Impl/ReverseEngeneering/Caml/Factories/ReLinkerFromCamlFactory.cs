using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using CamlexNET.Interfaces.ReverseEngeneering;

namespace CamlexNET.Impl.ReverseEngeneering.Caml.Factories
{
    internal class ReLinkerFromCamlFactory : IReLinkerFactory
    {
        public IReLinker Create(IReTranslator translator)
        {
            if (translator == null)
            {
                throw new ArgumentNullException("translator");
            }

            var translatorFromCaml = translator as ReTranslatorFromCaml;
            if (translatorFromCaml == null)
            {
                throw new LinkerFromCamlRequiresTranslatorFromCamlException(translator.GetType());
            }
            return new ReLinkerFromCaml(translatorFromCaml.Where, translatorFromCaml.OrderBy,
                translatorFromCaml.GroupBy, translatorFromCaml.ViewFields);
        }
    }
}
