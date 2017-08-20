using Rebirth.Common.IO;
using Rebirth.Common.Protocol.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rebirth.World.Datas.Shortcuts
{
    public class ItemShortcut : ShortcutRecord
    {
        public int ItemId { get; set; }
        public int ItemGuid { get; set; }

        public ItemShortcut(sbyte slot, int itemId, int itemGuid) : base(slot)
        {
            ItemId = itemId;
            ItemGuid = itemGuid;
        }

        public ItemShortcut(IDataReader reader) : base(reader)
        {
            this.ItemId = reader.ReadInt();
            this.ItemGuid = reader.ReadInt();
        }

        public override byte[] GetDatas()
        {
            var writer = new BigEndianWriter();
            writer.WriteBytes(base.GetDatas());
            writer.WriteInt(ItemId);
            writer.WriteInt(ItemGuid);
            return writer.Data;
        }

        public override Shortcut GetNetworkShortcut()
        {
            return new ShortcutObjectItem(Slot, this.ItemGuid, this.ItemId);
        }
    }
}