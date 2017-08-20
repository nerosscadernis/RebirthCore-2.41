using Rebirth.Common.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rebirth.World.Datas.Quests
{
    public class ObjectiveDiscoverMap : ObjectiveTemplate
    {
        #region Properties
        public override bool IsFinish

        {
            get
            {
                return _finish;
            }
        }
        #endregion

        #region Constructors / Datas
        public ObjectiveDiscoverMap() : base()
        { }

        public ObjectiveDiscoverMap(IDataReader reader) : base(reader)
        {
        }

        public override byte[] GetDatas()
        {
            var writer = new BigEndianWriter();
            writer.WriteBytes(base.GetDatas());
            return writer.Data;
        }
        #endregion

        #region Public Methods
        public override void ValidParam(int paramId, int paramCount)
        {
            if (MapId == 0 || paramId == MapId)
            {
                _finish = true;
                base.ValidParam(paramId, paramCount);
            }
        }
        #endregion
    }
}
