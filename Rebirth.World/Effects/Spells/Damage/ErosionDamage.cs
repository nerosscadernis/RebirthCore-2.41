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
    [EffectHandler(EffectsEnum.Effect_Erosion_Air), EffectHandler(EffectsEnum.Effect_Erosion_Fire)]
    public class ErosionDamage : SpellEffectHandler
    {
        public ErosionDamage(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }
        public override bool Apply(object token, bool declanched = false)
        {
            foreach (Fighter current in base.GetAffectedActors())
            {
                var damage = new Fights.Other.Damage(current.Stats.Health.PermanentDamages * (Effect as EffectDice).DiceNum / 100, Caster, current, Spell);
                current.InflictDamage(damage);
            }
            return true;
        }

        private static EffectSchoolEnum GetEffectSchool(EffectsEnum effect)
        {
            EffectSchoolEnum result;
            switch (effect)
            {
                case EffectsEnum.Effect_DamageWater:
                    result = EffectSchoolEnum.Water;
                    break;
                case EffectsEnum.Effect_DamageEarth:
                    result = EffectSchoolEnum.Earth;
                    break;
                case EffectsEnum.Effect_Erosion_Air:
                    result = EffectSchoolEnum.Air;
                    break;
                case EffectsEnum.Effect_Erosion_Fire:
                    result = EffectSchoolEnum.Fire;
                    break;
                case EffectsEnum.Effect_DamageNeutral:
                    result = EffectSchoolEnum.Neutral;
                    break;
                default:
                    throw new System.Exception(string.Format("Effect {0} has not associated School Type", effect));
            }
            return result;
        }
    }
}
