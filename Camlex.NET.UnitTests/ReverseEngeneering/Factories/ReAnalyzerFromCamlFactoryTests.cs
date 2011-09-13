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
        public void test_THAT_for_where_binary_operation_analyzer_IS_returned()
        {
            Assert.Fail("Not implemented");
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
