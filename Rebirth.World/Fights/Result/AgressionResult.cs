using Rebirth.Common.Protocol.Data;
using Rebirth.Common.Thread;
using Rebirth.World.Datas.Characters;
using Rebirth.World.Datas.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Fights.Result
{
    public class AgressionResult : AbstractResult
    {
        public AgressionResult()
        { }

        public AgressionResult(Character character) : base(character)
        {
        }
    }
}
