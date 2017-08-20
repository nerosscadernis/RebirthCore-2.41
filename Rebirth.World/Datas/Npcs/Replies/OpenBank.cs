
//using Rebirth.Common.Protocol.Data;
//using Rebirth.Common.Protocol.Enums;
//using Rebirth.Common.Protocol.Messages;
//using Rebirth.World.Datas;
//using Rebirth.World.Datas.Npcs;
//using Rebirth.World.Frames.World;
//using Rebirth.World.Game.Bank;
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
//    [Discriminator("OpenBank", typeof(NpcReply), new System.Type[]
//    {
//        typeof(NpcReplyRecord)
//    })]
//    public class OpenBank : NpcReply
//    {    
//        public OpenBank(NpcReplyRecord record)
//            : base(record)
//        {
//        }
//        public override bool Execute(NpcSpawn npc, Character character)
//        {
//            character.Client.Send(new ExchangeStartedWithStorageMessage(23, 2147783647));
//            character.Client.Send(new StorageInventoryContentMessage(BankManager.Instance.GetItemsByOwner(character.AccountId),(uint) character.Client.Bank.Kamas));
//            return true;
//        }
//    }
//}
