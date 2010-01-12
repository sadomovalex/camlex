using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using CamlexNET.Impl.Factories;
using CamlexNET.Impl.Operations.Results;
using NUnit.Framework;

namespace CamlexNET.UnitTests.Factories
{
    [TestFixture]
    public class OperationResultBuilderTests
    {
        [Test]
        public void test_WHEN_single_xelement_passed_THEN_xelement_operation_result_is_created()
        {
            var operationResultBuilder = new OperationResultBuilder();
            var result = operationResultBuilder.CreateResult(new XElement("foo"));
            Assert.That(result, Is.InstanceOf<XElementOperationResult>());
        }

        [Test]
        public void test_WHEN_multiple_xelements_passed_THEN_xelement_array_operation_result_is_created()
        {
            var operationResultBuilder = new OperationResultBuilder();
            var result = operationResultBuilder.CreateResult(new[] { new XElement("foo"), new XElement("foo") });
            Assert.That(result, Is.InstanceOf<XElementArrayOperationResult>());
        }
    }
}
