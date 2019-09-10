using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CamlexNET.Impl.Operations.NotIncludes;
using CamlexNET.Interfaces;
using CamlexNET.Interfaces.ReverseEngeneering;

namespace CamlexNET.Impl.ReverseEngeneering.Caml.Analyzers
{
    internal class ReNotIncludesAnalyzer : ReBinaryExpressionBaseAnalyzer
    {
        public ReNotIncludesAnalyzer(XElement el, IReOperandBuilder operandBuilder) :
            base(el, operandBuilder)
        {
        }

        public override bool IsValid()
        {
            if (!base.IsValid()) return false;
            if (el.Name != Tags.NotIncludes) return false;
            return true;
        }

        public override IOperation GetOperation()
        {
            return getOperation((fieldRefOperand, valueOperand) =>
                new NotIncludesOperation(null, fieldRefOperand, valueOperand));
        }
    }
}
