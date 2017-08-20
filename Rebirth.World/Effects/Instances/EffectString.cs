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
    public class EffectString : EffectBase
    {
        protected string m_value;
        public override int ProtocoleId
        {
            get
            {
                return 74;
            }
        }
        public override byte SerializationIdenfitier
        {
            get
            {
                return 10;
            }
        }
        public EffectString()
        {
        }
        public EffectString(EffectString copy)
            : this(copy.Id, copy.m_value, copy)
        {
        }
        public EffectString(short id, string value, EffectBase effect)
            : base(id, effect)
        {
            this.m_value = value;
        }
        public EffectString(EffectInstanceString effect)
            : base(effect)
        {
            this.m_value = effect.text;
        }
        public override object[] GetValues()
        {
            return new object[]
			{
				this.m_value
			};
        }
        public override ObjectEffect GetObjectEffect()
        {
            return new ObjectEffectString((uint)base.Id, this.m_value);
        }
        public override EffectInstance GetEffectInstance()
        {
            return new EffectInstanceString
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
                text = this.m_value
            };
        }
        public override EffectBase GenerateEffect(EffectGenerationContext context, EffectGenerationType type = EffectGenerationType.Normal)
        {
            return new EffectString(this);
        }
        protected override void InternalSerialize(ref System.IO.BinaryWriter writer)
        {
            base.InternalSerialize(ref writer);
            writer.Write(this.m_value);
        }
        internal override void DeSerialize(IDataReader reader)
        {
            base.DeSerialize(reader);
            this.m_value = reader.ReadUTF();
        }
        public override byte[] Serialize()
        {
            var writer = new BigEndianWriter();
            writer.WriteBytes(base.Serialize());
            writer.WriteUTF(m_value);
            return writer.Data;
        }
        public override bool Equals(object obj)
        {
            return obj is EffectString && base.Equals(obj) && this.m_value == (obj as EffectString).m_value;
        }
        public static bool operator ==(EffectString a, EffectString b)
        {
            return object.ReferenceEquals(a, b) || (a != null && b != null && a.Equals(b));
        }
        public static bool operator !=(EffectString a, EffectString b)
        {
            return !(a == b);
        }
        public bool Equals(EffectString other)
        {
            return !object.ReferenceEquals(null, other) && (object.ReferenceEquals(this, other) || (base.Equals(other) && object.Equals(other.m_value, this.m_value)));
        }
        public override int GetHashCode()
        {
            return base.GetHashCode() * 397 ^ ((this.m_value != null) ? this.m_value.GetHashCode() : 0);
        }
    }
}
