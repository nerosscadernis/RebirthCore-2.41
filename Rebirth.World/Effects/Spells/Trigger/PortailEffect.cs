using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Effects.Handlers;
using Rebirth.World.Effects.Handlers.Spells;
using Rebirth.World.Game.Effect.Instances;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Game.Fights.Marks;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Game.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rebirth.World.Datas.Spells;

namespace Rebirth.World.Game.Effect.Spells.Trigger
{
    [EffectHandler(EffectsEnum.Effect_AddPortail)]
    public class PortailEffect : SpellEffectHandler
    {
        public PortailEffect(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }
        public override bool Apply(object token, bool declanched = false)
        {
            var glyph = new Portail((short)Fight.MarkManager.NextId(), Spell, Caster, Caster.Point.Point.OrientationTo(TargetedPoint, false), TargetedCell, Fight, Dice.ZoneShape, (byte)Dice.ZoneSize, Dice.Value);

            Fight.MarkManager.AddPortail(glyph);
            return true;
        }
    }
}
