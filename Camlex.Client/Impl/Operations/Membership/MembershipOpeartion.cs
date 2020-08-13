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
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;
using CamlexNET.Impl.Operands;
using CamlexNET.Interfaces;

namespace CamlexNET.Impl.Operations.Membership
{
    internal class MembershipOpeartion : OperationBase
    {
        protected IOperand fieldRefOperand;
        protected Camlex.MembershipType membershipType;

        public MembershipOpeartion(IOperationResultBuilder operationResultBuilder, IOperand fieldRefOperand, Camlex.MembershipType membershipType) :
            base(operationResultBuilder)
        {
            this.fieldRefOperand = fieldRefOperand;
            this.membershipType = membershipType;
        }

        public override IOperationResult ToResult()
        {
            XElement result;
            if (membershipType is Camlex.SPGroup)
            {
                result = new XElement(Tags.Membership,
                                            new XAttribute(Attributes.Type, membershipType),
                                            new XAttribute(Attributes.ID, ((Camlex.SPGroup)membershipType).GroupId),
                                            fieldRefOperand.ToCaml());
            }
            else
            {
                result = new XElement(Tags.Membership,
                                            new XAttribute(Attributes.Type, membershipType),
                                            fieldRefOperand.ToCaml());
            }

            return operationResultBuilder.CreateResult(result);
        }

        public override Expression ToExpression()
        {
            if (this.fieldRefOperand == null)
            {
                throw new NullReferenceException("fieldRefOperand");
            }
            if (this.membershipType == null)
            {
                throw new NullReferenceException("membershipType");
            }

            var mi = typeof(Camlex).GetMethod(ReflectionHelper.Membership,
                                              new[]
                                                  {
                                                       typeof (object), typeof (Camlex.MembershipType)
                                                  });

            NewExpression membershipTypeExpression;

            if (membershipType is Camlex.SPGroup)
            {
                membershipTypeExpression = Expression.New(membershipType.GetType().GetConstructor(new[] { typeof(int) }), 
                                                            Expression.Constant(((Camlex.SPGroup)membershipType).GroupId));
            }
            else
            {
                membershipTypeExpression = Expression.New(membershipType.GetType());
            }

            return Expression.Call(mi, this.fieldRefOperand.ToExpression(), membershipTypeExpression);
        }
    }
}
