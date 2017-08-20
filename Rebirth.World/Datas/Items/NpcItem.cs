using PetaPoco.NetCore;
using Rebirth.Common.Protocol.Data;
using Rebirth.Common.Protocol.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebirth.World.Datas.Items
{
    [TableName("npcs_items")]
    public class NpcItem : ItemToSell
    {
        private string m_buyCriterion;
        private double? m_customPrice;
        public int NpcShopId
        {
            get;
            set;
        }
        public double Price
        {
            get
            {
                return this.CustomPrice.HasValue ? this.CustomPrice.Value : base.Item.price;
            }
        }
        public double? CustomPrice
        {
            get
            {
                return this.m_customPrice;
            }
            set
            {
                this.m_customPrice = value;
            }
        }

        public string BuyCriterion
        {
            get
            {
                return this.m_buyCriterion;
            }
            set
            {
                this.m_buyCriterion = (value ?? string.Empty);
            }
        }
        public bool MaxStats
        {
            get;
            set;
        }
        public NpcItem()
        {
        }
        private ObjectItemToSellInNpcShop BuildObjectItemToSellInNpcShop()
        {
            return new ObjectItemToSellInNpcShop((uint)base.Item.id,
                (from entry in base.Item.possibleEffects
                select ParseEffect(entry)).ToArray(), (uint)(this.CustomPrice.HasValue ? this.CustomPrice.Value : base.Item.price), this.BuyCriterion ?? string.Empty);
        }

        private ObjectEffect ParseEffect(EffectInstance effect)
        {
            if (effect is EffectInstanceDice)
            {
                var newEff = effect as EffectInstanceDice;
                return new ObjectEffectDice(effect.effectId, newEff.diceNum, newEff.diceSide, (uint)newEff.value);
            }
            if (effect is EffectInstanceDate)
            {
                var newEff = effect as EffectInstanceDate;
                return new ObjectEffectDate(effect.effectId, newEff.year, (sbyte)newEff.month, (sbyte)newEff.day, (sbyte)newEff.hour, (sbyte)newEff.minute);
            }
            if (effect is EffectInstanceMinMax)
            {
                var newEff = effect as EffectInstanceMinMax;
                return new ObjectEffectMinMax(effect.effectId, newEff.min, newEff.max);
            }
            return new ObjectEffect(effect.effectId);
        }

        public override Rebirth.Common.Protocol.Types.Item GetNetworkItem()
        {
            return BuildObjectItemToSellInNpcShop();
        }
    }
}

