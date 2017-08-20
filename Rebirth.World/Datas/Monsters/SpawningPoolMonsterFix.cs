using Rebirth.Common.GameData.D2O;
using Rebirth.Common.Pool;
using Rebirth.Common.Protocol.Data;
using Rebirth.Common.Thread;
using Rebirth.Common.Timers;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rebirth.World.Datas.Monsters
{
    public class SpawningPoolMonsterFix : ISpawn
    {
        private MapTemplate _map;
        private int m_interval = 50;
        private MonsterSpawn _monsters;

        private readonly UniqueIdProvider m_contextualIds;

        public int MaxGroup = 3;

        public SpawningPoolMonsterFix(MapTemplate map)
        {
            _map = map;
            m_contextualIds = new UniqueIdProvider(50);
            new TimerCore(SpawnerTick, 3000);
            _monsters = MonsterManager.Instance.GetMonsterSpawn(_map.Id);
        }


        private void SpawnerTick()
        {
            if (_monsters == null) /// spawner automatique  interdit au zaap ou dans les maisons
                return;

            if (MaxGroup == _map.MonsterSpawn.Count)
            {
                new TimerCore(SpawnerTick, NextInterval());
                return;
            }

            foreach (var item in _monsters.Monsters)
            {
                if (_map.HasMonster(item.Item1, item.Item2))
                    continue;

                var group = new MonsterGroup(m_contextualIds.Pop() * -1);
                group.CellId = item.Item3;
                group.Direction = 1;

                group.AddMonster(new MonsterTemplate(item.Item2, ObjectDataManager.Get<Monster>(item.Item1)));
                _map.AddMonster(group);
            }
            new TimerCore(SpawnerTick, NextInterval());
        }


        private int NextInterval()
        {
            AsyncRandom asyncRandom = new AsyncRandom();
            int result;
            if (asyncRandom.Next(0, 2) == 0)
            {
                result = (int)(((double)m_interval - asyncRandom.NextDouble() * (double)m_interval / 4.0) * 1000.0);
            }
            else
            {
                result = (int)(((double)m_interval + asyncRandom.NextDouble() * (double)m_interval / 4.0) * 1000.0);
            }
            return result;
        }
    }
}
