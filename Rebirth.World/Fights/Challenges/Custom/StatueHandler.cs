using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Game.Fights.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Fights.Challenges.Custom
{
    [ChallengeAttribute(ChallengeEnums.Statue)]
    public class StatueHandler : ChallengeHandler
    {
        public StatueHandler(Fight fight, ChallengeEnums chall) : base(fight, chall)
        {
            BoostSolo = 10;
            BoostGroup = 10;
        }

        public override void Apply(ChallengeActionEnum action, object Token)
        {
            if (IsValidated)
                return;
            if (action == ChallengeActionEnum.PassTurn && Token is CharacterFighter 
                && (Token as CharacterFighter).StartTurnPoint.CellId != (Token as CharacterFighter).Point.Point.CellId)
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
            Fight.ChallUpdate(this);
        }
    }
}
