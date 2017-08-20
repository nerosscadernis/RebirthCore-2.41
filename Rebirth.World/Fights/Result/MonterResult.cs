using Rebirth.World.Datas.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Fights.Result
{
    public class MonsterResult : AbstractResult
    {
        private MonsterGroup Group;

        public MonsterResult(MonsterGroup group)
        {
            Group = group;
        }

        #region Function

        public override void AssignResult()
        {
            if (Group != null)
            {
                Group.Kamas += Kamas;
            }
        }
        #endregion
    }
}
