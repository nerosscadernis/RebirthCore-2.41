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
    [EffectHandler(EffectsEnum.Effect_GiveHPPercent)]
    public class GiveHPPercent : SpellEffectHandler
    {
        public GiveHPPercent(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }
        public override bool Apply(object token, bool declanched = false)
        {
            bool result;
            int num = (Effect as EffectDice).DiceNum;
            var heal = (num * Caster.Stats.Health.Total) / 100;
            Caster.InflictDamage(new Fights.Other.Damage(heal, Caster, Caster, Spell));
            foreach (Fighter current in base.GetAffectedActors())
            {
                current.Heal(heal, Caster, TargetedPoint.DistanceTo(current.Point.Point), Boost, false);
            }
            result = true;
            return result;
        }
    }
}
