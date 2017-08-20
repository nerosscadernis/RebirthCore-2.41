using Rebirth.World.Datas.Characters;
using Rebirth.World.Datas.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rebirth.World.Exchanges
{
    public class Trader
    {
        public List<TradeItem> Items { get; set; }
        public int kamas { get; set; }
        private Character Owner;
        private bool Status;

        public event Action<TradeItem, int> ItemAdd;
        public event Action<TradeItem, int> ItemRemoved;
        public event Action<TradeItem, int> ItemQuantityChanged;
        public event Action<bool, int> IsValidate;
        public event Action<int, int> ReplayChanged;
        public event Action<int, int> KamasChanged;

        public Trader(Character Character)
        {
            Items = new List<TradeItem>();
            Owner = Character;
            Owner.Trader = this;
        }

        public void AddItem(uint itemId, int quantity)
        {
            PlayerItem item = Owner.Inventory.GetItemByGUID(itemId);
            if (item != null)
            {
                TradeItem containItem = Items.FirstOrDefault(x => x._item.Guid == itemId);
                if (containItem != null)
                {
                    if (containItem.Quantity + quantity > item.Quantity)
                    {
                        quantity = item.Quantity - containItem.Quantity;
                    }
                    containItem.Quantity += quantity;
                    if (containItem.Quantity <= 0)
                    {
                        Items.Remove(containItem);
                        this.ItemRemoved?.Invoke(containItem, (int)Owner.Infos.Id);
                    }
                    else
                    {
                        this.ItemQuantityChanged?.Invoke(containItem, (int)Owner.Infos.Id);
                    }
                }
                else
                {
                    if (quantity > item.Quantity)
                    {
                        quantity = item.Quantity;
                    }
                    Items.Add(new TradeItem(item, quantity));
                    this.ItemAdd?.Invoke(Items.Last(), (int)Owner.Infos.Id);
                }
            }
        }

        public void ModifyKamas(int quantity)
        {
            if (quantity >= 0 && Owner.Inventory.Kamas >= quantity)
            {
                kamas = quantity;
                KamasChanged?.Invoke(kamas, (int)Owner.Infos.Id);
            }
        }

        public bool ContainsItem(int key, int quantity)
        {
            return Items.FirstOrDefault(x => x._item.Template.id == key && x.Quantity >= quantity) != null;
        }

        public void Validate(bool ready)
        {
            Status = ready;
            Action<bool, int> action = this.IsValidate;
            if (action != null)
                action(Status, (int)Owner.Infos.Id);
        }

        public void ChangeReplay(int count)
        {
            Action<int, int> action = this.ReplayChanged;
            if (action != null)
                action(count, (int)Owner.Infos.Id);
        }
    }
}
