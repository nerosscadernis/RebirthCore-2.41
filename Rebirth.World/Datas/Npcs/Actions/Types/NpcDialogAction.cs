using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Datas.Npcs;
using Rebirth.World.Datas.Characters;
using Rebirth.World.Game.Npcs.Dialogs;
using Rebirth.World.Datas.Characters;

namespace Rebirth.World.Game.Npcs.Actions.Types
{
    public class NpcDialogAction : NpcAction
    {
        public override NpcActionTypeEnum ActionType
        {
            get
            {
                return NpcActionTypeEnum.ACTION_TALK;
            }
        }

        public override void Execute(NpcSpawn npc, Character character)
        {
            NpcDialog dial = new NpcDialog(character, npc);
            character.Activity = dial;
            dial.Open();
        }
    }
}
