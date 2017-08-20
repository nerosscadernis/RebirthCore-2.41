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
using Rebirth.World.Datas.Spells;
using System.Threading.Tasks;
using Rebirth.Common.Protocol.Data;
using Rebirth.Common.GameData.D2O;

namespace Rebirth.World.Game.Effect.Spells.Trigger
{
    [EffectHandler(EffectsEnum.Effect_DeclanchGlyph)]
    public class DeclanchGlyph : SpellEffectHandler
    {
        public DeclanchGlyph(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }
        public override bool Apply(object token, bool declanched = false)
        {
            var spellGlyph = ObjectDataManager.Get<Spell>(Dice.DiceNum);
            if (spellGlyph != null)
            {
                Fight.MarkManager.DeclancheGlyph(Caster);
            }
            return true;
        }
    }
}
