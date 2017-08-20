using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Effects.Handlers;
using Rebirth.World.Effects.Handlers.Spells;
using Rebirth.World.Game.Effect.Instances;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Game.Fights.Buffs;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Datas.Monsters;
using Rebirth.World.Game.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rebirth.World.Datas.Spells;

namespace Rebirth.World.Game.Heal
{
    [EffectHandler(EffectsEnum.Effect_Double)]
    public class Double : SpellEffectHandler
    {
        public Double(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }
        public override bool Apply(object token, bool declanched = false)
        {
            bool result;
            Caster.AddSummon(new SummonDouble(Caster.Team, Fight, Fight.GetNextContextualId(), Caster, new Fights.Other.FightDisposition(TargetedCell.Id, Caster.Point.Point.OrientationTo(TargetedPoint, false))));
            result = true;
            return result;
        }
    }
}
