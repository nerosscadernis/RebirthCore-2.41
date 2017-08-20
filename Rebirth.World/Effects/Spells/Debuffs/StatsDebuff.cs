using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Effects.Handlers;
using Rebirth.World.Effects.Handlers.Spells;
using Rebirth.World.Game.Effect.Instances;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Game.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rebirth.World.Datas.Spells;

namespace Rebirth.World.Game.Effect.Spells.Debuffs
{
    [EffectHandler(EffectsEnum.Effect_SubLock), EffectHandler(EffectsEnum.Effect_SubDodgeAPProbability), EffectHandler(EffectsEnum.Effect_SubDamageBonusPercent), EffectHandler(EffectsEnum.Effect_SubDodge), EffectHandler(EffectsEnum.Effect_SubStrength), EffectHandler(EffectsEnum.Effect_SubChance), EffectHandler(EffectsEnum.Effect_SubDamageBonus), EffectHandler(EffectsEnum.Effect_SubRange_135), EffectHandler(EffectsEnum.Effect_SubDodgeMPProbability), EffectHandler(EffectsEnum.Effect_SubWisdom), EffectHandler(EffectsEnum.Effect_SubCriticalHit), EffectHandler(EffectsEnum.Effect_SubIntelligence), EffectHandler(EffectsEnum.Effect_SubAgility), EffectHandler(EffectsEnum.Effect_SubRange), EffectHandler(EffectsEnum.Effect_SubVitality), EffectHandler(EffectsEnum.Effect_SubPMAttack)]
    public class StatsDebuff : SpellEffectHandler
    {
        public StatsDebuff(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }
        public override bool Apply(object token, bool declanched = false)
        {
            bool result;
            foreach (Fighter current in base.GetAffectedActors())
            {
                EffectInteger effectInteger = base.GenerateEffect();
                if (effectInteger == null)
                {
                    result = false;
                    return result;
                }
                if (this.Effect.Duration > 0 || Effect.Duration < 0)
                {
                    PlayerFields field = StatsDebuff.GetEffectCaracteristic(this.Effect.EffectId);
                    base.AddStatBuff(current, -effectInteger.Value, field, true, declanched);
                }
            }
            result = true;
            return result;
        }
        public static PlayerFields GetEffectCaracteristic(EffectsEnum effect)
        {
            if (effect <= EffectsEnum.Effect_SubDamageBonus)
            {
                if (effect == EffectsEnum.Effect_SubRange || effect == EffectsEnum.Effect_SubRange_135)
                {
                    PlayerFields result = PlayerFields.Range;
                    return result;
                }
                if (effect == EffectsEnum.Effect_SubDamageBonus)
                {
                    PlayerFields result = PlayerFields.DamageBonus;
                    return result;
                }
            }
            else
            {
                if (effect <= EffectsEnum.Effect_SubCriticalHit)
                {
                    switch (effect)
                    {
                        case EffectsEnum.Effect_SubChance:
                            {
                                PlayerFields result = PlayerFields.Chance;
                                return result;
                            }
                        case EffectsEnum.Effect_SubVitality:
                            {
                                PlayerFields result = PlayerFields.Health;
                                return result;
                            }
                        case EffectsEnum.Effect_IncreaseWeight:
                        case EffectsEnum.Effect_DecreaseWeight:
                        case EffectsEnum.Effect_IncreaseAPAvoid:
                        case EffectsEnum.Effect_IncreaseMPAvoid:
                            break;
                        case EffectsEnum.Effect_SubAgility:
                            {
                                PlayerFields result = PlayerFields.Agility;
                                return result;
                            }
                        case EffectsEnum.Effect_SubIntelligence:
                            {
                                PlayerFields result = PlayerFields.Intelligence;
                                return result;
                            }
                        case EffectsEnum.Effect_SubWisdom:
                            {
                                PlayerFields result = PlayerFields.Wisdom;
                                return result;
                            }
                        case EffectsEnum.Effect_SubStrength:
                            {
                                PlayerFields result = PlayerFields.Strength;
                                return result;
                            }
                        case EffectsEnum.Effect_SubDodgeAPProbability:
                            {
                                PlayerFields result = PlayerFields.DodgeAPProbability;
                                return result;
                            }
                        case EffectsEnum.Effect_SubDodgeMPProbability:
                            {
                                PlayerFields result = PlayerFields.DodgeMPProbability;
                                return result;
                            }
                        default:
                            if (effect == EffectsEnum.Effect_SubCriticalHit)
                            {
                                PlayerFields result = PlayerFields.CriticalHit;
                                return result;
                            }
                            break;
                    }
                }
                else
                {
                    if (effect == EffectsEnum.Effect_SubDamageBonusPercent)
                    {
                        PlayerFields result = PlayerFields.DamageBonusPercent;
                        return result;
                    }
                    switch (effect)
                    {
                        case EffectsEnum.Effect_SubDodge:
                            {
                                PlayerFields result = PlayerFields.TackleEvade;
                                return result;
                            }
                        case EffectsEnum.Effect_SubLock:
                            {
                                PlayerFields result = PlayerFields.TackleBlock;
                                return result;
                            }
                        case EffectsEnum.Effect_SubPMAttack:
                            {
                                PlayerFields result = PlayerFields.MPAttack;
                                return result;
                            }
                    }
                }
            }
            throw new System.Exception(string.Format("'{0}' has no binded caracteristic", effect));
        }
    }
}
