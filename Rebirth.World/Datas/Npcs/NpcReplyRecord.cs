using PetaPoco.NetCore;
using Rebirth.World.Datas.Bdd;
using Rebirth.World.Game.Npcs.Replies;
using Rebirth.World.Managers;
using Rebirth.World.Npcs;

namespace Rebirth.World.Datas.Npcs
{
    [TableName("npcs_replies")]
    public class NpcReplyRecord : ParameterizableRecord
    {
        private string m_criteria;
        private ConditionExpression m_criteriaExpression;
        private NpcMessageRecord m_message;
        public int Id
        {
            get;
            set;
        }
        public string Type
        {
            get;
            set;
        }
        public int ReplyId
        {
            get;
            set;
        }
        public int MessageId
        {
            get;
            set;
        }
        public string Criteria
        {
            get
            {
                return this.m_criteria;
            }
            set
            {
                this.m_criteria = value;
                this.m_criteriaExpression = null;
            }
        }
        [Ignore]
        public ConditionExpression CriteriaExpression
        {
            get
            {
                ConditionExpression result;
                if (string.IsNullOrEmpty(this.Criteria) || this.Criteria == "null")
                {
                    result = null;
                }
                else
                {
                    ConditionExpression arg_47_0;
                    if ((arg_47_0 = this.m_criteriaExpression) == null)
                    {
                        arg_47_0 = (this.m_criteriaExpression = ConditionExpression.Parse(this.Criteria));
                    }
                    result = arg_47_0;
                }
                return result;
            }
            set
            {
                this.m_criteriaExpression = value;
                this.Criteria = value.ToString();
            }
        }
        public NpcMessageRecord Message
        {
            get
            {
                NpcMessageRecord arg_23_0;
                if ((arg_23_0 = this.m_message) == null)
                {
                    arg_23_0 = (this.m_message = NpcManager.Instance.GetNpcMessageRecord(this.MessageId));
                }
                return arg_23_0;
            }
            set
            {
                this.m_message = value;
                this.MessageId =(int) value.MessageId;
            }
        }
        public NpcReply GenerateReply()
        {
            return DiscriminatorManager<NpcReply>.Instance.Generate<NpcReplyRecord>(this.Type, this);
        }
    }
}
