using CamlexNET.Impl.Operands;
using NUnit.Framework;

namespace CamlexNET.UnitTests.Operands
{
    [TestFixture]
    public class FieldRefOperandTests
    {
        [Test]
        public void test_THAT_field_ref_IS_rendered_to_caml_properly()
        {
            var fr = new FieldRefOperand("Title");
            string caml = fr.ToCaml().ToString();
            Assert.That(caml, Is.EqualTo("<FieldRef Name=\"Title\" />"));
        }
    }
}


