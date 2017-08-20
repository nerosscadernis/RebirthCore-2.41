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
using Rebirth.World.Datas.Spells;
using System.Text;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Effect.Spells.Buffs
{
    [EffectHandler(EffectsEnum.Effect_SpellBoostDamage), EffectHandler(EffectsEnum.Effect_BoostSpellPO)]
    public class SpellBoost : SpellEffectHandler
    {
        public SpellBoost(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }
        public override bool Apply(object token, bool declanched = false)
        {
            bool result;
            EffectDice effectDice = (EffectDice)Effect;
            foreach (Fighter current in base.GetAffectedActors())
            {
                SpellBoostBuff buff = new SpellBoostBuff(current.PopNextBuffId(), current, Caster, Effect, Spell, (short)effectDice.Value, effectDice.DiceNum, Critical, false, declanched, GetBoostType());
                current.AddAndApplyBuff(buff);
            }
            result = true;
            return result;
        }
        public CharacterSpellModificationTypeEnum GetBoostType()
        {
            switch (base.Effect.EffectId)
            {
                case EffectsEnum.Effect_BoostSpellPO:
                    return CharacterSpellModificationTypeEnum.RANGE;
                case EffectsEnum.Effect_SpellBoostDamage:
                    return CharacterSpellModificationTypeEnum.BASE_DAMAGE;
            }
            return CharacterSpellModificationTypeEnum.INVALID_MODIFICATION;
        }
    }
}
