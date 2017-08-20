using PetaPoco.NetCore;
using Rebirth.Common.IO;
using Rebirth.World.Game.Npcs.Replies;
using Rebirth.World.Npcs;

namespace Rebirth.World.Datas.Npcs
{

    [TableName("npcs_messages")]
    public sealed class NpcMessageRecord
    {
        private System.Collections.Generic.IList<string> m_parameters;
        private string m_parametersCSV;
        private System.Collections.Generic.List<NpcReply> m_replies;
     
        public uint MessageId
        {
            get;
            set;
        }
        public string ParametersCSV
        {
            get
            {
                return this.m_parametersCSV;
            }
            set
            {
                this.m_parametersCSV = value;
                this.m_parameters = value.FromCSV<string>("|");
            }
        }
        [Ignore]
        public System.Collections.Generic.IList<string> Parameters
        {
            get
            {
                return this.m_parameters;
            }
            set
            {
                this.m_parameters = value;
                this.ParametersCSV = value.ToCSV("|");
            }
        }
        public System.Collections.Generic.List<NpcReply> Replies
        {
            get
            {
                System.Collections.Generic.List<NpcReply> arg_23_0;
                if ((arg_23_0 = this.m_replies) == null)
                {
                    arg_23_0 = (this.m_replies = NpcManager.Instance.GetMessageReplies((int)this.MessageId));
                }
                return arg_23_0;
            }
        }
       
    }
}
