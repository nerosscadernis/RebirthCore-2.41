using Rebirth.Common.Protocol.Enums;
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
using Rebirth.World.Datas.Spells;
using System.Text;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Effect.Spells.Debuffs
{
    [EffectHandler(EffectsEnum.Effect_SubAP)]
    public class APDebuffFixe : SpellEffectHandler
    {
        public APDebuffFixe(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }
        public override bool Apply(object token, bool declanched = false)
        {
            bool result;
            foreach (Fighter current in base.GetAffectedActors())
            {
                if (this.Effect.Duration > 0)
                {
                    base.AddStatBuff(current, -Dice.DiceNum, PlayerFields.AP, true, 169, declanched);
                }
                else
                {
                    current.UseMP(Dice.DiceNum, true);
                }
            }
            result = true;
            return result;
        }
    }
}
