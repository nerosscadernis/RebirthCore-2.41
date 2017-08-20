using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Effects.Handlers;
using Rebirth.World.Effects.Handlers.Spells;
using Rebirth.World.Game.Effect.Instances;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Game.Fights.Buffs;
using Rebirth.World.Game.Fights.Other;
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
    [EffectHandler(EffectsEnum.Effect_HealByAttack)]
    public class HealAttackPourcent : SpellEffectHandler
    {
        public HealAttackPourcent(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }
        public override bool Apply(object token, bool declanched = false)
        {
            bool result;
            foreach (Fighter current in base.GetAffectedActors())
            {
                var trigger = new TriggerBuff(current.PopNextBuffId(), current, base.Caster, (Effect as EffectDice), base.Spell, null, Dice.DiceNum, false, false, TriggerBuffType.AFTER_ATTACKED, declanched);
                trigger.Applyed += HealBuffTrigger;
                current.AddAndApplyBuff(trigger, true);
            }
            result = true;
            return result;
        }
        private static void HealBuffTrigger(TriggerBuff buff, object token)
        {
            if (token is Damage)
            {
                Damage damage = token as Damage;
                if (damage.Source.Team == buff.Caster.Team)
                {
                    damage.Source.Heal(damage.AfterReduction * buff.Value / 100, buff.Caster, 0, 1, false);
                }
            }
        }
    }
}
