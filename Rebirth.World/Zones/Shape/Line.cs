using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Datas.Maps;

namespace Rebirth.World.Game.Zones.Shape
{
    public class Line : IShape
    {
        public uint Surface
        {
            get
            {
                return (uint)(this.Radius + 1);
            }
        }
        public byte MinRadius
        {
            get;
            set;
        }
        public DirectionsEnum Direction
        {
            get;
            set;
        }
        public bool IsInvers
        {
            get;
            set;
        }
        public byte Radius
        {
            get;
            set;
        }
        public bool StopAtTarget
        {
            get;
            set;
        }

        public bool FromCaster
        {
            get;
            set;
        }

        public int CasterCellId
        {
            get;
            set;
        }

        public Line(byte radius)
        {
            this.Radius = radius;
            this.Direction = DirectionsEnum.DIRECTION_SOUTH_EAST;
        }
        public CellMap[] GetCells(CellMap centerCell, MapTemplate map)
        {
            //if (this.FromCaster && this.StopAtTarget)
            //{
            //    distance = origin.distanceToCell(MapPoint.fromCellId(cellId));
            //    length = distance < length ? distance : length;
            //}
            if (FromCaster)
            {
                MinRadius = 1;
                Radius -= 1;
            }
            MapPoint mapPoint = !FromCaster ? new MapPoint(centerCell) : new MapPoint((short)CasterCellId);
            System.Collections.Generic.List<CellMap> list = new System.Collections.Generic.List<CellMap>();
            for (int i = (int)this.MinRadius; i <= (int)this.Radius; i++)
            {
                switch (this.Direction)
                {
                    case DirectionsEnum.DIRECTION_EAST:
                        Line.AddCellIfValid(mapPoint.X + i, mapPoint.Y + i, map, list);
                        break;
                    case DirectionsEnum.DIRECTION_SOUTH_EAST:
                        Line.AddCellIfValid(mapPoint.X + i, mapPoint.Y, map, list);
                        break;
                    case DirectionsEnum.DIRECTION_SOUTH:
                        Line.AddCellIfValid(mapPoint.X + i, mapPoint.Y - i, map, list);
                        break;
                    case DirectionsEnum.DIRECTION_SOUTH_WEST:
                        Line.AddCellIfValid(mapPoint.X, mapPoint.Y - i, map, list);
                        break;
                    case DirectionsEnum.DIRECTION_WEST:
                        Line.AddCellIfValid(mapPoint.X - i, mapPoint.Y - i, map, list);
                        break;
                    case DirectionsEnum.DIRECTION_NORTH_WEST:
                        Line.AddCellIfValid(mapPoint.X - i, mapPoint.Y, map, list);
                        break;
                    case DirectionsEnum.DIRECTION_NORTH:
                        Line.AddCellIfValid(mapPoint.X - i, mapPoint.Y + i, map, list);
                        break;
                    case DirectionsEnum.DIRECTION_NORTH_EAST:
                        Line.AddCellIfValid(mapPoint.X, mapPoint.Y + i, map, list);
                        break;
                }
            }
            return list.ToArray();
        }
        private static void AddCellIfValid(int x, int y, MapTemplate map, System.Collections.Generic.IList<CellMap> container)
        {
            if (MapPoint.IsInMap(x, y))
            {
                container.Add(map.Cells[(int)((System.UIntPtr)MapPoint.CoordToCellId(x, y))]);
            }
        }
    }
}
