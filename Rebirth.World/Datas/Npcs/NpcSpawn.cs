using PetaPoco.NetCore;
using Rebirth.Common.GameData.D2O;
using Rebirth.Common.Protocol.Data;
using Rebirth.Common.Protocol.Enums;
using Rebirth.Common.Protocol.Types;
using Rebirth.World.Datas.Characters;
using Rebirth.World.Datas.Interactives;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Game.Npcs.Actions;
using Rebirth.World.Managers;
using Rebirth.World.Npcs;
using System.Collections.Generic;
using System.Linq;

namespace Rebirth.World.Datas.Npcs
{
    [TableName("npcs_spawns")]
    public class NpcSpawn
    {
         public static string FetchQuery = "SELECT * FROM npcs_spawns";
        public int Id
        {
            get;
            set;
        }
        private Look m_entityLook;
        private string m_lookAsString;
        private Npc m_template;
      
        public int NpcId
        {
            get;
            set;
        }
        [Ignore]
        public Npc Template
        {
            get
            {

                return ObjectDataManager.Get<Npc>(this.NpcId);
            }
            set
            {
                this.m_template = value;
                this.NpcId = value.id;
            }
        }
        public int MapId
        {
            get;
            set;
        }
        public int CellId
        {
            get;
            set;
        }
        public DirectionsEnum Direction
        {
            get;
            set;
        }
        public string LookAsString
        {
            get;
            set;
        }

        public void Init()
        {
            Quests = new Dictionary<int, Quest>();
            Template = ObjectDataManager.Get<Npc>(this.NpcId);
            var ids = Starter.Database.Fetch<int>("select QuestId from npcs_quest where NpcSpawn='" + Id + "'");
            var replys = Starter.Database.Fetch<int>("select Reply from npcs_quest where NpcSpawn='" + Id + "'");
            for (int i = 0; i < ids.Count; i++)
            {
                var quest = ObjectDataManager.Get<Quest>(ids[i]);
                if (quest != null)
                    Quests.Add(replys[i], quest);
            }
        }

        [Ignore]
        public Dictionary<int, Quest> Quests { get; set; }

        [Ignore]
        public Look Look
        {
            get
            {
                return LookAsString != "" ? Look.Parse(this.LookAsString) : Look.Parse(this.Template.look);
            }
            set
            {
                this.m_entityLook = value;
                if (value != null)
                {
                    this.m_lookAsString = value.ToString();
                }
            }
        }
        public ObjectPosition GetPosition()
        {

            MapTemplate map =  MapManager.Instance.GetMap(this.MapId);
            if (map == null)
            {
                throw new System.Exception(string.Format("Cannot load NpcSpawn id={0}, map {1} isn't found", this.Id, this.MapId));
            }
            CellMap cell = map.Cells[this.CellId];
            return new ObjectPosition(map, cell, this.Direction);
        }
        private System.Collections.Generic.List<NpcActionDatabase> m_actions = null;
        
        [Ignore]
        public System.Collections.Generic.List<NpcActionDatabase> Actions
        {
            get
            {
                System.Collections.Generic.List<NpcActionDatabase> arg_23_0;
                if ((arg_23_0 = this.m_actions) == null)
                {
                    arg_23_0 = (this.m_actions = NpcManager.Instance.GetNpcActions(this.Id));
                }
                return arg_23_0;
            }
        }
    
        public void InteractWith(NpcActionTypeEnum actionType, Character dialoguer)
        {
            if (this.CanInteractWith(actionType, dialoguer))
            {
                Rebirth.World.Game.Npcs.Actions.NpcAction npcAction = this.Actions.First((Rebirth.World.Game.Npcs.Actions.NpcAction entry) => entry.ActionType == actionType && entry.CanExecute(this, dialoguer));
                npcAction.Execute(this, dialoguer);
            }
        }


        public bool CanInteractWith(NpcActionTypeEnum action, Character dialoguer)
        {
            return !(dialoguer.Map.Id != this.MapId) && this.Actions.Count > 0 && this.Actions.Any((Rebirth.World.Game.Npcs.Actions.NpcAction entry) => entry.ActionType == action && entry.CanExecute(this, dialoguer));
        }

        public bool HasQuest(Character character)
        {
            return Quests.Values.Any(x => character.Quests.IsrepeatOrActivable((int)x.id) && x.levelMin <= character.Infos.Level);
        }

        public bool HasQuest(int questId)
        {
            return Quests.Values.Any(x => x.id == questId);
        }

        public uint[] GetQuests(Character character)
        {
            return Quests.Values.ToList().FindAll(x => character.Quests.IsrepeatOrActivable((int)x.id) && x.levelMin <= character.Infos.Level).Select(x => x.id).ToArray();
        }

        public GameRolePlayNpcInformations GetGameRolePlayNpcInformations(Character character)
        {
            var quest = character.Quests.GetQuestsWithNpc(NpcId);
            if (quest.Count > 0 || Quests.Count(x => x.Value.levelMin <= character.Infos.Level) > 0)
                return new GameRolePlayNpcWithQuestInformations(Id * -1, Look.GetEntityLook(), new EntityDispositionInformations((short)CellId,
                    (byte)Direction), (uint)NpcId, Template.gender != 0u, 0, new GameRolePlayNpcQuestFlag(quest.Select(x => x.Id).ToArray(), Quests.Values.ToList().FindAll(x => character.Quests.IsrepeatOrActivable((int)x.id) && x.levelMin <= character.Infos.Level).Select(x => x.id).ToArray()));
            return new GameRolePlayNpcInformations(Id * -1, Look.GetEntityLook(), new EntityDispositionInformations((short)CellId, (byte)Direction), (uint)NpcId, Template.gender != 0u, 0);
        }
    }
}
