using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Game.Fights.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Fights.Challenges.Custom
{
    [ChallengeAttribute(ChallengeEnums.Designe)]
    public class DesigneHandler : ChallengeHandler
    {
        public DesigneHandler(Fight fight, ChallengeEnums chall) : base(fight, chall)
        {
            BoostSolo = 10;
            BoostGroup = 10;
            Target = Fight.Fighters.FindAll(x => x is MonsterFighter).First();
        }

        public override void Apply(ChallengeActionEnum action, object Token)
        {
            if (IsValidated)
                return;
            if (action == ChallengeActionEnum.Die && Token is MonsterFighter)
            {
                if ((Token as MonsterFighter).Id == Target.Id)
                {
                    IsFail = false;
                    Fight.ChallUpdate(this);
                    IsValidated = true;
                }
                else
                {
                    IsFail = true;
                    Fight.ChallUpdate(this);
                    IsValidated = true;
                }
            }
        }
    }
}
