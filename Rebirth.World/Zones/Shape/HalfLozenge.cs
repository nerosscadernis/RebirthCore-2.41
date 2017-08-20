using System;
using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Datas.Maps;

namespace Rebirth.World.Game.Zones.Shape
{
    public class HalfLozenge : IShape
    {
        public uint Surface
        {
            get
            {
                return (uint)(this.Radius * 2 + 1);
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
        public byte Radius
        {
            get;
            set;
        }

        public bool FromCaster
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool StopAtTarget
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public int CasterCellId
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public HalfLozenge(byte minRadius, byte radius)
        {
            this.MinRadius = minRadius;
            this.Radius = radius;
            this.Direction = DirectionsEnum.DIRECTION_NORTH;
        }
        public CellMap[] GetCells(CellMap centerCell, MapTemplate map)
        {
            MapPoint mapPoint = new MapPoint(centerCell);
            System.Collections.Generic.List<CellMap> list = new System.Collections.Generic.List<CellMap>();
            if (this.MinRadius == 0)
            {
                list.Add(centerCell);
            }
            for (int i = 1; i <= (int)this.Radius; i++)
            {
                switch (this.Direction)
                {
                    case DirectionsEnum.DIRECTION_SOUTH_EAST:
                        HalfLozenge.AddCellIfValid(mapPoint.X - i, mapPoint.Y + i, map, list);
                        HalfLozenge.AddCellIfValid(mapPoint.X - i, mapPoint.Y - i, map, list);
                        break;
                    case DirectionsEnum.DIRECTION_SOUTH_WEST:
                        HalfLozenge.AddCellIfValid(mapPoint.X - i, mapPoint.Y + i, map, list);
                        HalfLozenge.AddCellIfValid(mapPoint.X + i, mapPoint.Y + i, map, list);
                        break;
                    case DirectionsEnum.DIRECTION_NORTH_WEST:
                        HalfLozenge.AddCellIfValid(mapPoint.X + i, mapPoint.Y + i, map, list);
                        HalfLozenge.AddCellIfValid(mapPoint.X + i, mapPoint.Y - i, map, list);
                        break;
                    case DirectionsEnum.DIRECTION_NORTH_EAST:
                        HalfLozenge.AddCellIfValid(mapPoint.X - i, mapPoint.Y - i, map, list);
                        HalfLozenge.AddCellIfValid(mapPoint.X + i, mapPoint.Y - i, map, list);
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
