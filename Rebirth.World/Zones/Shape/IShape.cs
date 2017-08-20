using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Datas.Maps;

namespace Rebirth.World.Game.Zones.Shape
{
    public interface IShape
    {
        uint Surface
        {
            get;
        }

        byte MinRadius
        {
            get;
            set;
        }

        bool FromCaster
        {
            get;
            set;
        }

        bool StopAtTarget
        {
            get;
            set;
        }

        int CasterCellId
        {
            get;
            set;
        }

        DirectionsEnum Direction
        {
            get;
            set;
        }

        byte Radius
        {
            get;
            set;
        }

        CellMap[] GetCells(CellMap centerCell, MapTemplate map);
    }
}
