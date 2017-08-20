using Rebirth.Common.Protocol.Data;
using Rebirth.Common.Protocol.Enums;
using Rebirth.Common.Thread;
using Rebirth.World.Datas.Items;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Game.Fights.Custom;
using Rebirth.World.Managers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rebirth.World.Game.Fights.Result
{
    public class GlobalResult
    {
        #region Constructor
        public GlobalResult(Fight fight)
        {
            Fight = fight;
        }
        #endregion

        #region Var
        private Fight Fight;
        private readonly double[] XP_GROUP = { 1, 1.1, 1.5, 2.3, 3.1, 3.6, 4.2, 4.7 };
        #endregion

        public void GenerateResult()
        {
            switch (Fight.Type)
            {
                case FightTypeEnum.FIGHT_TYPE_PVP_ARENA:
                    foreach (CharacterFighter fighter in Fight.Winner.Fighters)
                    {
                        SetKoliResult(fighter, true);
                    }
                    foreach (CharacterFighter fighter in Fight.Loser.Fighters)
                    {
                        SetKoliResult(fighter, false);
                    }
                    break;
                case Common.Protocol.Enums.FightTypeEnum.FIGHT_TYPE_CHALLENGE:
                    foreach (CharacterFighter fighter in Fight.Winner.Fighters)
                    {
                        SetNullResult(fighter);
                    }
                    foreach (CharacterFighter fighter in Fight.Loser.Fighters)
                    {
                        SetNullResult(fighter);
                    }
                    break;
                case Common.Protocol.Enums.FightTypeEnum.FIGHT_TYPE_AGRESSION:
                    foreach (CharacterFighter fighter in Fight.Winner.Fighters)
                    {
                        SetAgressionResult(fighter, true);
                    }
                    foreach (CharacterFighter fighter in Fight.Loser.Fighters)
                    {
                        SetAgressionResult(fighter, false);
                    }
                    break;
                case Common.Protocol.Enums.FightTypeEnum.FIGHT_TYPE_PvM:
                    if (Fight.Winner.Team == Common.Protocol.Enums.TeamEnum.TEAM_CHALLENGER)
                    {
                        foreach (CharacterFighter fighter in Fight.Winner.Fighters.FindAll(x => x is CharacterFighter))
                        {
                            SetPvmResult(fighter, true);
                        }
                        foreach (MonsterFighter fighter in Fight.Loser.Fighters.FindAll(x => x is MonsterFighter))
                        {
                            SetMonterResult(fighter);
                        }
                        foreach (var item in Fight.Loser.Fighters)
                        {
                            SetNullResult(item);
                        }
                    }
                    else
                    {
                        foreach (CharacterFighter fighter in Fight.Loser.Fighters.FindAll(x => x is CharacterFighter))
                        {
                            SetPvmResult(fighter, false);
                        }
                        foreach (MonsterFighter fighter in Fight.Winner.Fighters.FindAll(x => x is MonsterFighter))
                        {
                            SetMonterResult(fighter);
                        }
                        foreach (var item in Fight.Loser.Fighters)
                        {
                            SetNullResult(item);
                        }
                    }
                    break;
                case FightTypeEnum.FIGHT_TYPE_PvPr:
                    break;
            }
        }

        public List<PlayerItem> Loots = new List<PlayerItem>();
        public double Experience = 0d;

        #region SetResult
        public void SetNullResult(Fighter fighter)
        {
            fighter.Result = new AbstractResult();
        }
        public int getEarnedHonor(CharacterFighter fighter)
        {
            int winnersLevel = 0;
            int losersLevel = 0;
            foreach (var winner in Fight.Winner.Fighters)
            {
                if (winner is CharacterFighter)
                {
                    CharacterFighter characterFighter = (CharacterFighter)winner;
                    winnersLevel += (int)characterFighter.Character.Infos.Level;
                }
            }

            foreach (var loser in Fight.Loser.Fighters)
            {
                if (loser is CharacterFighter)
                {
                    CharacterFighter characterFighter = (CharacterFighter)loser;
                    losersLevel += (int)characterFighter.Character.Infos.Level;
                }
            }

            return (int)Math.Round(Math.Sqrt((fighter.Character.Infos.Level) * 10.0 * (winnersLevel / losersLevel)));
        }
        public void SetAgressionResult(CharacterFighter fighter, bool isWinner)
        {
            if (isWinner)
            {
                fighter.Result = new AgressionResult(fighter.Character);
                fighter.Result.Honor = getEarnedHonor(fighter);
            }
            else
            {
                fighter.Result = new AgressionResult(fighter.Character);
                fighter.Result.Honor = -getEarnedHonor(fighter);
            }
        }
        public void SetAvAResult(CharacterFighter fighter, bool isWinner)
        {
            if (isWinner)
            {
                Dictionary<int, int> test = new Dictionary<int, int>();
                var rdn = new AsyncRandom();
                foreach (var item in Loots)
                {
                    if (rdn.Next(0, 100) >= 50)
                    {
                        var quantity = rdn.Next(0, item.Quantity);
                        fighter.Character.Inventory.AddItem(new PlayerItem(fighter.Character, item.Template.id, quantity, item.Effects));
                    }
                }
            }
            else
            {
                foreach(var item in fighter.Character.Inventory.Items)
                {
                    Loots.Add(item);
                }
                Experience += (double)fighter.Character.Infos.Experience * 80d / 100d;
            }
        }
        public void SetPvmResult(CharacterFighter fighter, bool isWinner)
        {
            if (isWinner)
            {
                fighter.Result = new PvmResult(fighter.Character);

                double pStarBonus = (Fight as PvmFight).Group.Age;
                double reelStarBonus = pStarBonus <= 0 ? 1 : 1 + pStarBonus / 100;              

                Dictionary<int, int> test = new Dictionary<int, int>();
                double monstersXp = 0;
                double monsterLevel = 0;
                double monsterHiddenLevel = 0;
                double maxLevelMonster = 0;
                foreach (MonsterFighter monster in Fight.Loser.Fighters)
                {
                    foreach (var drop in monster.Template.Template.drops.FindAll(x => !x.hasCriteria))
                    {
                        var rdn = new AsyncRandom();
                        var basePourcent = GetPourcentDrop(monster.Template.GradeID, drop);
                        var pourcent = (fighter.Stats.Prospecting.Total / 100d) * (basePourcent * 6d * reelStarBonus);
                        var result = rdn.Next(0, 100);
                        if (result < pourcent)
                        {
                            bool isStuff = ItemManager.Instance.IsStuff(drop.objectId);
                            var amount = isStuff ? 1 : new AsyncRandom().Next(1, 3 + (int)(0.1d * basePourcent));
                            if (!test.ContainsKey(drop.objectId))
                            {
                                test.Add(drop.objectId, amount);
                            }
                            else if(!isStuff)
                            {
                                test[drop.objectId] += amount;
                            }
                        }
                    }
                    fighter.Result.Kamas += (int)((double)monster.Level * (fighter.Stats.Prospecting.Total / 100d));
                    fighter.Result.MonstersEat.Add(monster.Template.Template.id);
                    monstersXp += monster.Template.Grade.gradeXp;
                    monsterLevel += monster.Level;
                    monsterHiddenLevel += monster.Template.Grade.hiddenLevel > 0 ? monster.Template.Grade.hiddenLevel : monster.Template.Grade.level;
                    if (monster.Template.Grade.level > maxLevelMonster)
                        maxLevelMonster = monster.Template.Grade.level;
                }

                double lvlPlayers = 0;
                double lvlMaxGroup = 0;
                double totalPlayerGroup = 0;
                foreach (CharacterFighter player in Fight.Winner.Fighters.Select(x => (x as CharacterFighter)))
                {
                    lvlPlayers = lvlPlayers + player.Level;
                    if (player.Level > lvlMaxGroup)
                        lvlMaxGroup = player.Level;
                }
                foreach (CharacterFighter player in Fight.Winner.Fighters.Select(x => (x as CharacterFighter)))
                {
                    if (player.Level >= lvlMaxGroup / 3)
                        totalPlayerGroup++;
                }

                double coeffDiffLvlGroup = 1;
                if (lvlPlayers - 5 > monsterLevel)
                {
                    coeffDiffLvlGroup = monsterLevel / lvlPlayers;
                }
                else if (lvlPlayers + 10 < monsterLevel)
                {
                    coeffDiffLvlGroup = (lvlPlayers + 10) / monsterLevel;
                }

                double coeffDiffLvlSolo = 1;
                if (fighter.Level - 5 > monsterLevel)
                {
                    coeffDiffLvlSolo = (monsterLevel - 6) / fighter.Level;
                }
                else if (fighter.Level + 10 < monsterLevel)
                {
                    coeffDiffLvlSolo = (fighter.Level + 10) / monsterLevel;
                }

                double v = Math.Min(fighter.Level, truncate(2.5 * maxLevelMonster));
                double xpLimitMaxLvlSolo = v / fighter.Level * 100;
                double xpLimitMaxLvlGroup = v / lvlPlayers * 100;
                /// Prioriter
                //    monstersXp = 20586;
                //    coeffDiffLvlSolo = 0.48;
                //    xpLimitMaxLvlSolo = 25;
                //    monstersXp -= 1058;
                double xpGroupAlone = truncate((monstersXp * XP_GROUP[0]) * coeffDiffLvlSolo);

                if (totalPlayerGroup == 0)
                {
                    totalPlayerGroup = 1;
                }
                double xpGroup = truncate(monstersXp * XP_GROUP[(int)totalPlayerGroup - 1] * coeffDiffLvlGroup);
                double xpNoSagesseAlone = truncate(xpLimitMaxLvlSolo / 100 * xpGroupAlone);
                double xpNoSagesseGroup = truncate(xpLimitMaxLvlGroup / 100 * xpGroup);
                if (lvlMaxGroup == 0)
                {
                    lvlMaxGroup = fighter.Level;
                }
                double lvlMalusIdols = Math.Min(4, (monsterHiddenLevel / Fight.Loser.Fighters.Count) / lvlMaxGroup);
                lvlMalusIdols = lvlMalusIdols * lvlMalusIdols;
                int idolsWisdomBonus = (int)((this.truncate((100d + fighter.Level * 2.5d)) * this.truncate(0 * lvlMalusIdols)) / 100d);
                double totalWisdom = Math.Max(fighter.Stats.Wisdom.Total + idolsWisdomBonus, 0);
                uint xpTotalOnePlayer = (uint)this.truncate(this.truncate(xpNoSagesseAlone * (100d + totalWisdom) / 100d) * reelStarBonus);
                uint xpTotalGroup = (uint)this.truncate(this.truncate(xpNoSagesseGroup * (100d + totalWisdom) / 100d) * reelStarBonus);
                double xpBonus = 1;
                double tmpSolo = xpTotalOnePlayer;
                double tmpGroup = xpTotalGroup;
                double ratioXpMontureSolo = 0;
                double ratioXpMontureGroup = 0;
                //if (fighter.Character.Mounts.Any(x => x.IsRiding))
                //{
                //    var ratio = fighter.Character.Mounts.First(x => x.IsRiding).GiveExp;
                //    ratioXpMontureSolo = tmpSolo * ratio / 100;
                //    ratioXpMontureGroup = tmpGroup * ratio / 100;
                //    tmpSolo = this.truncate(tmpSolo - ratioXpMontureSolo);
                //    tmpGroup = this.truncate(tmpGroup - ratioXpMontureGroup);
                //}
                tmpSolo = tmpSolo * xpBonus;
                tmpGroup = tmpGroup * xpBonus;
                double ratioXpGuildSolo = 0;
                double ratioXpGuildGroup = 0;
                //if (fighter.Character.GuildMember != null && fighter.Character.GuildMember.GivenPercent > 0)
                //{
                //    ratioXpGuildSolo = tmpSolo * fighter.Character.GuildMember.GivenPercent / 100;
                //    ratioXpGuildGroup = tmpGroup * fighter.Character.GuildMember.GivenPercent / 100;
                //    tmpSolo = tmpSolo - ratioXpGuildSolo;
                //    tmpGroup = tmpGroup - ratioXpGuildGroup;
                //}
                //if (pPlayerData.xpAlliancePrismBonusPercent > 0)
                //{
                //    xpAlliancePrismBonus = 1 + pPlayerData.xpAlliancePrismBonusPercent / 100;
                //    tmpSolo = tmpSolo * xpAlliancePrismBonus;
                //    tmpGroup = tmpGroup * xpAlliancePrismBonus;
                //}
                xpTotalOnePlayer = (uint)this.truncate(tmpSolo);
                xpTotalGroup = (uint)this.truncate(tmpGroup);
                double bonusChall = 0d;
                foreach (var item in Fight.Challenges.FindAll(x => x.Success))
                    bonusChall += Fight.Winner.Fighters.Count > 0 ? item.BoostGroup : item.BoostSolo;
                int _xpSolo = monstersXp > 0 ? (int)Math.Max(xpTotalOnePlayer, 1) : 0;
                _xpSolo += (int)(_xpSolo * bonusChall / 100); 
                int _xpGroup = monstersXp > 0 ? (int)(Math.Max(xpTotalGroup, 1)) : 0;
                _xpGroup += (int)(_xpGroup * bonusChall / 100);

                fighter.Result.Experience += Fight.Winner.Fighters.Count > 0 ? _xpGroup * 6  : _xpSolo * 6 ;
                fighter.Result.GuildeExp += Fight.Winner.Fighters.Count > 0 ? ratioXpGuildGroup * 6 : ratioXpGuildSolo * 6;
                fighter.Result.MountExp += Fight.Winner.Fighters.Count > 0 ? ratioXpMontureGroup * 6 : ratioXpMontureSolo * 6;
                
                fighter.Result.Items = test;
            }
            else
            {
                fighter.Result = new PvmResult(fighter.Character);

                foreach (MonsterFighter monster in Fight.Winner.Fighters.FindAll(x => x.IsDead))
                {
                    fighter.Result.Experience += monster.Template.Grade.gradeXp;
                }
            }
        }
        public void SetKoliResult(CharacterFighter fighter, bool isWinner)
        {
            if (isWinner)
            {
                fighter.Result = new PvmResult(fighter.Character);

                Dictionary<int, int> test = new Dictionary<int, int>();
                test.Add(12736, 30);

                double exp = 0d;
                if (fighter.Level == 200)
                    exp = 0d;
                else if (fighter.Level == 199)
                    exp = (ExperienceManager.Instance.GetCharacterNextLevelExperience((byte)fighter.Level) -
                            ExperienceManager.Instance.GetCharacterLevelExperience((byte)fighter.Level)) * 0.5d / 100d;
                else
                    exp = (ExperienceManager.Instance.GetCharacterNextLevelExperience((byte)fighter.Level) -
                            ExperienceManager.Instance.GetCharacterLevelExperience((byte)fighter.Level)) * 0.5d / 100d;

                fighter.Result.Experience += exp < 0 ? 0 : exp;

                fighter.Result.Items = test;
            }
            else
            {
                fighter.Result = new PvmResult(fighter.Character);
                Dictionary<int, int> test = new Dictionary<int, int>();
                test.Add(12736, 5);
                fighter.Result.Items = test;
            }
        }
        public void SetMonterResult(MonsterFighter monster)
        {
            monster.Result = new MonsterResult((Fight as PvmFight).Group);
        }
        #endregion

        #region Formulas
        public float GetPourcentDrop(int grade, MonsterDrop drop)
        {
            switch (grade)
            {
                case 1:
                    return drop.percentDropForGrade1;
                case 2:
                    return drop.percentDropForGrade2;
                case 3:
                    return drop.percentDropForGrade3;
                case 4:
                    return drop.percentDropForGrade4;
                case 5:
                    return drop.percentDropForGrade5;
                default:
                    return 0f;
            }
        }
        public double GetRationParty()
        {
            switch (Fight.Winner.Fighters.Count)
            {
                case 1:
                    return 1;
                case 2:
                    return 1.1;
                case 3:
                    return 1.5;
                case 4:
                    return 2.3;
                case 5:
                    return 3.1;
                case 6:
                    return 3.6;
                case 7:
                    return 4.2;
                case 8:
                    return 4.7;
                default:
                    return 0.5d;
            }
        }
        private int truncate(double val)
        {
            var multiplier = Math.Pow(10, 0);
            var truncatedVal = val * multiplier;
            return (int)(truncatedVal / multiplier);
        }
        #endregion
    }
}
