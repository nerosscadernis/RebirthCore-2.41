//using NLog;
//using Rebirth.Common.Protocol.Data;
//using Rebirth.Common.Protocol.Enums;
//using Rebirth.World.Datas;
//using Rebirth.World.Datas.Items.Shops;
//using Rebirth.World.Datas.Npcs;
//using Rebirth.World.Datas.Characters;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Rebirth.World.Game.Npcs.Actions
//{

//    [Discriminator("Shop", typeof(NpcActionDatabase), new System.Type[]
//    {
//        typeof(NpcActionRecord)
//    })]
//    public class NpcBuySellAction : NpcActionDatabase
//    {
//        public const string Discriminator = "Shop";
//        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
//        private System.Collections.Generic.List<NpcItem> m_items;
//        private Item m_token;
//        public System.Collections.Generic.List<NpcItem> Items
//        {
//            get
//            {
//                if ( this.m_items== null)
//                {
//                    this.m_items = (this.m_items =ItemManager.Instance.GetNpcShopItems(base.Record.Id));
//                }
//                return this.m_items;
//            }
//        }

//            public int TokenId
//            {
//                get
//                {
//                    return base.Record.GetParameter<int>(0u, true);
//                }
//                set
//                {
//                    base.Record.SetParameter<int>(0u, value);
//                }
//            }
//            public Item Token
//            {
//                get
//                {
//                    if (this.TokenId <= 0)
//                    {
//                        return null;
//                    }
//                    else
//                    {
//                        if (this.m_token == null)
//                        {
//                            this.m_token = ItemManager.Instance.GetItemTemplateById(this.TokenId);
//                        }
//                    }
//                    return m_token;
//                }
//                set
//                {
//                    this.m_token = value;
//                    this.TokenId = ((value == null) ? 0 : this.m_token.id);
//                }
//            }
          
//            public bool CanSell
//            {
//                get
//                {
//                    return base.Record.GetParameter<bool>(1, true);
//                }
//                set
//                {
//                    base.Record.SetParameter<bool>(1, value);
//                }
//            }
//            public bool MaxStats
//            {
//                get
//                {
//                    return base.Record.GetParameter<bool>(2, true);
//                }
//                set
//                {
//                    base.Record.SetParameter<bool>(2, value);
//                }
//            }
//        public bool ItemByType
//        {
//            get
//            {
//                return (Convert.ToBoolean(base.Record.GetParameter<int>(3)));
//            }
//            set
//            {
//                base.Record.SetParameter<int>(3,Convert.ToInt32(value));
//            }
//        }
//        public string TypeId
//        {
//            get
//            {
//                return base.Record.GetParameter<string>(4);
//            }
//            set
//            {
//                base.Record.SetParameter<string>(4, value);
//            }
//        }
//        public override NpcActionTypeEnum ActionType
//        {
//            get
//            {
//                if (Token != null)
//                    return NpcActionTypeEnum.ACTION_BUY_TOKEN;
//                return NpcActionTypeEnum.ACTION_BUY_SELL;
//            }
//        }
//        public NpcBuySellAction(NpcActionRecord record) : base(record)
//        {
//        }

//        public override void Execute(NpcSpawn npc, Character character)
//        {
//            m_items = new List<NpcItem>();
//            if (ItemByType)
//            {
//                var allitems = new List<Item>();
//                foreach (var item in TypeId.Split(','))
//                {
//                    allitems.AddRange(ItemManager.Instance.GetItemTemplateByType(Convert.ToInt32(item)));
//                }
//                int id = 0;
//                foreach (var item in allitems)
//                {
//                    if(!item.criteria.Contains("PX"))
//                    {
//                        m_items.Add(new NpcItem() { CustomPrice = 1, Id = id, Item = item, ItemId = item.id, MaxStats = this.MaxStats, NpcShopId = npc.Id });
//                        id++;
//                    }
//                }
//            }
//            else
//            {
//                int id = 0;
//                foreach (var baseItem in NpcManager.Instance.GetNpcItemBySpawn(npc.Id))
//                {
//                    m_items.Add(new NpcItem() { CustomPrice = baseItem.Price, Id = id, Item = baseItem.Item, ItemId = baseItem.Item.id, MaxStats = this.MaxStats, NpcShopId = npc.Id });
//                    id++;
//                }
//            }
         
//            NpcShopDialogLogger npcShopDialogLogger = new NpcShopDialogLogger(character, npc, this.Items.OrderBy(e=> e.Item.level).ToList(), this.Token)
//            {
//                CanSell = this.CanSell,
//                MaxStats = this.MaxStats
//            };
//            npcShopDialogLogger.Open();
//        }
//    }
//}