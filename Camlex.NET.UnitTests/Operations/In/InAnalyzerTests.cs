using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CamlexNET.Impl.Operations.In;
using Microsoft.SharePoint;
using NUnit.Framework;

namespace CamlexNET.UnitTests.Operations.In
{
    [TestFixture]
    public class InAnalyzerTests
    {
        [Test]
        public void test_THAT_in_string_expression_IS_valid()
        {
            var a = new InAnalyzer(null, null);
            IEnumerable<string> values = new []{ "1", "2", "3" };
            Expression<Func<SPListItem, bool>> expr = x => values.Contains((string)x["test"]);
            Assert.IsTrue(a.IsValid(expr));
        }
    }
}
