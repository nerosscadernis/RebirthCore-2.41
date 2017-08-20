using Rebirth.World.Datas.Bdd;
using Rebirth.World.Datas.Characters;
using Rebirth.World.Datas.Npcs;
using Rebirth.World.Game.Npcs.Dialogs;
using Rebirth.World.Npcs;

namespace Rebirth.World.Game.Npcs.Replies
{
    [Discriminator("Dialog", typeof(NpcReply), new System.Type[]
    {
        typeof(NpcReplyRecord)
    })]
    public class ContinueDialogReply : NpcReply
    {
        private NpcMessageRecord m_message;
        public int NextMessageId
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
        public NpcMessageRecord NextMessage
        {
            get
            {
                if (this.m_message == null)
                    this.m_message = NpcManager.Instance.GetNpcMessageRecord(this.NextMessageId);
             
                return NpcManager.Instance.GetNpcMessageRecord(this.NextMessageId);
            }
            set
            {
                this.m_message = value;
                this.NextMessageId = (int)value.MessageId;
            }
        }
        public ContinueDialogReply(NpcReplyRecord record) : base(record)
        {
        }

        public override bool CanExecute(NpcSpawn npc, Character character)
        {
            return base.CanExecute(npc, character);
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
                ((NpcDialog)character.Activity).ChangeMessage(this.NextMessage);
                result = true;
            }
            return result;
        }
    }
}

