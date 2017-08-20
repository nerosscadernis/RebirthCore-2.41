//using Rebirth.Common.Protocol.Data;
//using Rebirth.Common.Protocol.Enums;
//using Rebirth.World.Datas;
//using Rebirth.World.Datas.Items.Shops;
//using Rebirth.World.Datas.Npcs;
//using Rebirth.World.Game.Bank;
//using Rebirth.World.Datas.Characters;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Rebirth.World.Game.Npcs.Actions
//{

//    [Discriminator("Bank", typeof(NpcActionDatabase), new System.Type[]
//    {
//        typeof(NpcActionRecord)
//    })]
//    public class BankAction : NpcActionDatabase
//    {
//        public const string Discriminator = "Bank";
//        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
      
//        public override NpcActionTypeEnum ActionType
//        {
//            get
//            {
//                return NpcActionTypeEnum.ACTION_TALK;
//            }
//        }
//        private NpcMessageRecord m_message = null;
//        public int MessageId
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
//        public NpcMessageRecord Message
//        {
//            get
//            {
//                if (this.m_message == null)
//                    this.m_message = NpcManager.Instance.GetNpcMessageRecord(this.MessageId);

//                return this.m_message;
//            }
//            set
//            {
//                this.m_message = value;
//                this.MessageId = (int)value.MessageId;
//            }
//        }
//        public BankAction(NpcActionRecord record) : base(record)
//        {
//        }

//        public override void Execute(NpcSpawn npc, Character character)
//        {

//            BankDialog npcBank = new BankDialog(character, npc) { PriceOpenedBank = 10};
//            npcBank.Open();
//            npcBank.ChangeMessage(Message);
//        }
//    }
//}
