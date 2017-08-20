using Rebirth.Common.Extensions;
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
    public class StepTemplate
    {
        #region Vars
        private double _startedTime;
        private int _diaglog;
        private List<ObjectiveTemplate> _objectives = new List<ObjectiveTemplate>();
        #endregion

        #region Properties
        public uint Id { get; set; }
        public bool KamasScale { get; set; }
        public float KamasRatio { get; set; }
        public float XpRatio { get; set; }
        public uint OptimalLevel { get; set; }
        public float Duration { get; set; }
        public bool IsValid { get { return _objectives.All(x => x.IsFinish); } }
        public bool HasNpc { get { return GetActiveObjectifs().Any(x => x is ObjectiveGoToNpc); } }
        public int[] Npcs { get { return GetActiveObjectifs().FindAll(x => x is ObjectiveGoToNpc && !x.IsFinish).Select(x => x.Parameters.parameter0).ToArray(); } }
        public QuestStepRewards[] Rewards { get { return ObjectDataManager.Get<QuestStep>(Id).rewardsIds.Select(x => ObjectDataManager.Get<QuestStepRewards>(x)).ToArray(); } }
        #endregion

        #region Constructor / Datas
        public StepTemplate(uint id)
        {
            var step = ObjectDataManager.Get<QuestStep>(id);
            Id = step.id;

            _startedTime = DateTime.Now.DateTimeToUnixTimestamp();
            _diaglog = step.dialogId;
            OptimalLevel = step.optimalLevel;
            Duration = step.duration;
            KamasRatio = step.kamasRatio;
            KamasScale = step.kamasScaleWithPlayerLevel;
            XpRatio = step.xpRatio;
            foreach (var item in step.objectiveIds)
            {
                var test = ObjectDataManager.Get<QuestObjective>(item);
                _objectives.Add((ObjectiveTemplate)test);
            }
        }

        public StepTemplate(IDataReader reader)
        {
            Id = reader.ReadUInt();
            _startedTime = reader.ReadDouble();
            _diaglog = reader.ReadInt();
            OptimalLevel = reader.ReadUInt();
            Duration = reader.ReadFloat();
            KamasRatio = reader.ReadFloat();
            KamasScale = reader.ReadBoolean();
            XpRatio = reader.ReadFloat();
            var count = reader.ReadInt();
            for (int i = 0; i < count; i++)
            {
                var type = reader.ReadUInt();
                switch (type)
                {
                    case 0:
                        _objectives.Add(new ObjectiveFreeForm(reader) { Type = type });
                        break;
                    case 1:
                    case 9:
                        _objectives.Add(new ObjectiveGoToNpc(reader) { Type = type });
                        break;
                    case 4:
                        _objectives.Add(new ObjectiveDiscoverMap(reader) { Type = type });
                        break;
                    case 6:
                        _objectives.Add(new ObjectiveFightMonster(reader) { Type = type });
                        break;
                    case 17:
                        _objectives.Add(new ObjectiveCraftItem(reader) { Type = type });
                        break;
                    default:
                        _objectives.Add(new ObjectiveTemplate(reader) { Type = type });
                        break;
                }
            }
        }

        public byte[] GetDatas()
        {
            var writer = new BigEndianWriter();
            writer.WriteUInt(Id);
            writer.WriteDouble(_startedTime);
            writer.WriteInt(_diaglog);
            writer.WriteUInt(OptimalLevel);
            writer.WriteFloat(Duration);
            writer.WriteFloat(KamasRatio);
            writer.WriteBoolean(KamasScale);
            writer.WriteFloat(XpRatio);
            writer.WriteInt(_objectives.Count);
            foreach (var item in _objectives)
            {
                writer.WriteBytes(item.GetDatas());
            }
            return writer.Data;
        }
        #endregion

        #region Methods
        public List<ObjectiveTemplate> GetActiveObjectifs()
        {
            return _objectives.FindAll(x => x.LastIds == null || x.LastIds.All(y => _objectives.Any(w => w.IsFinish && w.Id == y)));
        }

        public QuestObjectiveInformations[] GetQuestObjectiveInformations()
        {
            return GetActiveObjectifs().Select(x => new QuestObjectiveInformations(x.Id, !x.IsFinish, x.Parameters.GetParams())).ToArray(); ;
        }

        public void ValidParams(int paramId, int paramCount, int paramType)
        {
            foreach (var item in GetActiveObjectifs().FindAll(x => x.Type == paramType))
            {
                item.ValidParam(paramId, paramCount);
            }
        }
        
        public void ValidObjective(int objectiveId)
        {
            var ob = GetActiveObjectifs().FirstOrDefault(x => x.Id == objectiveId);
            if (ob == null)
                return;
            ob.ValidObjective();
        }
        #endregion
    }
}
