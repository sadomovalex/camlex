using System.Xml.Linq;
using Camlex.NET.Impl.Operations.AndAlso;
using Camlex.NET.Impl.Operations.Eq;
using Camlex.NET.UnitTests.Helpers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Camlex.NET.UnitTests.Operations.AndAlso
{
    [TestFixture]
    public class AndAlsoOperationTests
    {
        [Test]
        public void test_THAT_andalso_with_2_eq_IS_translated_to_caml_properly()
        {
            // arrange
            var leftOperation = MockRepository.GenerateStub<EqOperation>(null, null);
            var rightOperation = MockRepository.GenerateStub<EqOperation>(null, null);

            leftOperation.Stub(o => o.ToCaml()).Return(new XElement("Eq1"));
            rightOperation.Stub(o => o.ToCaml()).Return(new XElement("Eq2"));

            var operation = new AndAlsoOperation(leftOperation, rightOperation);

            // act
            string caml = operation.ToCaml().ToString();

            // assert
            string expected =
                @"<And>
                    <Eq1 />
                    <Eq2 />
                </And>";
            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_andalso_with_nested_andalso_IS_translated_to_caml_properly()
        {
            // arrange
            var leftEqOperation = MockRepository.GenerateStub<EqOperation>(null, null);
            var rightEqOperation = MockRepository.GenerateStub<EqOperation>(null, null);
            var leftOperation = new AndAlsoOperation(leftEqOperation, rightEqOperation);

            leftEqOperation.Stub(o => o.ToCaml()).Return(new XElement("Eq1"));
            rightEqOperation.Stub(o => o.ToCaml()).Return(new XElement("Eq2"));

            var operation = new AndAlsoOperation(leftOperation, rightEqOperation);

            // act
            string caml = operation.ToCaml().ToString();

            // assert
            string expected =
                @"<And>
                    <And>
                        <Eq1 />
                        <Eq2 />
                    </And>
                    <Eq2 />
                </And>";
            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_andalso_with_2_nested_andalso_IS_translated_to_caml_properly()
        {
            // arrange
            var leftEqOperation = MockRepository.GenerateStub<EqOperation>(null, null);
            var rightEqOperation = MockRepository.GenerateStub<EqOperation>(null, null);
            var leftOperation = new AndAlsoOperation(leftEqOperation, rightEqOperation);
            var rightOperation = new AndAlsoOperation(leftEqOperation, rightEqOperation);

            leftEqOperation.Stub(o => o.ToCaml()).Return(new XElement("Eq1"));
            rightEqOperation.Stub(o => o.ToCaml()).Return(new XElement("Eq2"));

            var operation = new AndAlsoOperation(leftOperation, rightOperation);

            // act
            string caml = operation.ToCaml().ToString();

            // assert
            string expected =
                @"<And>
                    <And>
                        <Eq1 />
                        <Eq2 />
                    </And>
                    <And>
                        <Eq1 />
                        <Eq2 />
                    </And>
                </And>";
            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_andalso_with_3_nested_andalso_IS_translated_to_caml_properly()
        {
            // arrange
            var leftEqOperation = MockRepository.GenerateStub<EqOperation>(null, null);
            var rightEqOperation = MockRepository.GenerateStub<EqOperation>(null, null);
            var leftOperation1 = new AndAlsoOperation(leftEqOperation, rightEqOperation);
            var leftOperation2 = new AndAlsoOperation(leftOperation1, rightEqOperation);

            leftEqOperation.Stub(o => o.ToCaml()).Return(new XElement("Eq1"));
            rightEqOperation.Stub(o => o.ToCaml()).Return(new XElement("Eq2"));

            var operation = new AndAlsoOperation(leftOperation2, rightEqOperation);

            // act
            string caml = operation.ToCaml().ToString();

            // assert
            string expected =
                @"<And>
                    <And>
                        <And>
                            <Eq1 />
                            <Eq2 />
                        </And>
                        <Eq2 />
                    </And>
                    <Eq2 />
                </And>";
            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }
    }
}


