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
    public class EffectMinMax : EffectBase
    {
        protected short m_maxvalue;
        protected short m_minvalue;
        public override int ProtocoleId
        {
            get
            {
                return 82;
            }
        }
        public override byte SerializationIdenfitier
        {
            get
            {
                return 8;
            }
        }
        public short ValueMin
        {
            get
            {
                return this.m_minvalue;
            }
            set
            {
                this.m_minvalue = value;
                base.IsDirty = true;
            }
        }
        public short ValueMax
        {
            get
            {
                return this.m_maxvalue;
            }
            set
            {
                this.m_maxvalue = value;
                base.IsDirty = true;
            }
        }
        public EffectMinMax()
        {
        }
        public EffectMinMax(EffectMinMax copy)
            : this(copy.Id, copy.ValueMin, copy.ValueMax, copy)
        {
        }
        public EffectMinMax(short id, short valuemin, short valuemax, EffectBase effect)
            : base(id, effect)
        {
            this.m_minvalue = valuemin;
            this.m_maxvalue = valuemax;
        }
        public EffectMinMax(EffectInstanceMinMax effect)
            : base(effect)
        {
            this.m_maxvalue = (short)effect.max;
            this.m_minvalue = (short)effect.min;
        }
        public override object[] GetValues()
        {
            return new object[]
			{
				this.ValueMin,
				this.ValueMax
			};
        }
        public override ObjectEffect GetObjectEffect()
        {
            return new ObjectEffectMinMax((uint)base.Id, (uint)this.ValueMin, (uint)this.ValueMax);
        }
        public override EffectInstance GetEffectInstance()
        {
            return new EffectInstanceMinMax
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
                max = (uint)this.ValueMax,
                min = (uint)this.ValueMin
            };
        }
        protected override void InternalSerialize(ref System.IO.BinaryWriter writer)
        {
            base.InternalSerialize(ref writer);
            writer.Write(this.m_maxvalue);
            writer.Write(this.m_minvalue);
        }
        internal override void DeSerialize(IDataReader reader)
        {
            base.DeSerialize(reader);
            this.m_maxvalue = reader.ReadShort();
            this.m_minvalue = reader.ReadShort();
        }
        public override byte[] Serialize()
        {
            var writer = new BigEndianWriter();
            writer.WriteBytes(base.Serialize());
            writer.WriteShort(m_maxvalue);
            writer.WriteShort(m_minvalue);
            return writer.Data;
        }
        public override EffectBase GenerateEffect(EffectGenerationContext context, EffectGenerationType type = EffectGenerationType.Normal)
        {
            AsyncRandom asyncRandom = new AsyncRandom();
            EffectBase result;
            if (type == EffectGenerationType.MaxEffects)
            {

                result = new EffectInteger(base.Id, (base.Template.@operator != "-") ? this.ValueMax : this.ValueMin, this);
            }
            else
            {
                if (type == EffectGenerationType.MinEffects)
                {
                    result = new EffectInteger(base.Id, (base.Template.@operator != "-") ? this.ValueMin : this.ValueMax, this);
                }
                else
                {
                    result = new EffectInteger(base.Id, (short)asyncRandom.Next((int)this.ValueMin, (int)(this.ValueMax + 1)), this);
                }
            }
            return new EffectInteger(base.Id, (short)asyncRandom.Next((int)this.ValueMin, (int)(this.ValueMax + 1)), this);
        }
        public override bool Equals(object obj)
        {
            bool result;
            if (!(obj is EffectMinMax))
            {
                result = false;
            }
            else
            {
                EffectMinMax effectMinMax = obj as EffectMinMax;
                result = (base.Equals(obj) && this.m_minvalue == effectMinMax.m_minvalue && this.m_maxvalue == effectMinMax.m_maxvalue);
            }
            return result;
        }
        public static bool operator ==(EffectMinMax a, EffectMinMax b)
        {
            return object.ReferenceEquals(a, b) || (a != null && b != null && a.Equals(b));
        }
        public static bool operator !=(EffectMinMax a, EffectMinMax b)
        {
            return !(a == b);
        }
        public bool Equals(EffectMinMax other)
        {
            return !object.ReferenceEquals(null, other) && (object.ReferenceEquals(this, other) || (base.Equals(other) && other.m_maxvalue == this.m_maxvalue && other.m_minvalue == this.m_minvalue));
        }
        public override int GetHashCode()
        {
            int num = base.GetHashCode();
            num = (num * 397 ^ this.m_maxvalue.GetHashCode());
            return num * 397 ^ this.m_minvalue.GetHashCode();
        }
    }
}
