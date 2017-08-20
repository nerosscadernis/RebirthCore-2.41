using Rebirth.World.Datas.Characters;
using Rebirth.World.Datas.Items;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Effects.Handlers;
using Rebirth.World.Game.Effect.Instances;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rebirth.World.Effects.Usables
{
    public abstract class UsableEffectHandler : EffectHandler
    {
        public Character Target
        {
            get;
            protected set;
        }
        public PlayerItem Item
        {
            get;
            protected set;
        }
        public CellMap TargetCell
        {
            get;
            set;
        }
        public uint NumberOfUses
        {
            get;
            set;
        }
        public uint UsedItems
        {
            get;
            protected set;
        }
        protected UsableEffectHandler(EffectBase effect, Character target, PlayerItem item)
            : base(effect)
        {
            this.Target = target;
            this.Item = item;
            this.NumberOfUses = 1u;
        }
    }
}
