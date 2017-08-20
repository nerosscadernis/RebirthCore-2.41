using PetaPoco.NetCore;
using Rebirth.Common.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rebirth.World.Datas.Monsters
{
    [TableName("monster_dungeons")]
    public class MonsterDungeon
    {
        public int MapID
        {
            get;
            set;
        }

        public string MonstersCSV
        {
            get;
            set;
        }

        public List<uint> Monsters
        {
            get
            {
                return MonstersCSV.FromCSV<uint>(",").ToList();
            }
        }
    }
}
