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
    [EffectHandler(EffectsEnum.Effect_SubLifePourcent), EffectHandler(EffectsEnum.Effect_SubLifePourcent_1033)]
    public class SubLifePercent : SpellEffectHandler
    {
        public SubLifePercent(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
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
                if (this.Effect.Duration > 0 || this.Effect.Duration < 0)
                {
                    base.AddStatBuff(current, (short)-(current.Stats.Health.Total * effectInteger.Value / 100), PlayerFields.Health, true, 153, declanched);
                }
            }
            result = true;
            return result;
        }
    }
}
