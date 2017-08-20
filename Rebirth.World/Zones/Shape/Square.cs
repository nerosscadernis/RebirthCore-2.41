using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Datas.Maps;

namespace Rebirth.World.Game.Zones.Shape
{
    public class Square : IShape
    {
        public bool DiagonalFree
        {
            get;
            set;
        }
        public uint Surface
        {
            get
            {
                return (uint)((this.Radius * 2 + 1) * (this.Radius * 2 + 1));
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

        public Square(byte minRadius, byte radius)
        {
            this.MinRadius = minRadius;
            this.Radius = radius;
        }
        public CellMap[] GetCells(CellMap centerCell, MapTemplate map)
        {
            MapPoint mapPoint = new MapPoint(centerCell);
            System.Collections.Generic.List<CellMap> list = new System.Collections.Generic.List<CellMap>();
            CellMap[] result;
            if (this.Radius == 0)
            {
                if (this.MinRadius == 0 && !this.DiagonalFree)
                {
                    list.Add(centerCell);
                }
                result = list.ToArray();
            }
            else
            {
                for (int i = mapPoint.X - (int)this.Radius; i <= mapPoint.X + (int)this.Radius; i++)
                {
                    for (int j = mapPoint.Y - (int)this.Radius; j <= mapPoint.Y - (int)this.Radius; j++)
                    {
                        if ((this.MinRadius == 0 || System.Math.Abs(mapPoint.X - i) + System.Math.Abs(mapPoint.Y - j) >= (int)this.MinRadius) && (!this.DiagonalFree || System.Math.Abs(mapPoint.X - i) != System.Math.Abs(mapPoint.Y - j)))
                        {
                            Square.AddCellIfValid(i, j, map, list);
                        }
                    }
                }
                result = list.ToArray();
            }
            return result;
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
