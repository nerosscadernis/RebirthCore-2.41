using Rebirth.World.Datas.Characters;
using Rebirth.World.Datas.Characters;
using Rebirth.World.Game.Fights.Team;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Fights.Actors
{
    public class SpectatorFighter : Fighter
    {
        public SpectatorFighter(Character character, FightTeam team, Fight fight, bool isReady = false) : base(team, fight, null)
        {
            Character = character;

            Id = (int)character.Infos.Id;
        }

        public Character Character
        {
            get;
            private set;
        }
    }
}
