using Rebirth.Common.IO;
using Rebirth.Common.Protocol.Data;
using Rebirth.Common.Protocol.Types;
using Rebirth.Common.Thread;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Effect.Instances
{
    public class EffectDice : EffectInteger
    {
        protected short m_diceface;
        protected short m_dicenum;
        public override int ProtocoleId
        {
            get
            {
                return 73;
            }
        }
        public override byte SerializationIdenfitier
        {
            get
            {
                return 4;
            }
        }
        public short DiceNum
        {
            get
            {
                return this.m_dicenum;
            }
            set
            {
                this.m_dicenum = value;
                base.IsDirty = true;
            }
        }
        public short DiceFace
        {
            get
            {
                return this.m_diceface;
            }
            set
            {
                this.m_diceface = value;
                base.IsDirty = true;
            }
        }
        public EffectDice()
        {
        }
        public EffectDice(EffectDice copy)
            : this(copy.Id, copy.Value, copy.DiceNum, copy.DiceFace, copy)
        {
        }
        public EffectDice(short id, int value, short dicenum, short diceface, EffectBase effect)
            : base(id, value, effect)
        {
            this.m_dicenum = dicenum;
            this.m_diceface = diceface;
        }
        public EffectDice(EffectInstanceDice effect)
            : base(effect)
        {
            this.m_dicenum = (short)effect.diceNum;
            this.m_diceface = (short)effect.diceSide;
        }
        public override object[] GetValues()
        {
            return new object[]
			{
				this.DiceNum,
				this.DiceFace,
				base.Value
			};
        }
        public override ObjectEffect GetObjectEffect()
        {
            return new ObjectEffectDice((uint)base.Id, (uint)this.DiceNum, (uint)this.DiceFace, (uint)base.Value);
        }
        public override EffectInstance GetEffectInstance()
        {
            return new EffectInstanceDice
            {
                effectId = (uint)base.Id,
                targetId = (int)base.Targets,
                delay = base.Delay,
                duration = base.Duration,
                group = base.Group,
                random = base.Random,
                modificator = base.Modificator,
                trigger = base.Trigger,
     
                zoneMinSize = base.ZoneMinSize,
                zoneSize = base.ZoneSize,
                zoneShape = (uint)base.ZoneShape,
                value = (int)base.Value,
                diceNum = (uint)this.DiceNum,
                diceSide = (uint)this.DiceFace
            };
        }
        public override EffectBase GenerateEffect(EffectGenerationContext context, EffectGenerationType type = EffectGenerationType.Normal)
        {
            AsyncRandom asyncRandom = new AsyncRandom();
            short num = (this.m_dicenum >= this.m_diceface) ? this.m_dicenum : this.m_diceface;
            short num2 = (this.m_dicenum <= this.m_diceface) ? this.m_dicenum : this.m_diceface;
            EffectBase result;
            if(EffectId == Common.Protocol.Enums.EffectsEnum.Effect_717)
            {
                return new EffectLadder(717, DiceNum, 0, this);
            }
            if (type == EffectGenerationType.MaxEffects)
            {
                if(IsSub)
                    result = new EffectInteger(base.Id, num2, this);
                else
                    result = new EffectInteger(base.Id, num, this);
            }
            else
            {
                if (type == EffectGenerationType.MinEffects)
                {
                    if (IsSub)
                        result = new EffectInteger(base.Id, num, this);
                    else
                        result = new EffectInteger(base.Id, num2, this);
                }
                //else if (type == EffectGenerationType.Radom)
                //{
                //    result = new EffectInteger(base.Id, (short)asyncRandom.Next((int)num2, (int)(num + 1)), this);
                //}
                else
                {
                    if (num2 == 0)
                    {
                        if (num == 0)
                        {
                            result = new EffectInteger(base.Id, this.m_value, this);
                        }
                        else
                        {
                            result = new EffectInteger(base.Id, num, this);
                        }
                    }
                    else
                    {
                        result = new EffectInteger(base.Id, (short)asyncRandom.Next((int)num2, (int)(num + 1)), this);
                    }
                }
            }
            return result;
        }
        protected override void InternalSerialize(ref System.IO.BinaryWriter writer)
        {
            base.InternalSerialize(ref writer);
            writer.Write(this.DiceNum);
            writer.Write(this.DiceFace);
        }
        internal override void DeSerialize(IDataReader reader)
        {
            base.DeSerialize(reader);
            this.m_dicenum = reader.ReadShort();
            this.m_diceface = reader.ReadShort();
        }
        public override byte[] Serialize()
        {
            var writer = new BigEndianWriter();
            writer.WriteBytes(base.Serialize());
            writer.WriteShort(m_dicenum);
            writer.WriteShort(m_diceface);
            return writer.Data;
        }
        public override bool Equals(object obj)
        {
            bool result;
            if (!(obj is EffectDice))
            {
                result = false;
            }
            else
            {
                EffectDice effectDice = obj as EffectDice;
                result = (base.Equals(obj) && this.m_diceface == effectDice.m_diceface && this.m_dicenum == effectDice.m_dicenum);
            }
            return result;
        }
        public static bool operator ==(EffectDice a, EffectDice b)
        {
            return object.ReferenceEquals(a, b) || (a != null && b != null && a.Equals(b));
        }
        public static bool operator !=(EffectDice a, EffectDice b)
        {
            return !(a == b);
        }
        public bool Equals(EffectDice other)
        {
            return !object.ReferenceEquals(null, other) && (object.ReferenceEquals(this, other) || (base.Equals(other) && other.m_diceface == this.m_diceface && other.m_dicenum == this.m_dicenum));
        }
        public override int GetHashCode()
        {
            int num = base.GetHashCode();
            num = (num * 397 ^ (int)this.m_diceface);
            return num * 397 ^ (int)this.m_dicenum;
        }
    }
}
