﻿#region Copyright(c) Alexey Sadomov, Vladimir Timashkov. All Rights Reserved.
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
using CamlexNET.Interfaces;
using CamlexNET.Interfaces.ReverseEngeneering;

namespace CamlexNET.Impl.ReverseEngeneering
{
    internal abstract class ReBinaryExpressionBaseAnalyzer : ReBaseAnalyzer
    {
        protected ReBinaryExpressionBaseAnalyzer(XElement el, IReOperandBuilder operandBuilder)
            : base(el, operandBuilder)
        {
        }

        public override bool IsValid()
        {
            if (!base.IsValid())
            {
                return false;
            }
            if (el.Attributes().Count() > 0)
            {
                return false;
            }

            // check presence of FieldRef tag with ID or Name attribute
            if (!this.hasValidFieldRefElement())
            {
                return false;
            }

            // check presence of Value tag with Type attribute
            if (!this.hasValidValueElement())
            {
                return false;
            }

            return true;
        }

        protected bool hasValidFieldRefElement()
        {
            if (el.Elements(Tags.FieldRef).Count() != 1) return false;
            var fieldRefElement = el.Elements(Tags.FieldRef).First();
            var isIdOrNamePresent = fieldRefElement.Attributes()
                .Any(a => a.Name == Attributes.ID || a.Name == Attributes.Name);
            if (!isIdOrNamePresent) return false;
            return true;
        }

        protected virtual bool hasValidValueElement()
        {
            if (el.Elements(Tags.Value).Count() != 1) return false;
            var valueElement = el.Elements(Tags.Value).First();
            return this.hasValidValueElement(valueElement);
        }

        protected bool hasValidValueElement(XElement valueElement)
        {
            var typeAttribute = valueElement.Attributes()
                .Where(a => a.Name == Attributes.Type).FirstOrDefault();
            if (typeAttribute == null) return false;

            // whether there is LookupId attribute in FieldRef tag
//            var fieldRefElement = el.Elements(Tags.FieldRef).First();
//            var isLookupId = fieldRefElement.Attributes()
//                .Any(a => a.Name == Attributes.LookupId);

            bool isIntegerForUserId = typeAttribute.Value == typeof(DataTypes.Integer).Name &&
                                      valueElement.Elements().Count() == 1 &&
                                      valueElement.Elements().Any(e => e.Name == ReflectionHelper.UserID);

            // value can't be empty for certain types
            if (!isIntegerForUserId && string.IsNullOrEmpty(valueElement.Value) && !this.isEmptyValueAcceptable(typeAttribute.Value))
            {
                return false;
            }

            // check whether we support this value type and whether the value is correct
            return isValueValid(typeAttribute.Value, valueElement.Value, isIntegerForUserId);
        }

        private bool isEmptyValueAcceptable(string valueType)
        {
            if (valueType == typeof(DataTypes.Calculated).Name) return true;
            if (valueType == typeof(DataTypes.Choice).Name) return true;
            if (valueType == typeof(DataTypes.Computed).Name) return true;
            if (valueType == typeof(DataTypes.GridChoice).Name) return true;
            if (valueType == typeof(DataTypes.LookupValue).Name) return true;
            if (valueType == typeof(DataTypes.LookupMultiValue).Name) return true;
            if (valueType == typeof(DataTypes.MultiChoice).Name) return true;
            if (valueType == typeof(DataTypes.Note).Name) return true;
            if (valueType == typeof(DataTypes.Text).Name) return true;
            if (valueType == typeof(DataTypes.URL).Name) return true;
            return false;
        }

