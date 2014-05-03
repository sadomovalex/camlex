using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CamlexNET.Impl.Factories;
using CamlexNET.Impl.Operands;
using CamlexNET.Impl.Operations.BeginsWith;
using CamlexNET.Impl.Operations.Constant;
using CamlexNET.Interfaces;
using Microsoft.SharePoint.Client;
using NUnit.Framework;
using Rhino.Mocks;

namespace CamlexNET.UnitTests.Operations.Constant
{
    [TestFixture]
    public class ConstantOperationTests
    {
        [Test]
        public void test_THAT_constant_operation_IS_rendered_to_caml_properly()
        {
            var operationResultBuilder = new OperationResultBuilder();
            var operand = new ConstantOperand(10, "foo");
            var o = new ConstantOperation(operationResultBuilder, operand);
            Assert.That(o.ToResult().ToString(), Is.EqualTo("<foo>10</foo>"));
        }
    }
}
