using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Camlex.NET.Impl;
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

        [Test]
        public void test_THAT_left_operand_IS_recognized_properly()
        {
            var analyzer = new EqAnalyzer();
            Expression<Func<SPItem, bool>> expr = x => (string)x["Title"] == "testValue";
            var leftOperand = analyzer.GetLeftOperand(expr);
            Assert.That(leftOperand, Is.InstanceOf(typeof(IndexerWithConstantParameterOperand)));
        }

        [Test]
        public void test_THAT_right_operand_IS_recognized_properly()
        {
            var analyzer = new EqAnalyzer();
            Expression<Func<SPItem, bool>> expr = x => (string)x["Title"] == "testValue";
            var rightOperand = analyzer.GetRightOperand(expr);
            Assert.That(rightOperand, Is.InstanceOf(typeof(ConstantOperand)));
        }
    }
}
