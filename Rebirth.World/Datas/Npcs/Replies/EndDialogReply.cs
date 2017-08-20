using Rebirth.World.Datas;
using Rebirth.World.Datas.Bdd;
using Rebirth.World.Datas.Characters;
using Rebirth.World.Datas.Npcs;
using Rebirth.World.Datas.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Npcs.Replies
{
    [Discriminator("EndDialog", typeof(NpcReply), new System.Type[]
     {
        typeof(NpcReplyRecord)
     })]
    public class EndDialogReply : NpcReply
    {
        public EndDialogReply(NpcReplyRecord record) : base(record)
        {
        }
        public override bool Execute(NpcSpawn npc, Character character)
        {
            bool result;
            if (!base.Execute(npc, character))
            {
                result = false;
            }
            else
            {
                character.Activity.Close();
                result = true;
            }
            return result;
        }
    }
}
