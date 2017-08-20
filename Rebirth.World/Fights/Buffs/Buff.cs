using Rebirth.Common.Protocol.Enums;
using Rebirth.Common.Protocol.Types;
using Rebirth.World.Datas.Spells;
using Rebirth.World.Game.Effect.Instances;
using Rebirth.World.Game.Fights.Actors;

namespace Rebirth.World.Game.Fights.Buffs
{
    public abstract class Buff
    {
        public const int CHARACTERISTIC_STATE = 71;
        public int Id
        {
            get;
            private set;
        }
        public Fighter Target
        {
            get;
            private set;
        }
        public Fighter Caster
        {
            get;
            private set;
        }
        public EffectBase Effect
        {
            get;
            set;
        }
        public SpellTemplate Spell
        {
            get;
            private set;
        }
        public short Duration
        {
            get;
            set;
        }
        public bool Critical
        {
            get;
            private set;
        }
        public bool Dispellable
        {
            get;
            private set;
        }
        public short? CustomActionId
        {
            get;
            private set;
        }
        public double Efficiency
        {
            get;
            set;
        }
        public virtual BuffType Type
        {
            get
            {
                BuffType result;
                if (this.Effect.Template.characteristic == 71)
                {
                    result = BuffType.CATEGORY_STATE;
                }
                else
                {
                    if (this.Effect.Template.@operator == "-")
                    {
                        result = (this.Effect.Template.active ? BuffType.CATEGORY_ACTIVE_MALUS : BuffType.CATEGORY_PASSIVE_MALUS);
                    }
                    else
                    {
                        if (this.Effect.Template.@operator == "+")
                        {
                            result = (this.Effect.Template.active ? BuffType.CATEGORY_ACTIVE_BONUS : BuffType.CATEGORY_PASSIVE_BONUS);
                        }
                        else
                        {
                            result = BuffType.CATEGORY_OTHER;
                        }
                    }
                }
                return result;
            }
        }
        protected Buff(int id, Fighter target, Fighter caster, EffectBase effect, SpellTemplate spell, bool critical, bool dispelable, bool declanched)
        {
            this.Id = id;
            this.Target = target;
            this.Caster = caster;
            this.Effect = effect;
            this.Spell = spell;
            this.Critical = critical;
            this.Dispellable = dispelable;
            if(declanched)
                this.Duration = (short)( Effect.Duration);
            else
                this.Duration = (short)(Effect.Delay > 0 ? Effect.Delay : Effect.Duration);
            this.Efficiency = 1.0;
        }
        protected Buff(int id, Fighter target, Fighter caster, EffectBase effect, SpellTemplate spell, bool critical, bool dispelable, short customActionId, bool declanched)
        {
            this.Id = id;
            this.Target = target;
            this.Caster = caster;
            this.Effect = effect;
            this.Spell = spell;
            this.Critical = critical;
            this.Dispellable = dispelable;
            this.CustomActionId = new short?(customActionId);
            if (declanched)
                this.Duration = (short)(Effect.Duration);
            else
                this.Duration = (short)(Effect.Delay > 0 ? Effect.Delay : Effect.Duration);
            this.Efficiency = 1.0;
        }
        public bool DecrementDuration(short value = 1)
        {
            return (this.Duration -= value) == 0;
        }
        public abstract void Apply(object token = null);
        public abstract void Dispell();
        public virtual short GetActionId()
        {
            short result;
            if (this.CustomActionId.HasValue)
            {
                result = this.CustomActionId.Value;
            }
            else
            {
                result = (short)this.Effect.EffectId;
            }
            return result;
        }
        public EffectInteger GenerateEffect()
        {
            EffectInteger effectInteger = this.Effect.GenerateEffect(EffectGenerationContext.Spell, EffectGenerationType.Normal) as EffectInteger;
            if (effectInteger != null)
            {
                effectInteger.Value = (short)((double)effectInteger.Value * this.Efficiency);
            }
            return effectInteger;
        }
        public FightDispellableEffectExtendedInformations GetFightDispellableEffectExtendedInformations()
        {
            return new FightDispellableEffectExtendedInformations((uint)GetActionId(), this.Caster.Id, this.GetAbstractFightDispellableEffect());
        }
        public abstract AbstractFightDispellableEffect GetAbstractFightDispellableEffect();
        public bool IsDispell = true;
    }
}
