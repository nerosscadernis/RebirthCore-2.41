using Rebirth.Common.GameData.D2O;
using Rebirth.Common.Pool;
using Rebirth.Common.Protocol.Enums;
using Rebirth.Common.Protocol.Messages;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Datas.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rebirth.Common.Extensions;
using Rebirth.Common.Protocol.Types;

namespace Rebirth.World.Game.Fights.Marks
{
    public class FightMarkManager
    {
        #region Constructeur
        public FightMarkManager(Fight fight)
        {
            Fight = fight;
            markProvider = new UniqueIdProvider(0);
        }
        #endregion

        #region Var
        public Fight Fight { get; set; }
        private UniqueIdProvider markProvider;
        #endregion

        #region Public Func
        public int NextId()
        {
            return markProvider.Pop();
        }
        #endregion

        #region Glyph
        public List<Glyph> Glyphs = new List<Glyph>();

        public void AddGlyph(Glyph glyph)
        {
            Glyphs.Add(glyph);
            Fight.GlyphAdd(glyph.Caster, glyph.GetGameActionMark());
            if (glyph.IsAura)
                glyph.Declanche(true);
        }

        public void RemoveGlyph(Glyph glyph)
        {
            Fight.GlyphRemoved(glyph);
            Glyphs.Remove(glyph);
        }

        public bool HasGlyph(int cellId)
        {
            return Glyphs.Any(x => x.Cells.Any(entry => entry.Id == cellId));
        }

        public List<Glyph> GetGlyphs(int cellId)
        {
            return Glyphs.FindAll(x => x.Cells.Any(entry => entry.Id == cellId));
        }

        public void DrecrementGlyph(Fighter fighter)
        {
            foreach (var item in Glyphs.FindAll(x => x.Caster.Id == fighter.Id))
            {
                item.Decrement();
            }
        }

        public void ExecuteGlyph(Fighter fighter, TriggerBuffType type)
        {
            foreach (var item in Glyphs.FindAll(x => x.Cells.Any(entry => entry.Id == fighter.Point.Point.CellId) && x.Type == type))
            {
                item.Execute(fighter);
            }
        }

        public void DeclancheGlyph(Fighter fighter)
        {
            foreach (var item in Glyphs.FindAll(x => !x.IsAura && x.Cells.Any(entry => entry.Id == fighter.Point.Point.CellId)))
            {
                item.Declanche();
            }
        }

        public bool IsInGlyph(Fighter fighter)
        {
            return Glyphs.Any(x => x.Cells.Any(entry => entry.Id == fighter.Point.Point.CellId));
        }
        #endregion

        #region Trap
        public List<Trap> Traps = new List<Marks.Trap>();

        public bool HasTrap(int cellId)
        {
            return Traps.Any(x => x.Cells.Any(entry => entry.Id == cellId));
        }

        public bool HasCenterTrap(int cellId)
        {
            return Traps.Any(x => x.TargetedCell.Id == cellId);
        }

        public void DeclancheTrap(int cellId)
        {
            List<Trap> actualTraps = Traps.FindAll(x => x.Cells.Any(entry => entry.Id == cellId));
            RemoveTrap(actualTraps.ToArray());
            foreach (var trap in actualTraps)
            {
                trap.Init(Fight.GetFighter(new MapPoint((short)cellId)));
            }
            foreach (var trap in actualTraps)
            {
                trap.Execute();
            }
        }

        public Trap[] GetTraps(int[] Cells)
        {
            return Traps.FindAll(x => x.Cells.Any(entry => Cells.Contains(entry.Id))).ToArray();
        }

        public void AddTrap(Trap trap)
        {
            Traps.Add(trap);
            Fight.TrapAdd(trap.Caster, trap.GetGameActionMark());
        }

        public void RemoveTrap(Trap[] traps)
        {
            foreach (var trap in traps)
            {
                Fight.TrapRemoved(trap);
                Traps.Remove(trap);
            }
        }
        #endregion

        #region Portail
        public List<Portail> Portails = new List<Portail>();

        public bool HasPortail(short cellId)
        {
            return Portails.Any(x => x.TargetedCell.Id == cellId && x.Active);
        }

        public Portail GetPortail(Fighter owner, short cellId)
        {
            return Portails.FirstOrDefault(x => x.Caster.Id == owner.Id && x.TargetedCell.Id == cellId);
        }

        public void CheckReactive()
        {
            foreach (var portail in Portails.FindAll(x => x.Active || x.IsActivable()))
            {
                if (GetCount(portail) >= 2)
                    portail.Active = true;
                else
                    portail.Active = false;
            }
        }

