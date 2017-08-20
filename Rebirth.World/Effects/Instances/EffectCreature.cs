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
    public class EffectCreature : EffectBase
    {
        protected short m_monsterfamily;
        public override int ProtocoleId
        {
            get
            {
                return 71;
            }
        }
        public override byte SerializationIdenfitier
        {
            get
            {
                return 2;
            }
        }
        public short MonsterFamily
        {
            get
            {
                return this.m_monsterfamily;
            }
        }
        public EffectCreature()
        {
        }
        public EffectCreature(EffectCreature copy)
            : this(copy.Id, copy.MonsterFamily, copy)
        {
        }
        public EffectCreature(short id, short monsterfamily, EffectBase effectBase)
            : base(id, effectBase)
        {
            this.m_monsterfamily = monsterfamily;
        }
        public EffectCreature(EffectInstanceCreature effect)
            : base(effect)
        {
            this.m_monsterfamily = (short)effect.monsterFamilyId;
        }
        public override object[] GetValues()
        {
            return new object[]
			{
				this.m_monsterfamily
			};
        }
        public override ObjectEffect GetObjectEffect()
        {
            return new ObjectEffectCreature((uint)base.Id,(uint) this.MonsterFamily);
        }
        public override EffectBase GenerateEffect(EffectGenerationContext context, EffectGenerationType type = EffectGenerationType.Normal)
        {
            return new EffectCreature(this);
        }
        public override EffectInstance GetEffectInstance()
        {
            return new EffectInstanceCreature
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
                monsterFamilyId = (uint)this.MonsterFamily
            };
        }
        public override byte[] Serialize()
        {
            var writer = new BigEndianWriter();
            writer.WriteBytes(base.Serialize());
            writer.WriteShort(MonsterFamily);
            return writer.Data;
        }
        protected override void InternalSerialize(ref System.IO.BinaryWriter writer)
        {
            base.InternalSerialize(ref writer);
            writer.Write(this.MonsterFamily);
        }
        internal override void DeSerialize(IDataReader reader)
        {
            base.DeSerialize(reader);
            this.m_monsterfamily = reader.ReadShort();
        }
        public override bool Equals(object obj)
        {
            return obj is EffectCreature && base.Equals(obj) && this.m_monsterfamily == (obj as EffectCreature).m_monsterfamily;
        }
        public static bool operator ==(EffectCreature a, EffectCreature b)
        {
            return object.ReferenceEquals(a, b) || (a != null && b != null && a.Equals(b));
        }
        public static bool operator !=(EffectCreature a, EffectCreature b)
        {
            return !(a == b);
        }
        public bool Equals(EffectCreature other)
        {
            return !object.ReferenceEquals(null, other) && (object.ReferenceEquals(this, other) || (base.Equals(other) && other.m_monsterfamily == this.m_monsterfamily));
        }
        public override int GetHashCode()
        {
            return base.GetHashCode() * 397 ^ this.m_monsterfamily.GetHashCode();
        }
    }
}
