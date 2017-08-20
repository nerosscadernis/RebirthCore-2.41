using Rebirth.Common.Extensions;
using Rebirth.Common.GameData.D2O;
using Rebirth.Common.Pool;
using Rebirth.Common.Protocol.Data;
using Rebirth.World.Game.Effect.Instances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rebirth.World.Managers
{
    public class ItemManager : DataManager<ItemManager>
    {
        #region Vars
        private UniqueIdProvider _provider;
        #endregion

        #region Init
        public void Init()
        {
            _provider = new UniqueIdProvider(CharacterManager.Instance.GetMaxItemId());
        }
        #endregion

        #region Methods
        public int Pop()
        { return _provider.Pop(); }

        public System.Collections.Generic.List<EffectBase> GenerateItemEffects(Item template, EffectGenerationType max = EffectGenerationType.Normal)
        {
            System.Collections.Generic.List<EffectBase> source = (
                from effect in EffectManager.Instance.ConvertExportedEffect(template.possibleEffects)
                select EffectManager.Instance.IsUnRandomableWeaponEffect(effect.EffectId) ? effect : effect.GenerateEffect(EffectGenerationContext.Item, max)).ToList<EffectBase>();
            return source.ToList<EffectBase>();
        }

        public bool IsStuff(int itemId)
        {
            var item = ObjectDataManager.Get<Item>(itemId);
            return item != null && (item.typeId == 16
                || item.typeId == 17
                || item.typeId == 81
                || item.typeId == 16
                || item.typeId == 10
                || item.typeId == 11
                || item.typeId == 1
                || item.typeId == 82
                || item.typeId == 4
                || item.typeId == 3
                || item.typeId == 19
                || item.typeId == 8
                || item.typeId == 5
                || item.typeId == 6
                || item.typeId == 21
                || item.typeId == 2
                || item.typeId == 7
                || item.typeId == 18
                || item.typeId == 121
                || item.typeId == 9
                || item.typeId == 23
                || item.typeId == 151);
        }
        #endregion
    }
}
