using Rebirth.Common.Protocol.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rebirth.World.Effects.Handlers
{
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]
    public class EffectHandlerAttribute : System.Attribute
    {
        public EffectsEnum Effect
        {
            get;
            private set;
        }
        public EffectHandlerAttribute(EffectsEnum effect)
        {
            this.Effect = effect;
        }
    }
}
