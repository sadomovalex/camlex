using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CamlexNET.Impl.ReverseEngeneering.Caml.Analyzers;
using CamlexNET.Impl.ReverseEngeneering.Caml.Factories;
using NUnit.Framework;

namespace CamlexNET.UnitTests.ReverseEngeneering.Factories
{
    [TestFixture]
    public class ReAnalyzerFromCamlFactoryTests
    {
        [Test]
        public void test_THAT_for_order_by_array_analyzer_IS_returned()
        {
            var xml =
                "  <OrderBy>" +
                "    <FieldRef Name=\"Title\" Ascending=\"True\" />" +
                "  </OrderBy>";

            var f = new ReAnalyzerFromCamlFactory(null);
            Assert.IsInstanceOf<ReArrayAnalyzer>(f.Create(XmlHelper.Get(xml)));
        }

        [Test]
        public void test_THAT_for_group_by_array_analyzer_IS_returned()
        {
            var xml =
                "  <GroupBy>" +
                "    <FieldRef Name=\"Title\" />" +
                "  </GroupBy>";

            var f = new ReAnalyzerFromCamlFactory(null);
            Assert.IsInstanceOf<ReArrayAnalyzer>(f.Create(XmlHelper.Get(xml)));
        }

        [Test]
        public void test_THAT_for_view_fields_array_analyzer_IS_returned()
        {
            var xml =
                "  <ViewFields>" +
                "    <FieldRef Name=\"Title\" />" +
                "  </ViewFields>";

            var f = new ReAnalyzerFromCamlFactory(null);
            Assert.IsInstanceOf<ReArrayAnalyzer>(f.Create(XmlHelper.Get(xml)));
        }

        [Test]
        [ExpectedException(typeof(CamlAnalysisException))]
        public void test_WHEN_where_is_empty_THEN_exception_is_thrown()
        {
            var xml =
                "<Where>" +
                "</Where>";
            var f = new ReAnalyzerFromCamlFactory(null);
            f.Create(XmlHelper.Get(xml));
        }

        [Test]
        [ExpectedException(typeof(CamlAnalysisException))]
        public void test_WHEN_where_contains_unknown_child_THEN_exception_is_thrown()
        {
            var xml =
                "<Where>" +
                "<foo></foo>" +
                "</Where>";
            var f = new ReAnalyzerFromCamlFactory(null);
            f.Create(XmlHelper.Get(xml));
        }

        [Test]
        public void test_THAT_for_where_correct_analyzer_IS_returned()
        {
            var f = new ReAnalyzerFromCamlFactory(null);
            Assert.IsInstanceOf<ReEqAnalyzer>(f.Create(XmlHelper.Get("<Where><Eq></Eq></Where>")));
            Assert.IsInstanceOf<ReGeqAnalyzer>(f.Create(XmlHelper.Get("<Where><Geq></Geq></Where>")));
            Assert.IsInstanceOf<ReGtAnalyzer>(f.Create(XmlHelper.Get("<Where><Gt></Gt></Where>")));
            Assert.IsInstanceOf<ReLeqAnalyzer>(f.Create(XmlHelper.Get("<Where><Leq></Leq></Where>")));
            Assert.IsInstanceOf<ReLtAnalyzer>(f.Create(XmlHelper.Get("<Where><Lt></Lt></Where>")));
            Assert.IsInstanceOf<ReNeqAnalyzer>(f.Create(XmlHelper.Get("<Where><Neq></Neq></Where>")));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void test_WHEN_null_is_specified_THEN_exception_is_thrown()
        {
            var f = new ReAnalyzerFromCamlFactory(null);
            f.Create(null);
        }

        [Test]
        [ExpectedException(typeof(CamlAnalysisException))]
        public void test_THAT_for_unknown_tag_exception_IS_thrown()
        {
            var f = new ReAnalyzerFromCamlFactory(null);
            f.Create(XmlHelper.Get("<foo></foo>"));
        }
    }
}
