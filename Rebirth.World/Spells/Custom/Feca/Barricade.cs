using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Effects.Handlers.Spells;
using Rebirth.World.Game.Effect.Instances;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Game.Fights.Buffs;
using Rebirth.World.Game.Fights.Other;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Game.Spells.Cast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rebirth.World.Datas.Spells;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Spells.Custom.Feca
{
    [SpellCastHandler(4)]
    public class Barricade : DefaultSpellCastHandler
    {
        public Barricade(Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(caster, spell, targetedCell, critical)
        {
        }

        public object TemporaryBoostStateEffect { get; private set; }

        public override void Execute(object token)
        {
            if (!this.m_initialized)
            {
                this.Initialize(true);
            }

            List<SpellEffectHandler> effects = base.Handlers.ToList();
            SpellEffectHandler tmp = effects[1];
            effects.RemoveAt(1);

            Handlers = effects.ToArray();

            base.Execute(token);

            Fighter target = Handlers[0].GetAffectedActors().FirstOrDefault();
            if (target != null)
            {
                int id = target.PopNextBuffId();
                var trigger = new TriggerBuff(id, target, base.Caster, tmp.Dice, base.Spell, null, tmp.Dice.DiceNum, false, false, TriggerBuffType.BEFORE_ATTACKED, false);
                trigger.Applyed += Apply;
                target.AddAndApplyBuff(trigger, true);
            }
        }

        private void Apply(TriggerBuff buff, object token)
        {
            if (token is Damage)
            {
                Damage damage = (token as Damage);
                if (damage.Source.Point.Point.DistanceTo(buff.Target.Point.Point) > 1)
                {
                    buff.Target.RegainMP((buff.Effect as EffectDice).DiceNum);
                }
            }
        }
    }
}
