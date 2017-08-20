using Rebirth.Common.GameData.D2O;
using Rebirth.Common.IO;
using Rebirth.Common.Protocol.Data;
using Rebirth.Common.Protocol.Enums;
using Rebirth.Common.Protocol.Types;
using Rebirth.World.Datas.Bdd;
using Rebirth.World.Datas.Characters;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Effects.Usables;
using Rebirth.World.Game.Effect.Instances;
using Rebirth.World.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Rebirth.World.Effects.Handlers.Items.ItemEffectHandler;

namespace Rebirth.World.Datas.Items
{
    public class PlayerItem
    {
        #region Vars
        private ConditionExpression _criteriaExpression;
        private Character _owner;
        #endregion

        #region Properties
        public int Guid
        { get; set; }
        public int Gid
        { get; set; }
        public int Quantity
        { get; set; }
        public List<EffectBase> Effects
        { get; set; }
        public CharacterInventoryPositionEnum Position
        { get; set; }
        public uint Weight
        { get { return unchecked(Template.weight * (uint)Quantity); } }
        public Common.Protocol.Data.Item Template
        {
            get { return ObjectDataManager.Get<Common.Protocol.Data.Item>(Gid); }
        }
        public ItemType Type
        {
            get { return ObjectDataManager.Get<ItemType>((int)Template.typeId); }
        }
        public virtual bool AllowDropping
        {
            get
            {
                return false;
            }
        }
        public virtual bool AllowFeeding
        {
            get
            {
                return false;
            }
        }
        public virtual bool IsUsable
        {
            get
            {
                return this.Template.usable;
            }
        }
        public ConditionExpression CriteriaExpression
        {
            get
            {
                ConditionExpression result;
                if (string.IsNullOrEmpty(this.Template.criteria) || this.Template.criteria == "null")
                {
                    result = null;
                }
                else
                {
                    ConditionExpression arg_47_0;
                    if ((arg_47_0 = this._criteriaExpression) == null)
                    {
                        arg_47_0 = (this._criteriaExpression = ConditionExpression.Parse(this.Template.criteria));
                    }
                    result = arg_47_0;
                }
                return result;
            }
            set
            {
                this._criteriaExpression = value;
                this.Template.criteria = ((value != null) ? value.ToString() : null);
            }
        }
        #endregion

        #region Constructors / Datas
        public PlayerItem(Character owner)
        {
            _owner = owner;
        }

