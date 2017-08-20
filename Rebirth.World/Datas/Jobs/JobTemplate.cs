using Rebirth.Common.GameData.D2O;
using Rebirth.Common.Protocol.Data;
using Rebirth.Common.Protocol.Types;
using Rebirth.World.Managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rebirth.World.Datas.Jobs
{
    public class JobTemplate
    {
        #region Vars
        public event EventHandler<UpLevelEventArgs> UpJobLevel;
        public event EventHandler<UpExperienceEventArgs> UpExperienceLevel;
        #endregion

        #region Properties
        public uint Id { get; set; }
        private long m_experience;
        public long Experience
        {
            get
            {
                return m_experience;
            }
            set
            {
                m_experience = value;
                if (m_experience > 0)
                {
                    OnExperienceChanged();
                    if ((value >= UpperBoundExperience && Level < ExperienceManager.Instance.HighestCharacterLevel) || value < LowerBoundExperience)
                    {
                        byte level = Level;
                        Level = ExperienceManager.Instance.GetJobLevel(Experience);
                        LowerBoundExperience = ExperienceManager.Instance.GetJobLevelExperience(Level);
                        UpperBoundExperience = ExperienceManager.Instance.GetJobNextLevelExperience(Level);
                        int difference = (int)(Level - level);
                        OnLevelChanged();
                    }
                }
            }
        }
        public double LowerBoundExperience
        {
            get;
            private set;
        }
        public double UpperBoundExperience
        {
            get;
            private set;
        }
        public byte Level { get; set; }
        public Skill[] Skills
        {
            get
            {
                return ObjectDataManager.GetAll<Skill>(x => x.parentJobId == Id && x.levelMin <= Level).ToArray();
            }
        }
        #endregion

        #region  Constructors
        public JobTemplate(uint Id, long Experience)
        {
            this.Id = Id;
            this.Level = ExperienceManager.Instance.GetJobLevel(Experience);
            this.Experience = Experience;
        }
        #endregion

        #region Methods
        private void OnLevelChanged()
        {
            var @event = new UpLevelEventArgs(this);
            if (UpJobLevel != null)
                UpJobLevel(this, @event);
        }

        private void OnExperienceChanged()
        {
            var @event = new UpExperienceEventArgs(this);
            if (UpExperienceLevel != null)
                UpExperienceLevel(this, @event);
        }

        public JobDescription GetJobDescription()
        {
            List<SkillActionDescription> list = new List<SkillActionDescription>();
            foreach (var skill in Skills)
            {
                if (skill.gatheredRessourceItem != -1)
                    list.Add(new SkillActionDescriptionCollect((uint)skill.id, 30, Level == 200 ? (uint)7 : 1, skill.levelMin == 200 ? 1 : (uint)(7 + ((Level - skill.levelMin) / 10))));
                else
                    list.Add(new SkillActionDescriptionCraft((uint)skill.id, 100));
            }
            return new JobDescription((sbyte)Id, list.ToArray());
        }
        public JobExperience GetJobExperience()
        {
            return new JobExperience((sbyte)Id, Level, Experience, ExperienceManager.Instance.GetJobLevelExperience(Level), ExperienceManager.Instance.GetJobNextLevelExperience(Level));
        }
        public JobCrafterDirectorySettings GetJobCrafterDirectorySettings()
        {
            return new JobCrafterDirectorySettings((sbyte)Id, 0, false);
        }
        #endregion

        #region Events
        public class UpLevelEventArgs : EventArgs
        {
            public JobTemplate Data { get; private set; }

            public UpLevelEventArgs(JobTemplate data)
            {
                Data = data;
            }
        }
        public class UpExperienceEventArgs : EventArgs
        {
            public JobTemplate Data { get; private set; }

            public UpExperienceEventArgs(JobTemplate data)
            {
                Data = data;
            }
        }
        #endregion
    }
}
