using Rebirth.Common.Protocol.Enums;
using Rebirth.Common.Protocol.Messages;
using Rebirth.Common.Protocol.Types;
using Rebirth.World.Datas.Spells;
using Rebirth.World.Game.Effect.Instances;
using Rebirth.World.Game.Fights.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Fights.Buffs
{
    public class StatBuff : Buff
    {
        public int Value
        {
            get;
            set;
        }

        public short CustomValue;
        public PlayerFields Caracteristic
        {
            get;
            set;
        }
        public uint ParentId;
        public StatBuff(int id, Fighter target, Fighter caster, EffectBase effect, SpellTemplate spell, int value, PlayerFields caracteristic, bool critical, bool dispelable, bool declanched) : base(id, target, caster, effect, spell, critical, dispelable, declanched)
        {
            this.Value = value;
            this.Caracteristic = caracteristic;
        }
        public StatBuff(int id, Fighter target, Fighter caster, EffectBase effect, SpellTemplate spell, int value, PlayerFields caracteristic, bool critical, bool dispelable, short customActionId, bool declanched) : base(id, target, caster, effect, spell, critical, dispelable, customActionId, declanched)
        {
            this.Value = value;
            this.Caracteristic = caracteristic;
        }
        public override void Apply(object token = null)
        {
            if(Caracteristic == PlayerFields.DamageMultiplicator)
            {
                if(Value > 100)
                    base.Target.Stats[this.Caracteristic].Context += this.Value - 100;
                else if (Value < 100)
                    base.Target.Stats[this.Caracteristic].Context -= this.Value;
            }
            else
                base.Target.Stats[this.Caracteristic].Context += this.Value;
        }
        public override void Dispell()
        {
            if (Caracteristic == PlayerFields.DamageMultiplicator)
            {
                if (Value > 100)
                    base.Target.Stats[this.Caracteristic].Context -= this.Value - 100;
                else if (Value < 100)
                    base.Target.Stats[this.Caracteristic].Context += this.Value;
            }
            else
                base.Target.Stats[this.Caracteristic].Context -= this.Value;
        }
        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            return new FightTemporaryBoostEffect((uint)Id, Target.Id, Duration, (sbyte)(Dispellable ? 0 : 1), (uint)Spell.Spell.id, Effect.Uid, 0, (short)(Value < 0 ? Value * -1 : Value));
        }
    }
}
