using Rebirth.Common.Protocol.Data;
using Rebirth.World.Game.Effect.Instances;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rebirth.World.Effects.Handlers
{
    public abstract class EffectHandler
    {
        public virtual EffectBase Effect
        {
            get;
            private set;
        }
        public double Efficiency
        {
            get;
            set;
        }
        public EffectHandler(EffectBase effect)
        {
            this.Effect = effect;
            this.Efficiency = 1.0;
        }
        public abstract bool Apply(object token, bool declanched = false);
    }
}
