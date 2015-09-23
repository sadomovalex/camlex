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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using CamlexNET.Impl.Factories;
using CamlexNET.Impl.Operands;
using CamlexNET.Interfaces;
using CamlexNET.Interfaces.ReverseEngeneering;

namespace CamlexNET.Impl.ReverseEngeneering.Caml.Factories
{
    internal class ReOperandBuilderFromCaml : IReOperandBuilder
    {
        public IOperand CreateFieldRefOperand(XElement el)
        {
            if (el == null)
            {
                throw new ArgumentNullException("el");
            }
            Guid? id = null;
            var idAttr = el.Attributes().FirstOrDefault(a => a.Name == Attributes.ID);
            if (idAttr != null)
            {
                try
                {
                    id = new Guid(idAttr.Value);
                }
                catch
                {
                    throw new CamlAnalysisException(string.Format("Value '{0}' is not correct for attribute '{1}'", idAttr.Value, Attributes.ID));
                }
            }

            string name = null;
            var nameAttr = el.Attributes().FirstOrDefault(a => a.Name == Attributes.Name);
            if (nameAttr != null)
            {
                name = nameAttr.Value;
            }

            if (id != null && !string.IsNullOrEmpty(name))
            {
                throw new CamlAnalysisException(string.Format("Only one from two attributes should be specified: {0} or {1}", Attributes.ID, Attributes.Name));
            }

            if (id == null && string.IsNullOrEmpty(name))
            {
                throw new CamlAnalysisException(string.Format("At least one from two attributes should be specified: {0} or {1}", Attributes.ID, Attributes.Name));
            }

            var attributes = el.Attributes().Where(
                attr =>
                {
                    return (attr.Name != Attributes.ID && attr.Name != Attributes.Name &&
                            !string.IsNullOrEmpty(attr.Value));
                })
                    .Select(attr => new KeyValuePair<string, string>(attr.Name.ToString(), attr.Value))
                    .ToList();

            return (id != null ? new FieldRefOperand(id.Value, attributes) : new FieldRefOperand(name, attributes));
        }

        public IOperand CreateFieldRefOperandWithOrdering(XElement el, Camlex.OrderDirection orderDirection)
        {
            var fieldRefOperand = this.CreateFieldRefOperand(el) as FieldRefOperand;
            if (fieldRefOperand == null)
            {
                throw new CamlAnalysisException(
                    string.Format(
                        "Can't create field ref operand with ordering: underlying field ref operand is null. Xml which causes issue:\n{0}",
                        el));
            }
            return new FieldRefOperandWithOrdering(fieldRefOperand, orderDirection);
        }

//        public IOperand CreateValueOperand(XElement operationElement)
//        {
//            return this.CreateValueOperand(operationElement, false);
//        }

        public IOperand CreateValueOperand(XElement operationElement, bool isComparision)
        {
            if (operationElement == null)
            {
                throw new ArgumentNullException("operationElement");
            }
            var valuesElement = operationElement.Elements(Tags.Values).FirstOrDefault();
            if (valuesElement == null)
            {
                return this.createValueOperand(operationElement, operationElement.Elements(Tags.Value).FirstOrDefault(), isComparision);
            }

            var values = new List<IOperand>();
            valuesElement.Elements(Tags.Value).ToList().ForEach(e => values.Add(this.createValueOperand(operationElement, e, isComparision)));
            return new ValuesValueOperand(values);
        }

        public IOperand CreateFieldRefOperandForProjectedField(XElement el)
        {
            var attributes = new List<KeyValuePair<string, string>>();

            Func<IEnumerable<XAttribute>, string, string> getAttr =
                (attrs, name) =>
                {
                    return el.Attributes().Any(a => a.Name == name) ? el.Attributes().FirstOrDefault(a => a.Name == name).Value : string.Empty;
                };

            attributes.Add(new KeyValuePair<string, string>(Attributes.Type, getAttr(el.Attributes(), Attributes.Type)));
            attributes.Add(new KeyValuePair<string, string>(Attributes.List, getAttr(el.Attributes(), Attributes.List)));
            attributes.Add(new KeyValuePair<string, string>(Attributes.ShowField, getAttr(el.Attributes(), Attributes.ShowField)));

            var fieldRef = (FieldRefOperand)this.CreateFieldRefOperand(el);
            fieldRef.TagName = Tags.Field;
            fieldRef.Attributes = attributes;
            return fieldRef;
        }

