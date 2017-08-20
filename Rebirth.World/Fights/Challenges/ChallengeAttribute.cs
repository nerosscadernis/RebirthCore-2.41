using Rebirth.Common.Protocol.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Fights.Challenges
{
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
    public class ChallengeAttribute : System.Attribute
    {
        public ChallengeEnums Effect
        {
            get;
            private set;
        }
        public ChallengeAttribute(ChallengeEnums effect)
        {
            this.Effect = effect;
        }
    }
}
