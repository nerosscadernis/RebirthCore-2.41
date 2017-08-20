using PetaPoco.NetCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rebirth.World.Datas.Monsters
{
    [TableName("monster_spawn")]
    public class MonsterSpawn
    {
        public int Id { get; set; }
        public int MapId { get; set; }
        public string Datas { get; set; }

        public List<Tuple<int, byte, short>> Monsters
        {
            get
            {
                var list = new List<Tuple<int, byte, short>>();
                var creatures = Datas.Split('|');
                foreach (var item in creatures)
                {
                    var monster = item.Split(',');
                    list.Add(new Tuple<int, byte, short>(Convert.ToInt32(monster[0]), Convert.ToByte(monster[1]), Convert.ToInt16(monster[2])));
                }
                return list;
            }
        }
    }
}
