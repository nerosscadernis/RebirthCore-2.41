using Rebirth.Common.GameData.D2O;
using Rebirth.Common.IO;
using Rebirth.Common.Protocol.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rebirth.World.Datas.Quests
{
    public class ObjectiveTemplate
    {
        #region Properties
        public uint Id { get; set; }
        public uint Type { get; set; }
        public int DialogId { get; set; }
        public Point Coords { get; set; }
        public int MapId { get; set; }
        public QuestObjectiveParameters Parameters { get; set; }
        public int[] LastIds { get; set; }

        protected bool _finish;
        public virtual bool IsFinish { get { return false; } }

        public event Action<ObjectiveTemplate> ParamValid;
        #endregion

        #region Constructors / Datas
        public ObjectiveTemplate()
        {
            LastIds = new int[0];
        }

        public ObjectiveTemplate(IDataReader reader)
        {
            LastIds = new int[0];
            Id = reader.ReadUInt();
            _finish = reader.ReadBoolean();
            if (reader.ReadBoolean())
                Coords = new Point(reader.ReadInt(), reader.ReadInt());
            MapId = reader.ReadInt();
            DialogId = reader.ReadInt();
            Parameters = new QuestObjectiveParameters()
            {
                dungeonOnly = reader.ReadBoolean(),
                numParams = reader.ReadUInt(),
                parameter0 = reader.ReadInt(),
                parameter1 = reader.ReadInt(),
                parameter2 = reader.ReadInt(),
                parameter3 = reader.ReadInt(),
                parameter4 = reader.ReadInt()
            };
            LastIds = Starter.Database.FirstOrDefault<string>("select LastId from objective_condition where Id='" + Id + "'")?.FromCSV<int>(";");
        }

        public virtual byte[] GetDatas()
        {
            var writer = new BigEndianWriter();
            writer.WriteUInt(Type);
            writer.WriteUInt(Id);
            writer.WriteBoolean(_finish);
            if (Coords != null)
            {
                writer.WriteBoolean(true);
                writer.WriteInt(Coords.x);
                writer.WriteInt(Coords.y);
            }
            else
                writer.WriteBoolean(false);
            writer.WriteInt(MapId);
            writer.WriteInt(DialogId);
            writer.WriteBoolean(Parameters.dungeonOnly);
            writer.WriteUInt(Parameters.numParams);
            writer.WriteInt(Parameters.parameter0);
            writer.WriteInt(Parameters.parameter1);
            writer.WriteInt(Parameters.parameter2);
            writer.WriteInt(Parameters.parameter3);
            writer.WriteInt(Parameters.parameter4);
            return writer.Data;
        }

        public virtual void Init()
        { }
        #endregion

        #region Methods
        public virtual void ValidParam(int paramId, int paramCount)
        { ParamValid?.Invoke(this); }
        public virtual void ValidObjective()
        {
            _finish = true;
        }
        #endregion

        #region Explicit Cast
        static public explicit operator ObjectiveTemplate(QuestObjective objective)
        {
            ObjectiveTemplate newObjective;
            if (objective is QuestObjectiveFreeForm)
                newObjective = new ObjectiveFreeForm()
                {
                    Coords = objective.coords,
                    Id = objective.id,
                    MapId = objective.mapId,
                    Parameters = objective.parameters,
                    Type = objective.typeId,
                    DialogId = objective.dialogId,
                    LastIds = Starter.Database.FirstOrDefault<string>("select LastId from objective_condition where Id='" + objective.id + "'")?.FromCSV<int>(";"),
                };
            else if (objective is QuestObjectiveGoToNpc)
                newObjective = new ObjectiveGoToNpc()
                {
                    Coords = objective.coords,
                    Id = objective.id,
                    MapId = objective.mapId,
                    Parameters = objective.parameters,
                    Type = objective.typeId,
                    DialogId = objective.dialogId,
                    LastIds = Starter.Database.FirstOrDefault<string>("select LastId from objective_condition where Id='" + objective.id + "'")?.FromCSV<int>(";"),
                };
            else if (objective is QuestObjectiveFightMonster)
                newObjective = new ObjectiveFightMonster()
                {
                    Coords = objective.coords,
                    Id = objective.id,
                    MapId = objective.mapId,
                    Parameters = objective.parameters,
                    Type = objective.typeId,
                    DialogId = objective.dialogId,
                    LastIds = Starter.Database.FirstOrDefault<string>("select LastId from objective_condition where Id='" + objective.id + "'")?.FromCSV<int>(";"),
                };
            else if (objective is QuestObjectiveDiscoverMap)
                newObjective = new ObjectiveDiscoverMap()
                {
                    Coords = objective.coords,
                    Id = objective.id,
                    MapId = objective.mapId,
                    Parameters = objective.parameters,
                    Type = objective.typeId,
                    DialogId = objective.dialogId,
                    LastIds = Starter.Database.FirstOrDefault<string>("select LastId from objective_condition where Id='" + objective.id + "'")?.FromCSV<int>(";"),
                };
            else if (objective is QuestObjectiveCraftItem)
                newObjective = new ObjectiveCraftItem()
                {
                    Coords = objective.coords,
                    Id = objective.id,
                    MapId = objective.mapId,
                    Parameters = objective.parameters,
                    Type = objective.typeId,
                    DialogId = objective.dialogId,
                    LastIds = Starter.Database.FirstOrDefault<string>("select LastId from objective_condition where Id='" + objective.id + "'")?.FromCSV<int>(";"),
                };
            else
                newObjective = new ObjectiveTemplate()
                {
                    Coords = objective.coords,
                    Id = objective.id,
                    MapId = objective.mapId,
                    Parameters = objective.parameters,
                    Type = objective.typeId,
                    DialogId = objective.dialogId,
                    LastIds = Starter.Database.FirstOrDefault<string>("select LastId from objective_condition where Id='" + objective.id + "'")?.FromCSV<int>(";"),
                };
            newObjective.Init();
            return newObjective;
        }
        #endregion
    }
}
