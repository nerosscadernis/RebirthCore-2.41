using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rebirth.World.Datas.Characters;
using Rebirth.Common.Protocol.Messages;

namespace Rebirth.World.Game.Fights.Custom
{
    public class KoliFight : Fight
    {
        #region Constructor
        public KoliFight(int id, MapTemplate map) : base (id, map)
        {
            StartTimer = new Common.Timers.TimerCore(StartFight, 30000);
            Teams[(byte)TeamEnum.TEAM_CHALLENGER].Type = TeamTypeEnum.TEAM_TYPE_PLAYER;
            Teams[(byte)TeamEnum.TEAM_DEFENDER].Type = TeamTypeEnum.TEAM_TYPE_PLAYER;
        }
        #endregion

        #region Properties
        public override FightTypeEnum Type
        {
            get
            {
                return FightTypeEnum.FIGHT_TYPE_PVP_ARENA;
            }
        }
        #endregion

        #region Enter
        public override void AddCharacter(Character character, TeamEnum team)
        {
            //character.KoliTeleport(Map.Id);
            base.AddCharacter(character, team);
        }
        #endregion

        #region End
        public override void ResetFightersProperties()
        {
            base.ResetFightersProperties();
        }

        public override void ReturnInMap(bool command = false)
        {
            foreach (CharacterFighter item in Fighters.FindAll(x => x is CharacterFighter))
            {
                item.Character.Client.Send(new GameContextDestroyMessage());
                item.Character.Client.Send(new GameContextCreateMessage(1));
                //item.Character.KoliExit();
            }
        }
        #endregion
    }
}
