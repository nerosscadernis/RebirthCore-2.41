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
    [EffectHandler(EffectsEnum.Effect_BePulled)]
    public class BePull : SpellEffectHandler
    {
        public BePull(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply(object token, bool declanched = false)
        {
            if (Effect is EffectDice)
            {
                if (Caster.HasState(97))
                    return false;
                int num = (Effect as EffectDice).DiceNum;
                Fighter item = Fight.GetFighter(TargetedCell);
                if (item != null)
                {
                    MapPoint startCell = Caster.Point.Point;
                    MapPoint lastCell = Caster.Point.Point;
                    Tuple<Portail, Portail> portail = null;
                    for (int i = 0; i < num; i++)
                    {
                        MapPoint nextCell;
                        if ((Spell.IsTrapSpell || Spell.IsGlyphSpell) && TargetedPoint.CellId == item.Point.Point.CellId)
                            return false;
                        if (TargetedPoint.CellId == Caster.Point.Point.CellId)
                            nextCell = lastCell.GetNearestCellInDirection(Caster.Point.Point.OrientationTo(item.Point.Point, true));
                        else
                            nextCell = lastCell.GetNearestCellInDirection(Caster.Point.Point.OrientationTo(TargetedPoint, true));
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
                            Caster.SlideTo(Caster, new MapPoint((short)portail.Item1.TargetedCell.Id), ActionsEnum.ACTION_CHARACTER_PUSH);
                            Fight.DeclancheTrap((short)portail.Item1.TargetedCell.Id);
                            portail.Item1.Execute(Caster);
                            portail.Item2.Execute(Caster);
                            Caster.ApplyTriggerBuff(TriggerBuffType.PORTAL, null);
                            Caster.TeleportTo(Caster, (short)portail.Item2.TargetedCell.Id, ActionsEnum.ACTION_CHARACTER_TELEPORT_ON_SAME_MAP);
                            Caster.SlideTo(Caster, lastCell, ActionsEnum.ACTION_CHARACTER_PUSH);
                            Fight.DeclancheTrap(lastCell.CellId);
                        }
                        else
                        {
                            Caster.SlideTo(Caster, lastCell, ActionsEnum.ACTION_CHARACTER_PUSH);
                            Fight.DeclancheTrap(lastCell.CellId);
                            Fight.MarkManager.ExecuteGlyph(Caster, TriggerBuffType.AURA);
                        }
                    }

                    return true;
                }
            }
            return false;
        }
    }
}
