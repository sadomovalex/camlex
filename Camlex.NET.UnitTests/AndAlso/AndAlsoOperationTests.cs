using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Linq;
using Camlex.NET.Impl.AndAlso;
using Camlex.NET.Impl.Eq;
using Camlex.NET.Impl.Operands;
using Camlex.NET.UnitTests.Helpers;
using Microsoft.SharePoint;
using NUnit.Framework;
using Rhino.Mocks;

namespace Camlex.NET.UnitTests.AndAlso
{
    [TestFixture]
    public class AndAlsoOperationTests
    {
        [Test]
        public void test_THAT_and_with_2_eq_IS_translated_to_caml_properly()
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
    }
}
