using Rebirth.Common.Protocol.Enums;
using Rebirth.Common.Thread;
using Rebirth.World.Effects.Handlers.Spells;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Game.Spells.Cast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rebirth.World.Datas.Spells;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Spells.Custom.Ecaflip
{
    [SpellCastHandler(120)]
    public class DestinHandler : DefaultSpellCastHandler
    {
        public DestinHandler(Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(caster, spell, targetedCell, critical)
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
            var tmp = effects[1];
            effects.RemoveAt(1);
            effects.RemoveAt(1);
            effects.RemoveAt(1);

            Handlers = effects.ToArray();
            base.Execute(token);

            var target = Fight.GetFighter(TargetedCell);
            if (target != null)
            {
                #region Pull
                MapPoint startCell = target.Point.Point;
                MapPoint lastCell = target.Point.Point;
                for (int i = 0; i < 4; i++)
                {
                    MapPoint nextCell;
                    if (Caster.Point.Point.CellId == target.Point.Point.CellId)
                        nextCell = lastCell.GetNearestCellInDirection(target.Point.Point.OrientationTo(Caster.Point.Point, true));
                    else
                        nextCell = lastCell.GetNearestCellInDirection(target.Point.Point.OrientationTo(Caster.Point.Point, true));
                    Fighter fighter = Fight.GetFighter(nextCell);
                    if (nextCell == null || fighter != null || Fight.Map.Cells[nextCell.CellId].NonWalkableDuringFight || !Fight.Map.Cells[nextCell.CellId].Mov)
                    {
                        break;
                    }
                    else if (Fight.HasTrap(nextCell.CellId))
                    {
                        lastCell = nextCell;
                        break;
                    }
                    else
                        lastCell = nextCell;
                }
                if (lastCell != startCell)
                {
                    target.SlideTo(Caster, lastCell, ActionsEnum.ACTION_CHARACTER_PUSH);
                    Fight.DeclancheTrap(lastCell.CellId);
                }
                #endregion

                #region Push
                startCell = target.Point.Point;
                lastCell = target.Point.Point;
                for (int i = 0; i < 4; i++)
                {
                    MapPoint nextCell;
                    if (Caster.Point.Point.CellId == target.Point.Point.CellId)
                        nextCell = lastCell.GetNearestCellInDirection(Caster.Point.Point.OrientationTo(target.Point.Point, true));
                    else
                        nextCell = lastCell.GetNearestCellInDirection(Caster.Point.Point.OrientationTo(target.Point.Point, true));
                    Fighter fighter = Fight.GetFighter(nextCell);
                    if (nextCell == null || fighter != null || Fight.Map.Cells[nextCell.CellId].NonWalkableDuringFight || !Fight.Map.Cells[nextCell.CellId].Mov)
                    {
                        target.InflictPushDamage(Caster, Spell, 4 - i);
                        if (fighter != null)
                        {
                            fighter.InflictPushDamage(Caster, Spell, 4, true);
                        }
                        tmp.SetAffectedActor(target);
                        Handlers = new SpellEffectHandler[] { tmp };
                        base.Execute(token);
                        break;
                    }
                    else if (Fight.HasTrap(nextCell.CellId))
                    {
                        lastCell = nextCell;
                        break;
                    }
                    else
                        lastCell = nextCell;
                }
                if (lastCell != startCell)
                {
                    target.SlideTo(Caster, lastCell, ActionsEnum.ACTION_CHARACTER_PUSH);
                    Fight.DeclancheTrap(lastCell.CellId);
                }
                #endregion
            }
        }
    }
}
