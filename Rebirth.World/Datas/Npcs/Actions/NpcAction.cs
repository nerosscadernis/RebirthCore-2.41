using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Datas.Characters;
using Rebirth.World.Datas.Npcs;

namespace Rebirth.World.Game.Npcs.Actions
{
    public abstract class NpcAction
    {
        public abstract NpcActionTypeEnum ActionType
        {
            get;
        }
        public abstract void Execute(NpcSpawn npc, Character character);
        public virtual bool CanExecute(NpcSpawn npc, Character character)
        {
            return true;
        }
    }
}
