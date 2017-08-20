using Rebirth.Common.GameData.D2O;
using Rebirth.Common.IO;
using Rebirth.Common.Protocol.Data;
using Rebirth.Common.Protocol.Messages;
using Rebirth.Common.Protocol.Types;
using Rebirth.World.Datas.Characters;
using Rebirth.World.Datas.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Rebirth.World.Datas.Jobs.JobTemplate;

namespace Rebirth.World.Datas.Jobs
{
    public class JobInfos
    {
        #region Vars
        private List<JobTemplate> Jobs = new List<JobTemplate>();
        private Character _owner;
        #endregion

        #region Properties
        public event Action<object, UpLevelEventArgs> LevelUp;
        public event Action<object, UpExperienceEventArgs> ExperienceUp;
        #endregion

        #region Constructor
        public JobInfos(Character character)
        {
            _owner = character;
            Init();
        }
        public JobInfos(Character character, byte[] datas)
        {
            _owner = character;
            var reader = new BigEndianReader(datas);
            var count = reader.ReadShort();
            if (count > 0)
                for (int i = 0; i < count; i++)
                {
                    var job = new JobTemplate(reader.ReadUInt(), reader.ReadLong());
                    job.UpExperienceLevel += OnExperienceUp;
                    job.UpJobLevel += OnLevelUp;
                    Jobs.Add(job);
                }
            else
                Init();
        }

        private void Init()
        {
            var newJob = new JobTemplate(28, 0);
            newJob.UpExperienceLevel += OnExperienceUp;
            newJob.UpJobLevel += OnLevelUp;
            Jobs.Add(newJob);

            newJob = new JobTemplate(36, 0);
            newJob.UpExperienceLevel += OnExperienceUp;
            newJob.UpJobLevel += OnLevelUp;
            Jobs.Add(newJob);

            newJob = new JobTemplate(11, 0);
            newJob.UpExperienceLevel += OnExperienceUp;
            newJob.UpJobLevel += OnLevelUp;
            Jobs.Add(newJob);

            newJob = new JobTemplate(65, 0);
            newJob.UpExperienceLevel += OnExperienceUp;
            newJob.UpJobLevel += OnLevelUp;
            Jobs.Add(newJob);

            newJob = new JobTemplate(15, 0);
            newJob.UpExperienceLevel += OnExperienceUp;
            newJob.UpJobLevel += OnLevelUp;
            Jobs.Add(newJob);

            newJob = new JobTemplate(26, 0);
            newJob.UpExperienceLevel += OnExperienceUp;
            newJob.UpJobLevel += OnLevelUp;
            Jobs.Add(newJob);

            newJob = new JobTemplate(62, 0);
            newJob.UpExperienceLevel += OnExperienceUp;
            newJob.UpJobLevel += OnLevelUp;
            Jobs.Add(newJob);

            newJob = new JobTemplate(48, 0);
            newJob.UpExperienceLevel += OnExperienceUp;
            newJob.UpJobLevel += OnLevelUp;
            Jobs.Add(newJob);

            newJob = new JobTemplate(41, 0);
            newJob.UpExperienceLevel += OnExperienceUp;
            newJob.UpJobLevel += OnLevelUp;
            Jobs.Add(newJob);

            newJob = new JobTemplate(16, 0);
            newJob.UpExperienceLevel += OnExperienceUp;
            newJob.UpJobLevel += OnLevelUp;
            Jobs.Add(newJob);

            newJob = new JobTemplate(27, 0);
            newJob.UpExperienceLevel += OnExperienceUp;
            newJob.UpJobLevel += OnLevelUp;
            Jobs.Add(newJob);

            newJob = new JobTemplate(2, 0);
            newJob.UpExperienceLevel += OnExperienceUp;
            newJob.UpJobLevel += OnLevelUp;
            Jobs.Add(newJob);

            newJob = new JobTemplate(63, 0);
            newJob.UpExperienceLevel += OnExperienceUp;
            newJob.UpJobLevel += OnLevelUp;
            Jobs.Add(newJob);

            newJob = new JobTemplate(13, 0);
            newJob.UpExperienceLevel += OnExperienceUp;
            newJob.UpJobLevel += OnLevelUp;
            Jobs.Add(newJob);

            newJob = new JobTemplate(24, 0);
            newJob.UpExperienceLevel += OnExperienceUp;
            newJob.UpJobLevel += OnLevelUp;
            Jobs.Add(newJob);

            newJob = new JobTemplate(60, 0);
            newJob.UpExperienceLevel += OnExperienceUp;
            newJob.UpJobLevel += OnLevelUp;
            Jobs.Add(newJob);

            newJob = new JobTemplate(44, 0);
            newJob.UpExperienceLevel += OnExperienceUp;
            newJob.UpJobLevel += OnLevelUp;
            Jobs.Add(newJob);

            newJob = new JobTemplate(64, 0);
            newJob.UpExperienceLevel += OnExperienceUp;
            newJob.UpJobLevel += OnLevelUp;
            Jobs.Add(newJob);
        }
        #endregion

