using Rebirth.World.Managers;
using Rebirth.World.Network;
using Rebirth.Common.Dispatcher;
using Rebirth.Common.Protocol.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using Rebirth.Common.Protocol.Types;

namespace Rebirth.World.Frames
{
    public class IdentificationFrame
    {
        [MessageHandler(AuthenticationTicketMessage.Id)]
        private void HandleAccountInformationsMessage(Client client, AuthenticationTicketMessage msg)
        {
            var acc = AccountManager.Instance.GetAccount(msg.ticket.Replace("\u000e", "").Replace("\0", "").Trim());
            if (acc != null)
            {
                client.Account = acc;

                client.UnRegister(typeof(IdentificationFrame));
                client.Register(typeof(CharacterFrame));

                client.Send(new AuthenticationTicketAcceptedMessage());
                client.Send(new ServerSettingsMessage("fr", 0, 0, 30));
                client.Send(new ServerOptionalFeaturesMessage(new sbyte[0]));
                client.Send(new ServerSessionConstantsMessage(new ServerSessionConstant[]
                {
                    new ServerSessionConstantInteger(1, 3900000),
                    new ServerSessionConstantLong(2, 7200000),
                    new ServerSessionConstantInteger(3, 30),
                    new ServerSessionConstantLong(4, 86400000),
                    new ServerSessionConstantLong(5, 60000),
                    new ServerSessionConstantInteger(6, 100),
                    new ServerSessionConstantLong(7, 2000)
                }));
                client.Send(new AccountCapabilitiesMessage(true, true, acc.Id, 262143, 262143, 0));
                client.Send(new TrustStatusMessage(true, false));
            }
            else
            {
                client.Send(new AuthenticationTicketRefusedMessage());
                client.Dispose();
            }
        }
    }
}
