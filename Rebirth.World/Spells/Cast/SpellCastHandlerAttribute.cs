using Rebirth.Common.Protocol.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Spells.Cast
{
    public class SpellCastHandlerAttribute : System.Attribute
    {
        public int Spell
        {
            get;
            set;
        }
        public SpellCastHandlerAttribute(int spellId)
        {
            this.Spell = spellId;
        }
        public SpellCastHandlerAttribute(SpellIdEnum spellId)
        {
            this.Spell = (int)spellId;
        }
    }
}
