using System.Xml.Linq;
using Camlex.NET.Impl.Operands;
using Camlex.NET.Impl.Operations.Geq;
using Camlex.NET.Impl.Operations.Gt;
using Camlex.NET.UnitTests.Helpers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Camlex.NET.UnitTests.Operations.Gt
{
    [TestFixture]
    public class GtOperationTests
    {
        [Test]
        public void test_THAT_gt_operation_IS_rendered_to_caml_properly()
        {
            // arrange
            var fieldRefOperandStub = MockRepository.GenerateStub<FieldRefOperand>("");
            var valueOperandStub = MockRepository.GenerateStub<IntegerValueOperand>(0);

            fieldRefOperandStub.Stub(o => o.ToCaml()).Return(new XElement("fieldRefOperandStub"));
            valueOperandStub.Stub(o => o.ToCaml()).Return(new XElement("valueOperandStub"));

            var operation =  new GtOperation(fieldRefOperandStub, valueOperandStub);

            // act
            string caml = operation.ToCaml().ToString();

            // assert
            string expected =
                @"<Gt>
                    <fieldRefOperandStub />
                    <valueOperandStub />
                </Gt>";
            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }
    }
}


