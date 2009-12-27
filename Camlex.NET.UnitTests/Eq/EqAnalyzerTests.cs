using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Camlex.NET.Impl.Eq;
using Microsoft.SharePoint;
using NUnit.Framework;

namespace Camlex.NET.UnitTests.Eq
{
    [TestFixture]
    public class EqAnalyzerTests
    {
        [Test]
        public void test_THAT_eq_expression_IS_valid()
        {
            var analyzer = new EqAnalyzer();
            Expression<Func<SPItem, bool>> expr = x => (string) x["Title"] == "testValue";
            Assert.That(analyzer.IsValid(expr), Is.True);
        }
    }
}
