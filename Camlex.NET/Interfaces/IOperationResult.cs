﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Camlex.NET.Interfaces
{
    public interface IOperationResult
    {
        object Value { get; }
    }

//    public interface IOperationMultipleResult : IOperationResult
//    {
//        XElement[] Values { get; }
//    }
}
