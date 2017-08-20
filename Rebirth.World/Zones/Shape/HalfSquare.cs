using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Datas.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Zones.Shape
{
    public class HalfSquare : IShape
    {
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

        public HalfSquare(byte minRadius, byte radius)
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
                list.Add(centerCell);
                result = list.ToArray();
            }
            else
            {
                mapPoint.X -= Radius; mapPoint.Y -= Radius;

                for (int i = 0; i < Radius * 2 + 1; i++)
                {
                    for (int j = 0; j < Radius * 2 + 1; j++)
                    {
                        HalfSquare.AddCellIfValid(mapPoint.X + i, mapPoint.Y + j, map, list);
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
