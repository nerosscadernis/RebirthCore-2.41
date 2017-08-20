//using Rebirth.Common.Protocol.Data;
//using Rebirth.Common.Protocol.Enums;
//using Rebirth.World.Datas;
//using Rebirth.World.Datas.Npcs;
//using Rebirth.World.Datas.Characters;
//using Rebirth.World.Game.Items;
//using Rebirth.World.Game.Items.Custom;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Rebirth.World.Game.Npcs.Replies
//{
//    [Discriminator("DeleteItem", typeof(NpcReply), new System.Type[]
//	{
//		typeof(NpcReplyRecord)
//	})]
//    public class DeleteItemReply : NpcReply
//    {
//        private Rebirth.Common.Protocol.Data.Item m_itemTemplate;
//        public int ItemId
//        {
//            get
//            {
//                return base.Record.GetParameter<int>(0, false);
//            }
//            set
//            {
//                base.Record.SetParameter<int>(0, value);
//            }
//        }

//        public Rebirth.Common.Protocol.Data.Item Item
//        {
//            get
//            {
//                if(m_itemTemplate == null)
//                    this.m_itemTemplate = ItemManager.Instance.GetItemTemplateById(this.ItemId);

//                return this.m_itemTemplate;
//            }
//            set
//            {
//                this.m_itemTemplate = value;
//                this.ItemId = value.id;
//            }
//        }
//        public uint Amount
//        {
//            get
//            {
//                return base.Record.GetParameter<uint>(1, false);
//            }
//            set
//            {
//                base.Record.SetParameter<uint>(1, value);
//            }
//        }
//        public DeleteItemReply(NpcReplyRecord record)
//            : base(record)
//        {
//        }
//        public override bool Execute(NpcSpawn npc, Character character)
//        {
//            bool result;
//            if (!base.Execute(npc, character))
//            {
//                result = false;
//            }
//            else
//            {
//                if(this.Item == null)
//                {
//                    result = false;
//                    character.Messenger.SendServerMessage("Impossible de trouver l'item Id.", System.Drawing.Color.Green);
//                }
//                else
//                {
//                    PlayerItem basePlayerItem = character.Inventory.GetItemByTemplateId((uint)this.Item.id);

//                    if (basePlayerItem == null)
//                    {
//                        result = false;
//                    }
//                    else
//                    {
//                        if (basePlayerItem.Quantity < this.Amount)
//                        {
//                            result = false;
//                        }
//                        else
//                        {
//                            character.Inventory.RemoveItem(basePlayerItem.Guid, this.Amount);
//                            character.Messenger.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 22, new string[]
//                            {
//                            this.Amount.ToString(),
//                              basePlayerItem.Template.id.ToString()
//                            });
//                            result = true;
//                        }
//                    }
//                }
//            }
//            return result;
//        }
//    }
//}
