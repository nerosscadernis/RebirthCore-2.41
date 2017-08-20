using Rebirth.Common.IO;
using Rebirth.Common.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rebirth.Common.Protocol.Messages
{

    public class FollowQuestObjectiveRequestMessage : NetworkMessage
    {

        public const uint Id = 6724;
        public uint MessageId
        {
            get { return Id; }
        }

        public  uint questId;
        public int objectiveId;


        public FollowQuestObjectiveRequestMessage()
        {
        }

        public FollowQuestObjectiveRequestMessage(int objectiveId, uint questId)
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
