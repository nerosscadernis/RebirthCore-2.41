using Rebirth.Common.Extensions;
using Rebirth.Common.GameData.D2O;
using Rebirth.Common.IO;
using Rebirth.Common.Protocol.Data;
using Rebirth.Common.Protocol.Enums;
using Rebirth.Common.Protocol.Messages;
using Rebirth.Common.Protocol.Types;
using Rebirth.World.Datas.Bdd;
using Rebirth.World.Datas.Items;
using Rebirth.World.Datas.Shortcuts;
using Rebirth.World.Game.Effect.Instances;
using Rebirth.World.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Rebirth.World.Effects.Handlers.Items.ItemEffectHandler;

namespace Rebirth.World.Datas.Characters
{
    public class Inventaire
    {
        #region Vars
        private Character _owner;
        #endregion

        #region Properties
        public List<PlayerItem> Items;
        public double Kamas { get; set; }
        public int MaxId { get { return Items.Count <= 0 ? 0 : Items.Select(x => x.Guid).OrderByDescending(x => x).First(); } }
        #endregion

        #region Constructor / Datas
        public Inventaire(Character character)
        {
            _owner = character;
            Items = new List<PlayerItem>();
        }

        public Inventaire(byte[] datas, Character owner)
        {
            _owner = owner;
            Items = new List<PlayerItem>();
            var reader = new BigEndianReader(datas);
            Kamas = reader.ReadDouble();
            var count = reader.ReadInt();
            for (int i = 0; i < count; i++)
            {
                Items.Add(new PlayerItem(owner, reader));
            }
            reader.Dispose();
        }

        public byte[] GetDatas()
        {
            var writer = new BigEndianWriter();
            writer.WriteDouble(Kamas);
            writer.WriteInt(Items.Count);
            foreach (var item in Items)
            {
                writer.WriteBytes(item.GetDatas());
            }
            return writer.Data;
        }
        #endregion

        #region Add / Remove
        public void AddItem(List<PlayerItem> items, bool checkedStack = true)
        {
            List<PlayerItem> Modified = new List<PlayerItem>();
            List<PlayerItem> Added = new List<PlayerItem>();
            foreach (var item in items)
            {
                var isStack = IsStack(item);
                if (isStack.Count > 0 && checkedStack)
                {
                    var currentItem = isStack.First();
                    currentItem.Quantity += item.Quantity;
                    Modified.Add(currentItem);
                }
                else
                {
                    Items.Add(item);
                    Added.Add(item);
                }
            }
            _owner.Client?.Send(new ObjectsAddedMessage((from x in Added
                                                          select x.GetObjectItem()).ToArray()));
            _owner.Client?.Send(new ObjectsQuantityMessage((from x in Modified
                                                             select x.GetObjectItemQuantity()).ToArray()));
            SendInventorWeight();
        }
        public void AddItem(PlayerItem item, bool checkedStack = true)
        {
            var isStack = IsStack(item);
            if (isStack.Count > 0 && checkedStack)
            {
                var currentItem = isStack.First();
                currentItem.Quantity += item.Quantity;
                _owner.Client.Send(new ObjectModifiedMessage(currentItem.GetObjectItem()));

                ShortcutRecord st = _owner.Shortcuts.GetShortcut(currentItem);
                if (st != null)
                    _owner.Client?.Send(new ShortcutBarRefreshMessage((sbyte)ShortcutBarEnum.GENERAL_SHORTCUT_BAR, st.GetNetworkShortcut()));
            }
            else
            {
                Items.Add(item);
                var obj = new ObjectAddedMessage(item.GetObjectItem());
                _owner.Client.Send(obj);
            }
            SendInventorWeight();
        }
        public void RemoveItem(PlayerItem item, uint quantity)
        {
            if (item.Quantity - quantity <= 0)
            {
                DeleteItem(item);
            }
            else
            {
                item.Quantity -= (int)quantity;
                _owner.Client.Send(new ObjectsQuantityMessage(new ObjectItemQuantity[] { new ObjectItemQuantity((uint)item.Guid, (uint)item.Quantity) }));
                SendInventorWeight();
                ShortcutRecord st = _owner.Shortcuts.GetShortcut(item);
                if (st != null)
                    _owner.Client?.Send(new ShortcutBarRefreshMessage((sbyte)ShortcutBarEnum.GENERAL_SHORTCUT_BAR, st.GetNetworkShortcut()));
            }
        }
        public void RemoveItem(int itemId, uint quantity)
        {
            PlayerItem item = Items.FirstOrDefault(x => x.Gid == itemId);
            if (item != null)
            {
                if (item.Quantity - quantity <= 0)
                {
                    DeleteItem(item);
                }
                else
                {
                    item.Quantity -= (int)quantity;
                    _owner.Client.Send(new ObjectsQuantityMessage(new ObjectItemQuantity[] { new ObjectItemQuantity((uint)item.Guid, (uint)item.Quantity) }));
                    SendInventorWeight();
                    ShortcutRecord st = _owner.Shortcuts.GetShortcut(item);
                    if (st != null)
                        _owner.Client?.Send(new ShortcutBarRefreshMessage((sbyte)ShortcutBarEnum.GENERAL_SHORTCUT_BAR, st.GetNetworkShortcut()));
                }
            }
        }
        private void DeleteItem(PlayerItem item)
        {
            Items.Remove(item);
            if (item.Position != CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED)
            {
                item.ApplyStats(HandlerOperation.UNAPPLY);
            }

            _owner.Client.Send(new ObjectDeletedMessage((uint)item.Guid));
            SendInventorWeight();

            ShortcutRecord st = _owner.Shortcuts.GetShortcut(item);
            if (st != null)
                _owner.Client?.Send(new ShortcutBarRefreshMessage((sbyte)ShortcutBarEnum.GENERAL_SHORTCUT_BAR, st.GetNetworkShortcut()));
        }
        #endregion

