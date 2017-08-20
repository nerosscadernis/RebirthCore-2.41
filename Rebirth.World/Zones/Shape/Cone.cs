﻿using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Datas.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Zones.Shape
{
    public class Cone : IShape
    {
        public uint Surface
        {
            get
            {
                return (uint)((this.Radius + 1) * (this.Radius + 1));
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

        public Cone(byte minRadius, byte radius)
        {
            this.MinRadius = minRadius;
            this.Radius = radius;
            this.Direction = DirectionsEnum.DIRECTION_SOUTH_EAST;
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
                int num = 0;
                int num2 = 1;
                switch (this.Direction)
                {
                    case DirectionsEnum.DIRECTION_SOUTH_EAST:
                        for (int i = mapPoint.X; i <= mapPoint.X + (int)this.Radius; i++)
                        {
                            for (int j = -num; j <= num; j++)
                            {
                                if (this.MinRadius == 0 || System.Math.Abs(mapPoint.X - i) + System.Math.Abs(j) >= (int)this.MinRadius)
                                {
                                    Cone.AddCellIfValid(i, j + mapPoint.Y, map, list);
                                }
                            }
                            num += num2;
                        }
                        break;
                    case DirectionsEnum.DIRECTION_SOUTH_WEST:
                        for (int j = mapPoint.Y; j >= mapPoint.Y - (int)this.Radius; j--)
                        {
                            for (int i = -num; i <= num; i++)
                            {
                                if (this.MinRadius == 0 || System.Math.Abs(i) + System.Math.Abs(mapPoint.Y - j) >= (int)this.MinRadius)
                                {
                                    Cone.AddCellIfValid(i + mapPoint.X, j, map, list);
                                }
                            }
                            num += num2;
                        }
                        break;
                    case DirectionsEnum.DIRECTION_NORTH_WEST:
                        for (int i = mapPoint.X; i >= mapPoint.X - (int)this.Radius; i--)
                        {
                            for (int j = -num; j <= num; j++)
                            {
                                if (this.MinRadius == 0 || System.Math.Abs(mapPoint.X - i) + System.Math.Abs(j) >= (int)this.MinRadius)
                                {
                                    Cone.AddCellIfValid(i, j + mapPoint.Y, map, list);
                                }
                            }
                            num += num2;
                        }
                        break;
                    case DirectionsEnum.DIRECTION_NORTH_EAST:
                        for (int j = mapPoint.Y; j <= mapPoint.Y + (int)this.Radius; j++)
                        {
                            for (int i = -num; i <= num; i++)
                            {
                                if (this.MinRadius == 0 || System.Math.Abs(i) + System.Math.Abs(mapPoint.Y - j) >= (int)this.MinRadius)
                                {
                                    Cone.AddCellIfValid(i + mapPoint.X, j, map, list);
                                }
                            }
                            num += num2;
                        }
                        break;
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
