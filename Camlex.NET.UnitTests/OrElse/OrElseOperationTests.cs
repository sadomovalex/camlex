using System.Xml.Linq;
using Camlex.NET.Impl.AndAlso;
using Camlex.NET.Impl.Eq;
using Camlex.NET.Impl.OrElse;
using Camlex.NET.UnitTests.Helpers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Camlex.NET.UnitTests.OrElse
{
    [TestFixture]
    public class OrElseOperationTests
    {
        [Test]
        public void test_THAT_orelse_with_2_eq_IS_translated_to_caml_properly()
        {
            // arrange
            var leftOperation = MockRepository.GenerateStub<EqOperation>(null, null);
            var rightOperation = MockRepository.GenerateStub<EqOperation>(null, null);

            leftOperation.Stub(o => o.ToCaml()).Return(new XElement("Eq1"));
            rightOperation.Stub(o => o.ToCaml()).Return(new XElement("Eq2"));

            var operation = new OrElseOperation(leftOperation, rightOperation);

            // act
            string caml = operation.ToCaml().ToString();

            // assert
            string expected =
                @"<Or>
                    <Eq1 />
                    <Eq2 />
                </Or>";
            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_orelse_with_nested_orelse_IS_translated_to_caml_properly()
        {
            // arrange
            var leftEqOperation = MockRepository.GenerateStub<EqOperation>(null, null);
            var rightEqOperation = MockRepository.GenerateStub<EqOperation>(null, null);
            var leftOperation = new OrElseOperation(leftEqOperation, rightEqOperation);

            leftEqOperation.Stub(o => o.ToCaml()).Return(new XElement("Eq1"));
            rightEqOperation.Stub(o => o.ToCaml()).Return(new XElement("Eq2"));

            var operation = new OrElseOperation(leftOperation, rightEqOperation);

            // act
            string caml = operation.ToCaml().ToString();

            // assert
            string expected =
                @"<Or>
                    <Or>
                        <Eq1 />
                        <Eq2 />
                    </Or>
                    <Eq2 />
                </Or>";
            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_orelse_with_2_nested_orelse_IS_translated_to_caml_properly()
        {
            // arrange
            var leftEqOperation = MockRepository.GenerateStub<EqOperation>(null, null);
            var rightEqOperation = MockRepository.GenerateStub<EqOperation>(null, null);
            var leftOperation = new OrElseOperation(leftEqOperation, rightEqOperation);
            var rightOperation = new OrElseOperation(leftEqOperation, rightEqOperation);

            leftEqOperation.Stub(o => o.ToCaml()).Return(new XElement("Eq1"));
            rightEqOperation.Stub(o => o.ToCaml()).Return(new XElement("Eq2"));

            var operation = new OrElseOperation(leftOperation, rightOperation);

            // act
            string caml = operation.ToCaml().ToString();

            // assert
            string expected =
                @"<Or>
                    <Or>
                        <Eq1 />
                        <Eq2 />
                    </Or>
                    <Or>
                        <Eq1 />
                        <Eq2 />
                    </Or>
                </Or>";
            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_orelse_with_3_nested_orelse_IS_translated_to_caml_properly()
        {
            // arrange
            var leftEqOperation = MockRepository.GenerateStub<EqOperation>(null, null);
            var rightEqOperation = MockRepository.GenerateStub<EqOperation>(null, null);
            var leftOperation1 = new OrElseOperation(leftEqOperation, rightEqOperation);
            var leftOperation2 = new OrElseOperation(leftOperation1, rightEqOperation);

            leftEqOperation.Stub(o => o.ToCaml()).Return(new XElement("Eq1"));
            rightEqOperation.Stub(o => o.ToCaml()).Return(new XElement("Eq2"));

            var operation = new OrElseOperation(leftOperation2, rightEqOperation);

            // act
            string caml = operation.ToCaml().ToString();

            // assert
            string expected =
                @"<Or>
                    <Or>
                        <Or>
                            <Eq1 />
                            <Eq2 />
                        </Or>
                        <Eq2 />
                    </Or>
                    <Eq2 />
                </Or>";
            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }
    }
}


