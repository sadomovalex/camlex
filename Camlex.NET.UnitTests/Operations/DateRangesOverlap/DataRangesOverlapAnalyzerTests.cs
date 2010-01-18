using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CamlexNET.Impl.Operations.DataRangesOverlap;
using Microsoft.SharePoint;
using NUnit.Framework;

namespace CamlexNET.UnitTests.Operations.DateRangesOverlap
{
    [TestFixture]
    public class DataRangesOverlapAnalyzerTests
    {
        [Test]
        public void test_THAT_datarangesoverlap_expression_with_native_constant_IS_valid()
        {
            var analyzer = new DataRangesOverlapAnalyzer(null, null);
            Expression<Func<SPItem, bool>> expr = x => Camlex.DateRangesOverlap(x["start"], x["stop"], x["recurrence"], DateTime.Now);
            Assert.That(analyzer.IsValid(expr), Is.True);
        }

        [Test]
        public void test_THAT_datarangesoverlap_expression_with_native_variable_IS_valid()
        {
            var analyzer = new DataRangesOverlapAnalyzer(null, null);
            var now = DateTime.Now;
            Expression<Func<SPItem, bool>> expr = x => Camlex.DateRangesOverlap(x["start"], x["stop"], x["recurrence"], now);
            Assert.That(analyzer.IsValid(expr), Is.True);
        }

        [Test]
        public void test_THAT_datarangesoverlap_expression_with_string_constant_IS_valid()
        {
            var analyzer = new DataRangesOverlapAnalyzer(null, null);
            Expression<Func<SPItem, bool>> expr = x => Camlex.DateRangesOverlap(x["start"], x["stop"], x["recurrence"], ((DataTypes.DateTime)"02.01.2010 03:04:05"));
            Assert.That(analyzer.IsValid(expr), Is.True);
        }

        [Test]
        public void test_THAT_datarangesoverlap_expression_with_string_variable_IS_valid()
        {
            var analyzer = new DataRangesOverlapAnalyzer(null, null);
            const string now = "02.01.2010 03:04:05";
            Expression<Func<SPItem, bool>> expr = x => Camlex.DateRangesOverlap(x["start"], x["stop"], x["recurrence"], ((DataTypes.DateTime)now));
            Assert.That(analyzer.IsValid(expr), Is.True);
        }

        [Test]
        public void test_THAT_datarangesoverlap_expression_with_special_constant_IS_valid()
        {
            var analyzer = new DataRangesOverlapAnalyzer(null, null);
            Expression<Func<SPItem, bool>> expr = x => Camlex.DateRangesOverlap(x["start"], x["stop"], x["recurrence"], (DataTypes.DateTime)Camlex.Month);
            Assert.That(analyzer.IsValid(expr), Is.True);
        }
    }
}
