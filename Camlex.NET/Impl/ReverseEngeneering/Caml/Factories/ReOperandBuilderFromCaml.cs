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

        public IOperand CreateValueOperand(XElement valueElement, XElement fieldRefElement)
        {
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
                type = typeof (DataTypes.LookupValue);

                if (fieldRefElement != null)
                {
                    var lookupIdAttr = fieldRefElement.Attributes().FirstOrDefault(a => a.Name == Attributes.LookupId);
                    if (lookupIdAttr != null)
                    {
                        bool isLookupId = false;
                        if (bool.TryParse(lookupIdAttr.Value, out isLookupId) && isLookupId)
                        {
                            type = typeof (DataTypes.LookupId);
                        }
                    }
                }
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

            // currently only string-based value operand will be returned
            // todo: add support of native operands here (see OperandBuilder.CreateValueOperand() for details)
            return OperandBuilder.CreateValueOperand(type, value, includeTimeValue, true);
        }
    }
}
