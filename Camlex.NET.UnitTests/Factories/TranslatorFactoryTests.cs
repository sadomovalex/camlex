using System;
using System.Linq.Expressions;
using Camlex.NET.Impl;
using Camlex.NET.Impl.Factories;
using Camlex.NET.Interfaces;
using Microsoft.SharePoint;
using NUnit.Framework;
using Rhino.Mocks;

namespace Camlex.NET.UnitTests.Factories
{
    [TestFixture]
    public class TranslatorFactoryTests
    {
        [Test]
        public void test_WHEN_eq_expression_THEN_generic_translator_is_created()
        {
            // arrange
            var analyzerFactory = MockRepository.GenerateStub<IAnalyzerFactory>();
            analyzerFactory.Stub(f => f.Create(null)).Return(null).IgnoreArguments();

            Expression<Func<SPItem, bool>> expr = x => (string) x["Foo"] == "foo";
            // act
            var tr = new TranslatorFactory(analyzerFactory).Create(expr);

            // assert
            Assert.That(tr, Is.InstanceOf<GenericTranslator>());
        }
    }
}


