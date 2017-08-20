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
using System.Linq;
using System.Text;
using Rebirth.World.Datas.Spells;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Effect.Spells.Damage
{
    [EffectHandler(EffectsEnum.Effect_DamagePercentNeutral), EffectHandler(EffectsEnum.Effect_DamagePercentAir), EffectHandler(EffectsEnum.Effect_DamagePercentWater), EffectHandler(EffectsEnum.Effect_DamagePercentFire), EffectHandler(EffectsEnum.Effect_DamagePercentEarth)]
    public class DamagePercent : SpellEffectHandler
    {
        public DamagePercent(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }
        public override bool Apply(object token, bool declanched = false)
        {
            double amount = (double)Caster.Stats.Health.Total * (double)Dice.DiceNum / 100d;
            foreach (Fighter current in base.GetAffectedActors())
            {
                if (this.Effect.Duration > 0)
                {
                    int id = current.PopNextBuffId();
                    var trigger = new TriggerBuff(id, current, base.Caster, Dice, base.Spell, null, (short)amount, false, false, TriggerBuffType.TURN_BEGIN, declanched);
                    trigger.Applyed += ApplyDamage;
                    base.AddTriggerBuff(current, trigger);
                }
                else
                {
                    current.InflictDamage(new Fights.Other.Damage(0, (int)amount, Caster, current, GetEffectSchool(Effect.EffectId), EffectGenerationType.MinEffects, Spell, Boost)
                    {
                        critical = base.Critical
                    });
                }
            }
            return true;
        }
        private void ApplyDamage(TriggerBuff buff, object token)
        {
            buff.Target.InflictDamage(new Fights.Other.Damage(0, buff.Value, Caster, buff.Target, GetEffectSchool(Effect.EffectId), EffectGenerationType.MinEffects, Spell, Boost)
            {
                critical = base.Critical
            }, true);
        }

        private EffectSchoolEnum GetEffectSchool(EffectsEnum effect)
        {
            EffectSchoolEnum result;
            switch (effect)
            {
                case EffectsEnum.Effect_DamagePercentWater:
                    result = EffectSchoolEnum.Water;
                    break;
                case EffectsEnum.Effect_DamagePercentEarth:
                    result = EffectSchoolEnum.Earth;
                    break;
                case EffectsEnum.Effect_DamagePercentAir:
                    result = EffectSchoolEnum.Air;
                    break;
                case EffectsEnum.Effect_DamagePercentFire:
                    result = EffectSchoolEnum.Fire;
                    break;
                case EffectsEnum.Effect_DamagePercentNeutral:
                    result = EffectSchoolEnum.Neutral;
                    break;
                default:
                    throw new System.Exception(string.Format("Effect {0} has not associated School Type", effect));
            }
            return result;
        }
    }
}
