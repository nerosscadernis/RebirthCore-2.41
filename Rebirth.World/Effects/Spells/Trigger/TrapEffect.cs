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
    [EffectHandler(EffectsEnum.Effect_Trap)]
    public class TrapEffect : SpellEffectHandler
    {
        public TrapEffect(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }
        public override bool Apply(object token, bool declanched = false)
        {
            var spellTrap = ObjectDataManager.Get<Spell>(Dice.DiceNum);
            if (spellTrap != null)
            {
                var spellLevel = ObjectDataManager.Get<SpellLevel>((int)spellTrap.spellLevels[Dice.DiceFace - 1]);
                var trap = new Trap((short)Fight.MarkManager.NextId(), spellLevel, Caster, Caster.Point.Point.OrientationTo(TargetedPoint, false), TargetedCell, Fight, Dice.ZoneShape, (byte)Dice.ZoneSize, Dice.Value, Spell.Spell.id);
                Fight.MarkManager.AddTrap(trap);
            }
            return true;
        }
    }
}
