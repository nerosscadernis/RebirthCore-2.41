//using Rebirth.Common.Protocol.Data;
//using Rebirth.Common.Protocol.Enums;
//using Rebirth.Common.Protocol.Messages;
//using Rebirth.Common.Protocol.Types;
//using Rebirth.World.Datas.Items.Shops;
//using Rebirth.World.Datas.Npcs;
//using Rebirth.World.Datas.Characters;
//using Rebirth.World.Game.Items.Custom;
//using Rebirth.World.Game.Npcs.Dialogs;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Rebirth.World.Game.Npcs
//{
//    public class NpcShopDialog : IDialog, IShopDialog
//    {
//        public DialogTypeEnum DialogType
//        {
//            get
//            {
//                return DialogTypeEnum.DIALOG_PURCHASABLE;
//            }
//        }
//        public System.Collections.Generic.IEnumerable<NpcItem> Items
//        {
//            get;
//            protected set;
//        }
//        public Common.Protocol.Data.Item Token
//        {
//            get;
//            protected set;
//        }
//        public Character Character
//        {
//            get;
//            protected set;
//        }
//        public NpcSpawn Npc
//        {
//            get;
//            protected set;
//        }
//        public bool CanSell
//        {
//            get;
//            set;
//        }
//        public bool MaxStats
//        {
//            get;
//            set;
//        }
//        public NpcShopDialog(Character character, NpcSpawn npc, List<NpcItem> items)
//        {
//            this.Character = character;
//            this.Npc = npc;
//            this.Items = items;
//            this.CanSell = true;
//        }
//        public NpcShopDialog(Character character, NpcSpawn npc, List<NpcItem> items, Common.Protocol.Data.Item token)
//        {
//            this.Character = character;
//            this.Npc = npc;
//            this.Items = items;
//            this.Token = token;
//            this.CanSell = true;
//        }
//        public void Open()
//        {
//            this.Character.Activity = this;
//            this.Character.Client.Send(new ExchangeStartOkNpcShopMessage(Npc.Id * -1, (uint)(Token != null ? Token.id : 0), (from entry in Items
//                                                                                                        select entry.GetNetworkItem() as ObjectItemToSellInNpcShop).ToArray()));
//        }
//        public void Close()
//        {
//            this.Character.Client.Send(new LeaveDialogMessage((sbyte)this.DialogType));
//            this.Character.Activity = null;
//        }
//        public virtual bool BuyItem(int itemId, uint amount)
//        {
//            if(amount < 1)
//            {
//                this.Character.Client.Send(new ExchangeErrorMessage((sbyte)ExchangeErrorEnum.BUY_ERROR));
//                this.Character.Messenger.SendServerMessage("Très bonne tentatives quantiter 0, echec !", System.Drawing.Color.DarkRed);
//                return false;
//            }
//            NpcItem npcItem = this.Items.FirstOrDefault((NpcItem entry) => entry.Item.id == itemId);
//            bool result;
//            if (npcItem == null)
//            {
//                this.Character.Client.Send(new ExchangeErrorMessage(8));
//                result = false;
//            }
//            else
//            {
//                uint num = (uint)(npcItem.Price * amount);
//                if (!this.CanBuy(npcItem, amount))
//                {
//                    this.Character.Client.Send(new ExchangeErrorMessage(8));
//                    result = false;
//                }
//                else
//                {
       
//                    Character.Messenger.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 21, new string[]
//                    {
//                        amount.ToString(),
//                        itemId.ToString()
//                    });
//                    PlayerItem item = ItemManager.Instance.CreatePlayerItem(this.Character, ItemManager.Instance.GetItemTemplateById(itemId), (int)amount,  Effect.Instances.EffectGenerationType.Normal);
//                    this.Character.Inventory.AddItem(item);
//                    if (this.Token != null)
//                    {
//                      //  this.Character.Inventory.UnStackItem(this.Character.Inventory.TryGetItem(this.Token), num);
//                    }
//                    else
//                    {
//                        var kamas = this.Character.Inventory.Kamas;
//                        this.Character.Inventory.Kamas = (int)(kamas - num);
//                        Character.Messenger.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 46, new string[]
//                        {
//                            num.ToString()
//                        });
//                    }
//                    this.Character.Client.Send(new ExchangeBuyOkMessage());
//                    result = true;
//                }
//            }
//            return result;
//        }
//        public bool CanBuy(NpcItem item, uint amount)
//        {
//            bool result;
//            if (this.Token != null)
//            {
//                PlayerItem basePlayerItem = this.Character.Inventory.TryGetItem(this.Token.id);
//                if (basePlayerItem == null || basePlayerItem.Quantity < item.Price * amount)
//                {
//                    result = false;
//                    return result;
//                }
//            }
//            else
//            {
//                if ((double)this.Character.Inventory.Kamas < item.Price * amount)
//                {
//                    result = false;
//                    return result;
//                }
//            }
//            result = true;
//            return result;
//        }
//        public bool SellItem(int guid, uint amount)
//        {
//            bool result;
//            if (!this.CanSell)
//            {
//                this.Character.Client.Send(new ExchangeErrorMessage(9));
//                result = false;
//            }
//            else
//            {
//                PlayerItem item = this.Character.Inventory.GetItemByGUID((uint)guid);
//                if (item == null)
//                {
//                    this.Character.Client.Send(new ExchangeErrorMessage(9));
//                    result = false;
//                }
//                else
//                {
//                    if (item.Quantity < amount)
//                    {
//                        this.Character.Client.Send(new ExchangeErrorMessage(9));
//                        result = false;
//                    }
//                    else
//                    {
//                        NpcItem npcItem = this.Items.FirstOrDefault((NpcItem entry) => entry.Item.id == item.Template.id);
//                        int num;
//                        if (npcItem != null)
//                        {
//                            num = (int)((long)((int)System.Math.Ceiling(npcItem.Price / 10.0)) * (long)((ulong)amount));
//                        }
//                        else
//                        {
//                            num = (int)((long)((int)System.Math.Ceiling(item.Template.price / 10.0)) * (long)((ulong)amount));
//                        }
//                        this.Character.Messenger.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 22, new string[]
//                        {
//                            amount.ToString(),
//                            item.Template.id.ToString()
//                        });
//                        this.Character.Inventory.RemoveItem(item, amount);
//                        if (this.Token != null)
//                        {
//                            //this.Character.Inventory.AddItem(this.Token, (uint)num);
//                        }
//                        else
//                        {
//                            var kamas = this.Character.Inventory.Kamas;
//                            this.Character.Inventory.Kamas = (int)(kamas + num);
//                            Character.Messenger.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 45, new string[]
//                            {
//                                num.ToString()
//                            });
//                        }
//                        this.Character.Client.Send(new ExchangeSellOkMessage());
//                        result = true;
//                    }
//                }
//            }
//            return result;
//        }
//    }
//}
