using Rebirth.Common.Extensions;
using Rebirth.Common.GameData.D2O;
using Rebirth.Common.Protocol.Data;
using Rebirth.World.Datas.Npcs;
using Rebirth.World.Datas.Items;
using Rebirth.World.Game.Npcs.Actions;
using Rebirth.World.Game.Npcs.Replies;
using Rebirth.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebirth.World.Npcs
{
    public class NpcManager : DataManager<NpcManager>
    {
        public string FetchQuery = "SELECT * FROM npcs_actions";

        private List<NpcMessageRecord> m_npcMessagesRecord = new List<NpcMessageRecord>();

        private List<NpcSpawn> m_npcspawn = new List<NpcSpawn>();
        private List<NpcActionRecord> m_npcsActions = new List<NpcActionRecord>();
        private List<NpcReply> m_npcsReplies = new List<NpcReply>();
        private List<NpcItem> m_npcItems = new List<NpcItem>();

        public void Init()
        {
            m_npcMessagesRecord = ObjectDataManager.GetAll<NpcMessage>().Select(i => new NpcMessageRecord() { MessageId = (uint)i.id, Parameters = i.messageParams }).ToList();
            Starter.Logger.Infos("Npcs message load = " + m_npcMessagesRecord.Count);

            this.m_npcsActions = Starter.Database.Fetch<NpcActionRecord>(FetchQuery, new object[0]);
            Starter.Logger.Infos("NPCAction in database load = " + m_npcsActions.Count);

            this.m_npcItems = Starter.Database.Fetch<NpcItem>("SELECT * FROM npcs_items");

            m_npcspawn = Starter.Database.Fetch<NpcSpawn>(NpcSpawn.FetchQuery).ToList();
            foreach (var item in m_npcspawn)
            {
                item.Init();
            }

            m_npcsReplies = Starter.Database.Fetch<NpcReplyRecord>("SELECT * FROM npcs_replies").Select(i => new NpcReply(i)).ToList();
            Starter.Logger.Infos("NpcSpawn load = " + m_npcspawn.Count);
        }
  
        public NpcMessageRecord GetNpcMessageRecord(int id)
        {
            return m_npcMessagesRecord.FirstOrDefault(i => i.MessageId == id);
        }

        public List<NpcItem> GetNpcItemBySpawn(int id)
        {
            return m_npcItems.FindAll(x => x.NpcShopId == id);
        }

        public List<NpcSpawn> GetNpcOnMap(int id)
        {
            return m_npcspawn.FindAll(entry => entry.MapId == id);
        }

        public List<NpcActionDatabase> GetNpcActions(int npcID)
        {
            return (
                   from entry in this.m_npcsActions
                   where entry.NpcId == npcID
                   select entry.GenerateAction()).ToList<NpcActionDatabase>();
        }

        public System.Collections.Generic.List<NpcReply> GetMessageReplies(int id)
        {
            return (
                from entry in this.m_npcsReplies
                where entry.MessageId == id
                select entry.Record.GenerateReply()).ToList<NpcReply>();
        }

      
    }
}
