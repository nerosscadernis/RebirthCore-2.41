using Rebirth.Common.Protocol.Data;
using Rebirth.World.Datas.Bdd;
using Rebirth.World.Datas.Characters;
using Rebirth.World.Datas.Npcs;

namespace Rebirth.World.Game.Npcs.Actions
{
    public abstract class NpcActionDatabase : NpcAction
    {
        public NpcActionRecord Record
        {
            get;
            private set;
        }
        public uint Id
        {
            get
            {
                return this.Record.Id;
            }
            set
            {
                this.Record.Id = value;
            }
        }
        public int NpcId
        {
            get
            {
                return this.Record.NpcId;
            }
            set
            {
                this.Record.NpcId = value;
            }
        }
        public Npc Template
        {
            get
            {
                return this.Record.Template;
            }
            set
            {
                this.Record.Template = value;
            }
        }
        public ConditionExpression ConditionExpression
        {
            get
            {
                return this.Record.ConditionExpression;
            }
            set
            {
                this.Record.ConditionExpression = value;
            }
        }
        public NpcActionDatabase(NpcActionRecord record)
        {
            this.Record = record;
        }
        public bool CanExecute(Npc npc, Character character)
        {
            return this.ConditionExpression == null || this.ConditionExpression.Eval(character);
        }

     
    }
}
