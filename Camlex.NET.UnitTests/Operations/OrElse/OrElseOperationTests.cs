#region Copyright(c) Alexey Sadomov, Vladimir Timashkov. All Rights Reserved.
// -----------------------------------------------------------------------------
// Copyright(c) 2007 Alexey Sadomov, Vladimir Timashkov. All Rights Reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//
//   1. No Trademark License - Microsoft Public License (Ms-PL) does not grant you rights to use
//      authors names, logos, or trademarks.
//   2. If you distribute any portion of the software, you must retain all copyright,
//      patent, trademark, and attribution notices that are present in the software.
//   3. If you distribute any portion of the software in source code form, you may do
//      so only under this license by including a complete copy of Microsoft Public License (Ms-PL)
//      with your distribution. If you distribute any portion of the software in compiled
//      or object code form, you may only do so under a license that complies with
//      Microsoft Public License (Ms-PL).
//   4. The names of the authors may not be used to endorse or promote products
//      derived from this software without specific prior written permission.
//
// The software is licensed "as-is." You bear the risk of using it. The authors
// give no express warranties, guarantees or conditions. You may have additional consumer
// rights under your local laws which this license cannot change. To the extent permitted
// under your local laws, the authors exclude the implied warranties of merchantability,
// fitness for a particular purpose and non-infringement.
// -----------------------------------------------------------------------------
#endregion
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


