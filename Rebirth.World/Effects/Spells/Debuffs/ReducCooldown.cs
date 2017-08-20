using Rebirth.Common.Network;
using Rebirth.Common.Protocol.Enums;
using Rebirth.Common.Protocol.Messages;
using Rebirth.Common.Thread;
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

namespace Rebirth.World.Game.Effect.Spells.Debuffs
{
    [EffectHandler(EffectsEnum.Effect_ReducCooldown)]
    public class ReducCooldown : SpellEffectHandler
    {
        public ReducCooldown(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }
        public override bool Apply(object token, bool declanched = false)
        {
            bool result;
            foreach (Fighter current in base.GetAffectedActors())
            {
                if (current is CharacterFighter)
                {
                    var spell = (current as CharacterFighter).Character.Spells.Spells.FirstOrDefault(x => x.Spell.id == (uint)Dice.DiceNum);
                    if (spell != null)
                    {
                        var canReduc = current.SpellHistory.ReducCooldown(spell, Dice.Value);
                        Fight.ChangeCooldown(Caster, current, spell, Dice.Value);
                    }
                }
            }
            result = true;
            return result;
        }
    }
}
