using Rebirth.Common.Protocol.Enums;
using Rebirth.Common.Thread;
using Rebirth.World.Effects.Handlers;
using Rebirth.World.Effects.Handlers.Spells;
using Rebirth.World.Game.Effect.Instances;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Game.Fights.Buffs;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Game.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using Rebirth.World.Datas.Spells;
using System.Text;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Effect.Spells.Damage
{
    [EffectHandler(EffectsEnum.Effect_DamageNeutral), EffectHandler(EffectsEnum.Effect_DamageAir), EffectHandler(EffectsEnum.Effect_DamageWater), EffectHandler(EffectsEnum.Effect_DamageFire), EffectHandler(EffectsEnum.Effect_DamageEarth)]
    public class DirectDamage : SpellEffectHandler
    {
        public DirectDamage(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }
        public override bool Apply(object token, bool declanched = false)
        {
            foreach (Fighter current in base.GetAffectedActors())
            {
                if (this.Effect.Duration > 0 && !(token is DelayedBuff))
                {
                    int id = current.PopNextBuffId();

                    var args = new short[] { 1, (short)(Dice.DiceFace - (Dice.DiceNum - 1)), (short)(Dice.DiceNum - 1) };

                    var trigger = new TriggerBuff(id, current, base.Caster, Dice, base.Spell, null, Dice.DiceNum, Critical, true, TriggerBuffType.TURN_BEGIN, args, declanched);
                    trigger.Applyed += ApplyDamage;
                    base.AddTriggerBuff(current, trigger);
                }
                else
                {
                    //SpellReflectionBuff bestReflectionBuff = current.GetBestReflectionBuff();
                    //if (bestReflectionBuff != null && bestReflectionBuff.ReflectedLevel >= (int)base.Spell.CurrentLevel && base.Spell.Template.Id != 0)
                    //{
                    //    this.NotifySpellReflected(current);
                    //    base.Caster.InflictDamage(new Fights.Damage(base.Dice, DirectDamage.GetEffectSchool(base.Dice.EffectId), current, base.Spell)
                    //    {
                    //        ReflectedDamages = true,
                    //        MarkTrigger = base.MarkTrigger,
                    //        critical = base.Critical

                    //    });
                    //}
                    //else
                    //{
                        current.InflictDamage(new Fights.Other.Damage(base.Dice, GetEffectSchool(base.Dice.EffectId), base.Caster, current, base.Spell, EffectGenerationType.Normal, 
                           Fight.FighterTeleport.Contains(current) ? (int)current.Point.Point.DistanceTo(TargetedPoint) : 0, Boost)
                        {
                            //MarkTrigger = base.MarkTrigger,
                            critical = base.Critical

                        });
                    //}
                }
            }
            return true;
        }
        private void ApplyDamage(TriggerBuff buff, object token)
        {
            buff.Target.InflictDamage(new Fights.Other.Damage((buff.Effect as EffectDice), GetEffectSchool(buff.Effect.EffectId), buff.Caster, buff.Target, buff.Spell, EffectGenerationType.Normal, 0, Boost)
            {
                critical = base.Critical,
                IsVenom = true                
            }, true);
        }

        private EffectSchoolEnum GetEffectSchool(EffectsEnum effect)
        {
            EffectSchoolEnum result;
            switch (effect)
            {
                case EffectsEnum.Effect_DamageWater:
                    result = EffectSchoolEnum.Water;
                    break;
                case EffectsEnum.Effect_DamageEarth:
                    result = EffectSchoolEnum.Earth;
                    break;
                case EffectsEnum.Effect_DamageAir:
                    result = EffectSchoolEnum.Air;
                    break;
                case EffectsEnum.Effect_DamageFire:
                    result = EffectSchoolEnum.Fire;
                    break;
                case EffectsEnum.Effect_DamageNeutral:
                    result = EffectSchoolEnum.Neutral;
                    break;
                default:
                    throw new System.Exception(string.Format("Effect {0} has not associated School Type", effect));
            }
            return result;
        }
    }
}
