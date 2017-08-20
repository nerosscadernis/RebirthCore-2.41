using Rebirth.Common.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rebirth.World.Datas.Quests
{
    public class ObjectiveCraftItem : ObjectiveTemplate
    {
        #region vars
        public int ItemId { get; set; }
        public int Count { get; set; }
        public int ActualCount { get; set; }
        #endregion

        #region Properties
        public override bool IsFinish
        {
            get
            {
                return _finish || ActualCount >= Count;
            }
        }
        #endregion

        #region Constructors / Datas
        public ObjectiveCraftItem() : base()
        { }

        public ObjectiveCraftItem(IDataReader reader) : base(reader)
        {
            ItemId = reader.ReadInt();
            Count = reader.ReadInt();
            ActualCount = reader.ReadInt();
        }

        public override byte[] GetDatas()
        {
            var writer = new BigEndianWriter();
            writer.WriteBytes(base.GetDatas());
            writer.WriteInt(ItemId);
            writer.WriteInt(Count);
            writer.WriteInt(ActualCount);
            return writer.Data;
        }
        #endregion

        #region Public Methods
        public override void Init()
        {
            if (Parameters.parameter0 != 0)
                ItemId = Parameters.parameter0;
            if (Parameters.parameter1 != 0)
                Count = Parameters.parameter1;
        }

        public override void ValidParam(int paramId, int paramCount)
        {
            if (paramId == ItemId)
            {
                ActualCount += paramCount;
                base.ValidParam(paramId, paramCount);
            }
        }
        #endregion
    }
}
