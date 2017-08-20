using Rebirth.Common.Protocol.Types;
using Rebirth.World.Datas.Spells;
using Rebirth.World.Game.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Fights.Other
{
    public class SpellsHistory
    {
        #region Var
        private Dictionary<int, int> CountInTurn = new Dictionary<int, int>();
        private Dictionary<int, Dictionary<int, int>> CountInTarget = new Dictionary<int, Dictionary<int, int>>();
        private Dictionary<int, int> Cooldowns = new Dictionary<int, int>();
        #endregion

        #region Public Function
        public void UsedSpell(SpellTemplate spell, int fighterId)
        {
            if (!Cooldowns.ContainsKey(spell.Spell.id) && spell.CurrentSpellLevel.minCastInterval > 0)
                Cooldowns.Add(spell.Spell.id, (int)spell.CurrentSpellLevel.minCastInterval);

            if (spell.CurrentSpellLevel.maxCastPerTurn > 0)
                if (CountInTurn.ContainsKey(spell.Spell.id) && spell.CurrentSpellLevel.maxCastPerTurn > 0)
                    CountInTurn[spell.Spell.id] += 1;
                else
                    CountInTurn.Add(spell.Spell.id, 1);

            if (fighterId != 0 && spell.CurrentSpellLevel.maxCastPerTarget > 0)
                if (CountInTarget.ContainsKey(spell.Spell.id))
                    if (CountInTarget[spell.Spell.id].ContainsKey(fighterId))
                        CountInTarget[spell.Spell.id][fighterId] += 1;
                    else
                        CountInTarget[spell.Spell.id].Add(fighterId, 1);
                else
                    CountInTarget.Add(spell.Spell.id, new Dictionary<int, int>()
                    {
                        {fighterId, 1 }
                    });
        }
        public void PassTurn()
        {
            CountInTarget.Clear();
            CountInTurn.Clear();

            var tmp = new Dictionary<int, int>();
            foreach (var item in Cooldowns)
            {
                if ((Cooldowns[item.Key] - 1) > 0)
                    tmp.Add(item.Key, item.Value - 1);
            }
            Cooldowns = tmp;
        }

        public bool CanUseSpell(SpellTemplate spell)
        {
            if (Cooldowns.ContainsKey(spell.Spell.id))
                return false;
            if (CountInTurn.ContainsKey(spell.Spell.id) && CountInTurn[spell.Spell.id] >= spell.CurrentSpellLevel.maxCastPerTurn)
                return false;
            return true;
        }
        public bool CanUseSpellInTarget(SpellTemplate spell, int fighterId)
        {
            if (CountInTarget.ContainsKey(spell.Spell.id) && CountInTarget[spell.Spell.id].ContainsKey(fighterId) 
                && spell.CurrentSpellLevel.maxCastPerTarget <= CountInTarget[spell.Spell.id][fighterId])
                return false;
            return true;
        }

        public bool ReducCooldown(SpellTemplate spell, int nbrTurn)
        {
            if (Cooldowns.ContainsKey(spell.Spell.id))
            {
                if (nbrTurn == 0)
                    Cooldowns.Remove(spell.Spell.id);
                else
                    this.Cooldowns[spell.Spell.id] = nbrTurn;
                return true;
            }
            else
            {
                this.Cooldowns.Add(spell.Spell.id, nbrTurn);
            }
            return false;
        }
        #endregion

        #region Get Function
        public GameFightSpellCooldown[] GetGameFightSpellCooldowns()
        {
            return (from x in Cooldowns
                    where x.Value > 0
                    select new GameFightSpellCooldown(x.Key, (sbyte)x.Value)).ToArray();
        }
        #endregion
    }
}
