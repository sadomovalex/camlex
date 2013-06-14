using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CamlexNET.Impl.Factories;
using CamlexNET.Impl.Operations.In;
using Microsoft.SharePoint;
using NUnit.Framework;

namespace CamlexNET.UnitTests.Operations.In
{
    [TestFixture]
    public class InAnalyzerTests
    {
        [Test]
        public void test_THAT_in_const_string_expression_IS_valid()
        {
            var a = new InAnalyzer(null, null);
            IEnumerable<string> values = new []{ "1", "2", "3" };
            Expression<Func<SPListItem, bool>> expr = x => values.Contains((string)x["test"]);
            Assert.IsTrue(a.IsValid(expr));
        }

        [Test]
        public void test_THAT_in_not_const_int_expression_IS_valid()
        {
            var a = new InAnalyzer(null, null);
            int i = 1;
            Func<int, int> f = p => (p + 1);
            IEnumerable values = new[] { i, f(2), foo() };
            Expression<Func<SPListItem, bool>> expr = x => values.Cast<int>().Contains((int)x["test"]);
            Assert.IsTrue(a.IsValid(expr));
        }

        int foo()
        {
            return 3;
        }

        [Test]
        public void test_THAT_in_not_const_int_yield_expression_IS_valid()
        {
            var a = new InAnalyzer(null, null);
            Expression<Func<SPListItem, bool>> expr = x => iterator().Cast<int>().Contains((int)x["test"]);
            Assert.IsTrue(a.IsValid(expr));
        }

        IEnumerable iterator()
        {
            for (int i = 0; i < 3; i++)
            {
                yield return i;
            }
        }

        [Test]
        public void test_THAT_operation_IS_created_sucessfully()
        {
            var a = new InAnalyzer(new OperationResultBuilder(), new OperandBuilder());
            Expression<Func<SPListItem, bool>> expr = x => iterator().Cast<int>().Contains((int)x["test"]);
            var op = a.GetOperation(expr);
            Assert.IsInstanceOf<InOperation>(op);
        }
    }
}
