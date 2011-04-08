using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CamlexNET.ReverseEngeneering;
using NUnit.Framework;

namespace CamlexNET.UnitTests.ReverseEngeneering
{
    [TestFixture]
    public class ExpressionBuilderTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void test_WHEN_argument_is_null_THEN_exception_is_thrown()
        {
            ExpressionBuilder.BuildFromXml(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void test_WHEN_argument_is_empty_THEN_exception_is_thrown()
        {
            ExpressionBuilder.BuildFromXml("");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void test_WHEN_argument_is_space_THEN_exception_is_thrown()
        {
            ExpressionBuilder.BuildFromXml(" ");
        }

        [Test]
        [ExpectedException(typeof(XmlNotWellFormedException))]
        public void test_WHEN_argument_is_not_well_formed_xml_THEN_exception_is_thrown()
        {
            ExpressionBuilder.BuildFromXml("foo");
        }
    }
}
