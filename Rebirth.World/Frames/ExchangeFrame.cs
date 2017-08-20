using Rebirth.Common.Dispatcher;
using Rebirth.Common.GameData.D2O;
using Rebirth.Common.Protocol.Data;
using Rebirth.Common.Protocol.Messages;
using Rebirth.World.Datas.Items;
using Rebirth.World.Game.Npcs.Dialogs;
using Rebirth.World.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rebirth.World.Frames
{
    public class ExchangeFrame
    {
        [MessageHandler(ExchangeObjectMoveMessage.Id)]
        private void HandleExchangeObjectMoveMessage(Client client, ExchangeObjectMoveMessage message)
        {
            if (client.Character.Trader != null)
                client.Character.Trader.AddItem(message.objectUID, message.quantity);
        }

        [MessageHandler(ExchangeObjectMoveKamaMessage.Id)]
        private void HandleExchangeObjectMoveKamaMessage(Client client, ExchangeObjectMoveKamaMessage message)
        {
            if (client.Character.Trader != null)
                client.Character.Trader.ModifyKamas((int)message.quantity);
        }

        [MessageHandler(ExchangeReadyMessage.Id)]
        private void HandleExchangeReadyMessage(Client client, ExchangeReadyMessage message)
        {
            if (client.Character.Trader != null)
                client.Character.Trader.Validate(message.ready);
        }

        [MessageHandler(ExchangeCraftCountRequestMessage.Id)]
        private void HandleExchangeCraftCountRequestMessage(Client client, ExchangeCraftCountRequestMessage message)
        {
            if (client.Character.Trader != null)
                client.Character.Trader.ChangeReplay(message.count);
        }

        [MessageHandler(ExchangeSetCraftRecipeMessage.Id)]
        private void HandleExchangeSetCraftRecipeMessage(Client client, ExchangeSetCraftRecipeMessage message)
        {
            if (client.Character.Trader != null)
            {
                Recipe recipe = ObjectDataManager.Get<Recipe>(message.objectGID);
                if (recipe != null)
                {
                    for (int i = 0; i < recipe.ingredientIds.Count; i++)
                    {
                        PlayerItem item = client.Character.Inventory.GetItem(recipe.ingredientIds[i]);
                        if (item != null)
                            client.Character.Trader.AddItem((uint)item.Guid, (int)recipe.quantities[i]);
                    }
                }
            }
        }

        [MessageHandler(ExchangeBuyMessage.Id)]
        private void HandleExchangeBuyMessage(Client client, ExchangeBuyMessage message)
        {
            if (client.Character.Activity == null)
                return;

            if (!(client.Character.Activity is IShopDialog))
                return;

            IShopDialog dialog = (IShopDialog)client.Character.Activity;
            dialog.BuyItem((int)message.objectToBuyId, message.quantity);
        }

        //[MessageHandler(ExchangePlayerRequestMessage.Id)]
        //private void HandleExchangePlayerRequestMessage(Client client, ExchangePlayerRequestMessage message)
        //{
        //    if (client.Character.Activity != null)
        //        return;

        //    var target = client.Character.Map.GetActors().FirstOrDefault(x => x.Id == message.target);
        //    if (target == null)
        //        return;

        //    if (target.Activity != null)
        //    {
        //        client.Character.Messenger.SendServerMessage("Ce joueur est occupé.", System.Drawing.Color.Red);
        //        return;
        //    }
        //    CharacterTradeDialog dialog = new CharacterTradeDialog(target, client.Character);
        //    dialog.Open();
        //}

        //[MessageHandler(ExchangeAcceptMessage.Id, PacketActivityEnum.RP)]
        //private void HandleExchangeAcceptMessage(Client client, ExchangeAcceptMessage message)
        //{
        //    if (client.Character.Activity == null)
        //        return;

        //    if (client.Character.Activity is CharacterTradeDialog)
        //    {
        //        var activity = (CharacterTradeDialog)client.Character.Activity;
        //        if (activity.Target.Id != client.Character.Id)
        //            return;
        //        else
        //            activity.StartExchange();
        //    }
        //}

        //[MessageHandler(ExchangeOnHumanVendorRequestMessage.Id, PacketActivityEnum.RP)]
        //private void HandleExchangeOnHumanVendorRequestMessage(Client client, ExchangeOnHumanVendorRequestMessage message)
        //{
        //    var entity = client.Character.Map.GetMerchant((int)message.humanVendorId, (int)message.humanVendorCell);
        //    if (entity == null)
        //        return;
        //    var dialog = new PlayerSellVendorDialog(client.Character, entity);
        //    dialog.Open();
        //}

        [MessageHandler(ExchangeShowVendorTaxMessage.Id)]
        private void HandleExchangeShowVendorTaxMessage(Client client, ExchangeShowVendorTaxMessage message)
        {
            client.Send(new ExchangeReplyTaxVendorMessage(0, 5));
        }

        //[MessageHandler(ExchangeRequestOnShopStockMessage.Id, PacketActivityEnum.RP)]
        //private void HandleExchangeRequestOnShopStockMessage(Client client, ExchangeRequestOnShopStockMessage message)
        //{
        //    var dialog = new VendorOrganizeDialog(client.Character);
        //    dialog.Open();

        //}
        //[MessageHandler(ExchangeStartAsVendorMessage.Id, PacketActivityEnum.RP)]
        //private void HandleExchangeStartAsVendorMessage(Client client, ExchangeStartAsVendorMessage message)
        //{
        //    if (client.Character.Map.GetInterctives().Any(x => x is InteractiveZaap))
        //    {
        //        client.Character.Messenger.SendServerMessage("Interdit de se placer en mode marchants sur cette map.");
        //        return;
        //    }

        //    if (PlayerVendorManager.Instance.GetMerchantsByMapId(client.Character.MapId).Length >= 5)
        //    {
        //        client.Character.Messenger.SendServerMessage("Interdit trop de vendeurs présent sur cette map.");
        //        return;
        //    }

        //    if (PlayerVendorManager.Instance.GetMerchantItems(client.Character.Id).Length > 0)
        //    {
        //        client.Character.Record.Look.SubLooks.Add(new SubActorLook(0, SubEntityBindingPointCategoryEnum.HOOK_POINT_CATEGORY_MERCHANT_BAG, new ActorLook(
        //                     237, new short[0], new Dictionary<int, Color>(), new short[] { 100 }, new SubActorLook[0], false)));

        //        var record = new Database.Vendor.PlayerVendorSpawnRecord()
        //        {
        //            Id = PlayerVendorManager.Instance.Pop(),
        //            MapID = client.Character.Map.Id,
        //            OwnerId = client.Character.Id,
        //            CellID = client.Character.CellId,
        //            Direction = client.Character.Direction,
        //            IsNew = true
        //        };
        //        var instance = new MarchantInstance(record);


        //        PlayerVendorManager.Instance.AddMerchantSpawn(record);
        //        client.Character.Map.AddMerchant(PlayerVendorManager.Instance.GetMerchantByOwnerId(client.Character.Id));
        //        client.Dispose();
        //    }
        //    else
        //    {
        //        client.Character.Messenger.SendServerMessage("Impossible aucun items en vente.");
        //    }
        //}
    }
}
