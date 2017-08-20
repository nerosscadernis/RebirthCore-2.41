using Rebirth.Common.Dispatcher;
using Rebirth.Common.Protocol.Messages;
using Rebirth.Common.Protocol.Types;
using Rebirth.World.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rebirth.World.Frames
{
    public class FriendFrame
    {
        [MessageHandler(FriendsGetListMessage.Id)]
        private void HandleFriendsGetListMessage(Client client, FriendsGetListMessage msg)
        {
            client.Send(new FriendsListMessage(new FriendInformations[0]));
        }

        [MessageHandler(SpouseGetInformationsMessage.Id)]
        private void HandleSpouseGetInformationsMessage(Client client, SpouseGetInformationsMessage msg)
        {
            //client.Send(new SpouseInformationsMessage(new FriendSpouseInformations(0, 0, "", 0, 0, 0, new EntityLook(0, new uint[0], new int[0], new int[0], new SubEntity[0]), new GuildInformations(0, "", 0, new GuildEmblem(0, 0, 0, 0)), 0)));
        }

        [MessageHandler(IgnoredGetListMessage.Id)]
        private void HandleIgnoredGetListMessage(Client client, IgnoredGetListMessage msg)
        {
            client.Send(new IgnoredListMessage(new IgnoredInformations[0]));
        }
    }
}
