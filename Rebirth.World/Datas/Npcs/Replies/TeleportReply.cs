using Rebirth.Common.GameData.D2O;
using Rebirth.Common.Protocol.Data;
using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Datas.Bdd;
using Rebirth.World.Datas.Characters;
using Rebirth.World.Datas.Npcs;
using System;

namespace Rebirth.World.Game.Npcs.Replies
{
    [Discriminator("Teleport", typeof(NpcReply), new System.Type[]
     {
        typeof(NpcReplyRecord)
     })]
    public class TeleportReply : NpcReply
    {
        private int m_cellId;
        private DirectionsEnum m_direction;
        private int m_mapId;
        public int MapId
        {
            get
            {
                return base.Record.GetParameter<int>(0, false);
            }
            set
            {
                base.Record.SetParameter<int>(0, value);
            }
        }
        public int CellId
        {
            get
            {
                return base.Record.GetParameter<int>(1, false);
            }
            set
            {
                base.Record.SetParameter<int>(1, value);
            }
        }
        
        public bool RemoveObject
        {
            get
            {
                return base.Record.GetParameter<bool>(2, false);
            }
            set
            {
                base.Record.SetParameter<bool>(2, value);
            }
        }

        public string Object
        {
            get
            {
                return base.Record.GetParameter<string>(3, false);
            }
            set
            {
                base.Record.SetParameter<string>(3, value);
            }
        }

        public string Gain
        {
            get
            {
                return base.Record.GetParameter<string>(4, false);
            }
            set
            {
                base.Record.SetParameter(4, value);
            }
        }

        public TeleportReply()
        {
            base.Record.Type = "Teleport";
        }

        public TeleportReply(NpcReplyRecord record) : base(record)
        {
        }
   
        public override bool Execute(NpcSpawn npc, Character character)
        {
            bool result;
            if (!base.Execute(npc, character))
            {
                result = false;
            }
            else
            {
                if (RemoveObject)
                {
                    //string[] objs = Object.Split(';');
                    //foreach (var item in objs)
                    //{
                    //    var id = uint.Parse(item.Split(':').First());
                    //    var count = item.Split(':').Last();
                    //    var obj = character.Inventory.GetItemByTemplateId(id);
                    //    character.Inventory.DeleteItem(obj);
                    //}
                }
                character.Teleport((int)MapId, (short)CellId);
                result = true;
            }
            if (Gain != null && Gain != "")
            {
                switch (Gain[0])
                {
                    case 'I':
                        var item = ObjectDataManager.Get<Item>((Convert.ToInt32(Gain.Substring(1))));
                        if (item != null)
                        {
                            character.Inventory.AddItem(new Datas.Items.PlayerItem(character, item.id, 1, Effect.Instances.EffectGenerationType.Normal));
                        }
                        break;
                    default:
                        break;
                }
            }
         
            return result;
        }


    }
}

