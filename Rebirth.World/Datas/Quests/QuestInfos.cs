using Rebirth.Common.GameData.D2O;
using Rebirth.Common.IO;
using Rebirth.Common.Protocol.Data;
using Rebirth.Common.Protocol.Messages;
using Rebirth.Common.Protocol.Types;
using Rebirth.World.Datas.Characters;
using Rebirth.World.Managers;
using Rebirth.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rebirth.World.Datas.Quests
{
    public class QuestInfos
    {
        #region Vars
        private Character owner;
        #endregion

        #region Properties
        public List<QuestTemplate> Quests { get; set; }
        public List<QuestTemplate> FinishedQuest { get { return Quests.FindAll(x => x.IsFinish); } }
        #endregion

        #region Constructor
        public QuestInfos(Character character)
        {
            Quests = new List<QuestTemplate>();
            owner = character;
        }
        public QuestInfos(Character character, byte[] datas)
        {
            owner = character;
            Quests = new List<QuestTemplate>();

            var reader = new BigEndianReader(datas);
            var count = reader.ReadInt();
            for (int i = 0; i < count; i++)
            {
                Quests.Add(new QuestTemplate(reader));
            }
        }
        #endregion

        #region Public Methods
        public bool IsrepeatOrActivable(int questId)
        {
            return !Quests.Any(x => x.Id == questId && !x.IsFinish) && 
                ((FinishedQuest.Any(x => x.Id == questId) && FinishedQuest.First(x => x.Id == questId).IsRenitialisable) || !FinishedQuest.Any(x => x.Id == questId));
        }

        public bool HasNpcQuest(int npcId)
        {
            return Quests.Any(x => !x.IsFinish && x.ActualStep.Npcs.Contains(npcId));
        }

        public void ValidationObjective(uint questId, uint objectiveId)
        {
            var quest = Quests.FirstOrDefault(x => x.Id == questId);
            if (quest == null)
                return;

            quest.ValidationObjective((int)objectiveId);
            CheckQuest(questId);
        }

        public void CheckQuest(uint questId)
        {
            var quest = Quests.FirstOrDefault(x => x.Id == questId);
            if (quest == null)
                return;

            if (!quest.ActualStep.IsValid)
                owner.Client.Send(new QuestStepInfoMessage(quest.GetQuestActiveInformations()));
            else
            {
                AssignReward(quest.ActualStep);
                quest.ChangeStep();
                owner.Client.Send(owner.Map.GetMapNpcsQuestStatusUpdateMessage(owner));
                if (quest.IsFinish)
                {
                    quest.FinishedCount++;
                    owner.Client.Send(new QuestValidatedMessage(quest.Id));
                }
                else
                {
                    owner.Client.Send(new QuestStepValidatedMessage(quest.Id, quest.LastStep.Id));
                    owner.Client.Send(new QuestStepStartedMessage(quest.Id, quest.ActualStep.Id));
                }
            }
        }

        private const double REWARD_SCALE_CAP = 1.5;     
        private const double REWARD_REDUCED_SCALE = 0.7;

        public void AssignReward(StepTemplate step)
        {
            var rewardLevel = 0d;
            var fixeOptimalLevelExperienceReward = 0d;
            var fixeLevelExperienceReward = 0d;
            var reducedOptimalExperienceReward = 0d;
            var reducedExperienceReward = 0d;
            var sumExperienceRewards = 0d;
            var result = 0d;
            var xpBonus = 1d + 200d / 100d;
            if(owner.Infos.Level > step.OptimalLevel)
            {
                rewardLevel = Math.Min(owner.Infos.Level, step.OptimalLevel * REWARD_SCALE_CAP);
                fixeOptimalLevelExperienceReward = GetFixeExperienceReward((int)step.OptimalLevel, step.Duration, step.XpRatio);
                fixeLevelExperienceReward = GetFixeExperienceReward((int)rewardLevel, step.Duration, step.XpRatio);
                reducedOptimalExperienceReward = (1 - REWARD_REDUCED_SCALE) * fixeOptimalLevelExperienceReward;
                reducedExperienceReward = REWARD_REDUCED_SCALE * fixeLevelExperienceReward;
                sumExperienceRewards = Math.Floor(reducedOptimalExperienceReward + reducedExperienceReward);
                result = Math.Floor(sumExperienceRewards * xpBonus);
            }
            else
                result = Math.Floor(GetFixeExperienceReward(owner.Infos.Level, step.Duration, step.XpRatio) * xpBonus);
            owner.AddExperience(result);
            foreach (var reward in step.Rewards)
            {
                foreach (var item in reward.itemsReward)
                {
                    owner.Inventory.AddItem(new Items.PlayerItem(owner, (int)item[0], (int)item[1]));
                }
            }
        }

        private double GetFixeExperienceReward(int level, double duration, double xpRatio)
        {
            var levelPow = Math.Pow(100 + 2 * level, 2);
            var result = level * levelPow / 20 * duration * xpRatio;
            return result;
        }

        public void ValidParam(int paramId, int paramCount, int paramType)
        {
            foreach (var item in Quests.FindAll(x => !x.IsFinish))
            {
                item.ValidationParam(paramId, paramCount, paramType);
                CheckQuest(item.Id);
            }
        }

        public void AddQuest(uint id)
        {
            if (id == 492)
                ValidationObjective(489, 3512);
            if (!Quests.Any(x => x.Id == id))
            {
                Quests.Add(new QuestTemplate(ObjectDataManager.Get<Quest>(id)));
                owner.Client?.Send(new QuestStartedMessage(id));
                owner.Client?.Send(owner.Map.GetMapNpcsQuestStatusUpdateMessage(owner));
            }
        }

        public QuestActiveInformations GetQuestStepInfos(uint questId)
        {
            var step = Quests.FirstOrDefault(x => x.Id == questId);
            if (step != null)
                return step.GetQuestActiveInformations();
            else
                return new QuestActiveInformations(questId);
        }

        public List<QuestTemplate> GetQuestsWithNpc(int npcId)
        {
            return Quests.FindAll(x => !x.IsFinish && x.ActualStep.Npcs.Contains(npcId));
        }

        public uint[] GetFinishedId()
        {
            return (from x in FinishedQuest
                    select x.Id).ToArray();
        }

        public uint[] GetReinitialisableQuest()
        {
            return (from x in FinishedQuest
                    where x.IsRenitialisable
                    select x.Id).ToArray();
        }

        public uint[] GetFinishedCount()
        {
            return (from x in FinishedQuest
                    select x.FinishedCount).ToArray();
        }

        public QuestActiveInformations[] GetActiveQuests()
        {
            return (from x in Quests
                    where !x.IsFinish
                    select x.GetQuestActiveInformations()).ToArray();
        }

        public byte[] GetDatas()
        {
            var writer = new BigEndianWriter();
            writer.WriteInt(Quests.Count);
            foreach (var quest in Quests)
            {
                writer.WriteBytes(quest.GetDatas());
            }
            return writer.Data;
        }
        #endregion

        #region Private Methods

        #endregion
    }
}
