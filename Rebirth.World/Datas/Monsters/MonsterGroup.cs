using Rebirth.Common.Extensions;
using Rebirth.Common.Protocol.Types;
using Rebirth.World.Datas.Items;
using Rebirth.World.Datas.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rebirth.World.Datas.Monsters
{
    public class MonsterGroup
    {
        public const short ClientStarsBonusLimit = 200;
        public static int StarsBonusInterval = 300;
        public static short StarsBonusIncrementation = 2;
        public static short StarsBonusLimit = 300;
        private readonly List<MonsterTemplate> m_monsters = new List<MonsterTemplate>();
        public List<PlayerItem> m_items = new List<PlayerItem>();

        #region Properties
        public int ContextualId
        {
            get;
            private set;
        }
        public int Kamas
        {
            get;
            set;
        }
        public bool WithAlternative
        {
            get
            {
                return Dungeon != null ? true : false;
            }
        }
        public int CellId
        {
            get;
            set;
        }
        public int Direction
        {
            get; set;
        }
        public MonsterTemplate Leader
        {
            get
            {
                if (WithAlternative)
                    return m_monsters.Count > 0 ? m_monsters.Any(x => x.Template.isBoss) ?
                        m_monsters.First(x => x.Template.isBoss) : m_monsters[0] : null;
                return m_monsters.Count > 0 ? m_monsters[0] : null;
            }
        }
        public List<MonsterTemplate> Monster
        {
            get
            {
                return m_monsters;
            }
        }
        public System.DateTime CreationDate
        {
            get;
            private set;
        }
        public MapTemplate Map
        {
            get;
            private set;
        }
        public MonsterDungeon Dungeon
        {
            get;
            private set;
        }
        public bool HaveItem
        { get { return m_items.Count > 0; } }
        public short Age
        {
            get
            {
                return (short)Math.Min(200d, (DateTime.Now.DateTimeToUnixTimestamp() - CreationDate.DateTimeToUnixTimestamp()) / 60000);
            }
        }
        #endregion

        #region Contructor
        public MonsterGroup(int id, bool IsStarted)
        {
            ContextualId = id;
            CreationDate = IsStarted ? System.DateTime.Now.AddMilliseconds(Starter.TimeStarted) : DateTime.Now;
            Dungeon = null;
        }
        public MonsterGroup(int id, MonsterDungeon dungeon, bool IsStarted)
        {
            ContextualId = id;
            CreationDate = IsStarted ? System.DateTime.Now.AddMilliseconds(Starter.TimeStarted) : DateTime.Now;
            Dungeon = dungeon;
        }
        #endregion

        #region Public Functions
        public void AddMonster(MonsterTemplate monster, bool isAlternative = false)
        {
            m_monsters.Add(monster);
        }
        public void RemoveMonster(MonsterTemplate monster)
        {
            m_monsters.Remove(monster);
        }

        public GameRolePlayGroupMonsterInformations GetGameRolePlayGroupMonsterInformations()
        {
            List<MonsterInGroupInformations> groupsInformation = new List<MonsterInGroupInformations>();

            if (WithAlternative)
            {
                MonsterInGroupInformations[] underlineInformation = (from x in m_monsters
                                                                     where x != Leader
                                                                     select new MonsterInGroupInformations(x.Template.id, (sbyte)x.GradeID, x.Look)).ToArray();
                List<AlternativeMonstersInGroupLightInformations> alernativesInfos = new List<AlternativeMonstersInGroupLightInformations>();
                List<MonsterInGroupLightInformations> alterlist = new List<MonsterInGroupLightInformations>();
                for (int i = 0; i < 4; i++)
                {
                    var monst = m_monsters[i];
                    if (monst != null)
                        alterlist.Add(new MonsterInGroupLightInformations(monst.Template.id, (sbyte)monst.GradeID));
                }
                alernativesInfos.Add(new AlternativeMonstersInGroupLightInformations(1, alterlist.ToArray()));
                for (int i = 5; i < 9; i++)
                {
                    alterlist.Clear();
                    for (int z = 0; z < i; z++)
                    {
                        var monst = m_monsters[z];
                        if (monst != null)
                            alterlist.Add(new MonsterInGroupLightInformations(monst.Template.id, (sbyte)monst.GradeID));
                    }
                    alernativesInfos.Add(new AlternativeMonstersInGroupLightInformations(i, alterlist.ToArray()));
                }
                var staticGroupAlter = new GroupMonsterStaticInformationsWithAlternatives(new MonsterInGroupLightInformations(Leader.Template.id, (sbyte)Leader.GradeID),
                    underlineInformation, alernativesInfos.ToArray());
                return new GameRolePlayGroupMonsterInformations(ContextualId, Leader.Look, new EntityDispositionInformations((short)CellId, (byte)Direction), false, HaveItem, false, staticGroupAlter,
                    CreationDate.DateTimeToUnixTimestamp(), 60000, -1, -1);
            }
            else
            {
                List<MonsterTemplate> monsters = this.Monster.Where(e => e != this.Leader).ToList();
                foreach (var monster in monsters)
                {
                    groupsInformation.Add(new MonsterInGroupInformations(monster.Template.id, (sbyte)monster.GradeID, monster.Look));
                }
            }



            // var xd = new GroupMonsterStaticInformationsWithAlternatives(new MonsterInGroupLightInformations(this.Leader.Template.id, (sbyte)this.Leader.GradeID), groupsInformation.ToArray(), alternativesInformation.ToArray());
            var staticGroup = new GroupMonsterStaticInformations(new MonsterInGroupLightInformations(Leader.Template.id, (sbyte)Leader.GradeID), groupsInformation.ToArray());
            return new GameRolePlayGroupMonsterInformations(ContextualId, Leader.Look, new EntityDispositionInformations((short)CellId, (byte)Direction), false, HaveItem, false, staticGroup,
                CreationDate.DateTimeToUnixTimestamp(), 60000, -1, -1);
        }

        public void AddItem(PlayerItem item)
        {
            m_items.Add(item);
        }
        #endregion
    }
}
