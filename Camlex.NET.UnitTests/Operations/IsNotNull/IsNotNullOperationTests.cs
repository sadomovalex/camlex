using System.Xml.Linq;
using CamlexNET.Impl.Factories;
using CamlexNET.Impl.Operands;
using CamlexNET.Impl.Operations.IsNotNull;
using CamlexNET.Impl.Operations.Lt;
using CamlexNET.UnitTests.Helpers;
using NUnit.Framework;
using Rhino.Mocks;

namespace CamlexNET.UnitTests.Operations.IsNotNull
{
    [TestFixture]
    public class IsNotNullOperationTests
    {
        [Test]
        public void test_THAT_isnotnull_operation_IS_rendered_to_caml_properly()
        {
            // arrange
            var fieldRefOperandStub = MockRepository.GenerateStub<FieldRefOperand>("");
            var valueOperandStub = MockRepository.GenerateStub<IntegerValueOperand>(0);

            fieldRefOperandStub.Stub(o => o.ToCaml()).Return(new XElement("fieldRefOperandStub"));
            valueOperandStub.Stub(o => o.ToCaml()).Return(new XElement("valueOperandStub"));

            var resultBuilder = new OperationResultBuilder();
            var operation = new IsNotNullOperation(resultBuilder, fieldRefOperandStub);

            // act
            string caml = operation.ToResult().ToString();

            // assert
            string expected =
                @"<IsNotNull>
                    <fieldRefOperandStub />
                </IsNotNull>";
            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }
    }
}


