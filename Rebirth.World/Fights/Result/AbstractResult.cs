using Rebirth.Common.GameData.D2O;
using Rebirth.Common.Protocol.Data;
using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Datas.Characters;
using Rebirth.World.Datas.Items;
using Rebirth.World.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Fights.Result
{
    public class AbstractResult
    {
        protected Character m_character;

        public AbstractResult()
        { }
        public AbstractResult(Character character)
        {
            m_character = character;
        }

        #region Properties
        public double Experience { get; set; }
        public int Honor { get; set; }
        public double GuildeExp { get; set; }
        public double MountExp { get; set; }
        public int Kamas { get; set; }

        public Dictionary<int, int> Items = new Dictionary<int, int>();
        public List<int> MonstersEat = new List<int>();
        public List<PlayerItem> PlayerItems = new List<PlayerItem>();
        #endregion

        #region Function

        public virtual void AssignResult()
        {
            if (m_character != null)
            {
                if (GuildeExp > 0)
                {
                    //m_character.GuildMember.AddXP((int)GuildeExp);
                    //m_character.Guild.AddExperience((int)GuildeExp);
                }
                if (MountExp > 0)
                {
                    //var mount = m_character.Mounts.First(x => x.IsRiding);
                    //mount.RemoveStats(m_character);
                    //m_character.Mounts.First(x => x.IsRiding).Experience += (int)MountExp;
                    //mount.AddStats(m_character);
                }
                m_character.AddExperience(Experience);
                m_character.Inventory.Kamas += Kamas;

                foreach (var itemId in Items)
                {
                    Item item = ObjectDataManager.Get<Item>(itemId.Key);
                    if (item != null)
                    {
                        var newitem = new PlayerItem(m_character, item.id, itemId.Value, Game.Effect.Instances.EffectGenerationType.Radom);
                        m_character.Inventory.AddItem(newitem);
                    }
                }
                //foreach (var item in PlayerItems)
                //{
                //    item.Record.OwnerId = m_character.Id;
                //    m_character.Inventory.AddItem(item);
                //}
                //var pet = m_character.Inventory.GetItemByPosition(CharacterInventoryPositionEnum.ACCESSORY_POSITION_PETS);
                //if (pet != null && (pet.Template.typeId == 18 || pet.Template.typeId == 121))
                //    foreach (var id in MonstersEat)
                //        (pet as PetItem).Eat(id);

                foreach (var id in MonstersEat)
                    m_character.Quests.ValidParam(id, 1, 6);

                m_character.RefreshStats();
            }
        }
        #endregion
    }
}