        #region Stack
        private List<PlayerItem> IsStack(PlayerItem item)
        {
            var items = Items.FindAll(entry => entry.Effects.CompareEnumerable(item.Effects) && entry.Gid == item.Gid && entry.Position == CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED).ToList();

            return items;
        }
        #endregion

        #region Positions
        public void SetItemPosition(PlayerItem item, CharacterInventoryPositionEnum position)
        {
                bool modif = false;
                if (item == null)
                    return;
                if (position == CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED)
                {
                    var currentItem = Items.FirstOrDefault(entry => entry.Guid == item.Guid);
                    //RefreshStats();
                    CheckPano(currentItem, true);
                    var isStack = IsStack(item);
                    if (isStack.Count > 0)
                    {
                        var unCutItem = isStack.First();

                        unCutItem.Quantity += item.Quantity;
                        currentItem.Position = CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED;
                        _owner.Client.Send(new ObjectMovementMessage((uint)item.Guid, (byte)position));

                        DeleteItem(item);
                        _owner.Client.Send(new ObjectModifiedMessage(unCutItem.GetObjectItem()));

                    }
                    else
                    {
                        currentItem.Position = CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED;
                        _owner.Client.Send(new ObjectMovementMessage((uint)item.Guid, (byte)position));
                    }
                    CheckEquipped();
                    modif = true;
                }
                else
                {
                    var itemOnPosition = GetItemByPosition(position);
                    switch (position)
                    {
                        case CharacterInventoryPositionEnum.ACCESSORY_POSITION_HAT:
                            if (item.Template.typeId == 16)
                                break;
                            else
                                return;
                        case CharacterInventoryPositionEnum.ACCESSORY_POSITION_CAPE:
                            if (item.Template.typeId == 17 || item.Template.typeId == 81)
                                break;
                            else
                                return;
                        case CharacterInventoryPositionEnum.ACCESSORY_POSITION_BELT:
                            if (item.Template.typeId == 10)
                                break;
                            else
                                return;
                        case CharacterInventoryPositionEnum.ACCESSORY_POSITION_BOOTS:
                            if (item.Template.typeId == 11)
                                break;
                            else
                                return;
                        case CharacterInventoryPositionEnum.ACCESSORY_POSITION_AMULET:
                            if (item.Template.typeId == 1)
                                break;
                            else
                                return;
                        case CharacterInventoryPositionEnum.ACCESSORY_POSITION_SHIELD:
                            if (item.Template.typeId == 82)
                                break;
                            else
                                return;
                        case CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON:
                            if (item.Template.typeId == 4 || item.Template.typeId == 3 || item.Template.typeId == 19 || item.Template.typeId == 8 ||
                                item.Template.typeId == 5 || item.Template.typeId == 6 || item.Template.typeId == 21 || item.Template.typeId == 2 ||
                                item.Template.typeId == 7)
                                break;
                            else
                                return;
                        case CharacterInventoryPositionEnum.ACCESSORY_POSITION_PETS:
                            if (item.Template.typeId == 18 || item.Template.typeId == 121)
                                break;
                            else
                            {
                                //if (itemOnPosition.Template.typeId == 18)
                                //    (itemOnPosition as PetItem).Eat(item);
                                return;
                            }
                        case CharacterInventoryPositionEnum.INVENTORY_POSITION_RING_LEFT:
                        case CharacterInventoryPositionEnum.INVENTORY_POSITION_RING_RIGHT:
                            if (item.Template.typeId == 9)
                                break;
                            else
                                return;
                        case CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_1:
                        case CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_2:
                        case CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_3:
                        case CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_4:
                        case CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_5:
                        case CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_6:
                            if (item.Template.typeId == 23 || item.Template.typeId == 151)
                                break;
                            else
                                return;
                        case CharacterInventoryPositionEnum.INVENTORY_POSITION_BOOST_FOOD:
                            if (item.Template.typeId == 28)
                                break;
                            else
                                return;
                        case CharacterInventoryPositionEnum.INVENTORY_POSITION_COMPANION:
                        default:
                            return;
                    }
                    // Aucun item
                    if (itemOnPosition == null)
                    {
                        if (CanEquip(item))
                        {
                            RemoveDuplicata(item);
                            if (item.Quantity > 1)
                            {
                                item.Quantity--;
                                _owner.Client.Send(new ObjectModifiedMessage(item.GetObjectItem()));

                                ///On créé un nouvelle items
                                var cutItem = new PlayerItem(_owner, item.Gid, 1, item.Effects.ToList());
                                AddItem(cutItem, false);
                                // on mets l'item
                                this.SetItemPosition(cutItem, position);
                            }
                            else
                            {
                                _owner.Client.Send(new ObjectMovementMessage((uint)item.Guid, (byte)position));
                                item.Position = position;
                                //RefreshStats();
                            }
                            CheckPano(item, false);
                            CheckEquipped();

                            //if (item.Type.id == 121)
                            //    _owner.UtilityLook.SetPetMount(item);

                            modif = true;
                        }
                    }
                    else
                    {
                        if (CanEquip(item))
                        {
                            RemoveDuplicata(item);
                            var currentItem = Items.FirstOrDefault(entry => entry.Position == position);
                            SetItemPosition(currentItem, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);
                            SetItemPosition(item, position);
                            modif = true;
                        }
                    }
                }

                if (modif)
                {
                    SendInventorWeight();
                    _owner.RefreshStats();
                    //_owner.RefreshActorLook();
                }
        }

