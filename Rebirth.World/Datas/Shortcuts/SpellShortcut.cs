using Rebirth.Common.IO;
using Rebirth.Common.Protocol.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rebirth.World.Datas.Shortcuts
{
    public class SpellShortcut : ShortcutRecord
    {
        public uint SpellId { get; set; }

        public SpellShortcut(sbyte slot, uint spellId) : base(slot)
        {
            SpellId = spellId;
        }

        public SpellShortcut(IDataReader reader) : base(reader)
        {
            this.SpellId = reader.ReadUInt();
        }

        public override byte[] GetDatas()
        {
            var writer = new BigEndianWriter();
            writer.WriteBytes(base.GetDatas());
            writer.WriteUInt(SpellId);
            return writer.Data;
        }

        public override Shortcut GetNetworkShortcut()
        {
            return new ShortcutSpell(Slot, this.SpellId);
        }
    }
}