        public void AddPortail(Portail glyph)
        {
            Portails.Add(glyph);
            Fight.GlyphAdd(glyph.Caster, glyph.GetGameActionMark());
            CheckReactive();
            if (Portails.Count(x => x.Caster.Id == glyph.Caster.Id) > 4)
                RemovePortail(Portails.FirstOrDefault(x => x.Caster.Id == glyph.Caster.Id));
        }

        public void RemovePortail(Portail glyph)
        {
            Fight.PortailRemoved(glyph);
            Portails.Remove(glyph);
        }

        public void RemovePortail(Fighter fighter)
        {
            foreach (var item in Portails.FindAll(x => x.Caster.Id == fighter.Id))
            {
                Fight.PortailRemoved(item);
                Portails.Remove(item);
            }
        }

        public int GetCount(Portail portail)
        {
            var myPortails = Portails.Count(x => x.Caster.Id == portail.Caster.Id && (x.Active || x.IsActivable()));
            return myPortails;
        }

        #region GetEndPortail
        private const int SAME = 0;

        private const int OPPOSITE = 1;

        private const int TRIGONOMETRIC = 2;

        private const int COUNTER_TRIGONOMETRIC = 3;

        public Tuple<Portail, Portail> GetEndPortailByAnkama(short cellId)
        {
            var enter = Portails.FirstOrDefault(x => x.TargetedCell.Id == cellId);
            if (enter == null)
                return null;
            var checkPoints = Portails.FindAll(x => x.Caster.Id == enter.Caster.Id && x.Active).Select(x => new MapPoint(x.TargetedCell)).ToList();
            MapPoint startPoint = new MapPoint(enter.TargetedCell);
            MapPoint next = null;
            int index = 0;
            if (checkPoints.Count == 1 && startPoint.CellId == checkPoints[0].CellId)
            {
                return null;
            }
            List<MapPoint> pointsList = new List<MapPoint>();
            for (int i = 0; i < checkPoints.Count; i++)
            {
                if (checkPoints[i].CellId != startPoint.CellId)
                {
                    pointsList.Add(checkPoints[i]);
                }
            }
            List<uint> res = new List<uint>();
            MapPoint current = startPoint;
            int maxTry = pointsList.Count + 1;
            while (maxTry > 0)
            {
                maxTry--;
                res.Add((uint)current.CellId);
                index = pointsList.IndexOf(current);
                if (index != -1)
                {
                    pointsList.RemoveAt(index);
                }
                next = this.getClosestPortal(current, pointsList);
                if (next == null)
                {
                    break;
                }
                current = next;
            }
            if (res.Count < 2)
            {
                return null;
            }
            return new Tuple<Portail, Portail>(GetPortail(enter.Caster, (short)res.First()), GetPortail(enter.Caster, (short)res.Last()));
        }

        public int GetCasesCount(short cellId)
        {
            int casesCount = 0;
            var enter = Portails.FirstOrDefault(x => x.TargetedCell.Id == cellId);
            if (enter == null)
                return casesCount;
            var checkPoints = Portails.FindAll(x => x.Caster.Id == enter.Caster.Id && x.Active).Select(x => new MapPoint(x.TargetedCell)).ToList();
            MapPoint startPoint = new MapPoint(enter.TargetedCell);
            MapPoint next = null;
            int index = 0;
            if (checkPoints.Count == 1 && startPoint.CellId == checkPoints[0].CellId)
            {
                return casesCount;
            }
            List<MapPoint> pointsList = new List<MapPoint>();
            for (int i = 0; i < checkPoints.Count; i++)
            {
                if (checkPoints[i].CellId != startPoint.CellId)
                {
                    pointsList.Add(checkPoints[i]);
                }
            }
            List<uint> res = new List<uint>();
            MapPoint current = startPoint;
            int maxTry = pointsList.Count + 1;
            while (maxTry > 0)
            {
                maxTry--;
                res.Add((uint)current.CellId);
                index = pointsList.IndexOf(current);
                if (index != -1)
                {
                    pointsList.RemoveAt(index);
                }
                next = this.getClosestPortal(current, pointsList);
                if (next == null)
                {
                    break;
                }
                current = next;
            }
            if (res.Count < 2)
            {
                return casesCount;
            }
            for (int i = 1; i < res.Count; i++)
            {
                var actual = new MapPoint((short)res[i - 1]);
                var end = new MapPoint((short)res[i]);
                casesCount += (int)actual.DistanceToCell(end);
            }
            return casesCount;
        }