        public void CheckEquipped()
        {
            foreach (var item in GetEquipedItems())
            {
                if (!CanEquip(item))
                    SetItemPosition(item, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);
            }
        }

        public PlayerItem[] GetEquipedItems()
        {
            var list = (
                from entry in Items
                where entry.IsEquiped()
                select entry).ToArray<PlayerItem>();
            return list;
        }

        public bool CanEquip(PlayerItem item)
        {
            bool result = true;

            if (item.Template.level > _owner.Infos.Level)
                result = false;

            if (item.Template.criteria != "null" && item.Template.criteria != "")
            {
                var condition = new ConditionParser(item.Template.criteria);
                if (!condition.Parse().Eval(_owner))
                    result = false;
            }
            return result;
        }

        public void RemoveDuplicata(PlayerItem item, bool noSet = false)
        {
            var lastItem = GetEquipedItems().FirstOrDefault(x => x.Position != CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED && x.Guid != item.Guid && x.Template.id == item.Template.id);
            if (lastItem != null && !noSet)
                SetItemPosition(lastItem, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);
            else if (lastItem != null && noSet)
                lastItem.Position = CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED;
        }

        public PlayerItem GetItemByPosition(CharacterInventoryPositionEnum position)
        {
            return Items.FirstOrDefault(entry => entry.Position == position); ;
        }
        #endregion

