using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Effects.Handlers;
using Rebirth.World.Effects.Handlers.Spells;
using Rebirth.World.Game.Effect.Instances;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Game.Spells;
using System;
using System.Collections.Generic;
using Rebirth.World.Datas.Spells;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Effect.Spells.Steal
{


[EffectHandler(EffectsEnum.Effect_RemoveAP)]
public class RemoveAP : SpellEffectHandler
{
    public RemoveAP(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
    {
    }
    public override bool Apply(object token, bool declanched = false)
    {
        foreach (Fighter current in base.GetAffectedActors())
        {
            int num = (Effect as EffectDice).DiceNum;
            if (num > 0)
            {
                    current.UseAP((short)num, true);
            }
        }
        return true;
    }
}
}
