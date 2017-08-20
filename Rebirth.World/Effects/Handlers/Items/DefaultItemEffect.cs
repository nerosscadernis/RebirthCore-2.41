using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Datas.Characters;
using Rebirth.World.Datas.Items;
using Rebirth.World.Game.Effect.Instances;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rebirth.World.Effects.Handlers.Items
{
    [DefaultEffectHandler]
    public class DefaultItemEffect : ItemEffectHandler
    {
        public delegate void EffectComputeHandler(Character target, EffectInteger effect, bool isBoost);
        public static readonly System.Collections.Generic.Dictionary<PlayerFields, DefaultItemEffect.EffectComputeHandler> m_addMethods = new System.Collections.Generic.Dictionary<PlayerFields, DefaultItemEffect.EffectComputeHandler>
        {

            {
                PlayerFields.Health,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddHealth)
            },

            {
                PlayerFields.Initiative,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddInitiative)
            },

            {
                PlayerFields.Prospecting,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddProspecting)
            },

            {
                PlayerFields.AP,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddAP)
            },

            {
                PlayerFields.MP,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddMP)
            },

            {
                PlayerFields.Strength,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddStrength)
            },

            {
                PlayerFields.Vitality,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddVitality)
            },

            {
                PlayerFields.Wisdom,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddWisdom)
            },

            {
                PlayerFields.Chance,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddChance)
            },

            {
                PlayerFields.Agility,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddAgility)
            },

            {
                PlayerFields.Intelligence,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddIntelligence)
            },

            {
                PlayerFields.Range,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddRange)
            },

            {
                PlayerFields.SummonLimit,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddSummonLimit)
            },

            {
                PlayerFields.DamageReflection,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddDamageReflection)
            },

            {
                PlayerFields.CriticalHit,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddCriticalHit)
            },

            {
                PlayerFields.CriticalMiss,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddCriticalMiss)
            },

            {
                PlayerFields.HealBonus,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddHealBonus)
            },

            {
                PlayerFields.DamageBonus,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddDamageBonus)
            },

            {
                PlayerFields.WeaponDamageBonus,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddWeaponDamageBonus)
            },

            {
                PlayerFields.DamageBonusPercent,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddDamageBonusPercent)
            },

            {
                PlayerFields.TrapBonus,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddTrapBonus)
            },

            {
                PlayerFields.TrapBonusPercent,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddTrapBonusPercent)
            },

            {
                PlayerFields.PermanentDamagePercent,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddPermanentDamagePercent)
            },

            {
                PlayerFields.TackleBlock,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddTackleBlock)
            },

            {
                PlayerFields.TackleEvade,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddTackleEvade)
            },

            {
                PlayerFields.APAttack,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddAPAttack)
            },

            {
                PlayerFields.MPAttack,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddMPAttack)
            },

            {
                PlayerFields.PushDamageBonus,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddPushDamageBonus)
            },

            {
                PlayerFields.CriticalDamageBonus,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddCriticalDamageBonus)
            },

            {
                PlayerFields.NeutralDamageBonus,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddNeutralDamageBonus)
            },

            {
                PlayerFields.EarthDamageBonus,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddEarthDamageBonus)
            },

            {
                PlayerFields.WaterDamageBonus,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddWaterDamageBonus)
            },

            {
                PlayerFields.AirDamageBonus,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddAirDamageBonus)
            },

            {
                PlayerFields.FireDamageBonus,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddFireDamageBonus)
            },

            {
                PlayerFields.DodgeAPProbability,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddDodgeAPProbability)
            },

            {
                PlayerFields.DodgeMPProbability,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddDodgeMPProbability)
            },

            {
                PlayerFields.NeutralResistPercent,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddNeutralResistPercent)
            },

            {
                PlayerFields.EarthResistPercent,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddEarthResistPercent)
            },

            {
                PlayerFields.WaterResistPercent,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddWaterResistPercent)
            },

            {
                PlayerFields.AirResistPercent,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddAirResistPercent)
            },

            {
                PlayerFields.FireResistPercent,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddFireResistPercent)
            },

            {
                PlayerFields.NeutralElementReduction,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddNeutralElementReduction)
            },

            {
                PlayerFields.EarthElementReduction,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddEarthElementReduction)
            },

            {
                PlayerFields.WaterElementReduction,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddWaterElementReduction)
            },

            {
                PlayerFields.AirElementReduction,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddAirElementReduction)
            },

            {
                PlayerFields.FireElementReduction,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddFireElementReduction)
            },

            {
                PlayerFields.PushDamageReduction,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddPushDamageReduction)
            },

            {
                PlayerFields.CriticalDamageReduction,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddCriticalDamageReduction)
            },

            {
                PlayerFields.PvpNeutralResistPercent,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddPvpNeutralResistPercent)
            },

            {
                PlayerFields.PvpEarthResistPercent,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddPvpEarthResistPercent)
            },

            {
                PlayerFields.PvpWaterResistPercent,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddPvpWaterResistPercent)
            },

            {
                PlayerFields.PvpAirResistPercent,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddPvpAirResistPercent)
            },

            {
                PlayerFields.PvpFireResistPercent,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddPvpFireResistPercent)
            },

            {
                PlayerFields.PvpNeutralElementReduction,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddPvpNeutralElementReduction)
            },

            {
                PlayerFields.PvpEarthElementReduction,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddPvpEarthElementReduction)
            },

            {
                PlayerFields.PvpWaterElementReduction,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddPvpWaterElementReduction)
            },

            {
                PlayerFields.PvpAirElementReduction,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddPvpAirElementReduction)
            },

            {
                PlayerFields.PvpFireElementReduction,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddPvpFireElementReduction)
            },

            {
                PlayerFields.GlobalDamageReduction,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddGlobalDamageReduction)
            },

            {
                PlayerFields.DamageMultiplicator,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddDamageMultiplicator)
            },

            {
                PlayerFields.PhysicalDamage,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddPhysicalDamage)
            },

            {
                PlayerFields.MagicDamage,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddMagicDamage)
            },

            {
                PlayerFields.PhysicalDamageReduction,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddPhysicalDamageReduction)
            },

            {
                PlayerFields.MagicDamageReduction,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.AddMagicDamageReduction)
            }
        };
        private static readonly System.Collections.Generic.Dictionary<PlayerFields, DefaultItemEffect.EffectComputeHandler> m_subMethods = new System.Collections.Generic.Dictionary<PlayerFields, DefaultItemEffect.EffectComputeHandler>
        {

            {
                PlayerFields.Health,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubHealth)
            },

            {
                PlayerFields.Initiative,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubInitiative)
            },

            {
                PlayerFields.Prospecting,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubProspecting)
            },

            {
                PlayerFields.AP,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubAP)
            },

            {
                PlayerFields.MP,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubMP)
            },

            {
                PlayerFields.Strength,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubStrength)
            },

            {
                PlayerFields.Vitality,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubVitality)
            },

            {
                PlayerFields.Wisdom,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubWisdom)
            },

            {
                PlayerFields.Chance,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubChance)
            },

            {
                PlayerFields.Agility,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubAgility)
            },

            {
                PlayerFields.Intelligence,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubIntelligence)
            },

            {
                PlayerFields.Range,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubRange)
            },

            {
                PlayerFields.SummonLimit,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubSummonLimit)
            },

            {
                PlayerFields.DamageReflection,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubDamageReflection)
            },

            {
                PlayerFields.CriticalHit,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubCriticalHit)
            },

            {
                PlayerFields.CriticalMiss,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubCriticalMiss)
            },

            {
                PlayerFields.HealBonus,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubHealBonus)
            },

            {
                PlayerFields.DamageBonus,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubDamageBonus)
            },

            {
                PlayerFields.WeaponDamageBonus,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubWeaponDamageBonus)
            },

            {
                PlayerFields.DamageBonusPercent,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubDamageBonusPercent)
            },

            {
                PlayerFields.TrapBonus,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubTrapBonus)
            },

            {
                PlayerFields.TrapBonusPercent,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubTrapBonusPercent)
            },

            {
                PlayerFields.PermanentDamagePercent,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubPermanentDamagePercent)
            },

            {
                PlayerFields.TackleBlock,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubTackleBlock)
            },

            {
                PlayerFields.TackleEvade,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubTackleEvade)
            },

            {
                PlayerFields.APAttack,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubAPAttack)
            },

            {
                PlayerFields.MPAttack,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubMPAttack)
            },

            {
                PlayerFields.PushDamageBonus,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubPushDamageBonus)
            },

            {
                PlayerFields.CriticalDamageBonus,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubCriticalDamageBonus)
            },

            {
                PlayerFields.NeutralDamageBonus,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubNeutralDamageBonus)
            },

            {
                PlayerFields.EarthDamageBonus,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubEarthDamageBonus)
            },

            {
                PlayerFields.WaterDamageBonus,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubWaterDamageBonus)
            },

            {
                PlayerFields.AirDamageBonus,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubAirDamageBonus)
            },

            {
                PlayerFields.FireDamageBonus,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubFireDamageBonus)
            },

            {
                PlayerFields.DodgeAPProbability,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubDodgeAPProbability)
            },

            {
                PlayerFields.DodgeMPProbability,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubDodgeMPProbability)
            },

            {
                PlayerFields.NeutralResistPercent,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubNeutralResistPercent)
            },

            {
                PlayerFields.EarthResistPercent,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubEarthResistPercent)
            },

            {
                PlayerFields.WaterResistPercent,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubWaterResistPercent)
            },

            {
                PlayerFields.AirResistPercent,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubAirResistPercent)
            },

            {
                PlayerFields.FireResistPercent,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubFireResistPercent)
            },

            {
                PlayerFields.NeutralElementReduction,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubNeutralElementReduction)
            },

            {
                PlayerFields.EarthElementReduction,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubEarthElementReduction)
            },

            {
                PlayerFields.WaterElementReduction,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubWaterElementReduction)
            },

            {
                PlayerFields.AirElementReduction,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubAirElementReduction)
            },

            {
                PlayerFields.FireElementReduction,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubFireElementReduction)
            },

            {
                PlayerFields.PushDamageReduction,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubPushDamageReduction)
            },

            {
                PlayerFields.CriticalDamageReduction,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubCriticalDamageReduction)
            },

            {
                PlayerFields.PvpNeutralResistPercent,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubPvpNeutralResistPercent)
            },

            {
                PlayerFields.PvpEarthResistPercent,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubPvpEarthResistPercent)
            },

            {
                PlayerFields.PvpWaterResistPercent,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubPvpWaterResistPercent)
            },

            {
                PlayerFields.PvpAirResistPercent,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubPvpAirResistPercent)
            },

            {
                PlayerFields.PvpFireResistPercent,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubPvpFireResistPercent)
            },

            {
                PlayerFields.PvpNeutralElementReduction,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubPvpNeutralElementReduction)
            },

            {
                PlayerFields.PvpEarthElementReduction,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubPvpEarthElementReduction)
            },

            {
                PlayerFields.PvpWaterElementReduction,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubPvpWaterElementReduction)
            },

            {
                PlayerFields.PvpAirElementReduction,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubPvpAirElementReduction)
            },

            {
                PlayerFields.PvpFireElementReduction,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubPvpFireElementReduction)
            },

            {
                PlayerFields.GlobalDamageReduction,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubGlobalDamageReduction)
            },

            {
                PlayerFields.DamageMultiplicator,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubDamageMultiplicator)
            },

            {
                PlayerFields.PhysicalDamage,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubPhysicalDamage)
            },

            {
                PlayerFields.MagicDamage,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubMagicDamage)
            },

            {
                PlayerFields.PhysicalDamageReduction,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubPhysicalDamageReduction)
            },

            {
                PlayerFields.MagicDamageReduction,
                new DefaultItemEffect.EffectComputeHandler(DefaultItemEffect.SubMagicDamageReduction)
            },
        };
        public static readonly System.Collections.Generic.Dictionary<EffectsEnum, PlayerFields> m_addEffectsBinds = new System.Collections.Generic.Dictionary<EffectsEnum, PlayerFields>
        {

            {
                EffectsEnum.Effect_AddHealth,
                PlayerFields.Health
            },

            {
                EffectsEnum.Effect_AddInitiative,
                PlayerFields.Initiative
            },

            {
                EffectsEnum.Effect_AddProspecting,
                PlayerFields.Prospecting
            },

            {
                EffectsEnum.Effect_AddAP_111,
                PlayerFields.AP
            },

            {
                EffectsEnum.Effect_RegainAP,
                PlayerFields.AP
            },

            {
                EffectsEnum.Effect_AddMP,
                PlayerFields.MP
            },

            {
                EffectsEnum.Effect_AddMP_128,
                PlayerFields.MP
            },

            {
                EffectsEnum.Effect_AddStrength,
                PlayerFields.Strength
            },

            {
                EffectsEnum.Effect_AddVitality,
                PlayerFields.Vitality
            },

            {
                EffectsEnum.Effect_AddWisdom,
                PlayerFields.Wisdom
            },

            {
                EffectsEnum.Effect_AddChance,
                PlayerFields.Chance
            },

            {
                EffectsEnum.Effect_AddAgility,
                PlayerFields.Agility
            },

            {
                EffectsEnum.Effect_AddIntelligence,
                PlayerFields.Intelligence
            },

            {
                EffectsEnum.Effect_AddRange,
                PlayerFields.Range
            },

            {
                EffectsEnum.Effect_AddSummonLimit,
                PlayerFields.SummonLimit
            },

            {
                EffectsEnum.Effect_AddDamageReflection,
                PlayerFields.DamageReflection
            },

            {
                EffectsEnum.Effect_AddCriticalHit,
                PlayerFields.CriticalHit
            },

            {
                EffectsEnum.Effect_AddCriticalMiss,
                PlayerFields.CriticalMiss
            },

            {
                EffectsEnum.Effect_AddHealBonus,
                PlayerFields.HealBonus
            },

            {
                EffectsEnum.Effect_AddDamageBonus,
                PlayerFields.DamageBonus
            },

            {
                EffectsEnum.Effect_AddDamageBonus_121,
                PlayerFields.DamageBonus
            },

            {
                EffectsEnum.Effect_IncreaseDamage_138,
                PlayerFields.DamageBonusPercent
            },

            {
                EffectsEnum.Effect_AddDamageBonusPercent,
                PlayerFields.DamageBonusPercent
            },

            {
                EffectsEnum.Effect_AddTrapBonus,
                PlayerFields.TrapBonus
            },

            {
                EffectsEnum.Effect_AddTrapBonusPercent,
                PlayerFields.TrapBonusPercent
            },

            {
                EffectsEnum.Effect_AddCriticalDamageBonus,
                PlayerFields.CriticalDamageBonus
            },

            {
                EffectsEnum.Effect_APAttack,
                PlayerFields.APAttack
            },

            {
                EffectsEnum.Effect_AddNeutralDamageBonus,
                PlayerFields.NeutralDamageBonus
            },

            {
                EffectsEnum.Effect_AddEarthDamageBonus,
                PlayerFields.EarthDamageBonus
            },

            {
                EffectsEnum.Effect_AddWaterDamageBonus,
                PlayerFields.WaterDamageBonus
            },

            {
                EffectsEnum.Effect_AddAirDamageBonus,
                PlayerFields.AirDamageBonus
            },

            {
                EffectsEnum.Effect_AddFireDamageBonus,
                PlayerFields.FireDamageBonus
            },

            {
                EffectsEnum.Effect_AddNeutralResistPercent,
                PlayerFields.NeutralResistPercent
            },

            {
                EffectsEnum.Effect_AddEarthResistPercent,
                PlayerFields.EarthResistPercent
            },

            {
                EffectsEnum.Effect_AddWaterResistPercent,
                PlayerFields.WaterResistPercent
            },

            {
                EffectsEnum.Effect_AddAirResistPercent,
                PlayerFields.AirResistPercent
            },

            {
                EffectsEnum.Effect_AddFireResistPercent,
                PlayerFields.FireResistPercent
            },

            {
                EffectsEnum.Effect_AddNeutralElementReduction,
                PlayerFields.NeutralElementReduction
            },

            {
                EffectsEnum.Effect_AddEarthElementReduction,
                PlayerFields.EarthElementReduction
            },

            {
                EffectsEnum.Effect_AddWaterElementReduction,
                PlayerFields.WaterElementReduction
            },

            {
                EffectsEnum.Effect_AddAirElementReduction,
                PlayerFields.AirElementReduction
            },

            {
                EffectsEnum.Effect_AddFireElementReduction,
                PlayerFields.FireElementReduction
            },

            {
                EffectsEnum.Effect_AddPushDamageReduction,
                PlayerFields.PushDamageReduction
            },

            {
                EffectsEnum.Effect_AddCriticalDamageReduction,
                PlayerFields.CriticalDamageReduction
            },

            {
                EffectsEnum.Effect_AddPvpNeutralResistPercent,
                PlayerFields.PvpNeutralResistPercent
            },

            {
                EffectsEnum.Effect_AddPvpEarthResistPercent,
                PlayerFields.PvpEarthResistPercent
            },

            {
                EffectsEnum.Effect_AddPvpWaterResistPercent,
                PlayerFields.PvpWaterResistPercent
            },

            {
                EffectsEnum.Effect_AddPvpAirResistPercent,
                PlayerFields.PvpAirResistPercent
            },

            {
                EffectsEnum.Effect_AddPvpFireResistPercent,
                PlayerFields.PvpFireResistPercent
            },

            {
                EffectsEnum.Effect_AddPvpNeutralElementReduction,
                PlayerFields.PvpNeutralElementReduction
            },

            {
                EffectsEnum.Effect_AddPvpEarthElementReduction,
                PlayerFields.PvpEarthElementReduction
            },

            {
                EffectsEnum.Effect_AddPvpWaterElementReduction,
                PlayerFields.PvpWaterElementReduction
            },

            {
                EffectsEnum.Effect_AddPvpAirElementReduction,
                PlayerFields.PvpAirElementReduction
            },

            {
                EffectsEnum.Effect_AddPvpFireElementReduction,
                PlayerFields.PvpFireElementReduction
            },

            {
                EffectsEnum.Effect_AddGlobalDamageReduction,
                PlayerFields.GlobalDamageReduction
            },

            {
                EffectsEnum.Effect_AddDamageMultiplicator,
                PlayerFields.DamageMultiplicator
            },

            {
                EffectsEnum.Effect_AddPhysicalDamage_137,
                PlayerFields.PhysicalDamage
            },

            {
                EffectsEnum.Effect_AddPhysicalDamage_142,
                PlayerFields.PhysicalDamage
            },

            {
                EffectsEnum.Effect_AddPhysicalDamageReduction,
                PlayerFields.PhysicalDamageReduction
            },

            {
                EffectsEnum.Effect_AddMagicDamageReduction,
                PlayerFields.MagicDamageReduction
            },
            {
                EffectsEnum.Effect_AddPushDamageBonus,
                PlayerFields.PushDamageBonus
            },
            {
                EffectsEnum.Effect_412,
                PlayerFields.MPAttack
            }
        };
        public static readonly System.Collections.Generic.Dictionary<EffectsEnum, PlayerFields> m_subEffectsBinds = new System.Collections.Generic.Dictionary<EffectsEnum, PlayerFields>
        {
            {
                EffectsEnum.Effect_SubInitiative,
                PlayerFields.Initiative
            },

            {
                EffectsEnum.Effect_SubProspecting,
                PlayerFields.Prospecting
            },

            {
                EffectsEnum.Effect_SubAP,
                PlayerFields.AP
            },

            {
                EffectsEnum.Effect_SubMP,
                PlayerFields.MP
            },

            {
                EffectsEnum.Effect_SubStrength,
                PlayerFields.Strength
            },

            {
                EffectsEnum.Effect_SubVitality,
                PlayerFields.Vitality
            },

            {
                EffectsEnum.Effect_SubWisdom,
                PlayerFields.Wisdom
            },

            {
                EffectsEnum.Effect_SubChance,
                PlayerFields.Chance
            },

            {
                EffectsEnum.Effect_SubAgility,
                PlayerFields.Agility
            },

            {
                EffectsEnum.Effect_SubIntelligence,
                PlayerFields.Intelligence
            },

            {
                EffectsEnum.Effect_SubRange,
                PlayerFields.Range
            },

            {
                EffectsEnum.Effect_SubCriticalHit,
                PlayerFields.CriticalHit
            },

            {
                EffectsEnum.Effect_SubHealBonus,
                PlayerFields.HealBonus
            },

            {
                EffectsEnum.Effect_SubDamageBonus,
                PlayerFields.DamageBonus
            },

            {
                EffectsEnum.Effect_SubDamageBonusPercent,
                PlayerFields.DamageBonusPercent
            },

            {
                EffectsEnum.Effect_SubPushDamageBonus,
                PlayerFields.PushDamageBonus
            },

            {
                EffectsEnum.Effect_SubCriticalDamageBonus,
                PlayerFields.CriticalDamageBonus
            },

            {
                EffectsEnum.Effect_SubNeutralDamageBonus,
                PlayerFields.NeutralDamageBonus
            },

            {
                EffectsEnum.Effect_SubEarthDamageBonus,
                PlayerFields.EarthDamageBonus
            },

            {
                EffectsEnum.Effect_SubWaterDamageBonus,
                PlayerFields.WaterDamageBonus
            },

            {
                EffectsEnum.Effect_SubAirDamageBonus,
                PlayerFields.AirDamageBonus
            },

            {
                EffectsEnum.Effect_SubFireDamageBonus,
                PlayerFields.FireDamageBonus
            },

            {
                EffectsEnum.Effect_SubDodgeAPProbability,
                PlayerFields.DodgeAPProbability
            },

            {
                EffectsEnum.Effect_SubDodgeMPProbability,
                PlayerFields.DodgeMPProbability
            },

            {
                EffectsEnum.Effect_SubNeutralResistPercent,
                PlayerFields.NeutralResistPercent
            },

            {
                EffectsEnum.Effect_SubEarthResistPercent,
                PlayerFields.EarthResistPercent
            },

            {
                EffectsEnum.Effect_SubWaterResistPercent,
                PlayerFields.WaterResistPercent
            },

            {
                EffectsEnum.Effect_SubAirResistPercent,
                PlayerFields.AirResistPercent
            },

            {
                EffectsEnum.Effect_SubFireResistPercent,
                PlayerFields.FireResistPercent
            },

            {
                EffectsEnum.Effect_SubNeutralElementReduction,
                PlayerFields.NeutralElementReduction
            },

            {
                EffectsEnum.Effect_SubEarthElementReduction,
                PlayerFields.EarthElementReduction
            },

            {
                EffectsEnum.Effect_SubWaterElementReduction,
                PlayerFields.WaterElementReduction
            },

            {
                EffectsEnum.Effect_SubAirElementReduction,
                PlayerFields.AirElementReduction
            },

            {
                EffectsEnum.Effect_SubFireElementReduction,
                PlayerFields.FireElementReduction
            },

            {
                EffectsEnum.Effect_SubPushDamageReduction,
                PlayerFields.PushDamageReduction
            },

            {
                EffectsEnum.Effect_SubCriticalDamageReduction,
                PlayerFields.CriticalDamageReduction
            },

            {
                EffectsEnum.Effect_SubPvpNeutralResistPercent,
                PlayerFields.PvpNeutralResistPercent
            },

            {
                EffectsEnum.Effect_SubPvpEarthResistPercent,
                PlayerFields.PvpEarthResistPercent
            },

            {
                EffectsEnum.Effect_SubPvpWaterResistPercent,
                PlayerFields.PvpWaterResistPercent
            },

            {
                EffectsEnum.Effect_SubPvpAirResistPercent,
                PlayerFields.PvpAirResistPercent
            },

            {
                EffectsEnum.Effect_SubPvpFireResistPercent,
                PlayerFields.PvpFireResistPercent
            },

            {
                EffectsEnum.Effect_SubPhysicalDamageReduction,
                PlayerFields.PhysicalDamageReduction
            },

            {
                EffectsEnum.Effect_SubMagicDamageReduction,
                PlayerFields.MagicDamageReduction
            },
            {
                EffectsEnum.Effect_IncreaseAPAvoid,
                PlayerFields.DodgeAPProbability
            },
             {
                EffectsEnum.Effect_IncreaseMPAvoid,
                PlayerFields.DodgeMPProbability
            }
        };
        //Effect_IncreaseAPAvoid,
        //Effect_IncreaseMPAvoid,
        public DefaultItemEffect(EffectBase effect, Character target, PlayerItem item)
            : base(effect, target, item)
        {
        }
        public DefaultItemEffect(EffectBase effect, Character target, bool apply)
            : base(effect, target, apply)
        {
        }
        public override bool Apply(object token, bool declanched = false)
        {
            bool result;
            if (!(this.Effect is EffectInteger))
            {
                result = false;
            }
            else
            {
                var EffectInteger = Effect as EffectInteger;
                DefaultItemEffect.EffectComputeHandler effectComputeHandler = null;
                if (DefaultItemEffect.m_addEffectsBinds.ContainsKey(this.Effect.EffectId))
                {
                    PlayerFields key = DefaultItemEffect.m_addEffectsBinds[this.Effect.EffectId];
                    if (!DefaultItemEffect.m_addMethods.ContainsKey(key) || !DefaultItemEffect.m_subMethods.ContainsKey(key))
                    {
                        result = false;
                    }
                    else
                        effectComputeHandler = ((base.Operation == ItemEffectHandler.HandlerOperation.APPLY) ? DefaultItemEffect.m_addMethods[key] : DefaultItemEffect.m_subMethods[key]);
                }
                else
                {
                    if (!DefaultItemEffect.m_subEffectsBinds.ContainsKey(this.Effect.EffectId))
                    {
                        result = false;
                    }
                    else
                    {
                        PlayerFields key = DefaultItemEffect.m_subEffectsBinds[this.Effect.EffectId];
                        if (!DefaultItemEffect.m_addMethods.ContainsKey(key) || !DefaultItemEffect.m_subMethods.ContainsKey(key))
                        {
                            result = false;
                        }
                        else
                        {
                            effectComputeHandler = ((base.Operation == ItemEffectHandler.HandlerOperation.APPLY) ? DefaultItemEffect.m_subMethods[key] : DefaultItemEffect.m_addMethods[key]);
                        }
                    }
                }
                if (Effect.EffectId == EffectsEnum.Effect_LearnAttitude)
                {
                    //if (Operation == HandlerOperation.APPLY)
                    //{
                    //    Target.AddAttitude((uint)EffectInteger.Value);
                    //}
                    //else
                    //{
                    //    Target.RemoveAttitude((uint)EffectInteger.Value);
                    //}
                }
                if (effectComputeHandler != null)
                {
                    effectComputeHandler(base.Target, this.Effect as EffectInteger, base.Boost);
                }
                result = true;
            }
            return result;
        }
        private static void AddHealth(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.Health].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.Health].Equiped += (int)effect.Value;
            }
        }
        private static void AddInitiative(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.Initiative].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.Initiative].Equiped += (int)effect.Value;
            }
        }
        private static void AddProspecting(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.Prospecting].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.Prospecting].Equiped += (int)effect.Value;
            }
        }
        private static void AddAP(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.AP].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.AP].Equiped += (int)effect.Value;
            }
        }
        private static void AddMP(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.MP].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.MP].Equiped += (int)effect.Value;
            }
        }
        private static void AddStrength(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.Strength].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.Strength].Equiped += (int)effect.Value;
            }
        }
        private static void AddVitality(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.Vitality].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.Vitality].Equiped += (int)effect.Value;
            }
        }
        private static void AddWisdom(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.Wisdom].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.Wisdom].Equiped += (int)effect.Value;
            }
        }
        private static void AddChance(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.Chance].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.Chance].Equiped += (int)effect.Value;
            }
        }
        private static void AddAgility(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.Agility].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.Agility].Equiped += (int)effect.Value;
            }
        }
        private static void AddIntelligence(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.Intelligence].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.Intelligence].Equiped += (int)effect.Value;
            }
        }
        private static void AddRange(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.Range].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.Range].Equiped += (int)effect.Value;
            }
        }
        private static void AddSummonLimit(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.SummonLimit].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.SummonLimit].Equiped += (int)effect.Value;
            }
        }
        private static void AddDamageReflection(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.DamageReflection].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.DamageReflection].Equiped += (int)effect.Value;
            }
        }
        private static void AddCriticalHit(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.CriticalHit].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.CriticalHit].Equiped += (int)effect.Value;
            }
        }
        private static void AddCriticalMiss(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.CriticalMiss].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.CriticalMiss].Equiped += (int)effect.Value;
            }
        }
        private static void AddHealBonus(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.HealBonus].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.HealBonus].Equiped += (int)effect.Value;
            }
        }
        private static void AddDamageBonus(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.DamageBonus].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.DamageBonus].Equiped += (int)effect.Value;
            }
        }
        private static void AddWeaponDamageBonus(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.WeaponDamageBonus].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.WeaponDamageBonus].Equiped += (int)effect.Value;
            }
        }
        private static void AddDamageBonusPercent(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.DamageBonusPercent].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.DamageBonusPercent].Equiped += (int)effect.Value;
            }
        }
        private static void AddTrapBonus(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.TrapBonus].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.TrapBonus].Equiped += (int)effect.Value;
            }
        }
        private static void AddTrapBonusPercent(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.TrapBonusPercent].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.TrapBonusPercent].Equiped += (int)effect.Value;
            }
        }
        private static void AddPermanentDamagePercent(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.PermanentDamagePercent].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.PermanentDamagePercent].Equiped += (int)effect.Value;
            }
        }
        private static void AddTackleBlock(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.TackleBlock].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.TackleBlock].Equiped += (int)effect.Value;
            }
        }
        private static void AddTackleEvade(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.TackleEvade].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.TackleEvade].Equiped += (int)effect.Value;
            }
        }
        private static void AddAPAttack(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.APAttack].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.APAttack].Equiped += (int)effect.Value;
            }
        }
        private static void AddMPAttack(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.MPAttack].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.MPAttack].Equiped += (int)effect.Value;
            }
        }
        private static void AddPushDamageBonus(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.PushDamageBonus].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.PushDamageBonus].Equiped += (int)effect.Value;
            }
        }
        private static void AddCriticalDamageBonus(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.CriticalDamageBonus].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.CriticalDamageBonus].Equiped += (int)effect.Value;
            }
        }
        private static void AddNeutralDamageBonus(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.NeutralDamageBonus].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.NeutralDamageBonus].Equiped += (int)effect.Value;
            }
        }
        private static void AddEarthDamageBonus(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.EarthDamageBonus].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.EarthDamageBonus].Equiped += (int)effect.Value;
            }
        }
        private static void AddWaterDamageBonus(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.WaterDamageBonus].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.WaterDamageBonus].Equiped += (int)effect.Value;
            }
        }
        private static void AddAirDamageBonus(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.AirDamageBonus].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.AirDamageBonus].Equiped += (int)effect.Value;
            }
        }
        private static void AddFireDamageBonus(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.FireDamageBonus].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.FireDamageBonus].Equiped += (int)effect.Value;
            }
        }
        private static void AddDodgeAPProbability(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.DodgeAPProbability].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.DodgeAPProbability].Equiped += (int)effect.Value;
            }
        }
        private static void AddDodgeMPProbability(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.DodgeMPProbability].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.DodgeMPProbability].Equiped += (int)effect.Value;
            }
        }
        private static void AddNeutralResistPercent(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.NeutralResistPercent].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.NeutralResistPercent].Equiped += (int)effect.Value;
            }
        }
        private static void AddEarthResistPercent(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.EarthResistPercent].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.EarthResistPercent].Equiped += (int)effect.Value;
            }
        }
        private static void AddWaterResistPercent(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.WaterResistPercent].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.WaterResistPercent].Equiped += (int)effect.Value;
            }
        }
        private static void AddAirResistPercent(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.AirResistPercent].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.AirResistPercent].Equiped += (int)effect.Value;
            }
        }
        private static void AddFireResistPercent(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.FireResistPercent].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.FireResistPercent].Equiped += (int)effect.Value;
            }
        }
        private static void AddNeutralElementReduction(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.NeutralElementReduction].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.NeutralElementReduction].Equiped += (int)effect.Value;
            }
        }
        private static void AddEarthElementReduction(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.EarthElementReduction].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.EarthElementReduction].Equiped += (int)effect.Value;
            }
        }
        private static void AddWaterElementReduction(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.WaterElementReduction].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.WaterElementReduction].Equiped += (int)effect.Value;
            }
        }
        private static void AddAirElementReduction(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.AirElementReduction].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.AirElementReduction].Equiped += (int)effect.Value;
            }
        }
        private static void AddFireElementReduction(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.FireElementReduction].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.FireElementReduction].Equiped += (int)effect.Value;
            }
        }
        private static void AddPushDamageReduction(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.PushDamageReduction].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.PushDamageReduction].Equiped += (int)effect.Value;
            }
        }
        private static void AddCriticalDamageReduction(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.CriticalDamageReduction].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.CriticalDamageReduction].Equiped += (int)effect.Value;
            }
        }
        private static void AddPvpNeutralResistPercent(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.PvpNeutralResistPercent].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.PvpNeutralResistPercent].Equiped += (int)effect.Value;
            }
        }
        private static void AddPvpEarthResistPercent(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.PvpEarthResistPercent].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.PvpEarthResistPercent].Equiped += (int)effect.Value;
            }
        }
        private static void AddPvpWaterResistPercent(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.PvpWaterResistPercent].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.PvpWaterResistPercent].Equiped += (int)effect.Value;
            }
        }
        private static void AddPvpAirResistPercent(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.PvpAirResistPercent].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.PvpAirResistPercent].Equiped += (int)effect.Value;
            }
        }
        private static void AddPvpFireResistPercent(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.PvpFireResistPercent].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.PvpFireResistPercent].Equiped += (int)effect.Value;
            }
        }
        private static void AddPvpNeutralElementReduction(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.PvpNeutralElementReduction].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.PvpNeutralElementReduction].Equiped += (int)effect.Value;
            }
        }
        private static void AddPvpEarthElementReduction(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.PvpEarthElementReduction].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.PvpEarthElementReduction].Equiped += (int)effect.Value;
            }
        }
        private static void AddPvpWaterElementReduction(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.PvpWaterElementReduction].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.PvpWaterElementReduction].Equiped += (int)effect.Value;
            }
        }
        private static void AddPvpAirElementReduction(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.PvpAirElementReduction].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.PvpAirElementReduction].Equiped += (int)effect.Value;
            }
        }
        private static void AddPvpFireElementReduction(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.PvpFireElementReduction].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.PvpFireElementReduction].Equiped += (int)effect.Value;
            }
        }
        private static void AddGlobalDamageReduction(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.GlobalDamageReduction].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.GlobalDamageReduction].Equiped += (int)effect.Value;
            }
        }
        private static void AddDamageMultiplicator(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.DamageMultiplicator].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.DamageMultiplicator].Equiped += (int)effect.Value;
            }
        }
        private static void AddPhysicalDamage(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.PhysicalDamage].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.PhysicalDamage].Equiped += (int)effect.Value;
            }
        }
        private static void AddMagicDamage(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.MagicDamage].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.MagicDamage].Equiped += (int)effect.Value;
            }
        }
        private static void AddPhysicalDamageReduction(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.PhysicalDamageReduction].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.PhysicalDamageReduction].Equiped += (int)effect.Value;
            }
        }
        private static void AddMagicDamageReduction(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.MagicDamageReduction].Given += (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.MagicDamageReduction].Equiped += (int)effect.Value;
            }
        }
        private static void SubHealth(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.Health].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.Health].Equiped -= (int)effect.Value;
            }
        }
        private static void SubInitiative(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.Initiative].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.Initiative].Equiped -= (int)effect.Value;
            }
        }
        private static void SubProspecting(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.Prospecting].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.Prospecting].Equiped -= (int)effect.Value;
            }
        }
        private static void SubAP(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.AP].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.AP].Equiped -= (int)effect.Value;
            }
        }
        private static void SubMP(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.MP].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.MP].Equiped -= (int)effect.Value;
            }
        }
        private static void SubStrength(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.Strength].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.Strength].Equiped -= (int)effect.Value;
            }
        }
        private static void SubVitality(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.Vitality].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.Vitality].Equiped -= (int)effect.Value;
            }
        }
        private static void SubWisdom(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.Wisdom].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.Wisdom].Equiped -= (int)effect.Value;
            }
        }
        private static void SubChance(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.Chance].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.Chance].Equiped -= (int)effect.Value;
            }
        }
        private static void SubAgility(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.Agility].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.Agility].Equiped -= (int)effect.Value;
            }
        }
        private static void SubIntelligence(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.Intelligence].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.Intelligence].Equiped -= (int)effect.Value;
            }
        }
        private static void SubRange(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.Range].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.Range].Equiped -= (int)effect.Value;
            }
        }
        private static void SubSummonLimit(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.SummonLimit].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.SummonLimit].Equiped -= (int)effect.Value;
            }
        }
        private static void SubDamageReflection(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.DamageReflection].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.DamageReflection].Equiped -= (int)effect.Value;
            }
        }
        private static void SubCriticalHit(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.CriticalHit].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.CriticalHit].Equiped -= (int)effect.Value;
            }
        }
        private static void SubCriticalMiss(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.CriticalMiss].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.CriticalMiss].Equiped -= (int)effect.Value;
            }
        }
        private static void SubHealBonus(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.HealBonus].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.HealBonus].Equiped -= (int)effect.Value;
            }
        }
        private static void SubDamageBonus(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.DamageBonus].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.DamageBonus].Equiped -= (int)effect.Value;
            }
        }
        private static void SubWeaponDamageBonus(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.WeaponDamageBonus].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.WeaponDamageBonus].Equiped -= (int)effect.Value;
            }
        }
        private static void SubDamageBonusPercent(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.DamageBonusPercent].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.DamageBonusPercent].Equiped -= (int)effect.Value;
            }
        }
        private static void SubTrapBonus(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.TrapBonus].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.TrapBonus].Equiped -= (int)effect.Value;
            }
        }
        private static void SubTrapBonusPercent(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.TrapBonusPercent].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.TrapBonusPercent].Equiped -= (int)effect.Value;
            }
        }
        private static void SubPermanentDamagePercent(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.PermanentDamagePercent].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.PermanentDamagePercent].Equiped -= (int)effect.Value;
            }
        }
        private static void SubTackleBlock(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.TackleBlock].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.TackleBlock].Equiped -= (int)effect.Value;
            }
        }
        private static void SubTackleEvade(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.TackleEvade].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.TackleEvade].Equiped -= (int)effect.Value;
            }
        }
        private static void SubAPAttack(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.APAttack].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.APAttack].Equiped -= (int)effect.Value;
            }
        }
        private static void SubMPAttack(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.MPAttack].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.MPAttack].Equiped -= (int)effect.Value;
            }
        }
        private static void SubPushDamageBonus(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.PushDamageBonus].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.PushDamageBonus].Equiped -= (int)effect.Value;
            }
        }
        private static void SubCriticalDamageBonus(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.CriticalDamageBonus].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.CriticalDamageBonus].Equiped -= (int)effect.Value;
            }
        }
        private static void SubNeutralDamageBonus(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.NeutralDamageBonus].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.NeutralDamageBonus].Equiped -= (int)effect.Value;
            }
        }
        private static void SubEarthDamageBonus(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.EarthDamageBonus].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.EarthDamageBonus].Equiped -= (int)effect.Value;
            }
        }
        private static void SubWaterDamageBonus(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.WaterDamageBonus].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.WaterDamageBonus].Equiped -= (int)effect.Value;
            }
        }
        private static void SubAirDamageBonus(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.AirDamageBonus].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.AirDamageBonus].Equiped -= (int)effect.Value;
            }
        }
        private static void SubFireDamageBonus(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.FireDamageBonus].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.FireDamageBonus].Equiped -= (int)effect.Value;
            }
        }
        private static void SubDodgeAPProbability(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.DodgeAPProbability].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.DodgeAPProbability].Equiped -= (int)effect.Value;
            }
        }
        private static void SubDodgeMPProbability(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.DodgeMPProbability].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.DodgeMPProbability].Equiped -= (int)effect.Value;
            }
        }
        private static void SubNeutralResistPercent(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.NeutralResistPercent].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.NeutralResistPercent].Equiped -= (int)effect.Value;
            }
        }
        private static void SubEarthResistPercent(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.EarthResistPercent].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.EarthResistPercent].Equiped -= (int)effect.Value;
            }
        }
        private static void SubWaterResistPercent(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.WaterResistPercent].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.WaterResistPercent].Equiped -= (int)effect.Value;
            }
        }
        private static void SubAirResistPercent(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.AirResistPercent].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.AirResistPercent].Equiped -= (int)effect.Value;
            }
        }
        private static void SubFireResistPercent(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.FireResistPercent].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.FireResistPercent].Equiped -= (int)effect.Value;
            }
        }
        private static void SubNeutralElementReduction(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.NeutralElementReduction].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.NeutralElementReduction].Equiped -= (int)effect.Value;
            }
        }
        private static void SubEarthElementReduction(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.EarthElementReduction].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.EarthElementReduction].Equiped -= (int)effect.Value;
            }
        }
        private static void SubWaterElementReduction(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.WaterElementReduction].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.WaterElementReduction].Equiped -= (int)effect.Value;
            }
        }
        private static void SubAirElementReduction(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.AirElementReduction].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.AirElementReduction].Equiped -= (int)effect.Value;
            }
        }
        private static void SubFireElementReduction(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.FireElementReduction].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.FireElementReduction].Equiped -= (int)effect.Value;
            }
        }
        private static void SubPushDamageReduction(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.PushDamageReduction].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.PushDamageReduction].Equiped -= (int)effect.Value;
            }
        }
        private static void SubCriticalDamageReduction(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.CriticalDamageReduction].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.CriticalDamageReduction].Equiped -= (int)effect.Value;
            }
        }
        private static void SubPvpNeutralResistPercent(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.PvpNeutralResistPercent].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.PvpNeutralResistPercent].Equiped -= (int)effect.Value;
            }
        }
        private static void SubPvpEarthResistPercent(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.PvpEarthResistPercent].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.PvpEarthResistPercent].Equiped -= (int)effect.Value;
            }
        }
        private static void SubPvpWaterResistPercent(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.PvpWaterResistPercent].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.PvpWaterResistPercent].Equiped -= (int)effect.Value;
            }
        }
        private static void SubPvpAirResistPercent(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.PvpAirResistPercent].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.PvpAirResistPercent].Equiped -= (int)effect.Value;
            }
        }
        private static void SubPvpFireResistPercent(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.PvpFireResistPercent].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.PvpFireResistPercent].Equiped -= (int)effect.Value;
            }
        }
        private static void SubPvpNeutralElementReduction(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.PvpNeutralElementReduction].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.PvpNeutralElementReduction].Equiped -= (int)effect.Value;
            }
        }
        private static void SubPvpEarthElementReduction(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.PvpEarthElementReduction].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.PvpEarthElementReduction].Equiped -= (int)effect.Value;
            }
        }
        private static void SubPvpWaterElementReduction(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.PvpWaterElementReduction].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.PvpWaterElementReduction].Equiped -= (int)effect.Value;
            }
        }
        private static void SubPvpAirElementReduction(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.PvpAirElementReduction].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.PvpAirElementReduction].Equiped -= (int)effect.Value;
            }
        }
        private static void SubPvpFireElementReduction(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.PvpFireElementReduction].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.PvpFireElementReduction].Equiped -= (int)effect.Value;
            }
        }
        private static void SubGlobalDamageReduction(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.GlobalDamageReduction].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.GlobalDamageReduction].Equiped -= (int)effect.Value;
            }
        }
        private static void SubDamageMultiplicator(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.DamageMultiplicator].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.DamageMultiplicator].Equiped -= (int)effect.Value;
            }
        }
        private static void SubPhysicalDamage(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.PhysicalDamage].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.PhysicalDamage].Equiped -= (int)effect.Value;
            }
        }
        private static void SubMagicDamage(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.MagicDamage].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.MagicDamage].Equiped -= (int)effect.Value;
            }
        }
        private static void SubPhysicalDamageReduction(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.PhysicalDamageReduction].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.PhysicalDamageReduction].Equiped -= (int)effect.Value;
            }
        }
        private static void SubMagicDamageReduction(Character target, EffectInteger effect, bool isBoost)
        {
            if (isBoost)
            {
                target.Stats[PlayerFields.MagicDamageReduction].Given -= (int)effect.Value;
            }
            else
            {
                target.Stats[PlayerFields.MagicDamageReduction].Equiped -= (int)effect.Value;
            }
        }
    }
}
