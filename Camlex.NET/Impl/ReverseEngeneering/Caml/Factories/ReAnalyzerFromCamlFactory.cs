using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using CamlexNET.Impl.Operations.Array;
using CamlexNET.Impl.ReverseEngeneering.Caml.Analyzers;
using CamlexNET.Interfaces.ReverseEngeneering;

namespace CamlexNET.Impl.ReverseEngeneering.Caml.Factories
{
    internal class ReAnalyzerFromCamlFactory : IReAnalyzerFactory
    {
        private readonly IReOperandBuilder operandBuilder;
        //private readonly ReOperationResultBuilder operationResultBuilder;

        public ReAnalyzerFromCamlFactory(IReOperandBuilder operandBuilder)
        {
            this.operandBuilder = operandBuilder;
        }

        public IReAnalyzer Create(XElement el)
        {
            if (el == null)
            {
                throw new ArgumentNullException("el");
            }

            if (el.Name == Tags.Where)
            {
                return this.getAnalyzerForWhere(el);
            }
            
            if (el.Name == Tags.OrderBy || el.Name == Tags.GroupBy ||
                el.Name == Tags.ViewFields)
            {
                return this.getAnalyzerForAray(el);
            }
            throw new CamlAnalysisException(string.Format("Tag '{0}' is not supported. Supported tags are: {1}, {2}, {3}, {4}", el.Name, Tags.Where, Tags.OrderBy, Tags.GroupBy, Tags.ViewFields));
        }

        private IReAnalyzer getAnalyzerForAray(XElement el)
        {
            return new ReArrayAnalyzer(el, this.operandBuilder);
        }

        private IReAnalyzer getAnalyzerForWhere(XElement el)
        {
            if (el.Elements().Count() != 1)
            {
                throw new CamlAnalysisException("Where tag should contain only 1 child element");
            }

            var element = el.Elements().FirstOrDefault(e => e.Name == Tags.Eq);
            if (element != null)
            {
                return new ReEqAnalyzer(element, operandBuilder);
            }
            element = el.Elements().FirstOrDefault(e => e.Name == Tags.Geq);
            if (element != null)
            {
                return new ReGeqAnalyzer(element, operandBuilder);
            }
            element = el.Elements().FirstOrDefault(e => e.Name == Tags.Gt);
            if (element != null)
            {
                return new ReGtAnalyzer(element, operandBuilder);
            }
            element = el.Elements().FirstOrDefault(e => e.Name == Tags.Leq);
            if (element != null)
            {
                return new ReLeqAnalyzer(element, operandBuilder);
            }
            element = el.Elements().FirstOrDefault(e => e.Name == Tags.Lt);
            if (element != null)
            {
                return new ReLtAnalyzer(element, operandBuilder);
            }
            element = el.Elements().FirstOrDefault(e => e.Name == Tags.Neq);
            if (element != null)
            {
                return new ReNeqAnalyzer(element, operandBuilder);
            }
            element = el.Elements().First();
            throw new CamlAnalysisException(string.Format("Where tag contain element which can't be translated: \n{0}", element));
        }
    }
}
