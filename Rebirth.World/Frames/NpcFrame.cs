using Rebirth.Common.Dispatcher;
using Rebirth.Common.Protocol.Messages;
using Rebirth.World.Game.Npcs.Dialogs;
using Rebirth.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rebirth.World.Frames
{
    public class NpcFrame
    {
        [MessageHandler(NpcGenericActionRequestMessage.Id)]
        private void HandleNpcGenericActionRequestMessage(Client client, NpcGenericActionRequestMessage message)
        {
            var npc = client.Character.Map.NpcSpawn.FirstOrDefault(x => x.Id == Math.Abs(message.npcId));

            if (npc == null)
                return;

            if (client.Character.Map.Id != message.npcMapId)
                return;

            var dialog = new NpcDialog(client.Character, npc);
            if (npc.CanInteractWith((Common.Protocol.Enums.NpcActionTypeEnum)message.npcActionId, client.Character))
            {
                dialog.Open();
                npc.InteractWith((Common.Protocol.Enums.NpcActionTypeEnum)message.npcActionId, client.Character);
            }
        }

        [MessageHandler(NpcDialogReplyMessage.Id)]
        private void HandleNpcDialogReplyMessage(Client client, NpcDialogReplyMessage message)
        {
            if (client.Character.State != Common.Protocol.Enums.PlayerState.Exchange_NPC)
                return;

            if (client.Character.Activity is NpcDialog)
                (client.Character.Activity as NpcDialog).Reply((short)message.replyId);

            //if (client.Character.Activity is BankDialog)
            //    (client.Character.Activity as BankDialog).Reply((short)message.replyId);

        }

        [MessageHandler(LeaveDialogRequestMessage.Id)]
        private void HandleLeaveDialogRequestMessage(Client client, LeaveDialogRequestMessage message)
        {
            var activity = client.Character.Activity;
            if (activity != null)
                activity.Close();
        }
    }
}
