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
    [EffectHandler(EffectsEnum.Effect_Brokle)]
    public class Brokle : SpellEffectHandler
    {
        public Brokle(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }
        public override bool Apply(object token, bool declanched = false)
        {
            bool result;
            foreach (Fighter current in base.GetAffectedActors())
            {
                var buff = new TriggerBuff(current.PopNextBuffId(), current, Caster, Effect, Spell, null, (short)Dice.Value, Critical, true, TriggerBuffType.BEFORE_ATTACKED, declanched);
                buff.Applyed += ApplyBrokle;
                base.AddTriggerBuff(current, buff);
            }
            result = true;
            return result;
        }
        private void ApplyBrokle(TriggerBuff buff, object token)
        {
            if (token is Fights.Other.Damage)
            {
                (token as Fights.Other.Damage).EffectGenerationType = EffectGenerationType.MaxEffects;
            }
        }
    }
}
