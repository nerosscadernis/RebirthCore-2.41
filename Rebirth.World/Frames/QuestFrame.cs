using Rebirth.Common.Dispatcher;
using Rebirth.Common.Protocol.Messages;
using Rebirth.Common.Protocol.Types;
using Rebirth.World.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rebirth.World.Frames
{
    public class QuestFrame
    {
        [MessageHandler(QuestListRequestMessage.Id)]
        public void HandleQuestListRequestMessage(Client client, QuestListRequestMessage message)
        {
            client.Send(new QuestListMessage(client.Character.Quests.GetFinishedId(), client.Character.Quests.GetFinishedCount(),
                client.Character.Quests.GetActiveQuests(), client.Character.Quests.GetReinitialisableQuest()));
        }
        [MessageHandler(QuestStepInfoRequestMessage.Id)]
        public void HandleQuestStepInfoRequestMessage(Client client, QuestStepInfoRequestMessage message)
        {
            client.Send(new QuestStepInfoMessage(client.Character.Quests.GetQuestStepInfos(message.questId)));
        }
        [MessageHandler(QuestObjectiveValidationMessage.Id)]
        public void HandleQuestObjectiveValidationMessage(Client client, QuestObjectiveValidationMessage message)
        {
            client.Character.Quests.ValidationObjective(message.questId, message.objectiveId);
        }
    }
}
