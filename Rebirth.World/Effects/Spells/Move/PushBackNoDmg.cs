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

namespace Rebirth.World.Game.Effect.Spells.Move
{
    [EffectHandler(EffectsEnum.Effect_PushBack_1103)]
    public class PushBackNoDmg : SpellEffectHandler
    {
        public PushBackNoDmg(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply(object token, bool declanched = false)
        {
            if (Effect is EffectDice)
            {
                int num = Dice.DiceNum;
                foreach (var item in GetAffectedActors().OrderByDescending(x => x.Point.Point.DistanceToCell(Caster.Point.Point)).ToList().FindAll(x => !x.HasState(157)))
                {
                    if (item.HasState(97))
                        continue;

                    MapPoint startCell = item.Point.Point;
                    MapPoint lastCell = item.Point.Point;
                    Tuple<Portail, Portail> portail = null;
                    for (int i = 0; i < num; i++)
                    {
                        MapPoint nextCell;
                        if ((Spell.IsTrapSpell || Spell.IsGlyphSpell) && TargetedPoint.CellId == item.Point.Point.CellId)
                            return false;
                        if (TargetedPoint.CellId == item.Point.Point.CellId)
                            nextCell = lastCell.GetNearestCellInDirection(Caster.Point.Point.OrientationTo(item.Point.Point, true));
                        else if (token != null && token is Trap)
                            nextCell = lastCell.GetNearestCellInDirection(TargetedPoint.OrientationTo((token as Trap).DeclanchedCell, true));
                        else
                            nextCell = lastCell.GetNearestCellInDirection(TargetedPoint.OrientationTo(item.Point.Point, true));
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
                        else if (Fight.MarkManager.HasPortail(nextCell.CellId))
                        {
                            portail = Fight.MarkManager.GetEndPortailByAnkama(nextCell.CellId);
                            lastCell = new MapPoint((short)portail.Item2.TargetedCell.Id);
                        }
                        else
                            lastCell = nextCell;
                    }
                    if (lastCell != startCell)
                    {
                        if (portail != null)
                        {
                            item.SlideTo(Caster, new MapPoint((short)portail.Item1.TargetedCell.Id), ActionsEnum.ACTION_CHARACTER_PUSH);
                            Fight.DeclancheTrap((short)portail.Item1.TargetedCell.Id);
                            portail.Item1.Execute(item);
                            portail.Item2.Execute(item);
                            item.ApplyTriggerBuff(TriggerBuffType.PORTAL, null);
                            item.TeleportTo(Caster, (short)portail.Item2.TargetedCell.Id, ActionsEnum.ACTION_CHARACTER_TELEPORT_ON_SAME_MAP);
                            item.SlideTo(Caster, lastCell, ActionsEnum.ACTION_CHARACTER_PUSH);
                            Fight.DeclancheTrap(lastCell.CellId);
                        }
                        else
                        {
                            item.SlideTo(Caster, lastCell, ActionsEnum.ACTION_CHARACTER_PUSH);
                            Fight.DeclancheTrap(lastCell.CellId);
                            Fight.MarkManager.ExecuteGlyph(item, TriggerBuffType.AURA);
                        }
                    }
                }
                return true;
            }
            return false;
        }
    }
}
