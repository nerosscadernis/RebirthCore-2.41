using Rebirth.Common.Utils;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Datas.Spells;
using Rebirth.World.Game.Effect.Instances;
using Rebirth.World.Game.Fights.Actors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rebirth.World.Effects.Handlers.Spells
{
    [DefaultEffectHandler]
    public class DefaultSpellEffect : SpellEffectHandler
    {
        public DefaultSpellEffect(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }
        public override bool Apply(object token, bool declanched = false)
        {
            var logger = LogManager.GetLoggerClass();
            logger.Error("Spell effectHander unknow : " + Effect.EffectId.ToString());
            return true;
        }
    }
}
