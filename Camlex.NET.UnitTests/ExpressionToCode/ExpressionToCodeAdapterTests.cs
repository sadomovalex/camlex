using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CamlexNET.ExpressionToCodeAdapter;
using NUnit.Framework;

namespace CamlexNET.UnitTests.ExpressionToCode
{
    [TestFixture]
    public class ExpressionToCodeAdapterTests
    {
        [Test]
        public void test_THAT_single_eq_expression_IS_translated_sucessfully()
        {
            string xml =
                "<Query>" +
                "   <Where>" +
                "       <Eq>" +
                "           <FieldRef Name=\"Title\" />" +
                "           <Value Type=\"Text\">testValue</Value>" +
                "       </Eq>" +
                "   </Where>" +
                "</Query>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            string code = Adapter.ToCode(expr);
            Assert.That(code, Is.EqualTo(""));
        }
    }
}
