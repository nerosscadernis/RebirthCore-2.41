using Rebirth.Common.IO;
using Rebirth.Common.Protocol.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rebirth.World.Datas.Shortcuts
{
    public class PresetShortCut : ShortcutRecord
    {
        public int PresetId { get; set; }

        public PresetShortCut(sbyte slot, int presetId) : base(slot)
        {
            PresetId = presetId;
        }

        public PresetShortCut(IDataReader reader) : base(reader)
        {
            this.PresetId = reader.ReadInt();
        }

        public override byte[] GetDatas()
        {
            var writer = new BigEndianWriter();
            writer.WriteBytes(base.GetDatas());
            writer.WriteInt(PresetId);
            return writer.Data;
        }

        public override Shortcut GetNetworkShortcut()
        {
            return new ShortcutObjectPreset(Slot, (sbyte)PresetId);
        }
    }
}