        public PlayerItem(Character owner, int gid, int amount, EffectGenerationType maxEffects = EffectGenerationType.Normal, int mountId = 0)
        {
            Guid = ItemManager.Instance.Pop();
            Gid = gid;
            var effects = ItemManager.Instance.GenerateItemEffects(Template, maxEffects);
            if (Template.possibleEffects.Any(x => x.effectId >= 970 && x.effectId <= 974))
            {
                var level = effects.FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_LivingObjectLevel) as EffectInteger;
                level.Value = 1;
                var mood = effects.FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_LivingObjectMood) as EffectInteger;
                mood.Value = 1;
                var skin = effects.FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_LivingObjectSkin) as EffectInteger;
                skin.Value = 0;
                var category = effects.FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_LivingObjectCategory) as EffectInteger;

            }
            Quantity = amount;
            Position = CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED;
            Effects = effects.FindAll(x => !(x is EffectDate) && !(x is EffectString)
                            && x.EffectId != (EffectsEnum)1175
                            && x.EffectId != (EffectsEnum)983
                            && x.EffectId != (EffectsEnum)806
                            && x.EffectId != (EffectsEnum)807
                            && x.EffectId != EffectsEnum.Effect_LastMeal).ToList();
        }

        public PlayerItem(Character owner, int gid, int amount, System.Collections.Generic.List<EffectBase> effects, int mountId = 0)
        {
            Guid = ItemManager.Instance.Pop();
            Gid = gid;
            if (Template.possibleEffects.Any(x => x.effectId >= 970 && x.effectId <= 974))
            {
                var level = effects.FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_LivingObjectLevel) as EffectInteger;
                level.Value = 1;
                var mood = effects.FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_LivingObjectMood) as EffectInteger;
                mood.Value = 1;
                var skin = effects.FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_LivingObjectSkin) as EffectInteger;
                skin.Value = 0;
                var category = effects.FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_LivingObjectCategory) as EffectInteger;

            }
            Quantity = amount;
            Position = CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED;
            Effects = effects.FindAll(x => !(x is EffectDate) && !(x is EffectString)
                            && x.EffectId != (EffectsEnum)1175
                            && x.EffectId != (EffectsEnum)983
                            && x.EffectId != (EffectsEnum)806
                            && x.EffectId != (EffectsEnum)807
                            && x.EffectId != EffectsEnum.Effect_LastMeal).ToList();
        }

        public PlayerItem(Character owner, IDataReader reader)
        {
            Effects = new List<EffectBase>();
            Guid = reader.ReadInt();
            Gid = reader.ReadInt();
            Quantity = reader.ReadInt();
            var count = reader.ReadShort();
            for (int i = 0; i < count; i++)
            {
                var effect = EffectBase.DeserializeEffect(reader);
                if(effect != null)
                    Effects.Add(effect);
            }
            Position = (CharacterInventoryPositionEnum)reader.ReadByte();
        }

        public byte[] GetDatas()
        {
            var writer = new BigEndianWriter();
            writer.WriteInt(Guid);
            writer.WriteInt(Gid);
            writer.WriteInt(Quantity);
            writer.WriteShort((short)Effects.Count);
            foreach (var effect in Effects)
            {
                writer.WriteBytes(effect.Serialize());
            }
            writer.WriteByte((byte)Position);
            return writer.Data;
        }
        #endregion

        #region Functions
        public virtual bool IsStackableWith(PlayerItem compared)
        {
            return compared.Gid == Gid && compared.Position == CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED && compared.Effects.Except(Effects).Count() == 0;
        }

        public void ApplyStats(HandlerOperation operation)
        {
            foreach (var effect in Effects)
            {
                var handler = EffectManager.Instance.GetItemEffectHandler(effect, _owner, this);
                handler.Operation = operation;
                handler.Apply(null);
            }
        }

        public virtual bool IsEquiped()
        {
            return this.Position != CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED;
        }

        public virtual bool Feed(PlayerItem food)
        {
            return false;
        }

        public virtual bool AreConditionFilled(Character character)
        {
            bool result;
            try
            {
                result = (this.CriteriaExpression == null || this.CriteriaExpression.Eval(character));
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public virtual uint UseItem(uint amount = 1u, CellMap targetCell = null, Character target = null)
        {
            uint num = 0u;
            foreach (UsableEffectHandler current in
                from effect in this.Effects
                select EffectManager.Instance.GetUsableEffectHandler(effect, target ?? _owner, this))
            {
                current.NumberOfUses = amount;
                current.TargetCell = targetCell;
                if (current.Apply(null))
                {
                    num = System.Math.Max(current.UsedItems, num);
                }
            }
            return num;
        }
        public virtual bool Drop(PlayerItem dropOnItem)
        {
            return false;
        }
        public ObjectItem GetObjectItem()
        {
            var effects = (from entry in this.Effects
                           where !entry.Hidden
                           select entry.GetObjectEffect()).ToArray();

            return new ObjectItem((byte)this.Position, (uint)this.Template.id, effects, (uint)this.Guid, (uint)this.Quantity);
        }

        public ObjectItemQuantity GetObjectItemQuantity()
        {
            return new ObjectItemQuantity((uint)Guid, (uint)Quantity);
        }

        public ObjectItemNotInContainer GetObjectItemNotInContainer()
        {
            return new ObjectItemNotInContainer((uint)this.Template.id, (from entry in this.Effects
                                                                         where !entry.Hidden
                                                                         select entry.GetObjectEffect()).ToArray(), (uint)this.Guid, (uint)this.Quantity);
        }
        public PlayerItem Clone()
        {
            return this.MemberwiseClone() as PlayerItem;
        }
        #endregion
    }
}
