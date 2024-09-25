using CamlexNET;

namespace CamlexOnlineX.Tests
{
    [TestFixture]
    public class ExpressionToCodeCamlexTests
    {
        [Test]
        public void test_THAT_eq_expression_with_string_IS_translated_sucessfully()
        {
            string xml =
                "<View><Query>" +
                "   <Where>" +
                "       <Eq>" +
                "           <FieldRef Name=\"Title\" />" +
                "           <Value Type=\"Text\">testValue</Value>" +
                "       </Eq>" +
                "   </Where>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            string code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().Where(x => (string)x[\"Title\"] == \"testValue\")"));
        }

        [Test]
        public void test_THAT_eq_expression_with_int_IS_translated_sucessfully()
        {
            string xml =
                "<View><Query>" +
                "   <Where>" +
                "       <Eq>" +
                "           <FieldRef Name=\"Count\" />" +
                "           <Value Type=\"Integer\">123</Value>" +
                "       </Eq>" +
                "   </Where>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            string code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().Where(x => (int)x[\"Count\"] == 123)"));
        }

        [Test]
        public void test_THAT_is_null_IS_translated_sucessfully()
        {
            string xml =
                "<View><Query>" +
                "   <Where>" +
                "       <IsNull>" +
                "           <FieldRef Name=\"Title\" />" +
                "       </IsNull>" +
                "   </Where>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            string code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().Where(x => x[\"Title\"] == null)"));
        }

        [Test]
        public void test_THAT_is_not_null_IS_translated_sucessfully()
        {
            string xml =
                "<View><Query>" +
                "   <Where>" +
                "       <IsNotNull>" +
                "           <FieldRef Name=\"Title\" />" +
                "       </IsNotNull>" +
                "   </Where>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            string code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().Where(x => x[\"Title\"] != null)"));
        }

        [Test]
        public void test_THAT_neq_or_isnull_expression_IS_translated_sucessfully()
        {
            string xml =
                "<View><Query>" +
                "  <Where>" +
                "    <Or>" +
                "      <Neq>" +
                "        <FieldRef Name=\"Status\" />" +
                "        <Value Type=\"Text\">Completed</Value>" +
                "      </Neq>" +
                "      <IsNull>" +
                "        <FieldRef Name=\"Status\" />" +
                "      </IsNull>" +
                "     </Or>" +
                "   </Where>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            string code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().Where(x => (string)x[\"Status\"] != \"Completed\" || x[\"Status\"] == null)"));
        }

        [Test]
        public void test_THAT_neq_and_isnull_with_orderby_expression_IS_translated_sucessfully()
        {
            string xml =
                "<View><Query>" +
                "  <Where>" +
                "    <And>" +
                "      <Neq>" +
                "        <FieldRef Name=\"Status\" />" +
                "        <Value Type=\"Text\">Completed</Value>" +
                "      </Neq>" +
                "      <IsNull>" +
                "        <FieldRef Name=\"Status\" />" +
                "      </IsNull>" +
                "     </And>" +
                "   </Where>" +
                "  <OrderBy>" +
                "    <FieldRef Name=\"Modified\" Ascending=\"False\" />" +
                "  </OrderBy>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            string code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().Where(x => (string)x[\"Status\"] != \"Completed\" && x[\"Status\"] == null).OrderBy(x => x[\"Modified\"] as Camlex.Desc)"));
        }

        [Test]
        public void test_THAT_beginswith_expression_IS_translated_sucessfully()
        {
            string xml =
                "<View><Query>" +
                "   <Where>" +
                "       <BeginsWith>" +
                "           <FieldRef Name=\"Count\" />" +
                "           <Value Type=\"Text\">foo</Value>" +
                "       </BeginsWith>" +
                "   </Where>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            string code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().Where(x => ((string)x[\"Count\"]).StartsWith(\"foo\"))"));
        }

        [Test]
        public void test_THAT_contains_expression_IS_translated_sucessfully()
        {
            string xml =
                "<View><Query>" +
                "   <Where>" +
                "       <Contains>" +
                "           <FieldRef Name=\"Count\" />" +
                "           <Value Type=\"Text\">foo</Value>" +
                "       </Contains>" +
                "   </Where>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            string code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().Where(x => ((string)x[\"Count\"]).Contains(\"foo\"))"));
        }

        [Test]
        public void test_THAT_contains_and_beginswith_expression_IS_translated_sucessfully()
        {
            string xml =
                "<View><Query>" +
                "   <Where>" +
                "       <And>" +
                "           <BeginsWith>" +
                "               <FieldRef Name=\"Title\" />" +
                "               <Value Type=\"Text\">Task</Value>" +
                "           </BeginsWith>" +
                "           <Contains>" +
                "               <FieldRef Name=\"Project\" />" +
                "               <Value Type=\"Text\">Camlex</Value>" +
                "           </Contains>" +
                "       </And>" +
                "   </Where>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            string code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().Where(x => ((string)x[\"Title\"]).StartsWith(\"Task\") && ((string)x[\"Project\"]).Contains(\"Camlex\"))"));
        }

        [Test]
        public void test_THAT_single_groupby_expression_IS_translated_sucessfully()
        {
            var xml =
                "<View><Query>" +
                "  <GroupBy>" +
                "    <FieldRef Name=\"field1\" />" +
                "  </GroupBy>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            string code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().GroupBy(x => x[\"field1\"])"));
        }

        [Test]
        public void test_THAT_multiple_groupby_expression_IS_translated_sucessfully()
        {
            var xml =
                "<View><Query>" +
                "  <GroupBy Collapse=\"True\" GroupLimit=\"10\">" +
                "    <FieldRef Name=\"field1\" />" +
                "    <FieldRef Name=\"field2\" />" +
                "  </GroupBy>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            string code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().GroupBy(x => new[] { x[\"field1\"], x[\"field2\"] }, true, 10)"));
        }

        [Test]
        public void test_THAT_data_ranges_overlap_expression_with_native_syntax_IS_translated_sucessfully()
        {
            var dt = new DateTime(2011, 10, 25, 21, 41, 44, 192);
            var xml =
                "<View><Query>" +
                "  <Where>" +
                "    <DateRangesOverlap>" +
                "      <FieldRef Name=\"StartField\" />" +
                "      <FieldRef Name=\"StopField\" />" +
                "      <FieldRef Name=\"RecurrenceID\" />" +
                "      <Value Type=\"DateTime\">" + dt.ToString("s") + "Z</Value>" +
                "    </DateRangesOverlap>" +
                "  </Where>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            string code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().Where(x => Camlex.DateRangesOverlap(x[\"StartField\"], x[\"StopField\"], x[\"RecurrenceID\"], new DateTime(2011, 10, 25, 21, 41, 44, 0)))"));
        }

        [Test]
        public void test_THAT_data_ranges_overlap_expression_with_string_contants_IS_translated_sucessfully()
        {
            var xml =
                "<View><Query>" +
                "  <Where>" +
                "    <DateRangesOverlap>" +
                "      <FieldRef Name=\"StartField\" />" +
                "      <FieldRef Name=\"StopField\" />" +
                "      <FieldRef Name=\"RecurrenceID\" />" +
                "      <Value Type=\"DateTime\"><Month /></Value>" +
                "    </DateRangesOverlap>" +
                "  </Where>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            string code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().Where(x => Camlex.DateRangesOverlap(x[\"StartField\"], x[\"StopField\"], x[\"RecurrenceID\"], (DataTypes.DateTime)Camlex.Month))"));
        }

        [Test]
        public void test_THAT_expression_with_native_and_string_based_syntax_IS_translated_sucessfully()
        {
            var xml =
                "<View><Query>" +
                "   <Where>" +
                "      <Or>" +
                "         <Eq>" +
                "            <FieldRef Name=\"Id\" />" +
                "            <Value Type=\"ContentTypeId\">0x05BB17ED9FB8406DA160511C12A3E0C2</Value>" +
                "         </Eq>" +
                "         <IsNotNull>" +
                "            <FieldRef Name=\"Description\" />" +
                "         </IsNotNull>" +
                "      </Or>" +
                "   </Where>" +
                "   <OrderBy>" +
                "      <FieldRef Name=\"_Author\" />" +
                "      <FieldRef Name=\"AuthoringDate\" />" +
                "      <FieldRef Name=\"AssignedTo\" Ascending=\"True\" />" +
                "   </OrderBy>" +
                "   <GroupBy Collapse=\"True\">" +
                "      <FieldRef Name=\"Title\" />" +
                "   </GroupBy>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            string code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().Where(x => x[\"Id\"] == (DataTypes.ContentTypeId)\"0x05BB17ED9FB8406DA160511C12A3E0C2\" || x[\"Description\"] != null).OrderBy(x => new[] { x[\"_Author\"], x[\"AuthoringDate\"], x[\"AssignedTo\"] as Camlex.Asc }).GroupBy(x => x[\"Title\"], true)"));
        }

        [Test]
        public void test_THAT_single_neq_expression_IS_translated_sucessfully()
        {
            string xml =
                "<View><Query>" +
                "   <Where>" +
                "       <Neq>" +
                "           <FieldRef Name=\"ID\" />" +
                "           <Value Type=\"Integer\">1</Value>" +
                "       </Neq>" +
                "   </Where>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            string code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().Where(x => (int)x[\"ID\"] != 1)"));
        }

        [Test]
        public void test_THAT_single_geq_expression_IS_translated_sucessfully()
        {
            string xml =
                "<View><Query>" +
                "   <Where>" +
                "       <Geq>" +
                "           <FieldRef Name=\"ID\" />" +
                "           <Value Type=\"Integer\">1</Value>" +
                "       </Geq>" +
                "   </Where>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            string code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().Where(x => (int)x[\"ID\"] >= 1)"));
        }

        [Test]
        public void test_THAT_single_gt_expression_IS_translated_sucessfully()
        {
            string xml =
                "<View><Query>" +
                "   <Where>" +
                "       <Gt>" +
                "           <FieldRef Name=\"ID\" />" +
                "           <Value Type=\"Integer\">1</Value>" +
                "       </Gt>" +
                "   </Where>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            string code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().Where(x => (int)x[\"ID\"] > 1)"));
        }

        [Test]
        public void test_THAT_single_leq_expression_IS_translated_sucessfully()
        {
            string xml =
                "<View><Query>" +
                "   <Where>" +
                "       <Leq>" +
                "           <FieldRef Name=\"ID\" />" +
                "           <Value Type=\"Integer\">1</Value>" +
                "       </Leq>" +
                "   </Where>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            string code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().Where(x => (int)x[\"ID\"] <= 1)"));
        }

        [Test]
        public void test_THAT_single_lt_expression_IS_translated_sucessfully()
        {
            string xml =
                "<View><Query>" +
                "   <Where>" +
                "       <Lt>" +
                "           <FieldRef Name=\"ID\" />" +
                "           <Value Type=\"Integer\">1</Value>" +
                "       </Lt>" +
                "   </Where>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            string code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().Where(x => (int)x[\"ID\"] < 1)"));
        }

        [Test]
        public void test_THAT_single_neq_expression_with_DataTypes_IS_translated_sucessfully()
        {
            var xml =
                "<View><Query>" +
                "   <Where>" +
                "       <Neq>" +
                "           <FieldRef Name=\"ID\" />" +
                "           <Value Type=\"Currency\">1.2345</Value>" +
                "       </Neq>" +
                "   </Where>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            string code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().Where(x => x[\"ID\"] != (DataTypes.Currency)\"1.2345\")"));
        }

        [Test]
        public void test_THAT_single_geq_expression_with_DataTypes_IS_translated_sucessfully()
        {
            var xml =
                "<View><Query>" +
                "   <Where>" +
                "       <Geq>" +
                "           <FieldRef Name=\"ID\" />" +
                "           <Value Type=\"Currency\">1.2345</Value>" +
                "       </Geq>" +
                "   </Where>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            string code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().Where(x => x[\"ID\"] >= (DataTypes.Currency)\"1.2345\")"));
        }

        [Test]
        public void test_THAT_single_gt_expression_with_DataTypes_IS_translated_sucessfully()
        {
            var xml =
                "<View><Query>" +
                "   <Where>" +
                "       <Gt>" +
                "           <FieldRef Name=\"ID\" />" +
                "           <Value Type=\"Currency\">1.2345</Value>" +
                "       </Gt>" +
                "   </Where>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            string code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().Where(x => x[\"ID\"] > (DataTypes.Currency)\"1.2345\")"));
        }

        [Test]
        public void test_THAT_single_leg_expression_with_DataTypes_IS_translated_sucessfully()
        {
            var xml =
                "<View><Query>" +
                "   <Where>" +
                "       <Leq>" +
                "           <FieldRef Name=\"ID\" />" +
                "           <Value Type=\"Currency\">1.2345</Value>" +
                "       </Leq>" +
                "   </Where>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            string code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().Where(x => x[\"ID\"] <= (DataTypes.Currency)\"1.2345\")"));
        }

        [Test]
        public void test_THAT_single_lt_expression_with_DataTypes_IS_translated_sucessfully()
        {
            var xml =
                "<View><Query>" +
                "   <Where>" +
                "       <Lt>" +
                "           <FieldRef Name=\"ID\" />" +
                "           <Value Type=\"Currency\">1.2345</Value>" +
                "       </Lt>" +
                "   </Where>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            string code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().Where(x => x[\"ID\"] < (DataTypes.Currency)\"1.2345\")"));
        }

        [Test]
        public void test_THAT_expression_IS_translated_sucessfully_with_query_tag()
        {
            var xml =
                "<View><Query>" +
                "   <Where>" +
                "       <Eq>" +
                "           <FieldRef Name=\"Title\" />" +
                "           <Value Type=\"Text\">testValue</Value>" +
                "       </Eq>" +
                "   </Where>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            var code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().Where(x => (string)x[\"Title\"] == \"testValue\")"));
        }

        [Test]
        public void test_THAT_expression_with_variable_guid_IS_translated_sucessfully()
        {
            var guid = "4feaf1f3-5b04-4d93-b0fc-4e48d0c60eed";
            var xml =
                "<View><Query>" +
                "   <Where>" +
                "       <Eq>" +
                "           <FieldRef ID=\"" + guid + "\" />" +
                "           <Value Type=\"Text\">val</Value>" +
                "       </Eq>" +
                "   </Where>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            var code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().Where(x => (string)x[new Guid(\"" + guid + "\")] == \"val\")"));
        }

        [Test]
        public void test_THAT_single_eq_expression_with_guid_variable_IS_translated_sucessfully()
        {
            var guid = "4feaf1f3-5b04-4d93-b0fc-4e48d0c60eed";
            var xml =
                "<View><Query>" +
                "   <Where>" +
                "       <Eq>" +
                "           <FieldRef Name=\"Foo\" />" +
                "           <Value Type=\"Guid\">" + guid + "</Value>" +
                "       </Eq>" +
                "   </Where>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            var code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().Where(x => (Guid)x[\"Foo\"] == new Guid(\"" + guid + "\"))"));
        }

        [Test]
        public void test_THAT_string_based_single_eq_expression_with_guid_variable_both_for_lvalue_and_rvalue_IS_translated_sucessfully()
        {
            var guid = "4feaf1f3-5b04-4d93-b0fc-4e48d0c60eed";
            string xml =
                "<View><Query>" +
                "   <Where>" +
                "       <Eq>" +
                "           <FieldRef ID=\"" + guid + "\" />" +
                "           <Value Type=\"Guid\">" + guid + "</Value>" +
                "       </Eq>" +
                "   </Where>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            var code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().Where(x => (Guid)x[new Guid(\"" + guid + "\")] == new Guid(\"" + guid + "\"))"));
        }

        [Test]
        public void test_THAT_groupby_expression_with_non_constant_parameters_IS_translated_sucessfully()
        {
            var xml =
                "<View><Query>" +
                "  <GroupBy>" +
                "    <FieldRef Name=\"field1\" />" +
                "  </GroupBy>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            var code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().GroupBy(x => x[\"field1\"])"));
        }

        [Test]
        public void test_THAT_2_eq_expression_with_andalso_ARE_translated_sucessfully()
        {
            string xml =
               "<View><Query>" +
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
               "    </Where>" +
               "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            var code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().Where(x => (string)x[\"Title\"] == \"testValue\" && (int)x[\"Count\"] == 1)"));
        }

        [Test]
        public void test_THAT_expression_with_1_andalso_and_1_orelse_ARE_translated_sucessfully()
        {
            string xml =
                "<View><Query>" +
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
                "   </Where>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            var code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().Where(x => (string)x[\"Title\"] == \"testValue\" && (int)x[\"Count1\"] == 1 || (int)x[\"Count2\"] == 2)"));
        }

        [Test]
        public void test_THAT_expression_with_2_andalso_and_1_orelse_ARE_translated_sucessfully()
        {
            string xml =
                "<View><Query>" +
                "<Where>" +
                "  <And>" +
                "    <Or>" +
                "      <And>" +
                "        <Eq>" +
                "          <FieldRef Name=\"Title\" />" +
                "          <Value Type=\"Text\">testValue</Value>" +
                "        </Eq>" +
                "        <Eq>" +
                "          <FieldRef Name=\"Count1\" />" +
                "          <Value Type=\"Integer\">1</Value>" +
                "        </Eq>" +
                "      </And>" +
                "      <Eq>" +
                "        <FieldRef Name=\"Count2\" />" +
                "        <Value Type=\"Integer\">2</Value>" +
                "      </Eq>" +
                "    </Or>" +
                "    <Eq>" +
                "      <FieldRef Name=\"foo\" />" +
                "      <Value Type=\"Integer\">1</Value>" +
                "    </Eq>" +
                "  </And>" +
                "</Where>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            var code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().Where(x => ((string)x[\"Title\"] == \"testValue\" && (int)x[\"Count1\"] == 1 || (int)x[\"Count2\"] == 2) && (int)x[\"foo\"] == 1)"));
        }

        [Test]
        public void test_THAT_lookup_id_field_ref_with_name_IS_translated_successfully()
        {
            string xml =
                "<View><Query>" +
                "   <Where>" +
                "       <Eq>" +
                "           <FieldRef Name=\"Ref\" LookupId=\"True\" />" +
                "           <Value Type=\"Lookup\">123</Value>" +
                "       </Eq>" +
                "   </Where>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            var code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().Where(x => x[\"Ref\"] == (DataTypes.LookupId)\"123\")"));
        }

        [Test]
        public void test_THAT_lookup_id_field_ref_with_guid_IS_translated_successfully()
        {
            string xml =
                "<View><Query>" +
                "   <Where>" +
                "       <Eq>" +
                "           <FieldRef ID=\"4feaf1f3-5b04-4d93-b0fc-4e48d0c60eed\" LookupId=\"True\" />" +
                "           <Value Type=\"Lookup\">123</Value>" +
                "       </Eq>" +
                "   </Where>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            var code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().Where(x => x[new Guid(\"4feaf1f3-5b04-4d93-b0fc-4e48d0c60eed\")] == (DataTypes.LookupId)\"123\")"));
        }

        [Test]
        public void test_THAT_lookup_value_IS_translated_successfully()
        {
            string xml =
                "<View><Query>" +
                "   <Where>" +
                "       <Eq>" +
                "           <FieldRef Name=\"Ref\" />" +
                "           <Value Type=\"Lookup\">123</Value>" +
                "       </Eq>" +
                "   </Where>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            var code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().Where(x => x[\"Ref\"] == (DataTypes.LookupValue)\"123\")"));
        }

        [Test]
        public void test_THAT_single_orderby_expression_IS_translated_sucessfully()
        {
            var xml =
                "<View><Query>" +
                "  <OrderBy>" +
                "    <FieldRef Name=\"field1\" Ascending=\"False\" />" +
                "  </OrderBy>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            var code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().OrderBy(x => x[\"field1\"] as Camlex.Desc)"));
        }

        [Test]
        public void test_THAT_multiple_orderby_expression_IS_translated_sucessfully()
        {
            var xml =
                "<View><Query>" +
                "  <OrderBy>" +
                "    <FieldRef Name=\"field1\" />" +
                "    <FieldRef Name=\"field2\" Ascending=\"False\" />" +
                "    <FieldRef Name=\"field3\" Ascending=\"True\" />" +
                "  </OrderBy>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            var code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().OrderBy(x => new[] { x[\"field1\"], x[\"field2\"] as Camlex.Desc, x[\"field3\"] as Camlex.Asc })"));
        }

        [Test]
        public void test_THAT_orderby_expression_with_non_constant_parameters_IS_translated_sucessfully()
        {
            var xml =
                "<View><Query>" +
                "<OrderBy>" +
                "  <FieldRef ID=\"fa564e0f-0c70-4ab9-b863-0177e6ddd247\" />" +
                "  <FieldRef ID=\"28cf69c5-fa48-462a-b5cd-27b6f9d2bd5f\" Ascending=\"True\" />" +
                "</OrderBy>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            var code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().OrderBy(x => new[] { x[new Guid(\"fa564e0f-0c70-4ab9-b863-0177e6ddd247\")], x[new Guid(\"28cf69c5-fa48-462a-b5cd-27b6f9d2bd5f\")] as Camlex.Asc })"));
        }

        [Test]
        public void test_THAT_2_order_by_with_differenet_params_ARE_joined_properly()
        {
            var xml =
                "<View><Query>" +
                "<OrderBy>" +
                "  <FieldRef ID=\"fa564e0f-0c70-4ab9-b863-0177e6ddd247\" Ascending=\"False\" />" +
                "  <FieldRef ID=\"28cf69c5-fa48-462a-b5cd-27b6f9d2bd5f\" Ascending=\"True\" />" +
                "</OrderBy>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            var code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().OrderBy(x => new[] { x[new Guid(\"fa564e0f-0c70-4ab9-b863-0177e6ddd247\")] as Camlex.Desc, x[new Guid(\"28cf69c5-fa48-462a-b5cd-27b6f9d2bd5f\")] as Camlex.Asc })"));
        }

        [Test]
        public void test_THAT_user_id_field_ref_with_name_IS_translated_successfully()
        {
            string xml =
                "<View><Query>" +
                "   <Where>" +
                "       <Eq>" +
                "           <FieldRef Name=\"foo\" LookupId=\"True\" />" +
                "           <Value Type=\"User\">123</Value>" +
                "       </Eq>" +
                "   </Where>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            var code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().Where(x => x[\"foo\"] == (DataTypes.UserId)\"123\")"));
        }

        [Test]
        public void test_THAT_user_id_field_ref_with_guid_IS_translated_successfully()
        {
            string xml =
                "<View><Query>" +
                "   <Where>" +
                "       <Eq>" +
                "           <FieldRef ID=\"4feaf1f3-5b04-4d93-b0fc-4e48d0c60eed\" LookupId=\"True\" />" +
                "           <Value Type=\"User\">123</Value>" +
                "       </Eq>" +
                "   </Where>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            var code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().Where(x => x[new Guid(\"4feaf1f3-5b04-4d93-b0fc-4e48d0c60eed\")] == (DataTypes.UserId)\"123\")"));
        }

        [Test]
        public void test_THAT_expression_with_user_IS_translated_successfully()
        {
            string xml =
                "<View><Query>" +
                "   <Where>" +
                "       <Eq>" +
                "           <FieldRef Name=\"foo\" />" +
                "           <Value Type=\"User\">Foo Bar</Value>" +
                "       </Eq>" +
                "   </Where>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            var code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().Where(x => x[\"foo\"] == (DataTypes.User)\"Foo Bar\")"));
        }

        [Test]
        public void test_THAT_expression_with_user_id_call_IS_translated_successfully()
        {
            string xml =
                "<View><Query>" +
                "   <Where>" +
                "       <Eq>" +
                "           <FieldRef Name=\"foo\" />" +
                "           <Value Type=\"Integer\"><UserID /></Value>" +
                "       </Eq>" +
                "   </Where>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            var code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().Where(x => x[\"foo\"] == (DataTypes.Integer)Camlex.UserID)"));
        }

        [Test]
        public void test_THAT_viewfields_with_single_field_title_IS_translated_sucessfully()
        {
            string xml =
                "<View><Query>" +
                "<ViewFields>" +
                "   <FieldRef Name=\"Title\" />" +
                "</ViewFields>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            var code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().ViewFields(x => x[\"Title\"], true)"));
        }

        [Test]
        public void test_THAT_viewfields_with_several_fields_IS_translated_sucessfully()
        {
            string xml =
                "<View><Query>" +
                "<ViewFields>" +
                "<FieldRef Name=\"Title\" /><FieldRef Name=\"Status\" />" +
                "</ViewFields>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            var code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().ViewFields(x => new[] { x[\"Title\"], x[\"Status\"] }, true)"));
        }

        [Test]
        public void test_THAT_viewfields_with_several_fields_ids_with_parent_tag_IS_translated_sucessfully()
        {
            string xml =
                "<View><Query>" +
                "<ViewFields>" +
                    "<FieldRef ID=\"fa564e0f-0c70-4ab9-b863-0177e6ddd247\" />" +
                    "<FieldRef ID=\"28cf69c5-fa48-462a-b5cd-27b6f9d2bd5f\" />" +
                "</ViewFields>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            var code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().ViewFields(x => new[] { x[new Guid(\"fa564e0f-0c70-4ab9-b863-0177e6ddd247\")], x[new Guid(\"28cf69c5-fa48-462a-b5cd-27b6f9d2bd5f\")] }, true)"));
        }

        [Test]
        public void test_THAT_in_expression_IS_translated_sucessfully()
        {
            var xml =
                "<View><Query>" +
                "  <Where>" +
                "    <In>" +
                "      <FieldRef Name=\"test\" />" +
                "      <Values>" +
                "        <Value Type=\"Integer\">0</Value>" +
                "        <Value Type=\"Integer\">1</Value>" +
                "        <Value Type=\"Integer\">2</Value>" +
                "      </Values>" +
                "    </In>" +
                "  </Where>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            var code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().Where(x => new[] { 0, 1, 2 }.Contains((int)x[\"test\"]))"));
        }

        [Test]
        public void test_THAT_includes_expression_IS_translated_sucessfully()
        {
            string xml =
                "<View><Query>" +
                "   <Where>" +
                "       <Includes>" +
                "           <FieldRef Name=\"Count\" />" +
                "           <Value Type=\"Text\">foo</Value>" +
                "       </Includes>" +
                "   </Where>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            string code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().Where(x => ((object)(string)x[\"Count\"]).Includes((object)\"foo\"))"));
        }

        [Test]
        public void test_THAT_notincludes_expression_IS_translated_sucessfully()
        {
            string xml =
                "<View><Query>" +
                "   <Where>" +
                "       <NotIncludes>" +
                "           <FieldRef Name=\"Count\" />" +
                "           <Value Type=\"Text\">foo</Value>" +
                "       </NotIncludes>" +
                "   </Where>" +
                "</Query></View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            string code = ExpressionToCodeLib.ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("Camlex.Query().Where(x => !((object)(string)x[\"Count\"]).Includes((object)\"foo\"))"));
        }
    }
}
