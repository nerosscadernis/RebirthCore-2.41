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
using Rebirth.World.Datas.Spells;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Effect.Spells.Damage
{
    [EffectHandler(EffectsEnum.Effect_Punishment_Damage)]
    public class Punishment : SpellEffectHandler
    {
        public Punishment(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }
        public override bool Apply(object token, bool declanched = false)
        {
            foreach (Fighter current in base.GetAffectedActors())
            {
                var viePourcent = (double)Caster.Stats.Health.Total * 100 / Caster.Stats.Health.TotalMax;
                var maxPourcent = (double)Dice.DiceNum / 100;
                var maxVie = (double)Caster.Stats.Health.TotalMax + (double)Caster.Stats.Health.PermanentDamages;

                var num = ((maxPourcent * Math.Pow((Math.Cos(2d * Math.PI * ((viePourcent / 100d) -0.5d)) + 1d), 2d)) / 4d) * maxVie;

                current.InflictDamage(new Fights.Other.Damage((int)num, Caster, current, Spell) { critical = Critical });
            }
            return true;
        }
    }
}
