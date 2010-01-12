using System.Xml.Linq;
using CamlexNET.Impl.Factories;
using CamlexNET.Impl.Operations.Eq;
using CamlexNET.Impl.Operations.OrElse;
using CamlexNET.Impl.Operations.Results;
using CamlexNET.UnitTests.Helpers;
using NUnit.Framework;
using Rhino.Mocks;

namespace CamlexNET.UnitTests.Operations.OrElse
{
    [TestFixture]
    public class OrElseOperationTests
    {
        XElementOperationResult xelementResult(string name)
        {
            return new XElementOperationResult(new XElement(name));
        }

        [Test]
        public void test_THAT_orelse_with_2_eq_IS_translated_to_caml_properly()
        {
            // arrange
            var leftOperation = MockRepository.GenerateStub<EqOperation>(null, null, null);
            var rightOperation = MockRepository.GenerateStub<EqOperation>(null, null, null);

            leftOperation.Stub(o => o.ToResult()).Return(xelementResult("Eq1"));
            rightOperation.Stub(o => o.ToResult()).Return(xelementResult("Eq2"));

            var resultBuilder = new OperationResultBuilder();
            var operation = new OrElseOperation(resultBuilder, leftOperation, rightOperation);

            // act
            string caml = operation.ToResult().ToString();

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
            var leftEqOperation = MockRepository.GenerateStub<EqOperation>(null, null, null);
            var rightEqOperation = MockRepository.GenerateStub<EqOperation>(null, null, null);

            var resultBuilder = new OperationResultBuilder();
            var leftOperation = new OrElseOperation(resultBuilder, leftEqOperation, rightEqOperation);

            leftEqOperation.Stub(o => o.ToResult()).Return(xelementResult("Eq1"));
            rightEqOperation.Stub(o => o.ToResult()).Return(xelementResult("Eq2"));

            var operation = new OrElseOperation(resultBuilder, leftOperation, rightEqOperation);

            // act
            string caml = operation.ToResult().ToString();

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
            var leftEqOperation = MockRepository.GenerateStub<EqOperation>(null, null, null);
            var rightEqOperation = MockRepository.GenerateStub<EqOperation>(null, null, null);

            var resultBuilder = new OperationResultBuilder();
            var leftOperation = new OrElseOperation(resultBuilder, leftEqOperation, rightEqOperation);
            var rightOperation = new OrElseOperation(resultBuilder, leftEqOperation, rightEqOperation);

            leftEqOperation.Stub(o => o.ToResult()).Return(xelementResult("Eq1"));
            rightEqOperation.Stub(o => o.ToResult()).Return(xelementResult("Eq2"));

            var operation = new OrElseOperation(resultBuilder, leftOperation, rightOperation);

            // act
            string caml = operation.ToResult().ToString();

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
            var leftEqOperation = MockRepository.GenerateStub<EqOperation>(null, null, null);
            var rightEqOperation = MockRepository.GenerateStub<EqOperation>(null, null, null);

            var resultBuilder = new OperationResultBuilder();
            var leftOperation1 = new OrElseOperation(resultBuilder, leftEqOperation, rightEqOperation);
            var leftOperation2 = new OrElseOperation(resultBuilder, leftOperation1, rightEqOperation);

            leftEqOperation.Stub(o => o.ToResult()).Return(xelementResult("Eq1"));
            rightEqOperation.Stub(o => o.ToResult()).Return(xelementResult("Eq2"));

            var operation = new OrElseOperation(resultBuilder, leftOperation2, rightEqOperation);

            // act
            string caml = operation.ToResult().ToString();

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


