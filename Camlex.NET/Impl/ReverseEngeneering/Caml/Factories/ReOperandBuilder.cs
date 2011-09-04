using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CamlexNET.Interfaces;
using CamlexNET.Interfaces.ReverseEngeneering;

namespace CamlexNET.Impl.ReverseEngeneering.Caml.Factories
{
    internal class ReOperandBuilder : IReOperandBuilder
    {
        public IOperand CreateFieldRefOperand(string input, IOperand valueOperand)
        {
            throw new NotImplementedException();
        }

        public IOperand CreateFieldRefOperandWithOrdering(string input, Camlex.OrderDirection orderDirection)
        {
            throw new NotImplementedException();
        }

        public IOperand CreateValueOperandForNativeSyntax(string input)
        {
            throw new NotImplementedException();
        }

        public IOperand CreateValueOperandForNativeSyntax(string input, Type explicitOperandType)
        {
            throw new NotImplementedException();
        }

        public IOperand CreateValueOperandForStringBasedSyntax(string input)
        {
            throw new NotImplementedException();
        }
    }
}
