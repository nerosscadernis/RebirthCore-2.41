using Rebirth.Common.Extensions;
using Rebirth.Common.GameData.D2O;
using Rebirth.Common.Protocol.Data;
using Rebirth.World.Datas.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rebirth.World.Managers
{
    public class MonsterManager : DataManager<MonsterManager>
    {
        private List<MonsterSpawn> _monsterSpawn;

        public void Init()
        {
            _monsterSpawn = Starter.Database.Fetch<MonsterSpawn>("SELECT * FROM monster_spawn");
        }

        public MonsterSpawn GetMonsterSpawn(int mapId)
        {
            return _monsterSpawn.FirstOrDefault(x => x.MapId == mapId);
        }

        public bool HasMonsterSpawn(int mapId)
        {
            return _monsterSpawn.Any(x => x.MapId == mapId);
        }

        public List<Monster> GetMonsterByFavoriteSubarea(int subarea)
        {
            int[] ids = new int[] { 374, 375, 377, 2793, 649, 2882, 3588, 3589, 3590, 3591, 3592, 494 };
            var test = ObjectDataManager.GetAll<Monster>(e => e.subareas.Contains((uint)subarea));
            return test.Where(e => !e.isBoss && !e.isMiniBoss && !e.isQuestMonster && !ids.Contains(e.id)).ToList();
        }

    }
}
