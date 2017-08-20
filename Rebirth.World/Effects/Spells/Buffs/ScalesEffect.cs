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

namespace Rebirth.World.Game.Effect.Spells.Buffs
{
    [EffectHandler(EffectsEnum.Effect_1060)]
    public class Dodge : SpellEffectHandler
    {
        public Dodge(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }
        public override bool Apply(object token, bool declanched = false)
        {
            foreach (Fighter current in base.GetAffectedActors())
            {
                if (current.BaseLook.Scales.Count > 0)
                    current.BaseLook.Scales[0] += Dice.DiceNum;
                else
                    current.BaseLook.Scales.Add((short)(100 + Dice.DiceNum));
                Fight.ModifyLook(current, current, current.BaseLook.GetEntityLook());
            }
            return true;
        }
    }
}
