using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using CamlexNET.Interfaces;

namespace CamlexNET.Impl.Operands
{
    // This is marker class which is used when passed value is null
    internal class NullValueOperand : IOperand
    {
        public XElement ToCaml()
        {
            throw new NullValueOperandCannotBeTranslatedToCamlException();
        }
    }
}
