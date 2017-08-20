//using Rebirth.Common.Protocol.Data;
//using Rebirth.World.Datas.Items.Shops;
//using Rebirth.World.Datas.Npcs;
//using Rebirth.World.Datas.Characters;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Rebirth.World.Game.Npcs
//{
//    public class NpcShopDialogLogger : NpcShopDialog
//    {
//        public NpcShopDialogLogger(Character character, NpcSpawn npc, List<NpcItem> items) : base(character, npc, items)
//        {
//            base.Character = character;
//            base.Npc = npc;
//            base.Items = items;
//            base.CanSell = true;
//        }
//        public NpcShopDialogLogger(Character character, NpcSpawn npc, List<NpcItem> items, Item token) : base(character, npc, items, token)
//        {
//            base.Character = character;
//            base.Npc = npc;
//            base.Items = items;
//            base.Token = token;
//            base.CanSell = true;
//        }
//        public override bool BuyItem(int itemId, uint amount)
//        {
//            bool result;
//            if (!base.BuyItem(itemId, amount))
//            {
//                result = false;
//            }
//            else
//            {
//                NpcItem npcItem = base.Items.FirstOrDefault((NpcItem entry) => entry.Item.id == itemId);
//                //BsonDocument document = new BsonDocument
//                //{

//                //    {
//                //        "AcctId",
//                //        base.Character.Account.Id
//                //    },

//                //    {
//                //        "CharacterId",
//                //        base.Character.Id
//                //    },

//                //    {
//                //        "ItemId",
//                //        npcItem.ItemId
//                //    },

//                //    {
//                //        "Amount",
//                //        (long)((ulong)amount)
//                //    },

//                //    {
//                //        "FinalPrice",
//                //        npcItem.Price * amount
//                //    },

//                //    {
//                //        "IsToken",
//                //        base.Token != null
//                //    },

//                //    {
//                //        "Date",
//                //        System.DateTime.Now.ToString(System.Globalization.CultureInfo.InvariantCulture)
//                //    }
//                //};
//                //Singleton<MongoLogger>.Instance.Insert("NpcShopBuy", document);
//                result = true;
//            }
//            return result;
//        }
//    }
//}
