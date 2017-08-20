using Rebirth.Common.GameData.D2O;
using Rebirth.Common.Protocol.Data;
using Rebirth.Common.Protocol.Enums;
using Rebirth.Common.Timers;
using Rebirth.World.Datas.Spells;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Game.Spells;
using Rebirth.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Fights.IA.Scipting.Abstract
{
    public class FighterIAEnvironnement
    {
        private Fight m_fight;
        private AbstractIA m_fighter;
        public FighterIAEnvironnement(Fight fight, AbstractIA fighter)
        {
            m_fight = fight;
            m_fighter = fighter;
        }

        public int getMe()
        {
            return m_fighter.Id;
        }

        public int getMP()
        {
            return m_fighter.Stats.MP.Total;
        }
        public int getAP()
        {
            return m_fighter.Stats.AP.Total;
        }
  
        public int getAgility()
        {
            return m_fighter.Stats[Common.Protocol.Enums.PlayerFields.Agility].Total;
        }
        public int getIntelligence()
        {
            return m_fighter.Stats[Common.Protocol.Enums.PlayerFields.Intelligence].Total;
        }
        public int getChance()
        {
            return m_fighter.Stats[Common.Protocol.Enums.PlayerFields.Chance].Total;
        }
        public int getStrength()
        {
            return m_fighter.Stats[Common.Protocol.Enums.PlayerFields.Strength].Total;
        }
        public int getShield()
        {
            return m_fighter.Stats[Common.Protocol.Enums.PlayerFields.Shield].Total;
        }

        public bool canInvok()
        {
            return m_fighter.Stats[PlayerFields.SummonLimit].Total > m_fighter.Summons.Count;
        }

        public int getLife()
        {
            return m_fighter.Stats[Common.Protocol.Enums.PlayerFields.Health].Total;
        }

        public int getCellID()
        {
            return m_fighter.Point.Point.CellId;
        }

        public int getCellID(int id)
        {
            var actor = m_fight.GetFighter(id);
            return actor.Point.Point.CellId;
        }

        public void turnPass()
        {
           new TimerCore(PassOK, 1000);        
        }

        private void PassOK()
        {
            m_fighter.ActionPass();
        }

        public int[] getFighterCanAttack()
        {
            List<int> list = new List<int>();
            foreach(var spell in m_fighter.Spells.FindAll(x => x.effects.Any(y => Enum.IsDefined(typeof(IADamageEffectEnum), (int)y.effectId))))
            {             
                foreach (var fighter in m_fight.Fighters.Where(e => e.Team.Team != m_fighter.Team.Team))
                {
                    var template = new SpellTemplate(ObjectDataManager.Get<Spell>((int)spell.spellId), 0);

                    if(m_fighter.CanCastSpell(template, fighter.Point.Point.CellId, null))
                    {
                        list.Add(fighter.Id);
                    }
                }
            }
            return list.ToArray();
        }

        public int[] getFighterCanBoost()
        {
            List<int> list = new List<int>();
            foreach (var spell in m_fighter.Spells.FindAll(x => x.effects.Any(y => Enum.IsDefined(typeof(IABoostEffectEnum), (int)y.effectId))))
            {
                foreach (var fighter in m_fight.Fighters.Where(e => e.Team.Team == m_fighter.Team.Team))
                {
                    var template = new SpellTemplate(ObjectDataManager.Get<Spell>((int)spell.spellId), 0);

                    if (m_fighter.CanCastSpell(template, fighter.Point.Point.CellId, null))
                    {
                        list.Add(fighter.Id);
                    }
                }
            }
            return list.ToArray();
        }

        public int[] getFighterCanHeal()
        {
            List<int> list = new List<int>();
            foreach (var spell in m_fighter.Spells.FindAll(x => x.effects.Any(y => Enum.IsDefined(typeof(IAHealEffectEnum), (int)y.effectId))))
            {
                foreach (var fighter in m_fight.Fighters.Where(e => e.Team.Team == m_fighter.Team.Team && e.Stats.Health.DamageTaken > 0))
                {
                    var template = new SpellTemplate(ObjectDataManager.Get<Spell>((int)spell.spellId), 0);

                    if (m_fighter.CanCastSpell(template, fighter.Point.Point.CellId, null))
                    {
                        list.Add(fighter.Id);
                    }
                }
            }
            return list.ToArray();
        }

        public int[] getSpellCanCast(int id)
        {
            List<int> list = new List<int>();
            var fighter = m_fight.Fighters.FirstOrDefault(e => e.Id == id);
            if (fighter == null)
                return new int[0];

            foreach (var spell in m_fighter.Spells.FindAll(x => x.effects.Any(y => Enum.IsDefined(typeof(IADamageEffectEnum), (int)y.effectId))))
            {
                    var template = new SpellTemplate(ObjectDataManager.Get<Spell>((int)spell.spellId), 0);

                    if (m_fighter.CanCastSpell(template, fighter.Point.Point.CellId, null))
                    {
                        list.Add((int)spell.spellId);
                    }
            }
            return list.ToArray();
        }

        public int[] getSpellBoostCanCast(int id)
        {
            List<int> list = new List<int>();
            var fighter = m_fight.Fighters.FirstOrDefault(e => e.Id == id);
            if (fighter == null)
                return new int[0];

            foreach (var spell in m_fighter.Spells.FindAll(x => x.effects.Any(y => Enum.IsDefined(typeof(IABoostEffectEnum), (int)y.effectId))))
            {
                var template = new SpellTemplate(ObjectDataManager.Get<Spell>((int)spell.spellId), 0);

                if (m_fighter.CanCastSpell(template, fighter.Point.Point.CellId, null))
                {
                    list.Add((int)spell.spellId);
                }
            }
            return list.ToArray();
        }

        public int[] getSpellHealCanCast(int id)
        {
            List<int> list = new List<int>();
            var fighter = m_fight.Fighters.FirstOrDefault(e => e.Id == id);
            if (fighter == null)
                return new int[0];

            foreach (var spell in m_fighter.Spells.FindAll(x => x.effects.Any(y => Enum.IsDefined(typeof(IAHealEffectEnum), (int)y.effectId))))
            {
                var template = new SpellTemplate(ObjectDataManager.Get<Spell>((int)spell.spellId), 0);

                if (m_fighter.CanCastSpell(template, fighter.Point.Point.CellId, null))
                {
                    list.Add((int)spell.spellId);
                }
            }
            return list.ToArray();
        }

        public int[] getSpellBoostOnMe()
        {
            List<int> list = new List<int>();
            foreach (var spell in m_fighter.Spells.FindAll(x => x.effects.Any(y => Enum.IsDefined(typeof(IABoostEffectEnum), (int)y.effectId))))
            {
                var template = new SpellTemplate(ObjectDataManager.Get<Spell>((int)spell.spellId), 0);

                if (m_fighter.CanCastSpell(template, m_fighter.Point.Point.CellId, null))
                {
                    list.Add((int)spell.spellId);
                }
            }
            return list.ToArray();
        }

        public bool canLaunchSpell(int playerId, int spellId)
        {
            var fighter = m_fight.GetFighter(playerId);
            var spell = m_fighter.Spells.FirstOrDefault(x => x.spellId == spellId);
            if (fighter != null && spell != null)
            {
                var template = new SpellTemplate(ObjectDataManager.Get<Spell>((int)spell.spellId), 0);
                if (m_fighter.CanCastSpell(template, fighter.Point.Point.CellId, null))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
