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
    [EffectHandler(EffectsEnum.Effect_Heal_Before_Attacked)]
    public class HealTakeDamage : SpellEffectHandler
    {
        public HealTakeDamage(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }
        public override bool Apply(object token, bool declanched = false)
        {
            foreach (Fighter current in base.GetAffectedActors())
            {
                var trigger = new TriggerBuff(current.PopNextBuffId(), current, base.Caster, Dice, base.Spell, null, (short)Dice.Value, false, false, TriggerBuffType.BEFORE_ATTACKED, declanched);
                trigger.Applyed += HealBuffTrigger;
                current.AddAndApplyBuff(trigger, true);
            }
            return true;
        }
        private static void HealBuffTrigger(TriggerBuff buff, object token)
        {
            if (token is Fights.Other.Damage)
            {
                var damage = token as Fights.Other.Damage;
                buff.Target.Heal(damage.AfterReduction, buff.Caster, 0, 1, false);
                damage.Amount = 0;
            }
        }
    }
}