        private IOperand createValueOperand(XElement operationElement, XElement valueElement, bool isComparision)
        {
            if (operationElement == null)
            {
                throw new ArgumentNullException("operationElement");
            }
            if (valueElement == null)
            {
                throw new ArgumentNullException("valueElement");
            }
            var typeAttr = valueElement.Attributes().FirstOrDefault(a => a.Name == Attributes.Type);
            if (typeAttr == null)
            {
                throw new CamlAnalysisException(
                    string.Format(
                        "Can't create value operand: Type attribute is missing. Xml which causes issue:\n{0}", valueElement));
            }

            string typeName = string.Format("{0}.{1}+{2}, {3}", typeof(DataTypes).Namespace, typeof(DataTypes).Name, typeAttr.Value, typeof(DataTypes).Assembly.FullName);
            Type type = null;
            try
            {
                type = Type.GetType(typeName, false);
            }
            catch
            {
                type = null;
            }

            if (type == null)
            {
                throw new CamlAnalysisException(
                    string.Format(
                        "Can't create value operand: Type attribute '{0}' is incorrect type name. It should have CAML-compatible type name", typeAttr.Value));
            }

            // DataTypes.Lookup is internal. Users should use LookupValue or LookupId. But in order to determine what exact type
            // should be used - we also need to analyze FieldRef operand because it contains LookupId="True" attribute (i.e. not Value
            // operand contains it)
            if (type == typeof(DataTypes.Lookup))
            {
                type = typeof(DataTypes.LookupValue);
                var fieldRefElement = operationElement.Elements(Tags.FieldRef).FirstOrDefault();
                if (fieldRefElement != null)
                {
                    var lookupIdAttr = fieldRefElement.Attributes().FirstOrDefault(a => a.Name == Attributes.LookupId);
                    if (lookupIdAttr != null)
                    {
                        bool isLookupId = false;
                        if (bool.TryParse(lookupIdAttr.Value, out isLookupId) && isLookupId)
                        {
                            type = typeof(DataTypes.LookupId);
                        }
                    }
                }
            }

            // The same about User and UserId. In direct-way translation, the developer should use either User or User ID,
            // but in referese-way translation, it should be determioned by LookupID="True" attribute
            if (type == typeof(DataTypes.User))
            {
                var fieldRefElement = operationElement.Elements(Tags.FieldRef).FirstOrDefault();
                if (fieldRefElement != null)
                {
                    var lookupIdAttr = fieldRefElement.Attributes().FirstOrDefault(a => a.Name == Attributes.LookupId);
                    if (lookupIdAttr != null)
                    {
                        bool isLookupId = false;
                        if (bool.TryParse(lookupIdAttr.Value, out isLookupId) && isLookupId)
                        {
                            type = typeof(DataTypes.UserId);
                        }
                    }
                }
            }

            bool isIntegerForUserId = false;
            if (type == typeof(DataTypes.Integer))
            {
                // DataTypes.Integer also can be used for <UserID />. See http://sadomovalex.blogspot.com/2011/08/camlexnet-24-is-released.html
                isIntegerForUserId =  valueElement.Elements().Count() == 1 && valueElement.Elements().Any(e => e.Name == ReflectionHelper.UserID);
            }

            bool includeTimeValue = false;
            var includeTimeValueAttr = valueElement.Attributes().FirstOrDefault(a => a.Name == Attributes.IncludeTimeValue);
            if (includeTimeValueAttr != null)
            {
                if (!bool.TryParse(includeTimeValueAttr.Value, out includeTimeValue))
                {
                    throw new CamlAnalysisException(
                        string.Format(
                            "Can't create value operand: attribute '{0}' has incorrect value '{1}'. It should have boolean value", includeTimeValueAttr.Name, includeTimeValueAttr.Value));
                }
            }

            string value = valueElement.Value;
            if (type == typeof(DataTypes.DateTime) && valueElement.Descendants().Count() == 1)
            {
                // for DateTime instead of real (Native) value there can be <Now />, <Today />, etc.
                // See the full list from DateTimeValueOperand
                value = valueElement.Descendants().First().Name.ToString();
            }

            var convertedType = this.convertToNativeIfPossible(type);

            // currently only string-based value operand will be returned
            // todo: add support of native operands here (see OperandBuilder.CreateValueOperand() for details)
            return OperandBuilder.CreateValueOperand(
                convertedType, value, includeTimeValue, true, isComparision, isIntegerForUserId);
        }

        public IOperand CreateConstantOperand(XElement operationElement, Type type)
        {
            if (operationElement == null)
            {
                throw new ArgumentNullException("operationElement");
            }
            return new ConstantOperand(Convert.ChangeType(operationElement.Value, type), operationElement.Name.ToString());
        }

        private Type convertToNativeIfPossible(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            if (type == typeof(DataTypes.Text))
            {
                return typeof(string);
            }
            if (type == typeof(DataTypes.Integer))
            {
                return typeof (int);
            }
            if (type == typeof(DataTypes.Boolean))
            {
                return typeof(bool);
            }
            if (type == typeof(DataTypes.DateTime))
            {
                return typeof(DateTime);
            }
            if (type == typeof(DataTypes.Guid))
            {
                return typeof(Guid);
            }
            // for rest of types use string based types
            return type;
        }
    }
}
