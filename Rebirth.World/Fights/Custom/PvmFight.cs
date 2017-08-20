using Rebirth.World.Datas.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Network;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Game.Fights.Result;
using Rebirth.World.Datas.Monsters;
using Rebirth.World.Game.Fights.Challenges;

namespace Rebirth.World.Game.Fights.Custom
{
    public class PvmFight : Fight
    {
        #region Constructor
        public MonsterGroup Group
        {
            get;
            set;
        }
        public PvmFight(int id, MapTemplate map) : base(id, map)
        {
            StartTimer = new Common.Timers.TimerCore(StartFight, 45000);
            Teams[(byte)TeamEnum.TEAM_CHALLENGER].Type = TeamTypeEnum.TEAM_TYPE_PLAYER;
            Teams[(byte)TeamEnum.TEAM_DEFENDER].Type = TeamTypeEnum.TEAM_TYPE_MONSTER;
        }
        #endregion

        public override void StartFight()
        {
            Challenges.Add(ChallengeManager.Instance.GetChall(this));
            base.StartFight();
        }

        #region Properties
        public override FightTypeEnum Type
        {
            get
            {
                return FightTypeEnum.FIGHT_TYPE_PvM;
            }
        }
        #endregion
    }
}
