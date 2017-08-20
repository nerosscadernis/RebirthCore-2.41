using Rebirth.Common.Protocol.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rebirth.World.Datas.Items
{
    public class TradeItem
    {
        public PlayerItem _item;
        public int Quantity { get; set; }

        public TradeItem(PlayerItem item, int quantity)
        {
            _item = item;
            Quantity = quantity;
        }

        public ObjectItem GetObjectItem()
        {
            return new ObjectItem((byte)_item.Position, (uint)_item.Template.id, (from entry in _item.Effects
                                                                                    where !entry.Hidden
                                                                                    select entry.GetObjectEffect()).ToArray(), (uint)_item.Guid, (uint)Quantity);
        }
    }
}
