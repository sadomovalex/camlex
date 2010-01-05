﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl.Operations.Results
{
    public class XElementArrayOperationResult : IOperationResult
    {
        private XElement[] elements;

        public XElementArrayOperationResult(XElement[] elements)
        {
            this.elements = elements;
        }

        public object Value
        {
            get { return this.elements; }
        }
    }
}