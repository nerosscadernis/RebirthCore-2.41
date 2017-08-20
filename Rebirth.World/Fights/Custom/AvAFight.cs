using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Fights.Custom
{
    public class AvAFight : Fight
    {
        #region Constructor
        public AvAFight(int id, MapTemplate map) : base (id, map)
        {
            StartTimer = new Common.Timers.TimerCore(StartFight, 45000);
            Teams[(byte)TeamEnum.TEAM_CHALLENGER].Type = TeamTypeEnum.TEAM_TYPE_PLAYER;
            Teams[(byte)TeamEnum.TEAM_DEFENDER].Type = TeamTypeEnum.TEAM_TYPE_MONSTER;
        }
        #endregion

        #region Properties
        public override FightTypeEnum Type
        {
            get
            {
                return FightTypeEnum.FIGHT_TYPE_PvPr;
            }
        }
        #endregion
    }
}
