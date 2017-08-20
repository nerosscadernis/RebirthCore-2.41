using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Datas.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Fights.Other
{
    public class FightDisposition
    {
        public FightDisposition(int CellId, DirectionsEnum Dir)
        {
            this.Dir = Dir;
            this.Point = new MapPoint((short)CellId);
        }
        public MapPoint Point
        {
            get;
            set;
        }
        public DirectionsEnum Dir
        {
            get;
            set;
        }
    }
}
