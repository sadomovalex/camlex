﻿using System;
using System.Xml.Linq;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Operands
{
    public class IntegerValueOperand : ValueOperand<int>
    {
        public IntegerValueOperand(int value) :
            base(DataType.Integer, value)
        {
        }

        public override XElement ToCaml()
        {
            return
                new XElement(Tags.Value, new XAttribute(Attributes.Type, this.type),
                    new XText(this.value.ToString()));
        }
    }
}

