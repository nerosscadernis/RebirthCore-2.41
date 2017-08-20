


















// Generated on 01/12/2017 03:52:52
using System;
using System.Collections.Generic;
using System.Linq;
using Rebirth.Common.Protocol.Types;
using Rebirth.Common.Network;
using Rebirth.Common.IO;

namespace Rebirth.Common.Protocol.Messages
{

public class BasicPingMessage : NetworkMessage
{

public const uint Id = 182;
public uint MessageId
{
    get { return Id; }
}

public bool quiet;
        

public BasicPingMessage()
{
}

public BasicPingMessage(bool quiet)
        {
            this.quiet = quiet;
        }
        

public void Serialize(IDataWriter writer)
{

writer.WriteBoolean(quiet);
            

}

public void Deserialize(IDataReader reader)
{

quiet = reader.ReadBoolean();
            

}


}


}