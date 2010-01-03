using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Camlex.NET.Interfaces;

namespace Camlex.NET.Impl
{
    public abstract class CompositeOperationBase : IOperation
    {
        protected IOperation leftOperation;
        protected IOperation rightOperation;

        protected CompositeOperationBase(IOperation leftOperation, IOperation rightOperation)
        {
            this.leftOperation = leftOperation;
            this.rightOperation = rightOperation;
        }

        public abstract XElement ToCaml();
    }
}
