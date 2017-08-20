using Rebirth.Common.Dispatcher;
using Rebirth.Common.Protocol.Enums;
using Rebirth.Common.Protocol.Messages;
using Rebirth.World.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rebirth.World.Frames
{
    public class ShortcutFrame
    {
        [MessageHandler(ShortcutBarAddRequestMessage.Id)]
        public void HandleShortcutBarAddRequestMessage(Client client, ShortcutBarAddRequestMessage message)
        {
            client.Character.Shortcuts.AddShortcut((ShortcutBarEnum)message.barType, message.shortcut);
        }

        [MessageHandler(ShortcutBarRemoveRequestMessage.Id)]
        public void HandleShortcutBarRemoveRequestMessage(Client client, ShortcutBarRemoveRequestMessage message)
        {
            client.Character.Shortcuts.RemoveShortcut((ShortcutBarEnum)message.barType, message.slot);
        }

        [MessageHandler(ShortcutBarSwapRequestMessage.Id)]
        public void HandleShortcutBarSwapRequestMessage(Client client, ShortcutBarSwapRequestMessage message)
        {
            client.Character.Shortcuts.SwapShortcuts((ShortcutBarEnum)message.barType, message.firstSlot, message.secondSlot);
        }
    }
}
