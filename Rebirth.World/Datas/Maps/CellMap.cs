using Rebirth.Common.GameData.Maps;
using Rebirth.Common.Protocol.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rebirth.World.Datas.Maps
{
    public class CellMap : CellData
    {
        public int Id;
        public MapPoint Point;
        public DirectionsEnum Direction;
        public CellMap(CellData cell, int id, DirectionsEnum dir = DirectionsEnum.DIRECTION_EAST)
            : base(cell)
        {
            Id = id;
            Point = new MapPoint((short)Id);
            Direction = dir;
        }

        public short GetCellAfterChangeMap(MapNeighbour mapneighbour)
        {
            short result;
            switch (mapneighbour)
            {
                case MapNeighbour.Right:
                    result = Convert.ToInt16(Id - 13);
                    break;
                case MapNeighbour.Top:
                    result = Convert.ToInt16(Id + 532);
                    break;
                case MapNeighbour.Left:
                    result = Convert.ToInt16(Id + 13);
                    break;
                case MapNeighbour.Bottom:
                    result = Convert.ToInt16(Id - 532);
                    break;
                default:
                    result = 0;
                    break;
            }
            return result;
        }
    }
}
