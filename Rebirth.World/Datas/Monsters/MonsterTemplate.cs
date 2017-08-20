using Rebirth.Common.Protocol.Data;
using Rebirth.Common.Protocol.Types;
using Rebirth.World.Datas.Characters;
using Rebirth.World.Game.Characters.Stats;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rebirth.World.Datas.Monsters
{
    public class MonsterTemplate : IStatsOwner
    {
        private StatsFields m_stats;
        private byte m_grade;

        public Monster Template { get; set; }
        public MonsterGrade Grade
        {
            get
            {
                return Template.grades[m_grade];
            }
        }
        public byte GradeID
        {
            get
            {
                return (byte)(m_grade + 1);
            }
        }
        public StatsFields Stats
        {
            get
            {
                return m_stats;
            }
        }

        public EntityLook Look
        {
            get { return m_look.GetEntityLook(); }
        }
        private Look m_look;
        public MonsterTemplate(byte grade, Monster template)
        {
            Template = template;
            m_grade = grade;
            m_stats = new StatsFields(this);
            m_stats.Initialize(Grade);
            m_look = Characters.Look.Parse(Template.look);
            if (string.IsNullOrEmpty(Template.look))
                Starter.Logger.Error(string.Join("No look for monster [Id<{0}>] !", Template.id));
        }
    }
}