        #region Methods
        public JobTemplate GetJob(int skillId)
        {
            return Jobs.FirstOrDefault(x => x.Skills.FirstOrDefault(y => y.id == skillId) != null);
        }
        public JobDescription[] GetJobDescription()
        {
            return (from x in Jobs
                    select x.GetJobDescription()).ToArray();
        }
        public JobExperience[] GetJobExperience()
        {
            return (from x in Jobs
                    select x.GetJobExperience()).ToArray();
        }
        public JobCrafterDirectorySettings[] GetJobCrafterDirectorySettings()
        {
            return (from x in Jobs
                    select x.GetJobCrafterDirectorySettings()).ToArray();
        }
        public bool ContainsSkill(uint skillId)
        {
            return Jobs.FirstOrDefault(x => x.Skills.FirstOrDefault(y => y.id == skillId) != null) != null;
        }

        public byte[] GetDatas()
        {
            var writer = new BigEndianWriter();
            writer.WriteShort((short)Jobs.Count);
            foreach (var item in Jobs)
            {
                writer.WriteUInt(item.Id);
                writer.WriteLong(item.Experience);
            }
            return writer.Data;
        }
        #endregion

        public void Recolte(uint skillId, short age)
        {
            JobTemplate job = Jobs.FirstOrDefault(x => x.Skills.FirstOrDefault(y => y.id == skillId) != null);
            if (job != null)
            {
                Skill skill = job.Skills.FirstOrDefault(x => x.id == skillId);

                Common.Protocol.Data.Item item = ObjectDataManager.Get<Common.Protocol.Data.Item>(skill.gatheredRessourceItem);
                if (item != null)
                {
                    int quantity = new Random().Next(job.Level == 200 ? 7 : 1, skill.levelMin == 200 ? 1 : (int)(7 + ((job.Level - skill.levelMin) / 10)));
                    _owner.Inventory.AddItem(new PlayerItem(_owner, item.id, quantity));
                    if (age > 0)
                    {
                        int quantityBonus = quantity * age / 100;
                        if (quantityBonus > 0)
                        {
                            _owner.Inventory.AddItem(new PlayerItem(_owner, item.id, quantityBonus));
                            _owner.Client.Send(new ObtainedItemWithBonusMessage((uint)item.id, (uint)quantity, (uint)quantityBonus));
                        }
                        else
                        {
                            _owner.Client.Send(new ObtainedItemMessage((uint)item.id, (uint)quantity));
                        }
                    }
                    else
                    {
                        _owner.Client.Send(new ObtainedItemMessage((uint)item.id, (uint)quantity));
                    }
                }

                int baseExp = (int)((5 * 10) + skill.levelMin);
                job.Experience += baseExp + (baseExp * age / 100);
                //Owner.Client.Send(new JobExperienceUpdateMessage(job.GetJobExperience()));
            }
        }

        #region Events
        public void OnLevelUp(object sender, JobTemplate.UpLevelEventArgs e)
        {
            LevelUp?.Invoke(sender, e);
        }
        public void OnExperienceUp(object sender, JobTemplate.UpExperienceEventArgs e)
        {
            ExperienceUp?.Invoke(sender, e);
        }
        #endregion
    }
}
