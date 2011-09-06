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
//    [TestFixture]
//    public class ReTranslatorFromCamlFactoryTests
//    {
//        [Test]
//        [ExpectedException(typeof(ArgumentNullException))]
//        public void test_WHEN_argument_is_null_THEN_exception_is_thrown()
//        {
//            var f = new ReTranslatorFromCamlFactory(null);
//            f.CreateForWhere(null);
//        }
//
//        [Test]
//        [ExpectedException(typeof(ArgumentNullException))]
//        public void test_WHEN_argument_is_empty_THEN_exception_is_thrown()
//        {
//            var f = new ReTranslatorFromCamlFactory(null);
//            f.CreateForWhere("");
//        }
//
//        [Test]
//        [ExpectedException(typeof(ArgumentNullException))]
//        public void test_WHEN_argument_is_space_THEN_exception_is_thrown()
//        {
//            var f = new ReTranslatorFromCamlFactory(null);
//            f.CreateForWhere(" ");
//        }
//
//        [Test]
//        [ExpectedException(typeof(XmlNotWellFormedException))]
//        public void test_WHEN_argument_is_not_well_formed_xml_THEN_exception_is_thrown()
//        {
//            var f = new ReTranslatorFromCamlFactory(null);
//            f.CreateForWhere("foo");
//        }
//
//        [Test]
//        [ExpectedException(typeof(XmlNotWellFormedException))]
//        public void test_WHEN_argument_is_not_within_query_THEN_exception_is_thrown()
//        {
//            var f = new ReTranslatorFromCamlFactory(null);
//            f.CreateForWhere("<foo></foo>");
//        }
//
//        [Test]
//        public void test_WHEN_tag_is_not_found_THEN_null_is_returned()
//        {
//            var af = MockRepository.GenerateStub<IReAnalyzerFactory>();
//            af.Stub(b => b.Create(null)).Return(null).IgnoreArguments();
//
//            var f = new ReTranslatorFromCamlFactory(af);
//            Assert.IsNull(f.CreateForWhere("<Query></Query>"));
//        }
//
//        [Test]
//        public void test_WHEN_tag_is_found_THEN_not_noll_translator_is_returned()
//        {
//            var analyzer = MockRepository.GenerateStub<IReAnalyzer>();
//            var af = MockRepository.GenerateStub<IReAnalyzerFactory>();
//            af.Stub(b => b.Create(null)).Return(analyzer).IgnoreArguments();
//
//            var f = new ReTranslatorFromCamlFactory(af);
//
//            var t = f.CreateForWhere("<Query><Where></Where></Query>");
//            Assert.IsNotNull(t);
//            Assert.IsInstanceOf<IReTranslator>(t);
//        }
//    }
}
