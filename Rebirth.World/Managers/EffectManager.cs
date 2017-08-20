using Rebirth.Common.Extensions;
using Rebirth.Common.GameData.D2O;
using Rebirth.Common.IO;
using Rebirth.Common.Protocol.Data;
using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Datas.Characters;
using Rebirth.World.Datas.Items;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Datas.Spells;
using Rebirth.World.Effects.Handlers;
using Rebirth.World.Effects.Handlers.Items;
using Rebirth.World.Effects.Handlers.Spells;
using Rebirth.World.Effects.Usables;
using Rebirth.World.Game.Effect.Instances;
using Rebirth.World.Game.Fights.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Rebirth.World.Managers
{
    public class EffectManager : DataManager<EffectManager>
    {
        private delegate ItemEffectHandler ItemEffectConstructor(EffectBase effect, Character target, PlayerItem item);
        private delegate ItemEffectHandler ItemSetEffectConstructor(EffectBase effect, Character target, bool apply);
        private delegate UsableEffectHandler UsableEffectConstructor(EffectBase effect, Character target, PlayerItem item);
        private delegate SpellEffectHandler SpellEffectConstructor(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical);
        private Dictionary<short, Common.Protocol.Data.Effect> m_effects = new System.Collections.Generic.Dictionary<short, Rebirth.Common.Protocol.Data.Effect>();
        private readonly Dictionary<EffectsEnum, ItemEffectConstructor> m_itemsEffectHandler = new System.Collections.Generic.Dictionary<EffectsEnum, EffectManager.ItemEffectConstructor>();
        private readonly Dictionary<EffectsEnum, ItemSetEffectConstructor> m_itemsSetEffectHandler = new System.Collections.Generic.Dictionary<EffectsEnum, EffectManager.ItemSetEffectConstructor>();
        private readonly Dictionary<EffectsEnum, UsableEffectConstructor> m_usablesEffectHandler = new System.Collections.Generic.Dictionary<EffectsEnum, EffectManager.UsableEffectConstructor>();
        private readonly Dictionary<EffectsEnum, SpellEffectConstructor> m_spellsEffectHandler = new System.Collections.Generic.Dictionary<EffectsEnum, EffectManager.SpellEffectConstructor>();
        private readonly Dictionary<EffectsEnum, List<Type>> m_effectsHandlers = new System.Collections.Generic.Dictionary<EffectsEnum, System.Collections.Generic.List<System.Type>>();
        private readonly EffectsEnum[] m_unRandomablesEffects = new EffectsEnum[]
        {
            EffectsEnum.Effect_DamageWater,
            EffectsEnum.Effect_DamageEarth,
            EffectsEnum.Effect_DamageAir,
            EffectsEnum.Effect_DamageFire,
            EffectsEnum.Effect_DamageNeutral,
            EffectsEnum.Effect_StealHPWater,
            EffectsEnum.Effect_StealHPEarth,
            EffectsEnum.Effect_StealHPAir,
            EffectsEnum.Effect_StealHPFire,
            EffectsEnum.Effect_StealHPNeutral,
            EffectsEnum.Effect_RemoveAP,
            EffectsEnum.Effect_RemainingFights,
            EffectsEnum.Effect_HealHP_108,
            EffectsEnum.Effect_HealHP_143,
            EffectsEnum.Effect_HealHP_81,
        };

        public void Initialize()
        {
            this.m_effects = ObjectDataManager.GetAll<Rebirth.Common.Protocol.Data.Effect>().ToDictionary(entry => (short)entry.id);
            this.InitializeHandlers();
        }
        private void InitializeHandlers()
        {
            foreach (var current in
                from entry in typeof(EffectHandler).GetTypeInfo().Assembly.GetTypes()
                where entry.GetTypeInfo().IsSubclassOf(typeof(EffectHandler)) && !entry.GetTypeInfo().IsAbstract
                select entry)
            {
                if (current.GetTypeInfo().GetCustomAttribute<DefaultEffectHandlerAttribute>() == null)
                {
                    EffectHandlerAttribute[] array = current.GetTypeInfo().GetCustomAttributes<EffectHandlerAttribute>().ToArray<EffectHandlerAttribute>();
                    if (array.Length == 0)
                    {
                        Starter.Logger.Error("EffectHandler '{0}' has no EffectHandlerAttribute", current.Name);
                    }
                    else
                    {
                        foreach (EffectsEnum current2 in array.Select((EffectHandlerAttribute entry) => entry.Effect))
                        {
                            if (current.GetTypeInfo().IsSubclassOf(typeof(ItemEffectHandler)))
                            {
                                System.Reflection.ConstructorInfo constructor = current.GetConstructor(new System.Type[]
                                {
                                    typeof(EffectBase),
                                    typeof(Character),
                                    typeof(PlayerItem)
                                });
                                this.m_itemsEffectHandler.Add(current2, constructor.CreateDelegate<EffectManager.ItemEffectConstructor>());
                                System.Reflection.ConstructorInfo constructor2 = current.GetConstructor(new System.Type[]
                                {
                                    typeof(EffectBase),
                                    typeof(Character),
                                    typeof(bool)
                                });
                                if (constructor2 != null)
                                {
                                    this.m_itemsSetEffectHandler.Add(current2, constructor2.CreateDelegate<EffectManager.ItemSetEffectConstructor>());
                                }
                            }
                            else
                            {
                                if (current.GetTypeInfo().IsSubclassOf(typeof(UsableEffectHandler)))
                                {
                                    System.Reflection.ConstructorInfo constructor = current.GetConstructor(new System.Type[]
                                    {
                                        typeof(EffectBase),
                                        typeof(Character),
                                        typeof(PlayerItem)
                                    });
                                    this.m_usablesEffectHandler.Add(current2, constructor.CreateDelegate<EffectManager.UsableEffectConstructor>());
                                }
                                else
                                {
                                    if (current.GetTypeInfo().IsSubclassOf(typeof(SpellEffectHandler)))
                                    {
                                        System.Reflection.ConstructorInfo constructor = current.GetConstructor(new System.Type[]
                                        {
                                            typeof(EffectDice),
                                            typeof(Fighter),
                                            typeof(SpellTemplate),
                                            typeof(CellMap),
                                            typeof(bool)
                                       });
                                        this.m_spellsEffectHandler.Add(current2, constructor.CreateDelegate<EffectManager.SpellEffectConstructor>());
                                    }
                                }
                            }
                            if (!this.m_effectsHandlers.ContainsKey(current2))
                            {
                                this.m_effectsHandlers.Add(current2, new System.Collections.Generic.List<System.Type>());
                            }
                            this.m_effectsHandlers[current2].Add(current);
                        }
                    }
                }
            }
        }
        public EffectBase ConvertExportedEffect(EffectInstance effect)
        {
            EffectBase result;
            if (effect is EffectInstanceLadder)
            {
                result = new EffectLadder(effect as EffectInstanceLadder);
            }
            else
            {
                if (effect is EffectInstanceCreature)
                {
                    result = new EffectCreature(effect as EffectInstanceCreature);
                }
                else
                {
                    if (effect is EffectInstanceDate)
                    {
                        result = new EffectDate(effect as EffectInstanceDate);
                    }
                    else
                    {
                        if (effect is EffectInstanceDice)
                        {
                            result = new EffectDice(effect as EffectInstanceDice);
                        }
                        else
                        {
                            if (effect is EffectInstanceDuration)
                            {
                                result = new EffectDuration(effect as EffectInstanceDuration);
                            }
                            else
                            {
                                if (effect is EffectInstanceMinMax)
                                {
                                    result = new EffectMinMax(effect as EffectInstanceMinMax);
                                }
                                else
                                {
                                    if (effect is EffectInstanceMount)
                                    {
                                        result = new EffectMount(effect as EffectInstanceMount);
                                    }
                                    else
                                    {
                                        if (effect is EffectInstanceString)
                                        {
                                            result = new EffectString(effect as EffectInstanceString);
                                        }
                                        else
                                        {
                                            if (effect is EffectInstanceInteger)
                                            {
                                                result = new EffectInteger(effect as EffectInstanceInteger);
                                            }
                                            else
                                            {
                                                result = new EffectBase(effect);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
        public IEnumerable<EffectBase> ConvertExportedEffect(IEnumerable<EffectInstance> effects)
        {
            return effects.Select(new Func<EffectInstance, EffectBase>(this.ConvertExportedEffect));
        }
        public Common.Protocol.Data.Effect GetTemplate(short id)
        {
            return (!this.m_effects.ContainsKey(id)) ? null : this.m_effects[id];
        }
        public IEnumerable<Common.Protocol.Data.Effect> GetTemplates()
        {
            return this.m_effects.Values;
        }
        public void AddItemEffectHandler(ItemEffectHandler handler)
        {
            System.Type type = handler.GetType();
            if (type.GetTypeInfo().GetCustomAttribute<DefaultEffectHandlerAttribute>() != null)
            {
                throw new System.Exception("Default handler cannot be added");
            }
            EffectHandlerAttribute[] array = type.GetTypeInfo().GetCustomAttributes<EffectHandlerAttribute>().ToArray<EffectHandlerAttribute>();
            if (array.Length == 0)
            {
                throw new System.Exception(string.Format("EffectHandler '{0}' has no EffectHandlerAttribute", type.Name));
            }
            System.Reflection.ConstructorInfo constructor = type.GetConstructor(new System.Type[]
            {
                typeof(EffectBase),
                typeof(Character),
                typeof(PlayerItem)
            });
            if (constructor == null)
            {
                throw new System.Exception("No valid constructors found !");
            }
            foreach (EffectsEnum current in
                from entry in array
                select entry.Effect)
            {
                this.m_itemsEffectHandler.Add(current, constructor.CreateDelegate<EffectManager.ItemEffectConstructor>());
                if (!this.m_effectsHandlers.ContainsKey(current))
                {
                    this.m_effectsHandlers.Add(current, new System.Collections.Generic.List<System.Type>());
                }
                this.m_effectsHandlers[current].Add(type);
            }
        }
        public ItemEffectHandler GetItemEffectHandler(EffectBase effect, Character target, PlayerItem item)
        {
            ItemEffectConstructor itemEffectConstructor;
            ItemEffectHandler result;
            if (this.m_itemsEffectHandler.TryGetValue(effect.EffectId, out itemEffectConstructor))
            {
                result = itemEffectConstructor(effect, target, item);
            }
            else
            {
                result = new DefaultItemEffect(effect, target, item);
            }
            return result;
        }
        public ItemEffectHandler GetItemEffectHandler(EffectBase effect, Character target, bool apply)
        {
            ItemSetEffectConstructor itemSetEffectConstructor;
            ItemEffectHandler result;
            if (this.m_itemsSetEffectHandler.TryGetValue(effect.EffectId, out itemSetEffectConstructor))
            {
                result = itemSetEffectConstructor(effect, target, apply);
            }
            else
            {
                result = new DefaultItemEffect(effect, target, apply);
            }
            return result;
        }
        public void AddUsableEffectHandler(UsableEffectHandler handler)
        {
            System.Type type = handler.GetType();
            if (type.GetTypeInfo().GetCustomAttribute<DefaultEffectHandlerAttribute>() != null)
            {
                throw new System.Exception("Default handler cannot be added");
            }
            EffectHandlerAttribute[] array = type.GetTypeInfo().GetCustomAttributes<EffectHandlerAttribute>().ToArray<EffectHandlerAttribute>();
            if (array.Length == 0)
            {
                throw new System.Exception(string.Format("EffectHandler '{0}' has no EffectHandlerAttribute", type.Name));
            }
            System.Reflection.ConstructorInfo constructor = type.GetConstructor(new System.Type[]
            {
                typeof(EffectBase),
                typeof(Character),
                typeof(PlayerItem)
            });
            if (constructor == null)
            {
                throw new System.Exception("No valid constructors found !");
            }
            foreach (EffectsEnum current in
                from entry in array
                select entry.Effect)
            {
                this.m_usablesEffectHandler.Add(current, constructor.CreateDelegate<EffectManager.UsableEffectConstructor>());
                if (!this.m_effectsHandlers.ContainsKey(current))
                {
                    this.m_effectsHandlers.Add(current, new System.Collections.Generic.List<System.Type>());
                }
                this.m_effectsHandlers[current].Add(type);
            }
        }
        public UsableEffectHandler GetUsableEffectHandler(EffectBase effect, Character target, PlayerItem item)
        {
            UsableEffectConstructor usableEffectConstructor;
            UsableEffectHandler result;
            if (this.m_usablesEffectHandler.TryGetValue(effect.EffectId, out usableEffectConstructor))
            {
                result = usableEffectConstructor(effect, target, item);
            }
            else
            {
                result = new DefaultUsableEffectHandler(effect, target, item);
            }
            return result;
        }

        public bool IsUnRandomableWeaponEffect(EffectsEnum effect)
        {
            return this.m_unRandomablesEffects.Contains(effect);
        }
        public EffectInstance GuessRealEffect(EffectInstance effect)
        {
            EffectInstance result;
            if (!(effect is EffectInstanceDice))
            {
                result = effect;
            }
            else
            {
                EffectInstanceDice effectInstanceDice = effect as EffectInstanceDice;
                if (effectInstanceDice.value == 0 && effectInstanceDice.diceNum > 0u && effectInstanceDice.diceSide > 0u)
                {
                    result = new EffectInstanceMinMax
                    {
                        duration = effectInstanceDice.duration,
                        effectId = effectInstanceDice.effectId,
                        max = effectInstanceDice.diceSide,
                        min = effectInstanceDice.diceNum,
                        modificator = effectInstanceDice.modificator,
                        random = effectInstanceDice.random,
                        targetId = effectInstanceDice.targetId,
                        trigger = effectInstanceDice.trigger,
                        zoneShape = effectInstanceDice.zoneShape,
                        zoneSize = effectInstanceDice.zoneSize
                    };
                }
                else
                {
                    if (effectInstanceDice.value == 0 && effectInstanceDice.diceNum == 0u && effectInstanceDice.diceSide > 0u)
                    {
                        result = new EffectInstanceMinMax
                        {
                            duration = effectInstanceDice.duration,
                            effectId = effectInstanceDice.effectId,
                            max = effectInstanceDice.diceSide,
                            min = effectInstanceDice.diceNum,
                            modificator = effectInstanceDice.modificator,
                            random = effectInstanceDice.random,
                            targetId = effectInstanceDice.targetId,
                            trigger = effectInstanceDice.trigger,
                            zoneShape = effectInstanceDice.zoneShape,
                            zoneSize = effectInstanceDice.zoneSize
                        };
                    }
                    else
                    {
                        result = effect;
                    }
                }
            }
            return result;
        }
        public byte[] SerializeEffect(EffectInstance effectInstance)
        {
            return this.ConvertExportedEffect(effectInstance).Serialize();
        }
        public byte[] SerializeEffect(EffectBase effect)
        {
            return effect.Serialize();
        }
        public byte[] SerializeEffects(System.Collections.Generic.IEnumerable<EffectBase> effects)
        {
            System.Collections.Generic.List<byte> list = new System.Collections.Generic.List<byte>();
            foreach (EffectBase current in effects)
            {
                list.AddRange(current.Serialize());
            }
            return list.ToArray();
        }
        public byte[] SerializeEffects(System.Collections.Generic.IEnumerable<EffectInstance> effects)
        {
            System.Collections.Generic.List<byte> list = new System.Collections.Generic.List<byte>();
            foreach (EffectInstance current in effects)
            {
                list.AddRange(this.SerializeEffect(current));
            }
            return list.ToArray();
        }
        public System.Collections.Generic.List<EffectBase> DeserializeEffects(byte[] buffer)
        {
            System.Collections.Generic.List<EffectBase> list = new System.Collections.Generic.List<EffectBase>();
            int num = 0;
            while (num + 1 < buffer.Length)
            {
                var effect = this.DeserializeEffect(new BigEndianReader(buffer));

                if (effect is EffectMount)
                {
                    var data = effect.GetValues();
                    list.Add(new EffectMount(995, (int)data[0], 999, 89, new EffectBase()));
                    break;
                }

                list.Add(effect);
            }
            return list;
        }
        public EffectBase DeserializeEffect(IDataReader reader)
        {
            if (reader.BytesAvailable <= 0)
            {
                throw new System.Exception("buffer too small to contain an Effect");
            }
            byte b = reader.ReadByte();
            EffectBase effectBase;
            switch (b)
            {
                case 1:
                    effectBase = new EffectBase();
                    break;
                case 2:
                    effectBase = new EffectCreature();
                    break;
                case 3:
                    effectBase = new EffectDate();
                    break;
                case 4:
                    effectBase = new EffectDice();
                    break;
                case 5:
                    effectBase = new EffectDuration();
                    break;
                case 6:
                    effectBase = new EffectInteger();
                    break;
                case 7:
                    effectBase = new EffectLadder();
                    break;
                case 8:
                    effectBase = new EffectMinMax();
                    break;
                case 9:
                    effectBase = new EffectMount();
                    break;
                case 10:
                    effectBase = new EffectString();
                    break;
                default:
                    throw new System.Exception(string.Format("Incorrect identifier : {0}", b));
            }
            effectBase.DeSerialize(reader);
            return effectBase;
        }
        public SpellEffectHandler GetSpellEffectHandler(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical)
        {
            EffectManager.SpellEffectConstructor spellEffectConstructor;
            SpellEffectHandler result;
            if (this.m_spellsEffectHandler.TryGetValue(effect.EffectId, out spellEffectConstructor))
            {
                result = spellEffectConstructor(effect, caster, spell, targetedCell, critical);
            }
            else
            {
                result = new DefaultSpellEffect(effect, caster, spell, targetedCell, critical);
            }
            return result;
        }
    }
}
