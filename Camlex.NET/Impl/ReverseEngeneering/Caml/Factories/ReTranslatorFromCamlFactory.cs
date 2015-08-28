#region Copyright(c) Alexey Sadomov, Vladimir Timashkov. All Rights Reserved.
// -----------------------------------------------------------------------------
// Copyright(c) 2010 Alexey Sadomov, Vladimir Timashkov. All Rights Reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//
//   1. No Trademark License - Microsoft Public License (Ms-PL) does not grant you rights to use
//      authors names, logos, or trademarks.
//   2. If you distribute any portion of the software, you must retain all copyright,
//      patent, trademark, and attribution notices that are present in the software.
//   3. If you distribute any portion of the software in source code form, you may do
//      so only under this license by including a complete copy of Microsoft Public License (Ms-PL)
//      with your distribution. If you distribute any portion of the software in compiled
//      or object code form, you may only do so under a license that complies with
//      Microsoft Public License (Ms-PL).
//   4. The names of the authors may not be used to endorse or promote products
//      derived from this software without specific prior written permission.
//
// The software is licensed "as-is." You bear the risk of using it. The authors
// give no express warranties, guarantees or conditions. You may have additional consumer
// rights under your local laws which this license cannot change. To the extent permitted
// under your local laws, the authors exclude the implied warranties of merchantability,
// fitness for a particular purpose and non-infringement.
// -----------------------------------------------------------------------------
#endregion
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
            var joins = this.createForTag(input, Tags.Joins);

            var analyzerForWhere = where != null ? this.analyzerFactory.Create(where) : null;
            var analyzerForOrderBy = orderBy != null ? this.analyzerFactory.Create(orderBy) : null;
            var analyzerForGroupBy = groupBy != null ? this.analyzerFactory.Create(groupBy) : null;
            var analyzerForViewFields = viewFields != null ? this.analyzerFactory.Create(viewFields) : null;
            var analyzerForJoins = joins != null ? this.analyzerFactory.Create(joins) : null;

            return new ReTranslatorFromCaml(analyzerForWhere, analyzerForOrderBy, analyzerForGroupBy, analyzerForViewFields, analyzerForJoins);
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
