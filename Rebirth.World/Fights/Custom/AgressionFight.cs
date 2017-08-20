using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Datas.Maps;

namespace Rebirth.World.Game.Fights.Custom
{
    public class AgressionFight : Fight
    {
        #region Constructor
        public AgressionFight(int id, MapTemplate map) : base (id, map)
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
                return FightTypeEnum.FIGHT_TYPE_AGRESSION;
            }
        }
        #endregion
    }
}
