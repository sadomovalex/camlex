using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace CamlexNET.Impl.Operands
{
    internal class LookupIdValueOperand : GenericStringBasedValueOperand
    {
        public LookupIdValueOperand(string value)
            : base(typeof(DataTypes.LookupId), value)
        {
            int id;
            if (!int.TryParse(value, out id))
            {
                throw new InvalidValueForOperandTypeException(value, this.Type);
            }
        }

        public override XElement ToCaml()
        {
            // there is no LookupId and LookupValue datatypes in CAML. There is only
            // Lookup datatype. We introduced different lookup datatypes in order
            // to simplify distinguish between lookup values and lookup ids search.
            // See http://camlex.codeplex.com/Thread/View.aspx?ThreadId=203560 for details
            return
                new XElement(Tags.Value, new XAttribute(Attributes.Type, typeof(DataTypes.Lookup).Name),
                    new XText(this.Value));
        }
    }
}
