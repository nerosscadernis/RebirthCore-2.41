using Rebirth.Common.Extensions;
using Rebirth.Common.GameData.D2O;
using Rebirth.Common.Protocol.Data;
using Rebirth.Common.Protocol.Enums;
using Rebirth.Common.Protocol.Messages;
using Rebirth.Common.Protocol.Types;
using Rebirth.World.Datas.Characters;
using Rebirth.World.Datas.Items;
using Rebirth.World.Game.Effect.Instances;
using Rebirth.World.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rebirth.World.Datas.Jobs
{
    public class Atelier
    {
        public JobTemplate Job { get; set; }
        public uint SkillId { get; set; }
        public Character User { get; set; }

        public int Replay = 1;

        public Atelier(JobTemplate job, Character character, uint skillId)
        {
            Job = job;
            User = character;
            SkillId = skillId;
        }

        public void Use()
        {
            User.Trader.ItemAdd += UserItemAdd;
            User.Trader.ItemQuantityChanged += UserItemModified;
            User.Trader.ItemRemoved += UserItemRemoved;
            User.Trader.IsValidate += UserValidate;
            User.Trader.ReplayChanged += UserChangeReplay;
            User.Client.Send(new ExchangeStartOkCraftWithInformationMessage(SkillId));
        }

        public void UserItemAdd(TradeItem item, int traderId)
        {
            User.Client.Send(new ExchangeObjectAddedMessage(false, item.GetObjectItem()));
        }

        public void UserItemModified(TradeItem item, int traderId)
        {
            User.Client.Send(new ExchangeObjectModifiedMessage(false, item.GetObjectItem()));
        }

        public void UserItemRemoved(TradeItem item, int traderId)
        {
            User.Client.Send(new ExchangeObjectRemovedMessage(false, (uint)item._item.Guid));
        }

        public void UserValidate(bool validate, int traderId)
        {
            List<Common.Protocol.Data.Recipe> recipes = ObjectDataManager.GetAll<Recipe>(x => x.jobId == Job.Id && x.resultLevel <= Job.Level);
            CraftResultEnum result = CraftResultEnum.CRAFT_SUCCESS;
            foreach (var recipe in recipes.Where(entry => entry.skillId == SkillId))
            {
                result = CraftResultEnum.CRAFT_SUCCESS;
                for (int i = 0; i < recipe.ingredientIds.Count; i++)
                {
                    if (!User.Trader.ContainsItem(recipe.ingredientIds[i], (int)recipe.quantities[i]))
                    {
                        result = CraftResultEnum.CRAFT_IMPOSSIBLE;
                        break;
                    }
                    else if (recipe.quantities[i] * Replay > User.Inventory.GetItem(recipe.ingredientIds[i]).Quantity)
                    {
                        result = CraftResultEnum.CRAFT_FAILED;
                        break;
                    }
                }
                if (result == CraftResultEnum.CRAFT_SUCCESS)
                {
                    Craft(recipe.resultId, recipe.ingredientIds, recipe.quantities);
                    return;
                }
            }
            switch (result)
            {
                case CraftResultEnum.CRAFT_IMPOSSIBLE:
                case CraftResultEnum.CRAFT_FAILED:
                    User.Client.Send(new ExchangeCraftResultMessage((sbyte)result));
                    break;
            }
        }

        public void UserChangeReplay(int count, int traderId)
        {
            Replay = count;
            User.Client.Send(new ExchangeCraftCountModifiedMessage(count));
        }

        public void Craft(int itemId, List<int> ingrediants, List<uint> quantities)
        {
            Common.Protocol.Data.Item item = ObjectDataManager.Get<Common.Protocol.Data.Item>(itemId);
            if (item != null)
            {
                for (int i = 0; i < ingrediants.Count; i++)
                {
                    User.Inventory.RemoveItem(ingrediants[i], (uint)(quantities[i] * Replay));
                }

                List<PlayerItem> newItems = new List<PlayerItem>();

                for (int i = 0; i < Replay; i++)
                {
                    PlayerItem newItem = new PlayerItem(User, item.id, 1, EffectGenerationType.Normal);

                    if (IsStack(newItem, newItems))
                    {
                        var currentItem = newItems.First(entry => entry.Effects.CompareEnumerable(newItem.Effects) && entry.Gid == newItem.Gid);
                        currentItem.Quantity += newItem.Quantity;
                    }
                    else
                        newItems.Add(newItem);
                }

                foreach (var newItem in newItems)
                {
                    User.Quests.ValidParam(newItem.Gid, newItem.Quantity, 17);
                }

                User.Inventory.AddItem(newItems);

                var exp = item.level < Job.Level ? (long)((item.level * 20) * Math.Pow(0.908, (Job.Level - item.level)) * Replay) * 3 : item.level * 20 * Replay * 3;
                Job.Experience += exp;

                User.Client.Send(new ExchangeCraftResultWithObjectDescMessage((sbyte)CraftResultEnum.CRAFT_SUCCESS, CreateObjectItemNotInContainer(newItems)));
                User.Client.Send(new JobExperienceUpdateMessage(Job.GetJobExperience()));
            }
            foreach (var itemClear in User.Trader.Items)
            {
                UserItemRemoved(itemClear, (int)User.Infos.Id);
            }
            User.Trader.Items.Clear();
            Replay = 1;
        }

        private bool IsStack(PlayerItem item, List<PlayerItem> m_items)
        {
            var items = m_items.Where(entry => entry.Effects.CompareEnumerable(item.Effects) && entry.Gid == item.Gid && entry.Position == CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED).ToList();

            if (items.Count > 0)
                return true;
            return false;
        }

        public ObjectItemNotInContainer CreateObjectItemNotInContainer(List<PlayerItem> items)
        {
            Dictionary<EffectsEnum, EffectMinMax> effects = new Dictionary<EffectsEnum, EffectMinMax>();
            foreach (var item in items)
            {
                foreach (var effect in item.Effects)
                {
                    if (effect is EffectInteger)
                    {
                        EffectInteger effectInt = effect as EffectInteger;
                        if (effects.ContainsKey(effect.EffectId))
                        {
                            if (effects[effect.EffectId].ValueMin > effectInt.Value)
                                effects[effect.EffectId].ValueMin = (short)effectInt.Value;
                            if (effects[effect.EffectId].ValueMax < effectInt.Value)
                                effects[effect.EffectId].ValueMax = (short)effectInt.Value;
                        }
                        else
                        {
                            effects.Add(effect.EffectId, new EffectMinMax((short)effect.EffectId, (short)effectInt.Value, (short)effectInt.Value, effect));
                        }
                    }
                    else if (effect is EffectMinMax)
                    {
                        if (!effects.ContainsKey(effect.EffectId))
                        {
                            effects.Add(effect.EffectId, effect as EffectMinMax);
                        }
                    }
                }
            }
            return new ObjectItemNotInContainer((uint)items[0].Template.id, (from x in effects
                                                                             select x.Value.GetObjectEffect()).ToArray(), 0, 1);
        }

        public bool Close()
        {
            User.Trader = null;
            return false;
        }

    }
}
