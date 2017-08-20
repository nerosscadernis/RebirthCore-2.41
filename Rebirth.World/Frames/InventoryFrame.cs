using Rebirth.Common.Dispatcher;
using Rebirth.Common.Protocol.Messages;
using Rebirth.World.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rebirth.World.Frames
{
    public class InventoryFrame
    {
        [MessageHandler(ObjectSetPositionMessage.Id)]
        private void HandleObjectSetPositionMessage(Client client, ObjectSetPositionMessage message)
        {
            var item = client.Character.Inventory.GetItemByGUID(message.objectUID);
            if (item == null)
                return;
            //if (item.Type.id == 190)// hanarchement
            //{
            //    var mount = client.Character.Mounts.FirstOrDefault(x => x.IsEquiped == true);
            //    if (mount == null)
            //        return;

            //    mount.HanarchementApparenceID = (int)item.Template.appearanceId;
            //    client.Character.RefreshActorLook();
            //    client.Character.Inventory.DeleteItem(item);
            //}
            //else
            //{
                client.Character.Inventory.SetItemPosition(item, (Common.Protocol.Enums.CharacterInventoryPositionEnum)message.position);
            //}
        }
    }
}
