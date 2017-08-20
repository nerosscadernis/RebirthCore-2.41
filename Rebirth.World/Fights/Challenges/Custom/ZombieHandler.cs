using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Game.Fights.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Fights.Challenges.Custom
{
    [ChallengeAttribute(ChallengeEnums.Zombie)]
    public class ZombieHandler : ChallengeHandler
    {
        public ZombieHandler(Fight fight, ChallengeEnums chall) : base(fight, chall)
        {
            BoostSolo = 10;
            BoostGroup = 10;
        }

        public override void Apply(ChallengeActionEnum action, object Token)
        {
            if (IsValidated)
                return;
            if(action == ChallengeActionEnum.Move && Token is CharacterFighter && (Token as CharacterFighter).MpUsedMove > 1)
            {
                IsFail = true;
                Fight.ChallUpdate(this);
                IsValidated = true;
            }
        }

        public override void Finish()
        {
            if (IsValidated)
                return;
            if (Fight.TimeLine.FighterPlaying.MpUsedMove == 0)
                IsFail = true;

            Fight.ChallUpdate(this);
        }
    }
}
