using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Camlex.NET;

namespace Camlex.NET.UnitTests
{
    [TestFixture]
    public class CamlexTests
    {
        [Test]
        public void test_THAT_single_eq_expression_IS_translated_sucessfully()
        {
            string caml = Camlex.Where(x => (string)x["Title"] == "testValue");

            string expected =
               @"<Where>
                  <Eq>
                     <FieldRef Name='Title' />
                     <Value Type='Text'>testValue</Value>
                  </Eq>
               </Where>";

            Assert.That(caml, Is.EqualTo(expected));
        }
    }
}
