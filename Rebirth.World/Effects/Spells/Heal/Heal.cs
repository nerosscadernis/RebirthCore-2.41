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
using System.Threading.Tasks;
using Rebirth.World.Datas.Spells;

namespace Rebirth.World.Game.Heal
{
    [EffectHandler(EffectsEnum.Effect_HealHP_81), EffectHandler(EffectsEnum.Effect_HealHP_143), EffectHandler(EffectsEnum.Effect_HealHP_108)]
    public class Heal : SpellEffectHandler
    {
        public Heal(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
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
                if (this.Effect.Duration > 0)
                {
                    var trigger = new TriggerBuff(current.PopNextBuffId(), current, base.Caster, (Effect as EffectDice), base.Spell, null, (short)effectInteger.Value, false, false, TriggerBuffType.TURN_BEGIN, declanched);
                    trigger.Applyed += HealBuffTrigger;
                    current.AddAndApplyBuff(trigger, true);
                }
                else
                {
                    current.Heal((int)effectInteger.Value, base.Caster, TargetedPoint.DistanceTo(current.Point.Point), Boost, true);
                }
            }
            result = true;
            return result;
        }
        private static void HealBuffTrigger(TriggerBuff buff, object token)
        {
            EffectInteger effectInteger = buff.GenerateEffect();
            if (!(effectInteger == null))
            {
                buff.Target.Heal((int)effectInteger.Value, buff.Caster, 0, 1, true);
            }
        }
    }
}
