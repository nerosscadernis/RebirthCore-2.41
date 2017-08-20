using Rebirth.Common.Protocol.Enums;
using Rebirth.Common.Thread;
using Rebirth.World.Datas.Spells;
using Rebirth.World.Game.Effect.Instances;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Game.Fights.Buffs;
using Rebirth.World.Game.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Fights.Other
{
    public class Damage 
    {
        #region Constructors
        public Damage(int amount, Fighter source, Fighter target, SpellTemplate spell)
        {
            Source = source;
            Target = target;
            Amount = amount;
            if (Target.Stats[PlayerFields.DamageMultiplicator].Total > 0)
                Amount = (int)((double)Amount * (double)Target.Stats[PlayerFields.DamageMultiplicator].Total / 100d);
            this.Spell = spell;
        }
        public Damage(Fighter source, Fighter target, int dist, SpellTemplate spell)
        {
            Dist = dist;
            Source = source;
            Target = target;
            this.Spell = spell;
        }
        public Damage(EffectDice effect, EffectSchoolEnum school, Fighter source, Fighter target, SpellTemplate spell, EffectGenerationType type, int dist, double boost)
        {
            this.School = school;
            this.Source = source;
            Target = target;
            this.Spell = spell;
            var spellBoost = Source != null ? Source.Stats.SpellBoosts.GetSpellBoost(Spell.Spell.id, CharacterSpellModificationTypeEnum.BASE_DAMAGE) : 0;
            this.BaseMaxDamages = (int)System.Math.Max(effect.DiceFace, effect.DiceNum) + spellBoost;
            this.BaseMinDamages = (int)System.Math.Min(effect.DiceFace, effect.DiceNum) + spellBoost;
            if (this.BaseMinDamages == 0)
                this.EffectGenerationType = EffectGenerationType.MaxEffects;
            EffectGenerationType = type;
            Dist = dist;
            if (boost > 0)
                Boost = boost;
            else
                Boost = 1;
        }
        public Damage(int max, int min, Fighter source, Fighter target, EffectSchoolEnum school, EffectGenerationType type, SpellTemplate spell, double boost)
        {
            this.Spell = spell;
            Source = source;
            Target = target;
            School = school;
            EffectGenerationType = type;
            var spellBoost = Source != null ? Source.Stats.SpellBoosts.GetSpellBoost(Spell.Spell.id, CharacterSpellModificationTypeEnum.BASE_DAMAGE) : 0;
            BaseMinDamages = min + spellBoost;
            BaseMaxDamages = max + spellBoost;
            if (boost > 0)
                Boost = boost;
            else
                Boost = 1;
        }
        #endregion

        #region Vars
        private double Boost;
        public int Dist;
        private int RdnGenerate;
        private int m_amount = -1;
        #endregion

        #region Properties
        public EffectSchoolEnum School
        {
            get;
            set;
        }
        public Fighter Source
        {
            get;
            set;
        }
        public Fighter Target
        {
            get;
            set;
        }
        private int m_baseMin;
        public int BaseMinDamages
        {
            get {
                return (Spell.IsWeapon && critical ? (m_baseMin + Spell.CritAddDamge) : m_baseMin);
            }
            set { m_baseMin = value; }
        }
        private int m_baseMax;
        public int BaseMaxDamages
        {
            get
            {
                return (Spell.IsWeapon && critical ? (m_baseMax + Spell.CritAddDamge) : m_baseMax);
            }
            set { m_baseMax = value; }
        }
        public bool critical
        {
            get;
            set;
        }
        public bool IsVenom
        {
            get;
            set;
        }
        public SpellTemplate Spell
        {
            get;
            set;
        }
        public int Permanant
        {
            get
            {
                return AfterReduction * Target.Stats[PlayerFields.Erosion].Total / 100;
            }
        }
        public int RealDamage
        {
            get
            {
                return AfterReduction - Permanant;
            }
        }
        public EffectGenerationType EffectGenerationType
        {
            get;
            set;
        }
        public int Amount
        {
            get
            {
                return m_amount;
            }
            set
            {
                if (Target.Stats.Health.Total <= value)
                    m_amount = Target.Stats.Health.Total;
                else
                    m_amount = value;
            }
        }
        public int AmountBefore
        { get; set; }
        public bool IgnoreDamageBoost
        {
            get;
            set;
        }

        private int MinDamageAmount
        {
            get
            {
                int damage = this.BaseMinDamages - (int)Math.Floor((double)(BaseMinDamages * (Dist * 10) / 100));
                var final = SetRealDamage(damage);
                if (IsVenom)
                    Amount = final;
                return final;
            }
        }
        private int MaxDamageAmount
        {
            get
            {
                int damage = this.BaseMaxDamages - (int)Math.Floor((double)(BaseMaxDamages * (Dist * 10) / 100));
                var final = SetRealDamage(damage);
                if (IsVenom)
                    Amount = final;
                return final;
            }
        }
        private int RdnDamageAmount
        {
            get
            {
                int damage = 0;
                if (RdnGenerate > 0)
                {
                    damage = RdnGenerate;
                }
                else
                {
                    if (BaseMinDamages <= 0)
                    {
                        return MaxDamageAmount;
                    }
                    else
                    {
                        AsyncRandom asyncRandom = new AsyncRandom();
                        damage = asyncRandom.Next(this.BaseMinDamages - (int)Math.Floor((double)(BaseMinDamages * (Dist * 10) / 100)), this.BaseMaxDamages + 1 - (int)Math.Floor((double)(BaseMaxDamages * (Dist * 10) / 100)));
                        RdnGenerate = damage;
                    }
                }
                var final = SetRealDamage(damage);
                if (IsVenom)
                    Amount = final;
                return final;
            }
        }

        public int AfterReduction
        {
            get
            {
                if (Amount >= 0)
                    return Amount;
                int damage = AutoSelectedDamage;
                AmountBefore = damage;
                int final = ApplyReduction(damage);
                if (Target.Stats.Health.Total <= final)
                {
                    Amount = Target.Stats.Health.Total; ;
                    return Target.Stats.Health.Total;
                }
                else
                {
                    Amount = final;
                    return final;
                }
            }
        }
        public int AutoSelectedDamage
        {
            get
            {
                if (Amount >= 0)
                    return Amount;
                if(!IsVenom)
                {
                    BaseMinDamages -= Target.Stats[PlayerFields.GlobalDamageReduction].Total;
                    BaseMaxDamages -= Target.Stats[PlayerFields.GlobalDamageReduction].Total;
                }
                switch (EffectGenerationType)
                {
                    case EffectGenerationType.MaxEffects:
                            return MaxDamageAmount;
                    case EffectGenerationType.MinEffects:
                            return MinDamageAmount;
                    case EffectGenerationType.Normal:
                    case EffectGenerationType.Radom:
                    default:
                            return RdnDamageAmount;
                }
            }
        }

        public void SetPushDamage(bool isSecond = false)
        {
            Amount = ((8 * Source.Level / 50) * Dist + Source.Stats.PushDamageBonus.Total) / (isSecond ? 2 : 1);
        }
        #endregion

        private int SetRealDamage(int damage)
        {
            var Stats = Source.Stats;
            if (!IgnoreDamageBoost)
            {
                switch (School)
                {
                    case EffectSchoolEnum.Neutral:
                        damage += (int)((double)damage * ((double)(Stats.Strength.Total + Stats[PlayerFields.DamageBonusPercent].Total + 
                            (Spell.IsTrapSpell ? Stats[PlayerFields.TrapBonusPercent].Total : 0) + (Spell.IsGlyphSpell ? Stats[PlayerFields.GlyphBonusPercent].Total : 0)
                            ) / 100) + Stats[PlayerFields.NeutralDamageBonus].Total + 
                            (Stats[PlayerFields.DamageBonus].Total + (Spell.IsTrapSpell ? Stats[ PlayerFields.TrapBonus].Total : 0) + (Spell.IsTrapSpell ? Stats[PlayerFields.GlyphBonus].Total : 0)) +
                            (critical ? Stats[PlayerFields.CriticalDamageBonus].Total : 0));
                        break;
                    case EffectSchoolEnum.Earth:
                        damage += (int)(damage * ((double)(Stats.Strength.Total + Stats[PlayerFields.DamageBonusPercent].Total +
                            (Spell.IsTrapSpell ? Stats[PlayerFields.TrapBonusPercent].Total : 0) + (Spell.IsGlyphSpell ? Stats[PlayerFields.GlyphBonusPercent].Total : 0)
                            ) / 100) + Stats[PlayerFields.EarthDamageBonus].Total +
                            (Stats[PlayerFields.DamageBonus].Total + (Spell.IsTrapSpell ? Stats[PlayerFields.TrapBonus].Total : 0) + (Spell.IsTrapSpell ? Stats[PlayerFields.GlyphBonus].Total : 0)) +
                            (critical ? Stats[PlayerFields.CriticalDamageBonus].Total : 0));
                        break;
                    case EffectSchoolEnum.Water:
                        damage += (int)(damage * ((double)(Stats.Chance.Total + Stats[PlayerFields.DamageBonusPercent].Total +
                            (Spell.IsTrapSpell ? Stats[PlayerFields.TrapBonusPercent].Total : 0) + (Spell.IsGlyphSpell ? Stats[PlayerFields.GlyphBonusPercent].Total : 0)
                            ) / 100) + Stats[PlayerFields.WaterDamageBonus].Total +
                            (Stats[PlayerFields.DamageBonus].Total + (Spell.IsTrapSpell ? Stats[PlayerFields.TrapBonus].Total : 0) + (Spell.IsTrapSpell ? Stats[PlayerFields.GlyphBonus].Total : 0)) +
                            (critical ? Stats[PlayerFields.CriticalDamageBonus].Total : 0));
                        break;
                    case EffectSchoolEnum.Air:
                        damage += (int)(damage * ((double)(Stats.Agility.Total + Stats[PlayerFields.DamageBonusPercent].Total +
                            (Spell.IsTrapSpell ? Stats[PlayerFields.TrapBonusPercent].Total : 0) + (Spell.IsGlyphSpell ? Stats[PlayerFields.GlyphBonusPercent].Total : 0)
                            ) / 100) + Stats[PlayerFields.AirDamageBonus].Total +
                            (Stats[PlayerFields.DamageBonus].Total + (Spell.IsTrapSpell ? Stats[PlayerFields.TrapBonus].Total : 0) + (Spell.IsTrapSpell ? Stats[PlayerFields.GlyphBonus].Total : 0)) +
                            (critical ? Stats[PlayerFields.CriticalDamageBonus].Total : 0));
                        break;
                    case EffectSchoolEnum.Fire:
                        damage += (int)(damage * ((double)(Stats.Intelligence.Total + Stats[PlayerFields.DamageBonusPercent].Total +
                            (Spell.IsTrapSpell ? Stats[PlayerFields.TrapBonusPercent].Total : 0) + (Spell.IsGlyphSpell ? Stats[PlayerFields.GlyphBonusPercent].Total : 0)
                            ) / 100) + Stats[PlayerFields.FireDamageBonus].Total +
                            (Stats[PlayerFields.DamageBonus].Total + (Spell.IsTrapSpell ? Stats[PlayerFields.TrapBonus].Total : 0) + (Spell.IsTrapSpell ? Stats[PlayerFields.GlyphBonus].Total : 0)) +
                            (critical ? Stats[PlayerFields.CriticalDamageBonus].Total : 0));
                        break;
                    default:
                        damage += (int)(damage * ((double)(Stats[PlayerFields.DamageBonusPercent].Total +
                            (Spell.IsTrapSpell ? Stats[PlayerFields.TrapBonusPercent].Total : 0) + (Spell.IsGlyphSpell ? Stats[PlayerFields.GlyphBonusPercent].Total : 0)
                            ) / 100) +
                            (Stats[PlayerFields.DamageBonus].Total + (Spell.IsTrapSpell ? Stats[PlayerFields.TrapBonus].Total : 0) + (Spell.IsGlyphSpell ? Stats[PlayerFields.GlyphBonus].Total : 0)) +
                            (critical ? Stats[PlayerFields.CriticalDamageBonus].Total : 0));
                        break;
                }
                if (Source is SummonBomb)
                    damage = (int)((double)damage * (double)(Stats[PlayerFields.BombBonus].Total / 100d + 1));
                damage = (int)((double) damage * Boost);
            }
            if (Target.Stats[PlayerFields.DamageMultiplicator].Total > 0)
                damage = (int)((double)damage * (1d + (double)Target.Stats[PlayerFields.DamageMultiplicator].Total / 100d));
            if (damage < 0)
                return 0;
            return damage;
        }
        private int ApplyReduction(int damage)
        {
            switch (School)
            {
                case EffectSchoolEnum.Neutral:
                    damage = damage - Target.Stats[PlayerFields.NeutralElementReduction].Total - (Target.Stats[PlayerFields.NeutralResistPercent].Total == 0 ? 0 : (int)Math.Round((double)damage * Target.Stats[PlayerFields.NeutralResistPercent].Total / 100));
                    break;
                case EffectSchoolEnum.Earth:
                    damage = damage - Target.Stats[PlayerFields.EarthElementReduction].Total - (Target.Stats[PlayerFields.EarthResistPercent].Total == 0 ? 0 : (int)Math.Round((double)damage * Target.Stats[PlayerFields.EarthResistPercent].Total / 100));
                    break;
                case EffectSchoolEnum.Water:
                    damage = damage - Target.Stats[PlayerFields.WaterElementReduction].Total - (Target.Stats[PlayerFields.WaterResistPercent].Total == 0 ?  0 : (int)Math.Round((double)damage * Target.Stats[PlayerFields.WaterResistPercent].Total / 100));
                    break;
                case EffectSchoolEnum.Air:
                    damage = damage - Target.Stats[PlayerFields.AirElementReduction].Total - (Target.Stats[PlayerFields.AirResistPercent].Total == 0 ? 0 : (int)Math.Round((double)damage * Target.Stats[PlayerFields.AirResistPercent].Total / 100));
                    break;
                case EffectSchoolEnum.Fire:
                    damage = damage - Target.Stats[PlayerFields.FireElementReduction].Total - (Target.Stats[PlayerFields.FireResistPercent].Total == 0 ? 0 : (int)Math.Round((double)damage * Target.Stats[PlayerFields.FireResistPercent].Total / 100));
                    break;
            }
            if(Source is SummonSynchro)
            {
                damage = (int)(damage * (Source.Stats[PlayerFields.FinalDamage] / 100 - 1));
            }
            else
            {
                if (Source.Stats[PlayerFields.FinalDamage].Total > 0)
                    damage += (int)((double)damage * (double)Source.Stats[PlayerFields.FinalDamage].Total / 100d);
            }
            return damage;
        }
    }
}
