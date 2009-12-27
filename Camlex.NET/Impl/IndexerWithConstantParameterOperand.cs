﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl
{
    public class IndexerWithConstantParameterOperand : IOperand
    {
        private readonly string type;
        private readonly string fieldName;

        public IndexerWithConstantParameterOperand(string type, string fieldName)
        {
            this.type = type;
            this.fieldName = fieldName;
        }

//        public IndexerWithConstantParameterLeftOperand(DataType type, string fieldName)
//            : this(type.ToString(), fieldName)
//        {
//        }

        public XElement ToCaml()
        {
            throw new NotImplementedException();
        }
    }
}
