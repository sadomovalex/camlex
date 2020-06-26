#region Copyright(c) Alexey Sadomov, Vladimir Timashkov. All Rights Reserved.
// -----------------------------------------------------------------------------
// Copyright(c) 2010 Alexey Sadomov, Vladimir Timashkov. All Rights Reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//
//   1. No Trademark License - Microsoft Public License (Ms-PL) does not grant you rights to use
//      authors names, logos, or trademarks.
//   2. If you distribute any portion of the software, you must retain all copyright,
//      patent, trademark, and attribution notices that are present in the software.
//   3. If you distribute any portion of the software in source code form, you may do
//      so only under this license by including a complete copy of Microsoft Public License (Ms-PL)
//      with your distribution. If you distribute any portion of the software in compiled
//      or object code form, you may only do so under a license that complies with
//      Microsoft Public License (Ms-PL).
//   4. The names of the authors may not be used to endorse or promote products
//      derived from this software without specific prior written permission.
//
// The software is licensed "as-is." You bear the risk of using it. The authors
// give no express warranties, guarantees or conditions. You may have additional consumer
// rights under your local laws which this license cannot change. To the extent permitted
// under your local laws, the authors exclude the implied warranties of merchantability,
// fitness for a particular purpose and non-infringement.
// -----------------------------------------------------------------------------
#endregion
using System;
using CamlexNET.Impl.Operands;
using CamlexNET.Impl.Operations.AndAlso;
using CamlexNET.Impl.Operations.Membership;
using CamlexNET.Impl.Operations.Eq;
using CamlexNET.Impl.Operations.OrElse;
using NUnit.Framework;

namespace CamlexNET.UnitTests.ReverseEngeneering.Operations
{
    [TestFixture]
    public class MembershipOperationTests
    {
        [Test]
        public void test_THAT_membership_operation_with_spweballusers_IS_converted_to_expression_correctly()
        {
            var op1 = new FieldRefOperand("Field");
            var mt = new Camlex.SPWebAllUsers();
            var typeName = mt.GetType().Name;
            var op = new MembershipOpeartion(null, op1, mt);

            var expr = op.ToExpression();
            Assert.That(expr.ToString(),
                Is.EqualTo(string.Format("Membership(x.get_Item(\"Field\"), new {0}())", typeName)));
        }

        [Test]
        public void test_THAT_membership_operation_with_spgroup_IS_converted_to_expression_correctly()
        {
            var op1 = new FieldRefOperand("Field");
            var mt = new Camlex.SPGroup(2);
            var typeName = mt.GetType().Name;
            var op = new MembershipOpeartion(null, op1, mt);

            var expr = op.ToExpression();
            Assert.That(expr.ToString(),
                Is.EqualTo(string.Format("Membership(x.get_Item(\"Field\"), new {0}({1}))", typeName, 2)));
        }

        [Test]
        public void test_THAT_membership_operation_with_spwebgroups_IS_converted_to_expression_correctly()
        {
            var op1 = new FieldRefOperand("Field");
            var mt = new Camlex.SPWebGroups();
            var typeName = mt.GetType().Name;
            var op = new MembershipOpeartion(null, op1, mt);

            var expr = op.ToExpression();
            Assert.That(expr.ToString(),
                Is.EqualTo(string.Format("Membership(x.get_Item(\"Field\"), new {0}())", typeName)));
        }

        [Test]
        public void test_THAT_membership_operation_with_currentusergroups_IS_converted_to_expression_correctly()
        {
            var op1 = new FieldRefOperand("Field");
            var mt = new Camlex.CurrentUserGroups();
            var typeName = mt.GetType().Name;
            var op = new MembershipOpeartion(null, op1, mt);

            var expr = op.ToExpression();
            Assert.That(expr.ToString(),
                Is.EqualTo(string.Format("Membership(x.get_Item(\"Field\"), new {0}())", typeName)));
        }

        [Test]
        public void test_THAT_membership_operation_with_spwebusers_IS_converted_to_expression_correctly()
        {
            var op1 = new FieldRefOperand("Field");
            var mt = new Camlex.SPWebUsers();
            var typeName = mt.GetType().Name;
            var op = new MembershipOpeartion(null, op1, mt);

            var expr = op.ToExpression();
            Assert.That(expr.ToString(),
                Is.EqualTo(string.Format("Membership(x.get_Item(\"Field\"), new {0}())", typeName)));
        }        
    }
}
