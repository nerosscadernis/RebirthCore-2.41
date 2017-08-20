using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Effects.Handlers;
using Rebirth.World.Effects.Handlers.Spells;
using Rebirth.World.Game.Effect.Instances;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Game.Fights.Buffs;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Game.Spells;
using System;
using System.Collections.Generic;
using Rebirth.World.Datas.Spells;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Effect.Spells.Steal
{
    [EffectHandler(EffectsEnum.Effect_StealHPAir), EffectHandler(EffectsEnum.Effect_StealHPNeutral), EffectHandler(EffectsEnum.Effect_StealHPWater), EffectHandler(EffectsEnum.Effect_StealHPFire), EffectHandler(EffectsEnum.Effect_StealHPEarth)]
    public class HPSteal : SpellEffectHandler
    {
        public HPSteal(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }
        public override bool Apply(object token, bool declanched = false)
        {
            foreach (Fighter current in base.GetAffectedActors())
            {
                if (this.Effect.Duration > 0 && declanched == false)
                {
                    if (Spell.Spell.id == 62)
                    {
                        int id = current.PopNextBuffId();
                        var args = new short[] { 1, (short)(Dice.DiceFace - (Dice.DiceNum - 1)), (short)(Dice.DiceNum - 1) };
                        var trigger = new TriggerBuff(id, current, base.Caster, Dice, base.Spell, null, Dice.DiceNum, false, true, TriggerBuffType.BEFORE_ATTACKED, args, declanched);
                        trigger.Applyed += Chakra;
                        base.AddTriggerBuff(current, trigger);
                    }
                    //base.AddTriggerBuff(current, true, TriggerBuffType.TURN_BEGIN);
                }             
                else
                {
                    int num = current.InflictDamage(new Fights.Other.Damage(Dice, GetEffectSchool(this.Effect.EffectId), base.Caster, current, base.Spell, EffectGenerationType.Normal, (int)current.Point.Point.DistanceTo(TargetedPoint), Boost)
                    {
                        critical = base.Critical
                    });

                    if ((num > 0 ? num / 2 : 0) > 0)
                    {
                        base.Caster.HealDirect((int)((short)((double)num / 2.0)), current);

                    }
                }
            }
            return true;
        }

        private static void StealHpBuffTrigger(TriggerBuff buff, object token)
        {
            EffectInteger effectInteger = buff.GenerateEffect();
            if (!(effectInteger == null))
            {
                buff.Target.Heal(effectInteger.Value, buff.Caster, 0, 1, true);
            }
        }

        private void Chakra(TriggerBuff buff, object token)
        {
            var damage = token as Fights.Other.Damage;
            if (damage != null && damage.Spell != null && damage.Spell.IsTrapSpell)
            {
                buff.Target.InflictDamage(new Fights.Other.Damage((buff.Effect as EffectDice), GetEffectSchool(buff.Effect.EffectId), buff.Caster, buff.Target, buff.Spell, EffectGenerationType.Normal, 0, Boost)
                {
                    critical = base.Critical
                }, true);
            }
        }

        private static EffectSchoolEnum GetEffectSchool(EffectsEnum effect)
        {
            EffectSchoolEnum result;
            switch (effect)
            {
                case EffectsEnum.Effect_StealHPWater:
                    result = EffectSchoolEnum.Water;
                    break;
                case EffectsEnum.Effect_StealHPEarth:
                    result = EffectSchoolEnum.Earth;
                    break;
                case EffectsEnum.Effect_StealHPAir:
                    result = EffectSchoolEnum.Air;
                    break;
                case EffectsEnum.Effect_StealHPFire:
                    result = EffectSchoolEnum.Fire;
                    break;
                case EffectsEnum.Effect_StealHPNeutral:
                    result = EffectSchoolEnum.Neutral;
                    break;
                default:
                    throw new System.Exception(string.Format("Effect {0} has not associated School Type", effect));
            }
            return result;
        }
    }
}
