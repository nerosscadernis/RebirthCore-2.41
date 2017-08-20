using Rebirth.Common.Protocol.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebirth.World.Datas.Items
{
    public abstract class ItemToSell
    {
        private Item m_item;
        public int Id
        {
            get;
            set;
        }
        public int ItemId
        {
            get;
            set;
        }
   
        public Item Item
        {
            get
            {
                //this.m_item = ItemManager.Instance.GetItemTemplateById(this.ItemId);
              
                return this.m_item;
            }
            set
            {
                this.m_item = value;
                this.ItemId = value.id;
            }
        }
        public abstract Rebirth.Common.Protocol.Types.Item GetNetworkItem();
    }
}
