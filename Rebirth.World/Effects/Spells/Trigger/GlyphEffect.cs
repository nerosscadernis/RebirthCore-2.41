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
using Rebirth.Common.Protocol.Data;
using Rebirth.Common.GameData.D2O;

namespace Rebirth.World.Game.Effect.Spells.Trigger
{
    [EffectHandler(EffectsEnum.Effect_Glyph), EffectHandler(EffectsEnum.Effect_Glyph_402)]
    public class GlyphEffect : SpellEffectHandler
    {
        public GlyphEffect(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }
        public override bool Apply(object token, bool declanched = false)
        {      
            var spellGlyph = ObjectDataManager.Get<Spell>(Dice.DiceNum);
            if (spellGlyph != null)
            {
                var spellLevel = ObjectDataManager.Get<SpellLevel>((int)spellGlyph.spellLevels[Dice.DiceFace - 1]);
                var glyph = new Glyph((short)Fight.MarkManager.NextId(), spellLevel, Caster, Caster.Point.Point.OrientationTo(TargetedPoint, false), TargetedCell, Fight, Dice.ZoneShape, (byte)Dice.ZoneSize, Dice.Value, Spell.Spell.id, (Spell.Spell.id == 13 ? TriggerBuffType.TURN_END : TriggerBuffType.TURN_BEGIN))
                {
                    Duration = Dice.Duration
                };

                Fight.MarkManager.AddGlyph(glyph);
            }
            return true;
        }
    }
}
