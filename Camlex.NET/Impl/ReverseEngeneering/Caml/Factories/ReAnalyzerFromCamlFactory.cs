using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using CamlexNET.Impl.Operations.Array;
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
            throw new NonSupportedTagException(el.Name.ToString());
        }

        private IReAnalyzer getAnalyzerForAray(XElement el)
        {
            return new ReArrayAnalyzer(el, this.operandBuilder);
        }

        private IReAnalyzer getAnalyzerForWhere(XElement el)
        {
            throw new NotImplementedException();
        }
    }
}
