using Rebirth.Common.GameData.D2O;
using Rebirth.Common.IO;
using Rebirth.Common.Protocol.Data;
using Rebirth.Common.Protocol.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rebirth.World.Datas.Quests
{
    public class QuestTemplate
    {
        #region Vars
        private List<StepTemplate> _steps = new List<StepTemplate>();
        private List<StepTemplate> _finishedSteps = new List<StepTemplate>();
        private uint _min;
        private uint _max;
        private uint _repeatType;
        private uint _repeatLimit;
        private uint _categorie;
        private bool _isDungeonQuest;
        private bool _isPartyQuest;
        private string _criterion;
        #endregion

        #region Properties
        public uint Id { get; private set; }
        public StepTemplate ActualStep { get; private set; }
        public uint FinishedCount { get; set; } 
        public bool IsFinish { get { return _steps.Count == 0; } }
        public StepTemplate LastStep { get { return _finishedSteps.LastOrDefault(); } }

        public bool IsRenitialisable
        {
            get
            {
                return _repeatLimit > 0 && _repeatLimit > FinishedCount;
            }
        }
        #endregion

        #region Constructor
        public QuestTemplate(Quest quest)
        {
            Id = quest.id;
            _steps = (from x in quest.stepIds
                      select new StepTemplate(x)).ToList();
            ActualStep = _steps.First();
            _min = quest.levelMin;
            _max = quest.levelMax;
            _repeatType = quest.repeatType;
            _repeatLimit = quest.repeatLimit;
            _categorie = quest.categoryId;
            _isDungeonQuest = quest.isDungeonQuest;
            _isPartyQuest = quest.isPartyQuest;
            _criterion = quest.startCriterion;
        }

        public QuestTemplate(IDataReader reader)
        {
            Id = reader.ReadUInt();
            FinishedCount = reader.ReadUInt();
            _min = reader.ReadUInt();
            _max = reader.ReadUInt();
            _repeatType = reader.ReadUInt();
            _repeatLimit = reader.ReadUInt();
            _categorie = reader.ReadUInt();
            _isDungeonQuest = reader.ReadBoolean();
            _isPartyQuest = reader.ReadBoolean();
            _criterion = reader.ReadUTF();
            var count = reader.ReadInt();
            for (int i = 0; i < count; i++)
            {
                _steps.Add(new StepTemplate(reader));
            }
            count = reader.ReadInt();
            for (int i = 0; i < count; i++)
            {
                _finishedSteps.Add(new StepTemplate(reader));
            }
            if(_steps.Count > 0)
                ActualStep = _steps.First();
        }
        #endregion

        #region Public Methods
        public void ValidationParam(int paramId, int paramCount, int paramType)
        {
            ActualStep?.ValidParams(paramId, paramCount, paramType);
        }

        public void ValidationObjective(int objectiveId)
        {
            ActualStep.ValidObjective(objectiveId);
        }

        public void ChangeStep()
        {
            _finishedSteps.Add(ActualStep);
            _steps.Remove(ActualStep);
            if(_steps.Count > 0)
                ActualStep = _steps.First();
        }

        public QuestActiveInformations GetQuestActiveInformations()
        {
            if (IsFinish)
                return new QuestActiveInformations(Id);
            return new QuestActiveDetailedInformations(Id, ActualStep.Id, ActualStep.GetQuestObjectiveInformations());
        }

        public byte[] GetDatas()
        {
            var writer = new BigEndianWriter();
            writer.WriteUInt(Id);
            writer.WriteUInt(FinishedCount);
            writer.WriteUInt(_min);
            writer.WriteUInt(_max);
            writer.WriteUInt(_repeatType);
            writer.WriteUInt(_repeatLimit);
            writer.WriteUInt(_categorie);
            writer.WriteBoolean(_isDungeonQuest);
            writer.WriteBoolean(_isPartyQuest);
            writer.WriteUTF(_criterion);
            writer.WriteInt(_steps.Count);
            foreach (var step in _steps)
            {
                writer.WriteBytes(step.GetDatas());
            }
            writer.WriteInt(_finishedSteps.Count);
            foreach (var step in _finishedSteps)
            {
                writer.WriteBytes(step.GetDatas());
            }
            return writer.Data;
        }
        #endregion
    }
}
