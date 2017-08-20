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
    public class EffectLadder : EffectCreature
    {
        protected short m_monsterCount;
        public short MonsterCount
        {
            get
            {
                return this.m_monsterCount;
            }
            set
            {
                this.m_monsterCount = value;
                base.IsDirty = true;
            }
        }
        public override int ProtocoleId
        {
            get
            {
                return 81;
            }
        }
        public override byte SerializationIdenfitier
        {
            get
            {
                return 7;
            }
        }
        public EffectLadder()
        {
        }
        public EffectLadder(EffectLadder copy)
            : this(copy.Id, copy.MonsterFamily, copy.MonsterCount, copy)
        {
        }
        public EffectLadder(short id, short monsterfamily, short monstercount, EffectBase effect)
            : base(id, monsterfamily, effect)
        {
            this.m_monsterCount = monstercount;
        }
        public EffectLadder(EffectInstanceLadder effect)
            : base(effect)
        {
            this.m_monsterCount = (short)effect.monsterCount;
        }
        public override object[] GetValues()
        {
            return new object[]
			{
				this.m_monsterCount,
				this.m_monsterfamily
			};
        }
        public override ObjectEffect GetObjectEffect()
        {
            return new ObjectEffectLadder((uint)base.Id, (uint)base.MonsterFamily, (uint)this.MonsterCount);
        }
        public override EffectInstance GetEffectInstance()
        {
            return new EffectInstanceLadder
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
                monsterCount = (uint)this.m_monsterCount,
                monsterFamilyId = (uint)this.m_monsterfamily
            };
        }
        public override EffectBase GenerateEffect(EffectGenerationContext context, EffectGenerationType type = EffectGenerationType.Normal)
        {
            return new EffectLadder(this);
        }
        protected override void InternalSerialize(ref System.IO.BinaryWriter writer)
        {
            base.InternalSerialize(ref writer);
            writer.Write(this.m_monsterCount);
        }
        internal override void DeSerialize(IDataReader reader)
        {
            base.DeSerialize(reader);
            this.m_monsterCount = reader.ReadShort();
        }
        public override byte[] Serialize()
        {
            var writer = new BigEndianWriter();
            writer.WriteBytes(base.Serialize());
            writer.WriteShort(m_monsterCount);
            return writer.Data;
        }
        public override bool Equals(object obj)
        {
            return obj is EffectLadder && base.Equals(obj) && this.m_monsterCount == (obj as EffectLadder).m_monsterCount;
        }
        public static bool operator ==(EffectLadder a, EffectLadder b)
        {
            return object.ReferenceEquals(a, b) || (a != null && b != null && a.Equals(b));
        }
        public static bool operator !=(EffectLadder a, EffectLadder b)
        {
            return !(a == b);
        }
        public bool Equals(EffectLadder other)
        {
            return !object.ReferenceEquals(null, other) && (object.ReferenceEquals(this, other) || (base.Equals(other) && other.m_monsterCount == this.m_monsterCount));
        }
        public override int GetHashCode()
        {
            return base.GetHashCode() * 397 ^ this.m_monsterCount.GetHashCode();
        }
    }
}
