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

namespace Rebirth.World.Game.Effect.Spells.Buffs
{
    [EffectHandler(EffectsEnum.Effect_AddResistances), EffectHandler(EffectsEnum.Effect_SubResistances)]
    public class Resistances : SpellEffectHandler
    {
        public Resistances(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }
        public override bool Apply(object token, bool declanched = false)
        {
            EffectInteger effectInteger = base.GenerateEffect();
            bool result;
            if (effectInteger == null)
            {
                result = false;
            }
            else
            {
                foreach (Fighter current in base.GetAffectedActors())
                {
                    ResistancesBuff buff = new ResistancesBuff(current.PopNextBuffId(), current, base.Caster, effectInteger, base.Spell, Convert.ToInt16((this.Effect.EffectId == EffectsEnum.Effect_SubResistances) ? (-effectInteger.Value) : effectInteger.Value), false, true, declanched);
                    current.AddAndApplyBuff(buff, true);
                }
                result = true;
            }
            return result;
        }
    }
}
