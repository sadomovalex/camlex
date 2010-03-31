using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CamlexNET.UnitTests.Helpers;
using NUnit.Framework;

namespace CamlexNET.UnitTests
{
    [TestFixture]
    public class CamlexJoinsTest
    {
        [Test]
        public void test_THAT_2_eq_expression_with_andalso_ARE_translated_sucessfully()
        {
            string caml = Camlex.Query().Where(x => (string)x["Title"] == "testValue" &&
                                            (int)x["Count"] == 1).ToString();

            string expected =
                //               "<Query>" +
               "    <Where>" +
               "        <And>" +
               "            <Eq>" +
               "                <FieldRef Name=\"Title\" />" +
               "                <Value Type=\"Text\">testValue</Value>" +
               "            </Eq>" +
               "            <Eq>" +
               "                <FieldRef Name=\"Count\" />" +
               "                <Value Type=\"Integer\">1</Value>" +
               "            </Eq>" +
               "        </And>" +
               "    </Where>";
            //               "</Query>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_expression_with_2_andalso_and_1_orelse_ARE_translated_sucessfully()
        {
            string caml = Camlex.Query().Where(x => ((string)x["Title"] == "testValue" &&
                                            (int)x["Count1"] == 1) || (int)x["Count2"] == 2).ToString();

            string expected =
                //                "<Query>" +
                "   <Where>" +
                "       <Or>" +
                "           <And>" +
                "               <Eq>" +
                "                   <FieldRef Name=\"Title\" />" +
                "                   <Value Type=\"Text\">testValue</Value>" +
                "               </Eq>" +
                "               <Eq>" +
                "                   <FieldRef Name=\"Count1\" />" +
                "                   <Value Type=\"Integer\">1</Value>" +
                "               </Eq>" +
                "           </And>" +
                "           <Eq>" +
                "               <FieldRef Name=\"Count2\" />" +
                "               <Value Type=\"Integer\">2</Value>" +
                "           </Eq>" +
                "       </Or>" +
                "   </Where>";
            //                "</Query>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

    }
}
