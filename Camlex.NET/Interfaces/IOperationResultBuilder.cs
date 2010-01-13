using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace CamlexNET.Interfaces
{
    internal interface IOperationResultBuilder
    {
        IOperationResult CreateResult(XElement value);
        IOperationResult CreateResult(XElement[] values);
    }
}
