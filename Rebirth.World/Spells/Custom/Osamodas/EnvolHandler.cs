using Rebirth.Common.Protocol.Data;
using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Effects.Handlers.Spells;
using Rebirth.World.Game.Effect.Instances;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Game.Fights.Buffs;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Game.Spells.Cast;
using System;
using System.Collections.Generic;
using System.Linq;
using Rebirth.World.Datas.Spells;
using System.Text;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Spells.Custom.Osamodas
{
    [SpellCastHandler(23)]
    public class EnvolHandler : DefaultSpellCastHandler
    {
        public EnvolHandler(Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(caster, spell, targetedCell, critical)
        {
        }

        public object TemporaryBoostStateEffect { get; private set; }

        public override void Execute(object token)
        {
            if (!this.m_initialized)
            {
                this.Initialize(true);
            }

            List<SpellEffectHandler> effects = base.Handlers.ToList();

            Fighter fighter = Fight.GetFighter(TargetedCell);
            if (fighter != null && fighter is SummonMonster && fighter.Team == Caster.Team)
            {
                effects[0].Apply(null);
                effects[1].Apply(null);

                effects[2].TargetedCell = Fight.Map.Cells[fighter.Point.Point.CellId];
                effects[2].TargetedPoint = fighter.Point.Point;
                effects[2].Apply(null);

                fighter.Die(Caster);
            }
        }
    }
}
