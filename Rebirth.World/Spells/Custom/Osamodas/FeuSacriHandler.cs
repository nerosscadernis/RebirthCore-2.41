using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Effects.Handlers.Spells;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Datas.Spells;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Game.Spells.Cast;
using Rebirth.World.Game.Zones;
using System.Collections.Generic;
using System.Linq;

namespace Rebirth.World.Game.Spells.Custom.Osamodas
{
    [SpellCastHandler(25)]
    public class FeuSacriHandler : DefaultSpellCastHandler
    {
        public FeuSacriHandler(Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(caster, spell, targetedCell, critical)
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

            Fighter summon = Fight.GetFighter(TargetedCell);
            if (summon != null && summon is SummonMonster && summon.Team == Caster.Team)
            {
                var zone = new Zone(SpellShapeEnum.C, 2, 0, this.Caster.Point.Point.OrientationTo(new MapPoint(TargetedCell), true));
                var cells = zone.GetCells(TargetedCell, Fight.Map);

                List<Fighter> fighters = (from entry in this.Fight.GetAllFighter(cells)
                                          where !entry.IsDead && entry.Team.Team == Caster.Team.Team
                                          select entry).ToList();
                foreach (var fighter in fighters)
                {
                    fighter.Heal(summon.Stats.Health.Total * 55 / 100, Caster, 0, 1, false);
                }

                summon.Die(Caster);
            }
        }
    }
}
