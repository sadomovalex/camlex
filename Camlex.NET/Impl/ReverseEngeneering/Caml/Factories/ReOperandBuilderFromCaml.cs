using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
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
                throw new CamlAnalysisException(string.Format("Can't create field ref operand with ordering: underlying field ref operand is null. Xml which causes issue: '{0}'", el));
            }
            return new FieldRefOperandWithOrdering(fieldRefOperand, orderDirection);
        }

        public IOperand CreateValueOperand(XElement el)
        {
            if (el == null)
            {
                throw new ArgumentNullException("el");
            }
            throw new NotImplementedException();
        }
    }
}
