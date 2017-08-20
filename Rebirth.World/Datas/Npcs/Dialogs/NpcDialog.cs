using Rebirth.Common.IStructures;
using Rebirth.Common.Protocol.Enums;
using Rebirth.Common.Protocol.Messages;
using Rebirth.World.Datas.Characters;
using Rebirth.World.Datas.Npcs;
using Rebirth.World.Datas.Characters;
using Rebirth.World.Game.Npcs.Replies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Npcs.Dialogs
{
    public class NpcDialog : IActivity
    {
        public DialogTypeEnum DialogType
        {
            get
            {
                return DialogTypeEnum.DIALOG_DIALOG;
            }
        }
        public Character Character
        {
            get;
            private set;
        }
        public NpcSpawn Npc
        {
            get;
            private set;
        }
        public NpcMessageRecord CurrentMessage
        {
            get;
            protected set;
        }
        public NpcDialog(Character character, NpcSpawn npc)
        {
            this.Character = character;
            this.Npc = npc;
        }
        public virtual void Open()
        {
            this.Character.State = PlayerState.Exchange_NPC;
            this.Character.Activity = this;
            this.Character.Client.Send(new NpcDialogCreationMessage(this.Npc.MapId, Npc.Id * -1));
        }
        public virtual void Close()
        {
            this.Character.State = PlayerState.Available;
            this.Character.Client.Send(new  LeaveDialogMessage((sbyte)this.DialogType));
            this.Character.Activity = null;
        }
        public virtual void Reply(short replyId)
        {
            Character.Quests.ValidParam(Npc.NpcId, replyId, 1);
            if (Npc.Quests.ContainsKey(replyId) && Npc.Quests[replyId].levelMin <= Character.Infos.Level)
                Character.Quests.AddQuest(Npc.Quests[replyId].id);
            NpcMessageRecord currentMessage = this.CurrentMessage;
            NpcReply[] array = (
                from entry in this.CurrentMessage.Replies
                where entry.ReplyId == (int)replyId
                select entry).ToArray<NpcReply>();
            if (array.Any((NpcReply x) => !x.CanExecute(this.Npc, this.Character)))
            {
                //TODO
                //this.Character.Messenger.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 34, new string[0]);
            }
            else
            {
                NpcReply[] array2 = array;
                for (int i = 0; i < array2.Length; i++)
                {
                    NpcReply reply = array2[i];
                    this.Reply(reply);
                }
                if (array.Length == 0 || currentMessage == this.CurrentMessage)
                {
                    this.Close();
                }
            }
        }
        public void Reply(NpcReply reply)
        {
            reply.Execute(this.Npc, this.Character);
        }
        public virtual void ChangeMessage(NpcMessageRecord message)
        {
            this.CurrentMessage = message;
            System.Collections.Generic.IEnumerable<uint> replies = (
                from entry in message.Replies
                where entry.CriteriaExpression == null || entry.CriteriaExpression.Eval(this.Character)
                select (uint)entry.ReplyId).Distinct<uint>();

            if(message.Parameters.Contains("name"))
                Character.Client.Send(new NpcDialogQuestionMessage(this.CurrentMessage.MessageId, new string[] { Character.Infos.Name }, replies.ToArray()));  
            else
                Character.Client.Send(new NpcDialogQuestionMessage(this.CurrentMessage.MessageId, new string[0], replies.ToArray()));
        }

    }
}
