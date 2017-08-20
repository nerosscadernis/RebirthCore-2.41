using Rebirth.Common.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rebirth.World.Datas.Quests
{
    public class ObjectiveGoToNpc : ObjectiveTemplate
    {
        #region vars
        public List<int> _params = new List<int>();
        public List<int> _finishParams = new List<int>();
        #endregion

        #region Properties
        public override bool IsFinish
        {
            get
            {
                return _params.Count == 0;
            }
        }
        #endregion

        #region Constructors / Datas
        public ObjectiveGoToNpc() : base()
        { }

        public ObjectiveGoToNpc(IDataReader reader) : base(reader)
        {
            var count = reader.ReadShort();
            for (int i = 0; i < count; i++)
            {
                _params.Add(reader.ReadInt());
            }
            count = reader.ReadShort();
            for (int i = 0; i < count; i++)
            {
                _finishParams.Add(reader.ReadInt());
            }
        }

        public override byte[] GetDatas()
        {
            var writer = new BigEndianWriter();
            writer.WriteBytes(base.GetDatas());
            writer.WriteShort((short)_params.Count);
            foreach (var param in _params)
            {
                writer.WriteInt(param);
            }
            writer.WriteShort((short)_finishParams.Count);
            foreach (var param in _finishParams)
            {
                writer.WriteInt(param);
            }
            return writer.Data;
        }
        #endregion

        #region Public Methods
        public override void Init()
        {
            if (Parameters.parameter0 != 0)
                _params.Add(Parameters.parameter0);
            if (Parameters.parameter1 != 0)
                _params.Add(Parameters.parameter1);
            if (Parameters.parameter2 != 0)
                _params.Add(Parameters.parameter2);
            if (Parameters.parameter3 != 0)
                _params.Add(Parameters.parameter3);
            if (Parameters.parameter4 != 0)
                _params.Add(Parameters.parameter4);
        }

        public override void ValidParam(int paramId, int paramCount)
        {
            if (DialogId > 0 && paramCount != DialogId)
                return;
            if (_params.Contains(paramId))
            {
                _params.Remove(paramId);
                _finishParams.Add(paramId);
                base.ValidParam(paramId, paramCount);
            }
        }
        #endregion
    }
}