        private MapPoint getClosestPortal(MapPoint refMapPoint, List<MapPoint> portals)
        {
            //MapPoint portal = null;
            uint dist = 0;
            List<MapPoint> closests = new List<MapPoint>();
            uint bestDist = 63;
            foreach (var portal in portals)
            {
                dist = refMapPoint.DistanceToCell(portal);
                if (dist < bestDist)
                {
                    closests.Clear();
                    closests.Add(portal);
                    bestDist = dist;
                }
                else if (dist == bestDist)
                {
                    closests.Add(portal);
                }
            }
            if (closests.Count == 0)
            {
                return null;
            }
            if (closests.Count == 1)
            {
                return closests[0];
            }
            return this.getBestNextPortal(refMapPoint, closests);
        }

        private MapPoint getBestNextPortal(MapPoint refCell, List<MapPoint> closests)
        {
            Point refCoord = null;
            Point nudge = null;
            if (closests.Count < 2)
            {
                throw new ArgumentException("closests should have a size of 2.");
            }
            refCoord = new Point(refCell.X, refCell.Y);
            nudge = new Point(refCoord.x, refCoord.y + 1);
            closests.Sort(delegate (MapPoint o1, MapPoint o2)
            {
                double tap = getPositiveOrientedAngle(refCoord, nudge, new Point(o1.X, o1.Y)) - getPositiveOrientedAngle(refCoord, nudge, new Point(o2.X, o2.Y));
                return tap > 0 ? 1 : tap < 0 ? -1 : 0;
            });
            MapPoint res = this.getBestPortalWhenRefIsNotInsideClosests(refCell, closests);
            if (res != null)
            {
                return res;
            }
            return closests[0];
        }

        private MapPoint getBestPortalWhenRefIsNotInsideClosests(MapPoint refCell, List<MapPoint> sortedClosests)
        {
            //MapPoint portal = null;
            if (sortedClosests.Count < 2)
            {
                return null;
            }
            MapPoint prev = sortedClosests[sortedClosests.Count - 1];
            foreach (var portal in sortedClosests)
            {
                switch (this.compareAngles(refCell.Coordinates, prev.Coordinates, portal.Coordinates))
                {
                    case OPPOSITE:
                        if (sortedClosests.Count <= 2)
                        {
                            return null;
                        }
                        break;
                    case COUNTER_TRIGONOMETRIC:
                        return prev;
                    default:
                        prev = portal;
                        continue;
                }
            }
            return null;
        }

        private double getPositiveOrientedAngle(Point refCell, Point cellA, Point cellB)
        {
            switch (this.compareAngles(refCell, cellA, cellB))
            {
                case SAME:
                    return 0;
                case OPPOSITE:
                    return Math.PI;
                case TRIGONOMETRIC:
                    return this.getAngle(refCell, cellA, cellB);
                case COUNTER_TRIGONOMETRIC:
                    return 2 * Math.PI - this.getAngle(refCell, cellA, cellB);
                default:
                    return 0;
            }
        }

        private double getAngle(Point coordRef, Point coordA, Point coordB)
        {
            var a = this.getComplexDistance(coordA, coordB);
            var b = this.getComplexDistance(coordRef, coordA);
            var c = this.getComplexDistance(coordRef, coordB);
            return Math.Acos((b * b + c * c - a * a) / (2 * b * c));
        }

        private double getComplexDistance(Point ref_start, Point ref_end)
        {
            return Math.Sqrt(Math.Pow(ref_start.x - ref_end.x, 2) + Math.Pow(ref_start.y - ref_end.y, 2));
        }

        private int compareAngles(Point refe, Point start, Point end)
        {
            Point aVec = this.vector(refe, start);
            Point bVec = this.vector(refe, end);
            int det = this.getDeterminant(aVec, bVec);
            if (det != 0)
            {
                return det > 0 ? TRIGONOMETRIC : COUNTER_TRIGONOMETRIC;
            }
            return aVec.x >= 0 == bVec.x >= 0 && aVec.y >= 0 == bVec.y >= 0 ? SAME : OPPOSITE;
        }

        private int getDeterminant(Point aVec, Point bVec)
        {
            return aVec.x * bVec.y - aVec.y * bVec.x;
        }

        private Point vector(Point start, Point end)
        {
            return new Point(end.x - start.x, end.y - start.y);
        }
        #endregion
        #endregion

        #region Wall
        public List<Wall> Walls = new List<Wall>();

        public bool HasWall(short cellId)
        {
            return Walls.Any(x => x.TargetedCell.Id == cellId);
        }

        public Wall GetWall(Fighter owner, short cellId)
        {
            return Walls.FirstOrDefault(x => x.Caster.Id == owner.Id && x.TargetedCell.Id == cellId);
        }

