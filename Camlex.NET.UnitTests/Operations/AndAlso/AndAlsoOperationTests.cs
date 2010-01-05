using System.Xml.Linq;
using Camlex.NET.Impl.Operations.AndAlso;
using Camlex.NET.Impl.Operations.Eq;
using Camlex.NET.Impl.Operations.Results;
using Camlex.NET.UnitTests.Helpers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Camlex.NET.UnitTests.Operations.AndAlso
{
    [TestFixture]
    public class AndAlsoOperationTests
    {
        XElementOperationResult xelementResult(string name)
        {
            return new XElementOperationResult(new XElement(name));
        }
        [Test]
        public void test_THAT_andalso_with_2_eq_IS_translated_to_caml_properly()
        {
            // arrange
            var leftOperation = MockRepository.GenerateStub<EqOperation>(null, null);
            var rightOperation = MockRepository.GenerateStub<EqOperation>(null, null);

            leftOperation.Stub(o => o.ToResult()).Return(xelementResult("Eq1"));
            rightOperation.Stub(o => o.ToResult()).Return(xelementResult("Eq2"));

            var operation = new AndAlsoOperation(null, leftOperation, rightOperation);

            // act
            string caml = operation.ToResult().ToString();

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
            var leftOperation = new AndAlsoOperation(null, leftEqOperation, rightEqOperation);

            leftEqOperation.Stub(o => o.ToResult()).Return(xelementResult("Eq1"));
            rightEqOperation.Stub(o => o.ToResult()).Return(xelementResult("Eq2"));

            var operation = new AndAlsoOperation(null, leftOperation, rightEqOperation);

            // act
            string caml = operation.ToResult().ToString();

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
            var leftOperation = new AndAlsoOperation(null, leftEqOperation, rightEqOperation);
            var rightOperation = new AndAlsoOperation(null, leftEqOperation, rightEqOperation);

            leftEqOperation.Stub(o => o.ToResult()).Return(xelementResult("Eq1"));
            rightEqOperation.Stub(o => o.ToResult()).Return(xelementResult("Eq2"));

            var operation = new AndAlsoOperation(null, leftOperation, rightOperation);

            // act
            string caml = operation.ToResult().ToString();

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
            var leftOperation1 = new AndAlsoOperation(null, leftEqOperation, rightEqOperation);
            var leftOperation2 = new AndAlsoOperation(null, leftOperation1, rightEqOperation);

            leftEqOperation.Stub(o => o.ToResult()).Return(xelementResult("Eq1"));
            rightEqOperation.Stub(o => o.ToResult()).Return(xelementResult("Eq2"));

            var operation = new AndAlsoOperation(null, leftOperation2, rightEqOperation);

            // act
            string caml = operation.ToResult().ToString();

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