        protected virtual bool isValueValid(string valueType, string value, bool isIntegerForUserId)
        {
            // note: currently we decided to not do checks that values in string based value operands
            // in order to have compatibility with direct camlex (c# -> caml) where we don't do them.
            // Also we don't want to make calls to the Sharepoint classes in order to have possiblity
            // to cimpile code for client OM
            try
            {
                //if (valueType == typeof(DataTypes.AllDayEvent).Name) new BooleanValueOperand(value);
                if (valueType == typeof(DataTypes.AllDayEvent).Name) { }
                //else if (valueType == typeof(DataTypes.Attachments).Name) new BooleanValueOperand(value);
                else if (valueType == typeof(DataTypes.Attachments).Name) { }
                else if (valueType == typeof(DataTypes.Boolean).Name) new BooleanValueOperand(value);
                else if (valueType == typeof(DataTypes.Calculated).Name) { }
                else if (valueType == typeof(DataTypes.Choice).Name) { }
                else if (valueType == typeof(DataTypes.Computed).Name) { }
                //else if (valueType == typeof(DataTypes.ContentTypeId).Name) new SPContentTypeId(value);
                else if (valueType == typeof(DataTypes.ContentTypeId).Name) { }
                //else if (valueType == typeof(DataTypes.Counter).Name) new IntegerValueOperand(value);
                else if (valueType == typeof(DataTypes.Counter).Name) { }
                //else if (valueType == typeof(DataTypes.CrossProjectLink).Name) new BooleanValueOperand(value);
                else if (valueType == typeof(DataTypes.CrossProjectLink).Name) { }
                //else if (valueType == typeof(DataTypes.Currency).Name) new IntegerValueOperand(value);
                else if (valueType == typeof(DataTypes.Currency).Name) { }
                //else if (valueType == typeof(DataTypes.DateTime).Name) new DateTimeValueOperand(value, false);
                else if (valueType == typeof(DataTypes.DateTime).Name) { }
                //else if (valueType == typeof(DataTypes.Error).Name) return false; // NOT SUPPORTED
                else if (valueType == typeof(DataTypes.Error).Name) { }
                else if (valueType == typeof(DataTypes.File).Name) { }
                else if (valueType == typeof(DataTypes.GridChoice).Name) { }
                else if (valueType == typeof(DataTypes.Guid).Name) new GuidValueOperand(value);
                else if (valueType == typeof(DataTypes.Integer).Name)
                {
                    // DataTypes.Integer also can be used for <UserID />. See http://sadomovalex.blogspot.com/2011/08/camlexnet-24-is-released.html
                    if (!isIntegerForUserId)
                    {
                        new IntegerValueOperand(value);
                    }
                }
                //else if (valueType == typeof(DataTypes.Invalid).Name) return false; // NOT SUPPORTED
                else if (valueType == typeof(DataTypes.Invalid).Name) { }
                //else if (valueType == typeof(DataTypes.LookupId).Name) new LookupIdValueOperand(value);
                //else if (valueType == typeof(DataTypes.LookupValue).Name) new LookupValueValueOperand(value);
                else if (valueType == typeof(DataTypes.Lookup).Name) { }
                else if (valueType == typeof(DataTypes.LookupMulti).Name) { }
                //else if (valueType == typeof(DataTypes.MaxItems).Name) new IntegerValueOperand(value);
                else if (valueType == typeof(DataTypes.MaxItems).Name) { }
                //else if (valueType == typeof(DataTypes.ModStat).Name)
                //{
                //    if (!IsEnumValueValid(typeof(SPModerationStatusType), value)) return false;
                //}
                else if (valueType == typeof(DataTypes.ModStat).Name) { }
                else if (valueType == typeof(DataTypes.MultiChoice).Name) { }
                else if (valueType == typeof(DataTypes.Note).Name) { }
                //else if (valueType == typeof(DataTypes.Number).Name) new IntegerValueOperand(value);
                else if (valueType == typeof(DataTypes.Number).Name) { }
                //else if (valueType == typeof(DataTypes.PageSeparator).Name) return false; // NOT SUPPORTED
                else if (valueType == typeof(DataTypes.PageSeparator).Name) { }
                //else if (valueType == typeof(DataTypes.Recurrence).Name) new BooleanValueOperand(value);
                else if (valueType == typeof(DataTypes.Recurrence).Name) { }
                else if (valueType == typeof(DataTypes.Text).Name) { }
                //else if (valueType == typeof(DataTypes.ThreadIndex).Name) new IntegerValueOperand(value);
                else if (valueType == typeof(DataTypes.ThreadIndex).Name) { }
                //else if (valueType == typeof(DataTypes.Threading).Name) new BooleanValueOperand(value);
                else if (valueType == typeof(DataTypes.Threading).Name) { }
                else if (valueType == typeof(DataTypes.URL).Name) { }
                //else if (valueType == typeof(DataTypes.User).Name)
                //{
                //    if (isLookupId) new UserIdValueOperand(value);
                //}
                else if (valueType == typeof(DataTypes.User).Name) {}
                //else if (valueType == typeof(DataTypes.WorkflowEventType).Name)
                //{
                //    if (!IsEnumValueValid(typeof(SPWorkflowHistoryEventType), value)) return false;
                //}
                else if (valueType == typeof(DataTypes.WorkflowEventType).Name) {}
                //else if (valueType == typeof(DataTypes.WorkflowStatus).Name)
                //{
                //    if (!IsEnumValueValid(typeof(SPWorkflowStatus), value)) return false;
                //}
                else if (valueType == typeof(DataTypes.WorkflowStatus).Name) {}
                else return false;
            }
            catch (InvalidValueForOperandTypeException) { return false; }
            return true;
        }

        protected IOperation getOperation<T>(Func<IOperand, IOperand, T> constructor)
            where T : IOperation
        {
            if (!this.IsValid())
            {
                throw new CamlAnalysisException(string.Format(
                    "Can't create {0} from the following xml: {1}", typeof(T).Name, el));
            }

            var fieldRefElement = el.Elements(Tags.FieldRef).First();
            var fieldRefOperand = operandBuilder.CreateFieldRefOperand(fieldRefElement);
            var valueOperand = operandBuilder.CreateValueOperand(el, this.isOperationComparison(el));
            return constructor(fieldRefOperand, valueOperand);
        }

//        private static bool IsEnumValueValid(Type enumType, string value)
//        {
//            int result;
//            if (int.TryParse(value, out result))
//            {
//                return Enum.GetValues(enumType).Cast<int>().Any(x => x == result);
//            }
//            return Enum.GetNames(enumType).Any(x => string.Compare(x, value, true) == 0);
//        }
    }
}