        public void UpdateWall()
        {
            List<SummonBomb> allBomb = Fight.Fighters.FindAll(x => x is SummonBomb).Select(x => x as SummonBomb).ToList(); ;
            foreach (SummonBomb firstBomb in allBomb)
            {
                foreach (SummonBomb secondBomb in allBomb.FindAll(x => x != firstBomb && x.Summoner == firstBomb.Summoner))
                {
                    if (firstBomb.Point.Point.IsLine(secondBomb.Point.Point) && firstBomb.Point.Point.DistanceToCell(secondBomb.Point.Point) < 9 
                        && firstBomb.Point.Point.DistanceToCell(secondBomb.Point.Point) > 1 && !BombHaveWallWith(firstBomb, secondBomb)
                        && firstBomb.SpellBomb.wallId == secondBomb.SpellBomb.wallId)
                    {
                        List<MapPoint> cells = (from x in firstBomb.Point.Point.GetCellsInLine(secondBomb.Point.Point)
                                                where (Fight.GetFighter(x) != null 
                                                && Fight.GetFighter(x).IsAlive 
                                                && Fight.GetFighter(x) is SummonBomb 
                                                && (Fight.GetFighter(x) as SummonBomb).Summoner != firstBomb.Summoner ) 
                                                select x).ToList();
                        if (cells.Count - 2 < 2)
                            AddWall(new Wall((short)NextId(), firstBomb.SpellChain, firstBomb.Summoner, firstBomb.Point.Point.OrientationTo(secondBomb.Point.Point, false), Fight, 0, firstBomb, secondBomb));
                    }
                }
            }
            for (int i = 0; i < Walls.Count;)
            {
                if (!Walls[i].IsValid())
                    RemoveWall(Walls[i]);
                else
                    i++;
            }
        }

        public void DeclancheWall(Fighter fihgter, bool noInvu = false)
        {
            foreach (var item in Walls.FindAll(x => x.Cells.Any(entry => entry.Id == fihgter.Point.Point.CellId)))
            {
                if (noInvu)
                    item.Declanche(fihgter);
                else
                    item.Execute(fihgter);
            }
        }

        public void ResetWallFighter(Fighter fighter)
        {
            foreach (var item in Walls)
            {
                item.Fighters.Remove(fighter);
            }
        }

        public void AddWall(Wall glyph)
        {
            Walls.Add(glyph);
            foreach (var item in glyph.Cells)
            {
                Fight.GlyphAdd(glyph.Caster, 
                    new GameActionMark(glyph.Caster.Id, (sbyte)glyph.Caster.Team.Team, 2825, 1, (short)(item.Id + glyph.Id), (sbyte)GameActionMarkTypeEnum.WALL, (short)item.Id,
                     new GameActionMarkedCell[]
                     { new GameActionMarkedCell((uint)item.Id, 0, glyph.Color, (sbyte)GameActionMarkCellsTypeEnum.CELLS_CIRCLE) }, true));
            }
            glyph.Execute();
        }

        public List<SummonBomb> GetBombLier(Wall wall)
        {
            List<SummonBomb> bombs = new List<SummonBomb>();
            bombs.Add(wall.FirstBomb);
            bombs.Add(wall.SecondBomb);
            foreach (var item in wall.Caster.Summons.FindAll(x => x is SummonBomb && 
            ((x.Point.Point.IsLine(wall.FirstBomb.Point.Point) && x.Point.Point.DistanceToCell(wall.FirstBomb.Point.Point) - 1 < 7 && x.Point.Point.DistanceToCell(wall.FirstBomb.Point.Point) - 1 >= 0)
            || (x.Point.Point.IsLine(wall.SecondBomb.Point.Point) && x.Point.Point.DistanceToCell(wall.SecondBomb.Point.Point) - 1 < 7 && x.Point.Point.DistanceToCell(wall.SecondBomb.Point.Point) - 1 >= 0))))
            {
                if(!bombs.Any(x => x.Id == item.Id))
                    bombs.Add(item as SummonBomb);
            }
            return bombs;
        }

        public bool BombHaveWallWith(SummonBomb first, SummonBomb second)
        {
            return Walls.FindAll(x => x.Caster == first.Summoner).Any(x => (x.FirstBomb == first && x.SecondBomb == second) || (x.FirstBomb == second && x.SecondBomb == first));
        }

        public void RemoveWall(Wall glyph)
        {
            Fight.WallRemoved(glyph);
            Walls.Remove(glyph);
        }
        #endregion
    }
}
