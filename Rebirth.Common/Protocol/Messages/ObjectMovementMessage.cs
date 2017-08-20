


















// Generated on 01/12/2017 03:53:04
using System;
using System.Collections.Generic;
using System.Linq;
using Rebirth.Common.Protocol.Types;
using Rebirth.Common.Network;
using Rebirth.Common.IO;

namespace Rebirth.Common.Protocol.Messages
{

public class ObjectMovementMessage : NetworkMessage
{

public const uint Id = 3010;
public uint MessageId
{
    get { return Id; }
}

public uint objectUID;
        public byte position;
        

public ObjectMovementMessage()
{
}

public ObjectMovementMessage(uint objectUID, byte position)
        {
            this.objectUID = objectUID;
            this.position = position;
        }
        

public void Serialize(IDataWriter writer)
{

writer.WriteVarInt((int)objectUID);
            writer.WriteByte(position);
            

}

public void Deserialize(IDataReader reader)
{

objectUID = reader.ReadVarUhInt();
            if (objectUID < 0)
                throw new System.Exception("Forbidden value on objectUID = " + objectUID + ", it doesn't respect the following condition : objectUID < 0");
            position = reader.ReadByte();
            if (position < 0 || position > 255)
                throw new System.Exception("Forbidden value on position = " + position + ", it doesn't respect the following condition : position < 0 || position > 255");
            

}


}


}