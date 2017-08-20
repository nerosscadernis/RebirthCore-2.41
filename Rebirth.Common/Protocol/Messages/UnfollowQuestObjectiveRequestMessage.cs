using Rebirth.Common.IO;
using Rebirth.Common.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rebirth.Common.Protocol.Messages
{
    public class UnfollowQuestObjectiveRequestMessage : NetworkMessage
    {

        public const uint Id = 6723;
        public uint MessageId
        {
            get { return Id; }
        }

        public uint questId;
        public int objectiveId;


        public UnfollowQuestObjectiveRequestMessage()
        {
        }

        public UnfollowQuestObjectiveRequestMessage(int objectiveId, uint questId)
        {
            this.questId = questId;
            this.objectiveId = objectiveId;
        }


        public void Serialize(IDataWriter writer)
        {

            writer.WriteVarShort((ushort)questId);
            writer.WriteShort((short)objectiveId);


        }

        public void Deserialize(IDataReader reader)
        {

            questId = (uint)reader.ReadVarShort();
            objectiveId = reader.ReadShort();


        }


    }


}
