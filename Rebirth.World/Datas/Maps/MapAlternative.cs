using PetaPoco.NetCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rebirth.World.Datas.Maps
{
    [TableName("maps_alternative")]
    public class MapAlternative
    {
        public int Id { get; set; }
        public int Enter { get; set; }
        public byte Direction { get; set; }
        public int Exit { get; set; }
    }
}
