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
using System.Linq;
using System.Xml.Linq;
using CamlexNET.Impl.Operands;
using CamlexNET.Impl.Operations.Membership;
using CamlexNET.Interfaces;
using CamlexNET.Interfaces.ReverseEngeneering;

namespace CamlexNET.Impl.ReverseEngeneering.Caml.Analyzers
{
    internal class ReMembershipAnalyzer : ReBaseAnalyzer
    {
        public ReMembershipAnalyzer(XElement el, IReOperandBuilder operandBuilder)
            : base(el, operandBuilder)
        {
        }

        public override bool IsValid()
        {
            if (!base.IsValid())
            {
                return false;
            }
            if (el.Attributes().Count() == 0)
            {
                return false;
            }
            if (!this.hasValidFieldRefElement())
            {
                return false;
            }
            if (!this.hasValidMembershipElement())
            {
                return false;
            }
            return true;
        }

        protected bool hasValidFieldRefElement()
        {
            if (el.Elements(Tags.FieldRef).Count() != 1)
            {
                return false;
            }
            var fieldRefElement = el.Elements(Tags.FieldRef).First();
            var isIdOrNamePresent = fieldRefElement.Attributes()
                    .Any(a => a.Name == Attributes.ID || a.Name == Attributes.Name);
            if (!isIdOrNamePresent) return false;

            return true;
        }

        protected bool hasValidMembershipElement()
        {
            if (el.Name != Tags.Membership)
            {
                return false;
            }
            var typeAttribute = el.Attributes()
                .FirstOrDefault(a => a.Name == Attributes.Type);
            if (typeAttribute == null)
            {
                return false;
            }

            var validTypes = new string[]
            {
                Camlex.Membership_SPWebAllUsers,
                Camlex.Membership_SPGroup,
                Camlex.Membership_SPWebGroups,
                Camlex.Membership_CurrentUserGroups,
                Camlex.Membership_SPWebUsers
            };

            if (!validTypes.Any(t => t == typeAttribute.Value))
            {
                return false;
            }


            if (typeAttribute.Value == Camlex.Membership_SPGroup)
            {
                var idAttribute = el.Attributes().FirstOrDefault(a => a.Name == Attributes.ID);
                if (idAttribute == null)
                {
                    return false;
                }
                if (!int.TryParse(idAttribute.Value, out var groupId))
                {
                    return false;
                }
            }

            return true;
        }

        public override IOperation GetOperation()
        {
            if (!this.IsValid())
            {
                throw new CamlAnalysisException(string.Format(
                    "Can't create {0} from the following xml: {1}", typeof(MembershipOpeartion).Name, el));
            }

            var fieldRefElement = el.Elements(Tags.FieldRef).First();
            var typeAttribute = el.Attributes(Attributes.Type).First();

            var fieldRefOperand = operandBuilder.CreateFieldRefOperand(fieldRefElement);
            Camlex.MembershipType membershipType = null;

            switch (typeAttribute.Value)
            {
                case Camlex.Membership_SPWebAllUsers:
                    membershipType = new Camlex.SPWebAllUsers();
                    break;

                case Camlex.Membership_SPGroup:
                    var groupId = int.Parse(el.Attributes(Attributes.ID).First().Value);
                    membershipType = new Camlex.SPGroup(groupId);
                    break;

                case Camlex.Membership_SPWebGroups:
                    membershipType = new Camlex.SPWebGroups();
                    break;

                case Camlex.Membership_CurrentUserGroups:
                    membershipType = new Camlex.CurrentUserGroups();
                    break;

                case Camlex.Membership_SPWebUsers:
                    membershipType = new Camlex.SPWebUsers();
                    break;
            }
            

            return new MembershipOpeartion(null,
                fieldRefOperand, membershipType);
        }
    }
}
