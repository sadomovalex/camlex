using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Camlex.NET.Impl.Factories;
using Camlex.NET.Impl.Operations.Array;
using Camlex.NET.Impl.Operands;
using Camlex.NET.UnitTests.Helpers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Camlex.NET.UnitTests.Array
{
    [TestFixture]
    public class ArrayOperationTest
    {
        [Test]
        public void test_THAT_operation_IS_rendered_to_caml_properly()
        {
            // arrange
            var fieldRefOperandStub1 = MockRepository.GenerateStub<FieldRefOperand>("");
            var fieldRefOperandStub2 = MockRepository.GenerateStub<FieldRefOperand>("");

            fieldRefOperandStub1.Stub(o => o.ToCaml()).Return(new XElement("fieldRefOperandStub1"));
            fieldRefOperandStub2.Stub(o => o.ToCaml()).Return(new XElement("fieldRefOperandStub2"));

            var resultBuilder = new OperationResultBuilder();
            var operation = new ArrayOperation(resultBuilder, fieldRefOperandStub1, fieldRefOperandStub2);

            // act
            string caml = operation.ToResult().ToString();

            // assert
            string expected =
                @"<fieldRefOperandStub1 />
                  <fieldRefOperandStub2 />";
            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }
    }
}
