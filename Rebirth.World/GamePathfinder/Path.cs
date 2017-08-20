using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Datas.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rebirth.World.GamePathfinder
{
    public class Path
    {
        private CellMap[] m_compressedPath;
        private MapTemplate m_map;
        public Path(MapTemplate map, List<CellMap> cells)
        {
            m_compressedPath = cells.ToArray();
            m_map = map;
        }
        public CellMap[] Cells
        {
            get { return m_compressedPath; }
        }
        public static Path BuildFromCompressedPath(MapTemplate map, System.Collections.Generic.IEnumerable<short> keys)
        {
            List<CellMap> cellSendFromClient = new List<CellMap>();

            foreach (var cell in keys)
            {
                var cellId = (int)(cell & 4095);
                var direction = (DirectionsEnum)(cell >> 12 & 7);
                cellSendFromClient.Add(new CellMap(map.Cells[cellId], (int)cellId, direction));
            }

            List<CellMap> cells = new List<CellMap>();
            cells.Add(cellSendFromClient.First());
            for (int i = 0; i < cellSendFromClient.Count - 1; i++)
            {
                var startCell = cellSendFromClient[i];
                var nextCell = cellSendFromClient[i + 1];

                var currentCell = startCell;
                MapPoint cellule = currentCell.Point;
                while (cellule.CellId != nextCell.Id)
                {
                    cellule = currentCell.Point.GetCellInDirection(currentCell.Direction, 1);
                    var added = new CellMap(map.Cells[cellule.CellId], (int)cellule.CellId, startCell.Direction);
                    currentCell = added;
                    if (added.Mov)
                    {
                        cells.Add(added);
                    }
                    else
                        return new Path(map, cells);
                }

            }

            return new Path(map, cells);
        }

        public static Path BuildFromCompressedPath(CellMap[] cellsMap, System.Collections.Generic.IEnumerable<short> keys)
        {
            List<CellMap> cellSendFromClient = new List<CellMap>();

            foreach (var cell in keys)
            {
                var cellId = (int)(cell & 4095);
                var direction = (DirectionsEnum)(cell >> 12 & 7);
                cellSendFromClient.Add(new CellMap(cellsMap[cellId], (int)cellId, direction));
            }

            List<CellMap> cells = new List<CellMap>();
            cells.Add(cellSendFromClient.First());
            for (int i = 0; i < cellSendFromClient.Count - 1; i++)
            {
                var startCell = cellSendFromClient[i];
                var nextCell = cellSendFromClient[i + 1];

                var currentCell = startCell;
                MapPoint cellule = currentCell.Point;
                while (cellule.CellId != nextCell.Id)
                {
                    cellule = currentCell.Point.GetCellInDirection(currentCell.Direction, 1);
                    var added = new CellMap(cellsMap[cellule.CellId], (int)cellule.CellId, startCell.Direction);
                    currentCell = added;
                    if (added.Mov)
                    {
                        cells.Add(added);
                    }
                    else
                        return new Path(null, cells);
                }

            }

            return new Path(null, cells);
        }

        public static Path BuildFromCompressedFightPath(MapTemplate map, short[] keys)
        {
            List<CellMap> cellSendFromClient = new List<CellMap>();

            foreach (var cell in keys)
            {
                var cellId = (int)(cell & 4095);
                var direction = (DirectionsEnum)(cell >> 12 & 7);
                cellSendFromClient.Add(new CellMap(map.Cells[cellId], (int)cellId, direction));
            }

            List<CellMap> cells = new List<CellMap>();
            cells.Add(cellSendFromClient.First());
            for (int i = 0; i < cellSendFromClient.Count - 1; i++)
            {
                var startCell = cellSendFromClient[i];
                var nextCell = cellSendFromClient[i + 1];

                var currentCell = startCell;
                MapPoint cellule = currentCell.Point;
                while (cellule.CellId != nextCell.Id)
                {
                    cellule = currentCell.Point.GetCellInDirection(currentCell.Direction, 1);
                    var added = new CellMap(map.Cells[cellule.CellId], (int)cellule.CellId, startCell.Direction);
                    currentCell = added;
                    if (added.Mov)
                    {
                        cells.Add(added);
                    }
                    else
                        return new Path(map, cells);
                }

            }

            return new Path(map, cells);
        }

        public int MPCost
        {
            get
            {
                return this.m_compressedPath.Length - 1;
            }
        }

        public CellMap[] BuildCompletePath()
        {
            System.Collections.Generic.List<CellMap> list = new System.Collections.Generic.List<CellMap>();
            for (int i = 0; i < this.m_compressedPath.Length - 1; i++)
            {
                list.Add(this.m_compressedPath[i]);
                int num = 0;
                MapPoint mapPoint = this.m_compressedPath[i].Point;
                while ((mapPoint = mapPoint.GetNearestCellInDirection(this.m_compressedPath[i].Direction)) != null && mapPoint.CellId != this.m_compressedPath[i + 1].Point.CellId)
                {
                    if ((long)num > 54L)
                    {
                        throw new System.Exception("Path too long. Maybe an orientation problem ?");
                    }
                    list.Add(m_map.Cells[(int)mapPoint.CellId]);
                    num++;
                }
            }
            list.Add(this.m_compressedPath[this.m_compressedPath.Length - 1]);
            return list.ToArray();
        }

        public List<short> GetCompressedPath()
        {
            var LenghtPath = m_compressedPath.Length;
            for (int i = 0; i < m_compressedPath.Length - 1; i++)
            {
                m_compressedPath[i].Direction = m_compressedPath[i].Point.OrientationTo(m_compressedPath[i].Point, false);
            }

            m_compressedPath[m_compressedPath.Length - 1].Direction = m_compressedPath[m_compressedPath.Length - 2].Direction;

            List<short> cellsIds = new List<short>();
            int orientation = (int)m_compressedPath[0].Direction;
            short result = (short)((orientation & 7) << 12 | m_compressedPath[0].Id & 4095);
            cellsIds.Add(result);
            if (m_compressedPath.Length > 0)
            {
                int _loc1_ = m_compressedPath.Length - 1;
                while (_loc1_ > 0)
                {
                    if (m_compressedPath[_loc1_].Direction == m_compressedPath[_loc1_ - 1].Direction)
                    {
                        _loc1_--;
                    }
                    else
                    {
                        int _loc3_ = (int)m_compressedPath[_loc1_].Direction;
                        result = (short)((_loc3_ & 7) << 12 | m_compressedPath[_loc1_].Id & 4095);
                        cellsIds.Add(result);
                        _loc1_--;
                    }
                }
            }
            orientation = (int)m_compressedPath[m_compressedPath.Length - 1].Direction;
            result = (short)((orientation & 7) << 12 | m_compressedPath[m_compressedPath.Length - 1].Id & 4095);
            cellsIds.Add(result);
            return cellsIds;
        }

        public void CutPath(int index)
        {
            if (index <= m_compressedPath.Count())
            {
                CellMap[] newCells = new CellMap[index];
                Array.Copy(m_compressedPath, newCells, index);
                m_compressedPath = newCells;
            }
        }

        public List<CellMap> GetCutPath(int index)
        {
            if (index <= m_compressedPath.Count())
            {
                var NextCells = new List<CellMap>();
                CellMap[] newCells = new CellMap[index];
                Array.Copy(m_compressedPath, newCells, index);
                NextCells = m_compressedPath.Except(newCells).ToList();
                m_compressedPath = newCells;
                return NextCells;
            }
            return null;
        }

        public static Path GetEmptyPath(MapTemplate map, CellMap startCell)
        {
            return new Path(map, new List<CellMap>
            {
                startCell
            });
        }
    }
}
