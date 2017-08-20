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
using System.Threading.Tasks;
using Rebirth.World.Datas.Spells;

namespace Rebirth.World.Game.Effect.Spells.Buffs
{
    [EffectHandler(EffectsEnum.Effect_RegainAP), EffectHandler(EffectsEnum.Effect_AddAP_111)]
    public class AddPA : SpellEffectHandler
    {
        public AddPA(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }
        public override bool Apply(object token, bool declanched = false)
        {
            bool result;
            foreach (Fighter current in base.GetAffectedActors())
            {
                EffectInteger effectInteger = base.GenerateEffect();
                if (effectInteger == null)
                {
                    result = false;
                    return result;
                }
                if (this.Effect.Duration > 0)
                {
                    base.AddStatBuff(current, (short)effectInteger.Value, PlayerFields.AP, true, declanched);
                }
                else
                {
                    current.RegainAP((short)effectInteger.Value);
                }
            }
            result = true;
            return result;
        }
    }
}
