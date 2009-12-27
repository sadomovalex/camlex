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
        private readonly string val;

        public ConstantOperand(string val)
        {
            this.val = val;
        }

        public XElement ToCaml()
        {
            throw new NotImplementedException();
        }
    }
}
