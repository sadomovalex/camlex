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
        public IOperand CreateFieldRefOperand(XElement el, IOperand valueOperand)
        {
            if (el == null)
            {
                throw new ArgumentNullException("el");
            }
            throw new NotImplementedException();
        }

        public IOperand CreateFieldRefOperandWithOrdering(XElement el, Camlex.OrderDirection orderDirection)
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
                    throw new NotCorrectAttrValueException(idAttr.Value, Attributes.ID);
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
                throw new OnlyOneAttributeShouldBeSpecified(Attributes.ID, Attributes.Name);
            }

            if (id == null && string.IsNullOrEmpty(name))
            {
                throw new AtLeastOneAttributeShouldBeSpecified(Attributes.ID, Attributes.Name);
            }

            var attributes = el.Attributes().Where(
                attr =>
                {
                    return (attr.Name != Attributes.ID && attr.Name != Attributes.Name &&
                            !string.IsNullOrEmpty(attr.Value));
                })
                    .Select(attr => new KeyValuePair<string, string>(attr.Name.ToString(), attr.Value))
                    .ToList();
            if (id != null)
            {
                return new FieldRefOperand(id.Value, attributes);
            }
            return new FieldRefOperand(name, attributes);
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
