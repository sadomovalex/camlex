using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace CamlexNET.Impl.Operands
{
    internal class UserIdConstValueOperand : GenericStringBasedValueOperand
    {
        public UserIdConstValueOperand()
            : base(typeof(DataTypes.Integer), Tags.UserID)
        {
        }

        public override XElement ToCaml()
        {
            return
                new XElement(Tags.Value, new XAttribute(Attributes.Type, this.TypeName),
                    new XElement(this.Value));
        }
    }
}
