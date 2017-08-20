using Rebirth.Common.IO;
using Rebirth.Common.Protocol.Data;
using Rebirth.Common.Protocol.Enums;
using Rebirth.Common.Protocol.Types;
using Rebirth.World.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Effect.Instances
{
    public class EffectBase
    {
        private int m_delay;
        private int m_duration;
        private int m_group;
        private bool m_hidden;
        private short m_id;
        private int m_modificator;
        private int m_random;
        private SpellTargetType m_targets;
        protected Rebirth.Common.Protocol.Data.Effect m_template;
        private bool m_trigger;
        private uint m_zoneMinSize;
        private SpellShapeEnum m_zoneShape;
        private uint m_zoneSize;
        private uint m_uid;
        public bool IsSub
        { get { return EffectId.ToString().Contains("Sub"); } }
        public virtual int ProtocoleId
        {
            get
            {
                return 76;
            }
        }
        public virtual byte SerializationIdenfitier
        {
            get
            {
                return 1;
            }
        }
        public short Id
        {
            get
            {
                return this.m_id;
            }
            set
            {
                this.m_id = value;
                this.IsDirty = true;
            }
        }
        public uint Uid
        {
            get
            {
                return this.m_uid;
            }
            set
            {
                this.m_uid = value;
                this.IsDirty = true;
            }
        }
        public EffectsEnum EffectId
        {
            get
            {
                return (EffectsEnum)this.Id;
            }
            set
            {
                this.Id = (short)value;
                this.IsDirty = true;
            }
        }
        public Rebirth.Common.Protocol.Data.Effect Template
        {
            get
            {
                Rebirth.Common.Protocol.Data.Effect arg_23_0;
                if ((arg_23_0 = this.m_template) == null)
                {
                    arg_23_0 = (this.m_template = EffectManager.Instance.GetTemplate(this.Id));
                }
                return arg_23_0;
            }
            set
            {
                this.m_template = value;
                this.IsDirty = true;
            }
        }
        public SpellTargetType Targets
        {
            get
            {
                return this.m_targets;
            }
            set
            {
                this.m_targets = value;
                this.IsDirty = true;
            }
        }
        public int Duration
        {
            get
            {
                return this.m_duration;
            }
            set
            {
                this.m_duration = value;
                this.IsDirty = true;
            }
        }
        public int Delay
        {
            get
            {
                return this.m_delay;
            }
            set
            {
                this.m_delay = value;
                this.IsDirty = true;
            }
        }
        public int Random
        {
            get
            {
                return this.m_random;
            }
            set
            {
                this.m_random = value;
                this.IsDirty = true;
            }
        }
        public int Group
        {
            get
            {
                return this.m_group;
            }
            set
            {
                this.m_group = value;
                this.IsDirty = true;
            }
        }
        public int Modificator
        {
            get
            {
                return this.m_modificator;
            }
            set
            {
                this.m_modificator = value;
                this.IsDirty = true;
            }
        }
        public bool Trigger
        {
            get
            {
                return this.m_trigger;
            }
            set
            {
                this.m_trigger = value;
                this.IsDirty = true;
            }
        }
        public bool Hidden
        {
            get
            {
                return this.m_hidden;
            }
            set
            {
                this.m_hidden = value;
                this.IsDirty = true;
            }
        }
        public bool VisibleInTooltip
        { get; set; }
        public uint ZoneSize
        {
            get
            {
                return (uint)((this.m_zoneSize >= 63u) ? 63 : ((byte)this.m_zoneSize));
            }
            set
            {
                this.m_zoneSize = value;
                this.IsDirty = true;
            }
        }
        public string TargetMask
        {
            get;
            set;
        }
        public SpellShapeEnum ZoneShape
        {
            get
            {
                return this.m_zoneShape;
            }
            set
            {
                this.m_zoneShape = value;
                this.IsDirty = true;
            }
        }
        public uint ZoneMinSize
        {
            get
            {
                return (uint)((this.m_zoneMinSize >= 63u) ? 63 : ((byte)this.m_zoneMinSize));
            }
            set
            {
                this.m_zoneMinSize = value;
                this.IsDirty = true;
            }
        }
        public bool IsDirty
        {
            get;
            set;
        }
        public EffectBase()
        {
        }
        public EffectBase(EffectBase effect)
        {
            this.m_id = effect.Id;
            this.m_template = EffectManager.Instance.GetTemplate(effect.Id);
            this.m_targets = effect.Targets;
            this.m_delay = effect.Delay;
            this.m_duration = effect.Duration;
            this.m_group = effect.Group;
            this.m_random = effect.Random;
            this.m_modificator = effect.Modificator;
            this.m_trigger = effect.Trigger;
            this.m_hidden = effect.Hidden;
            this.m_zoneSize = effect.m_zoneSize;
            this.m_zoneMinSize = effect.m_zoneMinSize;
            this.m_zoneShape = effect.ZoneShape;
        }
        public EffectBase(short id, EffectBase effect)
        {
            this.m_id = id;
            this.m_template = EffectManager.Instance.GetTemplate(id);
            this.m_targets = effect.Targets;
            this.m_delay = effect.Delay;
            this.m_duration = effect.Duration;
            this.m_group = effect.Group;
            this.m_random = effect.Random;
            this.m_modificator = effect.Modificator;
            this.m_trigger = effect.Trigger;
            this.m_hidden = effect.Hidden;
            this.m_zoneSize = effect.m_zoneSize;
            this.m_zoneMinSize = effect.m_zoneMinSize;
            this.m_zoneShape = effect.ZoneShape;
        }
        public EffectBase(EffectInstance effect)
        {
            if(effect != null)
            {
                this.m_id = (short)effect.effectId;
                this.m_uid = effect.effectUid;
                this.m_template = EffectManager.Instance.GetTemplate(this.Id);
                this.TargetMask = effect.targetMask;
                this.m_targets = ParseTargets(effect.targetMask);
                this.m_delay = effect.delay;
                this.m_duration = effect.duration;
                this.m_group = effect.group;
                this.m_random = effect.random;
                this.m_modificator = effect.modificator;
                this.m_trigger = effect.trigger;
                this.VisibleInTooltip = effect.visibleInTooltip;
                this.ParseRawZone(effect.rawZone);
            }
        }
        private SpellTargetType ParseTargets(string targets)
        {
            SpellTargetType type = new SpellTargetType();
            foreach (string target in targets.Split(','))
            {
                switch (target)
                {
                    case "a":
                    case "l":
                        type |= SpellTargetType.ALLY_ALL;
                        break;
                    case "g":
                        type |= SpellTargetType.ALLY_NoSELF;
                        break;
                    case "A":
                    case "L":
                    case "G":
                        type |= SpellTargetType.ENEMY_ALL;
                        break;
                    case "J":
                    case "I":
                        type |= SpellTargetType.ENNEMY_SUMMONS;
                        break;
                    case "j":
                    case "i":
                        type |= SpellTargetType.ALLY_SUMMONS;
                        break;
                    case "C":
                        type |= SpellTargetType.ONLY_SELF;
                        break;
                    case "c":
                        type |= SpellTargetType.SELF;
                        break;
                    case "T":
                        type |= SpellTargetType.TELEPORT_FIGHTER;
                        break;
                }
            }
            return type;
        }

        public object Clone()
        {
            return base.MemberwiseClone();
        }
        protected void ParseRawZone(string rawZone)
        {
            if (string.IsNullOrEmpty(rawZone))
            {
                this.m_zoneShape = (SpellShapeEnum)0;
                this.m_zoneSize = 0u;
                this.m_zoneMinSize = 0u;
            }
            else
            {
                SpellShapeEnum zoneShape = (SpellShapeEnum)rawZone[0];
                byte zoneSize = 0;
                byte zoneMinSize = 0;
                int num = rawZone.IndexOf(',');
                string[] datas = rawZone.Split(',');
                try
                {
                    if (datas[0].Count() > 1)
                        zoneSize = byte.Parse(datas[0].Substring(1));
                    else
                        zoneSize = 0;
                    if(datas.Count() > 1)
                        zoneMinSize = byte.Parse(datas[1]);
                    if (datas.Count() > 2)
                        StopAtTarget = bool.Parse(datas[2]);
                }
                catch (System.Exception)
                {
                    this.m_zoneShape = (SpellShapeEnum)0;
                    this.m_zoneSize = 0u;
                    this.m_zoneMinSize = 0u;
                    Starter.Logger.Error("ParseRawZone() => Cannot parse {0}", rawZone);
                }
                this.m_zoneShape = zoneShape;
                this.m_zoneSize = (uint)zoneSize;
                this.m_zoneMinSize = (uint)zoneMinSize;
            }
        }
        public bool StopAtTarget
        {
            get;
            set;
        }
        protected string BuildRawZone()
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            stringBuilder.Append((char)this.ZoneShape);
            stringBuilder.Append(this.ZoneSize);
            if (this.ZoneMinSize > 0u)
            {
                stringBuilder.Append(",");
                stringBuilder.Append(this.ZoneMinSize);
            }
            return stringBuilder.ToString();
        }
        public virtual object[] GetValues()
        {
            return new object[0];
        }
        public virtual EffectBase GenerateEffect(EffectGenerationContext context, EffectGenerationType type = EffectGenerationType.Normal)
        {
            return new EffectBase(this);
        }
        public virtual ObjectEffect GetObjectEffect()
        {
            return new ObjectEffect((uint)this.Id);
        }
        public virtual EffectInstance GetEffectInstance()
        {
            return new EffectInstance
            {
                effectId = (uint)this.Id,
                targetId = (int)this.Targets,
                delay = this.Delay,
                duration = this.Duration,
                group = this.Group,
                random = this.Random,
                modificator = this.Modificator,
                trigger = this.Trigger,
                zoneMinSize = this.ZoneMinSize,
                zoneSize = this.ZoneSize,
                zoneShape = (uint)this.ZoneShape
            };
        }

        protected virtual void InternalSerialize(ref System.IO.BinaryWriter writer)
        {
            writer.Write((byte)1);
                writer.Write((int)this.Targets);
                writer.Write(this.Id);
                writer.Write(this.Duration);
                writer.Write(this.Delay);
                writer.Write(this.Random);
                writer.Write(this.Group);
                writer.Write(this.Modificator);
                writer.Write(this.Trigger);
                writer.Write(this.Hidden);
                string text = this.BuildRawZone();
                if (text == null)
                {
                    writer.Write(string.Empty);
                }
                else
                {
                    writer.Write(text);
                }
        }
        public static EffectBase DeserializeEffect(IDataReader reader)
        {
            EffectBase effectBase;
            switch (reader.ReadByte())
            {
                case 1:
                    effectBase = new EffectBase();
                    break;
                case 2:
                    effectBase = new EffectCreature();
                    break;
                case 3:
                    effectBase = new EffectDate();
                    break;
                case 4:
                    effectBase = new EffectDice();
                    break;
                case 5:
                    effectBase = new EffectDuration();
                    break;
                case 6:
                    effectBase = new EffectInteger();
                    break;
                case 7:
                    effectBase = new EffectLadder();
                    break;
                case 8:
                    effectBase = new EffectMinMax();
                    break;
                case 9:
                    effectBase = new EffectMount();
                    break;
                case 10:
                    effectBase = new EffectString();
                    break;
                default:
                    return null;
            }
            effectBase.DeSerialize(reader);
            return effectBase;
        }

        internal virtual void DeSerialize(IDataReader reader)
        {
            this.m_targets = (SpellTargetType)reader.ReadInt();
            this.m_id = reader.ReadShort();
            this.m_duration = reader.ReadInt();
            this.m_delay = reader.ReadInt();
            this.m_random = reader.ReadInt();
            this.m_group = reader.ReadInt();
            this.m_modificator = reader.ReadInt();
            this.m_trigger = reader.ReadBoolean();
            this.m_hidden = reader.ReadBoolean();
            this.ParseRawZone(reader.ReadUTF());
        }
        public virtual byte[] Serialize()
        {
            var writer = new BigEndianWriter();
            writer.WriteByte(SerializationIdenfitier);
            writer.WriteInt((int)this.Targets);
            writer.WriteShort(this.Id);
            writer.WriteInt(this.Duration);
            writer.WriteInt(this.Delay);
            writer.WriteInt(this.Random);
            writer.WriteInt(this.Group);
            writer.WriteInt(this.Modificator);
            writer.WriteBoolean(this.Trigger);
            writer.WriteBoolean(this.Hidden);
            string text = this.BuildRawZone();
            if (text == null)
            {
                writer.WriteUTF(string.Empty);
            }
            else
            {
                writer.WriteUTF(text);
            }
            return writer.Data;
        }
        public override bool Equals(object obj)
        {
            return !object.ReferenceEquals(null, obj) && (object.ReferenceEquals(this, obj) || (!(obj.GetType() != typeof(EffectBase)) && this.Equals((EffectBase)obj)));
        }
        public static bool operator ==(EffectBase left, EffectBase right)
        {
            return object.Equals(left, right);
        }
        public static bool operator !=(EffectBase left, EffectBase right)
        {
            return !object.Equals(left, right);
        }
        public bool Equals(EffectBase other)
        {
            return !object.ReferenceEquals(null, other) && (object.ReferenceEquals(this, other) || other.Id == this.Id);
        }
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
