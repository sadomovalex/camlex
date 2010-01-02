using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Camlex.NET.Impl;
using Camlex.NET.Impl.Eq;
using Camlex.NET.Impl.Factories;
using Camlex.NET.Interfaces;
using NUnit.Framework;
using Rhino.Mocks;

namespace Camlex.NET.UnitTests
{
    [TestFixture]
    public class TranslatorFactoryTests
    {
        [Test]
        public void test_WHEN_eq_expression_THEN_generic_translator_is_created()
        {
            // arrange
            var analyzerFactory = MockRepository.GenerateStub<IAnalyzerFactory>();
            analyzerFactory.Stub(f => f.Create(ExpressionType.Equal)).Return(null);

            // act
            var tr = new TranslatorFactory(analyzerFactory).Create(ExpressionType.Equal);

            // assert
            Assert.That(tr, Is.InstanceOf(typeof(GenericTranslator)));
        }
    }
}
