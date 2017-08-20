using Rebirth.Common.GameData.D2O;
using Rebirth.Common.Protocol.Data;
using Rebirth.Common.Protocol.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rebirth.World.Datas.Spells
{
    public class SpellTemplate
    {
        public Spell Spell;
        public byte Grade;
        public SpellTemplate(Spell spell, byte grade, SpellLevel customSpell = null)
        {
            this.Spell = spell;
            Grade = grade;
            m_customSpellLevel = customSpell;
        }
        private SpellLevel m_customSpellLevel;
        public SpellLevel CurrentSpellLevel
        {
            get
            {
                return m_customSpellLevel != null ? m_customSpellLevel : ObjectDataManager.Get<SpellLevel>(Spell.spellLevels[Grade]);
            }
        }
        public bool IsTrapSpell { get; set; }
        public bool IsGlyphSpell { get; set; }
        public bool IsWeapon { get; set; }
        public int CritAddDamge { get; set; }
        public SpellItem GetSpellItem()
        {
            return new SpellItem(Spell.id, (sbyte)(Grade + 1));
        }
    }
}
