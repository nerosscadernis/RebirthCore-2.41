using Rebirth.Common.IO;
using Rebirth.Common.Protocol.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rebirth.World.Datas.Shortcuts
{
    public class ShortcutRecord
    {
        public sbyte Slot { get; set; }

        public ShortcutRecord(sbyte slot)
        {
            Slot = slot;
        }

        public ShortcutRecord(IDataReader reader)
        {
            Slot = reader.ReadSByte();
        }

        public virtual byte[] GetDatas()
        {
            var writer = new BigEndianWriter();
            writer.WriteSByte(Slot);
            return writer.Data;
        }

        public virtual Shortcut GetNetworkShortcut()
        { return new Shortcut(Slot); }
    }
}
