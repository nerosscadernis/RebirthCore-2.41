


















// Generated on 01/12/2017 03:52:52
using System;
using System.Collections.Generic;
using System.Linq;
using Rebirth.Common.Protocol.Types;
using Rebirth.Common.Network;
using Rebirth.Common.IO;

namespace Rebirth.Common.Protocol.Messages
{

public class IdentificationFailedMessage : NetworkMessage
{

public const uint Id = 20;
public uint MessageId
{
    get { return Id; }
}

public sbyte reason;
        

public IdentificationFailedMessage()
{
}

public IdentificationFailedMessage(sbyte reason)
        {
            this.reason = reason;
        }
        

public void Serialize(IDataWriter writer)
{

writer.WriteSByte(reason);
            

}

public void Deserialize(IDataReader reader)
{

reason = reader.ReadSByte();
            if (reason < 0)
                throw new System.Exception("Forbidden value on reason = " + reason + ", it doesn't respect the following condition : reason < 0");
            

}


}


}