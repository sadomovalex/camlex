using CamlexNET.ExpressionToCodeAdapter;
using NUnit.Framework;

namespace CamlexNET.ExpressionToCode.UnitTests
{
    [TestFixture]
    public class ExpressionToCodeAdapterTests
    {
        [Test]
        public void test_THAT_eq_expression_IS_translated_sucessfully()
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
            Assert.That(code, Is.EqualTo("Camlex.Query().Where(x => (string)x[\"Title\"] == \"testValue\")"));
        }

        [Test]
        public void test_THAT_is_null_IS_translated_sucessfully()
        {
            string xml =
                "<Query>" +
                "   <Where>" +
                "       <IsNull>" +
                "           <FieldRef Name=\"Title\" />" +
                "       </IsNull>" +
                "   </Where>" +
                "</Query>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            string code = Adapter.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().Where(x => x[\"Title\"] == null)"));
        }

        [Test]
        public void test_THAT_is_not_null_IS_translated_sucessfully()
        {
            string xml =
                "<Query>" +
                "   <Where>" +
                "       <IsNotNull>" +
                "           <FieldRef Name=\"Title\" />" +
                "       </IsNotNull>" +
                "   </Where>" +
                "</Query>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            string code = Adapter.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().Where(x => x[\"Title\"] != null)"));
        }
    }
}
