using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CamlexNET.Impl.ReverseEngeneering.Caml.Factories;
using CamlexNET.Interfaces.ReverseEngeneering;
using NUnit.Framework;
using Rhino.Mocks;


namespace CamlexNET.UnitTests.ReverseEngeneering.Factories
{
    [TestFixture]
    public class ReTranslatorFromCamlFactoryTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void test_WHEN_argument_is_null_THEN_exception_is_thrown()
        {
            var f = new ReTranslatorFromCamlFactory(null);
            f.Create(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void test_WHEN_argument_is_empty_THEN_exception_is_thrown()
        {
            var f = new ReTranslatorFromCamlFactory(null);
            f.Create("");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void test_WHEN_argument_is_space_THEN_exception_is_thrown()
        {
            var f = new ReTranslatorFromCamlFactory(null);
            f.Create(" ");
        }

        [Test]
        [ExpectedException(typeof(XmlNotWellFormedException))]
        public void test_WHEN_argument_is_not_well_formed_xml_THEN_exception_is_thrown()
        {
            var f = new ReTranslatorFromCamlFactory(null);
            f.Create("foo");
        }

        [Test]
        [ExpectedException(typeof(XmlNotWellFormedException))]
        public void test_WHEN_argument_is_not_within_query_THEN_exception_is_thrown()
        {
            var f = new ReTranslatorFromCamlFactory(null);
            f.Create("<foo></foo>");
        }
    }
}
