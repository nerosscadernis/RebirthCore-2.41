


















// Generated on 01/12/2017 03:52:55
using System;
using System.Collections.Generic;
using System.Linq;
using Rebirth.Common.Protocol.Types;
using Rebirth.Common.Network;
using Rebirth.Common.IO;

namespace Rebirth.Common.Protocol.Messages
{

public class GameContextDestroyMessage : NetworkMessage
{

public const uint Id = 201;
public uint MessageId
{
    get { return Id; }
}



public GameContextDestroyMessage()
{
}



public void Serialize(IDataWriter writer)
{



}

public void Deserialize(IDataReader reader)
{



}


}


}