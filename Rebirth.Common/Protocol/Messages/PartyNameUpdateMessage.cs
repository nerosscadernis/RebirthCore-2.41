


















// Generated on 01/12/2017 03:52:59
using System;
using System.Collections.Generic;
using System.Linq;
using Rebirth.Common.Protocol.Types;
using Rebirth.Common.Network;
using Rebirth.Common.IO;

namespace Rebirth.Common.Protocol.Messages
{

public class PartyNameUpdateMessage : AbstractPartyMessage
{

public const uint Id = 6502;
public uint MessageId
{
    get { return Id; }
}

public string partyName;
        

public PartyNameUpdateMessage()
{
}

public PartyNameUpdateMessage(uint partyId, string partyName)
         : base(partyId)
        {
            this.partyName = partyName;
        }
        

public void Serialize(IDataWriter writer)
{

base.Serialize(writer);
            writer.WriteUTF(partyName);
            

}

public void Deserialize(IDataReader reader)
{

base.Deserialize(reader);
            partyName = reader.ReadUTF();
            

}


}


}