        #region Panoplie
        private Dictionary<uint, int> PanoBoost = new Dictionary<uint, int>();
        private void CheckPano(PlayerItem item, bool remove)
        {
            var pano = ObjectDataManager.Get<ItemSet>(item.Template.itemSetId);
            if (pano != null)
            {
                var itemCount = CountOfItemSet(pano.id);
                if (itemCount > 0)
                {
                    var effects = (
                        from effect in EffectManager.Instance.ConvertExportedEffect(pano.effects[itemCount])
                        select EffectManager.Instance.IsUnRandomableWeaponEffect(effect.EffectId) ? effect : effect.GenerateEffect(EffectGenerationContext.Item, EffectGenerationType.Normal)).ToArray<EffectBase>();
                    if (remove)
                    {
                        if (PanoBoost.ContainsKey(pano.id))
                        {
                            var oldEffects = (
                                from effect in EffectManager.Instance.ConvertExportedEffect(pano.effects[PanoBoost[pano.id]])
                                select EffectManager.Instance.IsUnRandomableWeaponEffect(effect.EffectId) ? effect : effect.GenerateEffect(EffectGenerationContext.Item, EffectGenerationType.Normal)).ToArray<EffectBase>();
                            ApplyStats(item, oldEffects, HandlerOperation.UNAPPLY);
                            if (itemCount == 1)
                                PanoBoost.Remove(pano.id);
                            else
                            {
                                PanoBoost[pano.id] = itemCount - 1;
                                effects = (
                                    from effect in EffectManager.Instance.ConvertExportedEffect(pano.effects[itemCount - 1])
                                    select EffectManager.Instance.IsUnRandomableWeaponEffect(effect.EffectId) ? effect : effect.GenerateEffect(EffectGenerationContext.Item, EffectGenerationType.Normal)).ToArray<EffectBase>();
                                ApplyStats(item, effects, HandlerOperation.APPLY);
                            }
                        }
                    }
                    else
                    {
                        if (PanoBoost.ContainsKey(pano.id))
                        {
                            var oldEffects = (
                                from effect in EffectManager.Instance.ConvertExportedEffect(pano.effects[PanoBoost[pano.id]])
                                select EffectManager.Instance.IsUnRandomableWeaponEffect(effect.EffectId) ? effect : effect.GenerateEffect(EffectGenerationContext.Item, EffectGenerationType.Normal)).ToArray<EffectBase>();
                            ApplyStats(item, oldEffects, HandlerOperation.UNAPPLY);
                            PanoBoost[pano.id] = itemCount;
                        }
                        else
                            PanoBoost.Add(pano.id, itemCount);
                        ApplyStats(item, effects, HandlerOperation.APPLY);
                    }
                    UpdateCountPano();
                }
                //else if(remove)
                //{
                //    if (PanoBoost.ContainsKey(pano.id))
                //    {
                //        var oldEffects = (
                //            from effect in EffectManager.Instance.ConvertExportedEffect(pano.effects[PanoBoost[pano.id]])
                //            select EffectManager.Instance.IsUnRandomableWeaponEffect(effect.EffectId) ? effect : effect.GenerateEffect(EffectGenerationContext.Item, EffectGenerationType.Normal)).ToArray<EffectBase>();
                //        ApplyStats(item, oldEffects, Effect.Handlers.Items.ItemEffectHandler.HandlerOperation.UNAPPLY);
                //        PanoBoost.Remove(pano.id);
                //    }
                //}

            }
        }
        private void ApplyStats(PlayerItem item, EffectBase[] effects, HandlerOperation operation)
        {
            foreach (var effect in effects)
            {
                var handler = EffectManager.Instance.GetItemEffectHandler(effect, _owner, item);
                handler.Operation = operation;
                handler.Apply(null);
            }
        }
        private int CountOfItemSet(uint itemSetId)
        {
            return GetEquipedItems().Count(x => x.Template.itemSetId == itemSetId) - 1;
        }
        private void UpdateCountPano()
        {
            int i = 0;
            foreach (var item in PanoBoost)
            {
                i += item.Value;
            }
            _owner.Stats[PlayerFields.BonusPano].Base = i;
        }
        #endregion

        #region Public Methods
        public void SendInventorWeight()
        {
            _owner.Client.Send(new InventoryWeightMessage((uint)Items.Sum(x => x.Weight), 1000));
        }
        public bool IsEquip(PlayerItem item)
        {
            return Items.Any(x => x.Guid == item.Guid && x.Position != CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);
        }
        public InventoryContentMessage GetInventoryContentMessage()
        {
            return new InventoryContentMessage(Items.Select(x => x.GetObjectItem()).ToArray(), (uint)Kamas);
        }
        public ObjectItem[] GetObjectItems()
        {
            return Items.Select(x => x.GetObjectItem()).ToArray();
        }
        public PlayerItem GetItemByGUID(uint id)
        {
            return Items.FirstOrDefault(entry => entry.Guid == id);
        }
        public PlayerItem GetItem(int id)
        {
            return Items.FirstOrDefault(x => x.Gid == id);
        }
        #endregion

        #region Private Methods

        #endregion
    }
}
