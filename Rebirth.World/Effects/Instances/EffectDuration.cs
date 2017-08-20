using Rebirth.Common.IO;
using Rebirth.Common.Protocol.Data;
using Rebirth.Common.Protocol.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Effect.Instances
{
    public class EffectDuration : EffectBase
    {
        protected short m_days;
        protected short m_hours;
        protected short m_minutes;
        public override int ProtocoleId
        {
            get
            {
                return 75;
            }
        }
        public override byte SerializationIdenfitier
        {
            get
            {
                return 5;
            }
        }
        public EffectDuration()
        {
        }
        public EffectDuration(EffectDuration copy)
            : this(copy.Id, copy.m_days, copy.m_hours, copy.m_minutes, copy)
        {
        }
        public EffectDuration(short id, short days, short hours, short minutes, EffectBase effect)
            : base(id, effect)
        {
            this.m_days = days;
            this.m_hours = hours;
            this.m_minutes = minutes;
        }
        public EffectDuration(EffectInstanceDuration effect)
            : base(effect)
        {
            this.m_days = (short)effect.days;
            this.m_hours = (short)effect.hours;
            this.m_minutes = (short)effect.minutes;
        }
        public override object[] GetValues()
        {
            return new object[]
			{
				this.m_days,
				this.m_hours,
				this.m_minutes
			};
        }
        public override ObjectEffect GetObjectEffect()
        {
            return new ObjectEffectDuration((uint)base.Id, (uint)this.m_days, (sbyte)this.m_hours, (sbyte)this.m_minutes);
        }
        public override EffectInstance GetEffectInstance()
        {
            return new EffectInstanceDuration
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
                days = (uint)this.m_days,
                hours = (uint)this.m_hours,
                minutes = (uint)this.m_minutes
            };
        }
        public override EffectBase GenerateEffect(EffectGenerationContext context, EffectGenerationType type = EffectGenerationType.Normal)
        {
            return new EffectDuration(this);
        }
        protected override void InternalSerialize(ref System.IO.BinaryWriter writer)
        {
            base.InternalSerialize(ref writer);
            writer.Write(this.m_days);
            writer.Write(this.m_hours);
            writer.Write(this.m_minutes);
        }
        internal override void DeSerialize(IDataReader reader)
        {
            base.DeSerialize(reader);
            this.m_days = reader.ReadShort();
            this.m_hours = reader.ReadShort();
            this.m_minutes = reader.ReadShort();
        }
        public override byte[] Serialize()
        {
            var writer = new BigEndianWriter();
            writer.WriteBytes(base.Serialize());
            writer.WriteShort(m_days);
            writer.WriteShort(m_hours);
            writer.WriteShort(m_minutes);
            return writer.Data;
        }
        public System.TimeSpan GetTimeSpan()
        {
            return new System.TimeSpan((int)this.m_days, (int)this.m_hours, (int)this.m_minutes, 0);
        }
        public override bool Equals(object obj)
        {
            return obj is EffectDuration && base.Equals(obj) && this.GetTimeSpan().Equals((obj as EffectDuration).GetTimeSpan());
        }
        public static bool operator ==(EffectDuration a, EffectDuration b)
        {
            return object.ReferenceEquals(a, b) || (a != null && b != null && a.Equals(b));
        }
        public static bool operator !=(EffectDuration a, EffectDuration b)
        {
            return !(a == b);
        }
        public bool Equals(EffectDuration other)
        {
            return !object.ReferenceEquals(null, other) && (object.ReferenceEquals(this, other) || (base.Equals(other) && other.m_days == this.m_days && other.m_hours == this.m_hours && other.m_minutes == this.m_minutes));
        }
        public override int GetHashCode()
        {
            int num = base.GetHashCode();
            num = (num * 397 ^ this.m_days.GetHashCode());
            num = (num * 397 ^ this.m_hours.GetHashCode());
            return num * 397 ^ this.m_minutes.GetHashCode();
        }
    }
}
