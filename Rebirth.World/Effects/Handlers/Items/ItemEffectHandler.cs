using Rebirth.Common.Protocol.Data;
using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Datas.Characters;
using Rebirth.World.Datas.Items;
using Rebirth.World.Game.Effect.Instances;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rebirth.World.Effects.Handlers.Items
{
    public abstract class ItemEffectHandler : EffectHandler
    {
        public enum HandlerOperation
        {
            APPLY,
            UNAPPLY
        }
        private ItemEffectHandler.HandlerOperation? m_operation;
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
        public bool ItemSetApply
        {
            get;
            set;
        }
        public bool Equiped
        {
            get
            {
                return this.Item != null && Target.Inventory.IsEquip(Item);
            }
        }
        public bool Boost
        {
            get
            {
                return this.Item != null && (ItemSuperTypeEnum)this.Item.Type.superTypeId == ItemSuperTypeEnum.SUPERTYPE_BOOST;
            }
        }
        public ItemEffectHandler.HandlerOperation Operation
        {
            get
            {
                return this.m_operation.HasValue ? this.m_operation.Value : ((this.Equiped || this.ItemSetApply) ? ItemEffectHandler.HandlerOperation.APPLY : ItemEffectHandler.HandlerOperation.UNAPPLY);
            }
            set
            {
                this.m_operation = new ItemEffectHandler.HandlerOperation?(value);
            }
        }
        protected ItemEffectHandler(EffectBase effect, Character target, PlayerItem item)
            : base(effect)
        {
            this.Target = target;
            this.Item = item;
        }
        protected ItemEffectHandler(EffectBase effect, Character target, bool apply)
            : base(effect)
        {
            this.Target = target;
            this.ItemSetApply = apply;
        }
    }
}
