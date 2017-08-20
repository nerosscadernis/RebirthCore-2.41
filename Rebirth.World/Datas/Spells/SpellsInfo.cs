using Rebirth.Common.GameData.D2O;
using Rebirth.Common.IO;
using Rebirth.Common.Protocol.Data;
using Rebirth.Common.Protocol.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rebirth.World.Datas.Spells
{
    public class SpellsInfo
    {
        #region vars

        #endregion

        #region Properties
        public List<SpellTemplate> Spells { get; set; }
        #endregion

        #region Constructors
        public SpellsInfo()
        {
            Spells = new List<SpellTemplate>();
        }

        public SpellsInfo(byte[] datas)
        {
            Spells = new List<SpellTemplate>();
            var reader = new BigEndianReader(datas);
            var count = reader.ReadInt();
            for (int i = 0; i < count; i++)
            {
                Spells.Add(new SpellTemplate(ObjectDataManager.Get<Spell>(reader.ReadInt()), reader.ReadByte()));
            }
        }

        public byte[] GetDatas()
        {
            var writer = new BigEndianWriter();
            writer.WriteInt(Spells.Count);
            foreach (var item in Spells)
            {
                writer.WriteInt(item.Spell.id);
                writer.WriteByte(item.Grade);
            }
            return writer.Data;
        }
        #endregion

        #region Methods
        public bool HasSpell(int id)
        {
            return Spells.Any(e => e.Spell.id == id);
        }

        public void AddSpell(int spellId)
        {
            var spellTemplate = ObjectDataManager.Get<Spell>(spellId);
            Spells.Add(new SpellTemplate(spellTemplate, 0));
        }

        public void RemoveSpell(int spellId)
        {
            var spell = Spells.FirstOrDefault(x => x.Spell.id == spellId);
            if (spell == null)
                return;
            Spells.Remove(spell);
        }

        public SpellListMessage GetSpellList()
        {
            return new SpellListMessage(true, (from x in Spells
                                                select x.GetSpellItem()).ToArray());
        }
        #endregion
    }
}
