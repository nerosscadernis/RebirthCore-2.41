using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Datas.Maps;

namespace Rebirth.World.Game.Zones.Shape
{
    public class Single : IShape
    {
        public uint Surface
        {
            get
            {
                return 1u;
            }
        }
        public byte MinRadius
        {
            get
            {
                return 1;
            }
            set
            {
            }
        }
        public DirectionsEnum Direction
        {
            get
            {
                return DirectionsEnum.DIRECTION_NORTH;
            }
            set
            {
            }
        }
        public byte Radius
        {
            get
            {
                return 1;
            }
            set
            {
            }
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

        public CellMap[] GetCells(CellMap centerCell, MapTemplate map)
        {
            return new CellMap[]
            {
                centerCell
            };
        }
    }
}
