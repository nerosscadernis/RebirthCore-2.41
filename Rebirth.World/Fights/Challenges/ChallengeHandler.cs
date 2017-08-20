using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Game.Fights.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Fights.Challenges
{
    [DefaultChallengeHandler]
    public class ChallengeHandler
    {
        public Fight Fight
        { get; set; }

        public ChallengeEnums Chall
        { get; set; }

        public bool Success
        { get { return !IsFail; } }

        protected bool IsFail
        { get; set; }

        protected bool IsValidated
        { get; set; }

        public Fighter Target
        { get; set; }

        public int BoostSolo
        { get; set; }

        public int BoostGroup
        { get; set; }

        public ChallengeHandler()
        { }

        public ChallengeHandler(Fight Fight, ChallengeEnums Chall)
        {
            this.Fight = Fight;
            this.Chall = Chall;
        }

        public virtual void Apply(ChallengeActionEnum action, object Token)
        { }

        public virtual void Update()
        { }

        public virtual void Finish()
        { }
    }
}
