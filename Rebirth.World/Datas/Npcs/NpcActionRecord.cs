using PetaPoco.NetCore;
using Rebirth.Common.GameData.D2O;
using Rebirth.Common.Protocol.Data;
using Rebirth.World.Datas.Bdd;
using Rebirth.World.Game.Npcs.Actions;
using Rebirth.World.Managers;
using Rebirth.World.Npcs;

namespace Rebirth.World.Datas.Npcs
{
    [TableName("npcs_actions")]
    public class NpcActionRecord : ParameterizableRecord
    {
        private string m_condition;
        private ConditionExpression m_conditionExpression;
        private Npc m_template;
        public uint Id
        {
            get;
            set;
        }
        public int NpcId
        {
            get;
            set;
        }
        public string Type
        {
            get;
            set;
        }
        [Ignore]
        public Npc Template
        {
            get
            {
                Npc arg_23_0;
                if ((arg_23_0 = this.m_template) == null)
                {
                    arg_23_0 = (this.m_template = ObjectDataManager.Get<Npc>(this.NpcId));
                }
                return arg_23_0;
            }
            set
            {
                this.m_template = value;
                this.NpcId = value.id;
            }
        }
        public string Condition
        {
            get
            {
                return this.m_condition;
            }
            set
            {
                this.m_condition = value;
                this.m_conditionExpression = null;
            }
        }
        [Ignore]
        public ConditionExpression ConditionExpression
        {
            get
            {
                ConditionExpression result;
                if (string.IsNullOrEmpty(this.Condition) || this.Condition == "null")
                {
                    result = null;
                }
                else
                {
                    ConditionExpression arg_47_0;
                    if ((arg_47_0 = this.m_conditionExpression) == null)
                    {
                        arg_47_0 = (this.m_conditionExpression = ConditionExpression.Parse(this.Condition));
                    }
                    result = arg_47_0;
                }
                return result;
            }
            set
            {
                this.m_conditionExpression = value;
                this.Condition = value.ToString();
            }
        }
        public NpcActionDatabase GenerateAction()
        {
            return DiscriminatorManager<NpcActionDatabase>.Instance.Generate<NpcActionRecord>(this.Type, this);
        }
    }
}
