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
    public class SpawningPoolMonster : ISpawn
    {
        private MapTemplate _map;
        private bool IsStarter = true;
        private int m_interval = 50;
        private List<Monster> _monsters;

        private readonly UniqueIdProvider m_contextualIds;

        public int MaxGroup = 3;

        public SpawningPoolMonster(MapTemplate map)
        {
            _map = map;
            m_contextualIds = new UniqueIdProvider(50);
            _monsters = MonsterManager.Instance.GetMonsterByFavoriteSubarea(_map.SubArea);
            new TimerCore(SpawnerTick, 3000);
        }


        private void SpawnerTick()
        {
            if (!_map.Coordinates.allowAggression || _monsters?.Count < 1) /// spawner automatique  interdit au zaap ou dans les maisons
                return;

            if (MaxGroup == _map.MonsterSpawn.Count)
            {
                new TimerCore(SpawnerTick, NextInterval());
                return;
            }

            AsyncRandom asyncRandom = new AsyncRandom();
            int count = MaxGroup - _map.MonsterSpawn.Count;
            List<MonsterGroup> newGroup = new List<MonsterGroup>();
            for (int i = 0; i < count; i++)
            {
                MonsterGroup group = new MonsterGroup(m_contextualIds.Pop() * -1, IsStarter);
                group.CellId = _map.GetCellFree().Id;
                group.Direction = 1;
                var monsterLenght = asyncRandom.Next(1, 8);
                for (int o = 0; o < monsterLenght; o++)
                {
                    int num = asyncRandom.Next(0, _monsters.Count);
                    var monster = _monsters[num];
                    var num1 = asyncRandom.Next(0, monster.grades.Count - 1);


                    var grade = monster.grades[num1];
                    if (monster != null)
                        group.AddMonster(new MonsterTemplate((byte)grade.grade, monster));
                    else
                        o--;
                }
                newGroup.Add(group);
            }

            foreach (var group in newGroup)
                _map.AddMonster(group);

            IsStarter = false;
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
