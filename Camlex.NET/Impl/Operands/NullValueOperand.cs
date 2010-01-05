using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Operands
{
    // This is marker class which is used when passed value is null
    public class NullValueOperand : IOperand
    {
        public XElement ToCaml()
        {
            throw new NullValueOperandCannotBeTranslatedToCamlException();
        }
    }
}
