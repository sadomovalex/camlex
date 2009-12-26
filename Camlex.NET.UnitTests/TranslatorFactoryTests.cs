using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Camlex.NET.Interfaces;
using NUnit.Framework;

namespace Camlex.NET.UnitTests
{
    [TestFixture]
    public class TranslatorFactoryTests
    {
        [Test]
        public void test_WHEN_expression_is_equal_THEN_eq_translator_is_created()
        {
            var tr = new TranslatorFactory().Create(ExpressionType.Equal);
            Assert.That(tr, Is.InstanceOf(typeof(EqTranslator)));
        }
    }
}
