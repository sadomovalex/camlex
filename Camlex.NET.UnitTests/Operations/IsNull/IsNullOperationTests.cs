using System.Xml.Linq;
using Camlex.NET.Impl.Factories;
using Camlex.NET.Impl.Operands;
using Camlex.NET.Impl.Operations.IsNotNull;
using Camlex.NET.Impl.Operations.IsNull;
using Camlex.NET.UnitTests.Helpers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Camlex.NET.UnitTests.Operations.IsNull
{
    [TestFixture]
    public class IsNullOperationTests
    {
        [Test]
        public void test_THAT_isnull_operation_IS_rendered_to_caml_properly()
        {
            // arrange
            var fieldRefOperandStub = MockRepository.GenerateStub<FieldRefOperand>("");
            var valueOperandStub = MockRepository.GenerateStub<IntegerValueOperand>(0);

            fieldRefOperandStub.Stub(o => o.ToCaml()).Return(new XElement("fieldRefOperandStub"));
            valueOperandStub.Stub(o => o.ToCaml()).Return(new XElement("valueOperandStub"));

            var resultBuilder = new OperationResultBuilder();
            var operation = new IsNullOperation(resultBuilder, fieldRefOperandStub);

            // act
            string caml = operation.ToResult().ToString();

            // assert
            string expected =
                @"<IsNull>
                    <fieldRefOperandStub />
                </IsNull>";
            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }
    }
}


