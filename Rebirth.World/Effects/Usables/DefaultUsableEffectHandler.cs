using Rebirth.World.Datas.Characters;
using Rebirth.World.Datas.Items;
using Rebirth.World.Effects.Handlers;
using Rebirth.World.Game.Effect.Instances;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rebirth.World.Effects.Usables
{
    [DefaultEffectHandler]
    public class DefaultUsableEffectHandler : UsableEffectHandler
    {
        public DefaultUsableEffectHandler(EffectBase effect, Character target, PlayerItem item)
            : base(effect, target, item)
        {
        }
        public override bool Apply(object token, bool declanched = false)
        {
            return true;
        }
    }
}
