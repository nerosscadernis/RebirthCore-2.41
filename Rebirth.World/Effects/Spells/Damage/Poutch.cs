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
    [EffectHandler(EffectsEnum.Effect_Poutch)]
    public class Poutch : SpellEffectHandler
    {
        public Poutch(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }
        public override bool Apply(object token, bool declanched = false)
        {
            Fighter fighter = Fight.GetFighter(TargetedCell);
            if (token != null && token is Fights.Other.Damage && fighter != null)
            {
                Fights.Other.Damage damage = (Fights.Other.Damage)token;
                foreach (var current in GetAffectedActors())
                {
                    Fights.Other.Damage newDamage = new Fights.Other.Damage(damage.AfterReduction * Dice.DiceNum / 100, fighter, current, Spell);
                    current.InflictDirectDamage(newDamage, true);
                }
            }
            return true;
        }
    }
}
