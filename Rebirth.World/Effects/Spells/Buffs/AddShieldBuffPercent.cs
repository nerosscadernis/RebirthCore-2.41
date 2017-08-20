
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
    [EffectHandler(EffectsEnum.Effect_AddShieldPercent)]
    public class AddShieldBuffPercent : SpellEffectHandler
    {
        public AddShieldBuffPercent(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
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
                if (this.Effect.Duration <= 0)
                {
                    result = false;
                    return result;
                }
                double num = (double)Caster.Stats.Health.TotalMax * ((double)effectInteger.Value / 100.0);
                
                base.AddStatBuff(current, (short)num, PlayerFields.Shield, true, 1040, declanched);
            }
            result = true;
            return result;
        }
    }
}
