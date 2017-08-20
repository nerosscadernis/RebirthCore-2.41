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

namespace Rebirth.World.Game.Heal
{
    [EffectHandler(EffectsEnum.Effect_Heal_Last_Damage)]
    public class HealLastDamage : SpellEffectHandler
    {
        public HealLastDamage(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }
        public override bool Apply(object token, bool declanched = false)
        {
            bool result;
            foreach (Fighter current in base.GetAffectedActors())
            {
                EffectInteger effectInteger = base.GenerateEffect();
                if(Caster.LastDamage != null)
                    current.Heal(Caster.LastDamage.AmountBefore * Dice.DiceNum / 100, base.Caster, TargetedPoint.DistanceTo(current.Point.Point), Boost, false);
            }
            result = true;
            return result;
        }
    }
}
