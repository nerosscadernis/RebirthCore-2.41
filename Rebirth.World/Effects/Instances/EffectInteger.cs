using Rebirth.Common.IO;
using Rebirth.Common.Protocol.Data;
using Rebirth.Common.Protocol.Enums;
using Rebirth.Common.Protocol.Types;

namespace Rebirth.World.Game.Effect.Instances
{
    public class EffectInteger : EffectBase
    {
        protected int m_value;
        private CharacterInventoryPositionEnum INVENTORY_POSITION_NOT_EQUIPED;
        private int p;
        public override int ProtocoleId
        {
            get
            {
                return 70;
            }
        }
        public override byte SerializationIdenfitier
        {
            get
            {
                return 6;
            }
        }
        public int Value
        {
            get
            {
                return this.m_value;
            }
            set
            {
                this.m_value = value;
                base.IsDirty = true;
            }
        }
        public EffectInteger()
        {
        }
        public EffectInteger(EffectInteger copy)
            : this(copy.Id, copy.Value, copy)
        {
        }
        public EffectInteger(short id, int value, EffectBase effect)
            : base(id, effect)
        {
            this.m_value = value;
        }
        public EffectInteger(EffectsEnum id, int value)
            : this((short)id, value, new EffectBase())
        {
        }
        public EffectInteger(EffectInstanceInteger effect)
            : base(effect)
        {
            this.m_value = effect.value;
        }

        public EffectInteger(CharacterInventoryPositionEnum INVENTORY_POSITION_NOT_EQUIPED, int p)
        {
            // TODO: Complete member initialization
            this.INVENTORY_POSITION_NOT_EQUIPED = INVENTORY_POSITION_NOT_EQUIPED;
            this.p = p;
        }
        public override object[] GetValues()
        {
            return new object[]
			{
				this.Value
			};
        }
        public override ObjectEffect GetObjectEffect()
        {
            return new ObjectEffectInteger((uint)base.Id, (uint)this.Value);
        }
        public override EffectBase GenerateEffect(EffectGenerationContext context, EffectGenerationType type = EffectGenerationType.Normal)
        {
            return new EffectInteger(this);
        }
        protected override void InternalSerialize(ref System.IO.BinaryWriter writer)
        {
            base.InternalSerialize(ref writer);
            writer.Write(this.m_value);
        }
        internal override void DeSerialize(IDataReader reader)
        {
            base.DeSerialize(reader);
            this.m_value = reader.ReadInt();
        }
        public override byte[] Serialize()
        {
            var writer = new BigEndianWriter();
            writer.WriteBytes(base.Serialize());
            writer.WriteInt(m_value);
            return writer.Data;
        }
        public bool Equals(EffectInteger other)
        {
            return !object.ReferenceEquals(null, other) && (object.ReferenceEquals(this, other) || (base.Equals(other) && other.m_value == this.m_value));
        }
        public override bool Equals(object obj)
        {
            return !object.ReferenceEquals(null, obj) && (object.ReferenceEquals(this, obj) || this.Equals(obj as EffectInteger));
        }
        public override int GetHashCode()
        {
            return base.GetHashCode() * 397 ^ this.m_value.GetHashCode();
        }
        public static bool operator ==(EffectInteger left, EffectInteger right)
        {
            return object.Equals(left, right);
        }
        public static bool operator !=(EffectInteger left, EffectInteger right)
        {
            return !object.Equals(left, right);
        }
    }
   
}
