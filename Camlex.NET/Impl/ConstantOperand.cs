using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl
{
    public class ConstantOperand : IOperand
    {
        private readonly object val;
        private readonly string type;

        public ConstantOperand(string type, object val)
        {
            this.val = val;
            this.type = type;
        }

        public XElement ToCaml()
        {
            throw new NotImplementedException();
        }
    }
}
