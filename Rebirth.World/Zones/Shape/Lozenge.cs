using System;
using System.Collections.Generic;
using System.Linq;
using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Datas.Spells;
using Rebirth.World.Effects.Handlers;
using Rebirth.World.Effects.Handlers.Spells;
using Rebirth.World.Game.Effect.Instances;
using Rebirth.World.Game.Fights.Actors;

namespace Rebirth.World.Game.Zones.Shape
{
    public class Lozenge : IShape
    {
        public uint Surface
        {
            get
            {
                return (uint)((this.Radius + 1) * (this.Radius + 1) + this.Radius * this.Radius);
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

        public Lozenge(byte minRadius, byte radius)
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
                if (this.MinRadius == 0)
                {
                    list.Add(centerCell);
                }
                result = list.ToArray();
            }
            else
            {
                int i = mapPoint.X - (int)this.Radius;
                int num = 0;
                int num2 = 1;
                while (i <= mapPoint.X + (int)this.Radius)
                {
                    for (int j = -num; j <= num; j++)
                    {
                        if (this.MinRadius == 0 || System.Math.Abs(mapPoint.X - i) + System.Math.Abs(j) >= (int)this.MinRadius)
                        {
                            Lozenge.AddCellIfValid(i, j + mapPoint.Y, map, list);
                        }
                    }
                    if (num == (int)this.Radius)
                    {
                        num2 = -num2;
                    }
                    num += num2;
                    i++;
                }
                result = list.ToArray();
            }
            if (MinRadius == 1)
            {
                result.ToList().Remove(centerCell);
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
