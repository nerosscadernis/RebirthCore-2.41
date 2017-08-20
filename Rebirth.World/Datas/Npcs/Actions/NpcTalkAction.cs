using Rebirth.Common.IO;
using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Datas.Bdd;
using Rebirth.World.Datas.Characters;
using Rebirth.World.Datas.Npcs;
using Rebirth.World.Game.Npcs.Dialogs;
using Rebirth.World.Npcs;
using System.Collections.Generic;
using System.Linq;

namespace Rebirth.World.Game.Npcs.Actions
{
    [Discriminator("Talk", typeof(NpcActionDatabase), new System.Type[]
	{
		typeof(NpcActionRecord)
	})]
    public class NpcTalkAction : NpcActionDatabase
    {
        private NpcMessageRecord m_message;
        public override NpcActionTypeEnum ActionType
        {
            get
            {
                return NpcActionTypeEnum.ACTION_TALK;
            }
        }
        public int MessageId
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
        public int[] Quest
        {
            get
            {
                return base.Record.GetParameter<string>(1, false).FromCSV<int>(";");
            }
            set
            {
                base.Record.SetParameter<string>(1, value.ToCSV(";"));
            }
        }
        public int[] QuestMessage
        {
            get
            {
                return base.Record.GetParameter<string>(2, false).FromCSV<int>(";");
            }
            set
            {
                base.Record.SetParameter<string>(2, value.ToCSV(";"));
            }
        }
        public int[] Objectif
        {
            get
            {
                return base.Record.GetParameter<string>(3, false).FromCSV<int>(";");
            }
            set
            {
                base.Record.SetParameter<string>(3, value.ToCSV(";"));
            }
        }
        public int[] ObjectifMessage
        {
            get
            {
                return base.Record.GetParameter<string>(4, false).FromCSV<int>(";");
            }
            set
            {
                base.Record.SetParameter<string>(4, value.ToCSV(";"));
            }
        }
        public NpcMessageRecord Message(int messageId)
        {
            this.m_message = NpcManager.Instance.GetNpcMessageRecord(messageId);

            return this.m_message;
        }
        public NpcTalkAction(NpcActionRecord record)
            : base(record)
        {
        }
        public override void Execute(NpcSpawn npc, Character character)
        {
            NpcDialog npcDialog = new NpcDialog(character, npc);
            character.Activity = npcDialog;
            npcDialog.Open();

            for (int i = 0; i < Quest.Length; i++)
            {
                if (npc.HasQuest(Quest[i]) && character.Quests.IsrepeatOrActivable(Quest[i]))
                {
                    npcDialog.ChangeMessage(Message(QuestMessage[i]));
                    return;
                }
            }

            npcDialog.ChangeMessage(Message(MessageId));
        }
        public override bool CanExecute(NpcSpawn npc, Character character)
        {
            return true;
        }
    }
}
