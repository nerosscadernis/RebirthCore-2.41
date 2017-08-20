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

namespace Rebirth.World.Game.Effect.Spells.Heal
{
    [EffectHandler(EffectsEnum.Effect_RestoreHPPercent)]
    public class RestoreHpPercent : SpellEffectHandler
    {
        public RestoreHpPercent(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
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
                    TriggerBuff buff = new TriggerBuff(current.PopNextBuffId(), current, Caster, Effect, Spell, null, (short)effectInteger.Value, Critical, true, TriggerBuffType.TURN_BEGIN, declanched);
                    buff.Applyed += OnBuffTriggered;
                    base.AddTriggerBuff(current, buff);
                }
                else
                {
                    this.HealHpPercent(current, (int)effectInteger.Value);
                }
            }
            result = true;
            return result;
        }
        private void OnBuffTriggered(TriggerBuff buff, object token)
        {
            EffectInteger effectInteger = base.GenerateEffect();
            if (!(effectInteger == null))
            {
                this.HealHpPercent(buff.Target, (int)effectInteger.Value);
            }
        }
        private void HealHpPercent(Fighter actor, int percent)
        {
            int healPoints = (int)((double)actor.Stats.Health.TotalMax * ((double)percent / 100.0));
            actor.Heal(healPoints, base.Caster, 0, 1, false);
        }
    }
}
