using Rebirth.World.Datas.Characters;
using Rebirth.World.Game.Fights.Team;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rebirth.World.Game.Characters.Stats;
using Rebirth.Common.Protocol.Types;
using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Game.Fights.Custom;
using Rebirth.World.Managers;

namespace Rebirth.World.Game.Fights.Actors
{
    public class CharacterFighter : Fighter
    {
        #region Constructor
        public CharacterFighter(Character character, FightTeam team, Fight fight, bool isReady = false) : base(team, fight, character.Look)
        {
            Character = character;
            Id = (int)character.Infos.Id;
            Stats = character.Stats;
            Level = (int)character.Infos.Level;
            StartLife = Stats.Health.DamageTaken;
        }
        #endregion

        #region Properties
        public Character Character
        {
            get;
            private set;
        }
        public int StartLife
        {
            get;
            private set;
        }
        #endregion

        #region Get Functions
        public override GameFightFighterInformations GetFighterInformations()
        {
            var stats = Character.Stats;

            #region Stats
            GameFightMinimalStats playerStats = new GameFightMinimalStats()
            {
                actionPoints = (int)Stats.AP.Total,
                airElementReduction = (int)Stats[PlayerFields.AirElementReduction].Total,
                airElementResistPercent = (int)Stats.AirResistPercent.Total,
                baseMaxLifePoints = (uint)Stats.Health.TotalBase,
                lifePoints = (uint)Stats.Health.Total,
                criticalDamageFixedResist = 0,
                dodgePALostProbability = (uint)Stats.DodgeAPProbability.Total,
                dodgePMLostProbability = (uint)Stats.DodgeMPProbability.Total,
                earthElementReduction = (int)Stats[PlayerFields.EarthElementReduction].Total,
                earthElementResistPercent = (int)Stats.EarthResistPercent.Total,
                fireElementReduction = (int)Stats[PlayerFields.FireElementReduction].Total,
                fireElementResistPercent = (int)Stats.FireResistPercent.Total,
                invisibilityState = 3,
                maxActionPoints = (int)Stats.AP.TotalMax,
                maxLifePoints = (uint)Stats.Health.TotalMax,
                maxMovementPoints = (int)Stats.MP.TotalMax,
                movementPoints = (int)Stats.MP.Total,
                neutralElementReduction = (int)Stats[PlayerFields.NeutralElementReduction].Total,
                neutralElementResistPercent = (int)Stats.NeutralResistPercent.Total,
                permanentDamagePercent = (uint)Stats[PlayerFields.PermanentDamagePercent].Total,
                pushDamageFixedResist = (int)Stats[PlayerFields.PushDamageBonus].Total,
                pvpAirElementReduction = (int)Stats[PlayerFields.PvpAirElementReduction].Total,
                pvpAirElementResistPercent = (int)Stats[PlayerFields.PvpAirResistPercent].Total,
                pvpEarthElementReduction = (int)Stats[PlayerFields.PvpEarthElementReduction].Total,
                pvpEarthElementResistPercent = (int)Stats[PlayerFields.PvpEarthResistPercent].Total,
                pvpFireElementReduction = (int)Stats[PlayerFields.PvpFireElementReduction].Total,
                pvpFireElementResistPercent = (int)Stats[PlayerFields.PvpFireResistPercent].Total,
                pvpNeutralElementReduction = (int)Stats[PlayerFields.PvpNeutralElementReduction].Total,
                pvpNeutralElementResistPercent = (int)Stats[PlayerFields.PvpNeutralResistPercent].Total,
                pvpWaterElementReduction = (int)Stats[PlayerFields.PvpWaterElementReduction].Total,
                pvpWaterElementResistPercent = (int)Stats[PlayerFields.PvpWaterResistPercent].Total,
                shieldPoints = (uint)Stats[PlayerFields.Shield].Total,
                waterElementReduction = (int)Stats[PlayerFields.PvpWaterElementReduction].Total,
                waterElementResistPercent = (int)Stats.WaterResistPercent.Total,
                summoned = false,
                summoner = 0,
                tackleBlock = Stats[PlayerFields.TackleBlock].Total,
                tackleEvade = Stats[PlayerFields.TackleEvade].Total
            };
            #endregion
            return new GameFightCharacterInformations(Id, GetCustomLook(), new FightEntityDispositionInformations(Point.Point.CellId, (byte)Point.Dir, 0), (sbyte)Team.Team, 0, true, 
                playerStats, new uint[0], this.Character.Infos.Name, new PlayerStatus(0), (byte)this.Character.Infos.Level, 
                new ActorAlignmentInformations(0, 0, 0, 0), (sbyte)this.Character.Infos.Breed, Convert.ToBoolean(this.Character.Infos.Sex));
        }
        public override GameFightFighterInformations GetFighterPreparation()
        {
            var stats = Character.Stats;

            #region Stats
            GameFightMinimalStatsPreparation playerStats = new GameFightMinimalStatsPreparation()
            {
                actionPoints = (int)Stats.AP.Total,
                airElementReduction = (int)Stats[PlayerFields.AirElementReduction].Total,
                airElementResistPercent = (int)Stats.AirResistPercent.Total,
                baseMaxLifePoints = (uint)Stats.Health.TotalBase,
                lifePoints = (uint)Stats.Health.Total,
                criticalDamageFixedResist = 0,
                dodgePALostProbability = (uint)Stats.DodgeAPProbability.Total,
                dodgePMLostProbability = (uint)Stats.DodgeMPProbability.Total,
                earthElementReduction = (int)Stats[PlayerFields.EarthElementReduction].Total,
                earthElementResistPercent = (int)Stats.EarthResistPercent.Total,
                fireElementReduction = (int)Stats[PlayerFields.FireElementReduction].Total,
                fireElementResistPercent = (int)Stats.FireResistPercent.Total,
                invisibilityState = 3,
                maxActionPoints = (int)Stats.AP.TotalMax,
                maxLifePoints = (uint)Stats.Health.TotalMax,
                maxMovementPoints = (int)Stats.MP.TotalMax,
                movementPoints = (int)Stats.MP.Total,
                neutralElementReduction = (int)Stats[PlayerFields.NeutralElementReduction].Total,
                neutralElementResistPercent = (int)Stats.NeutralResistPercent.Total,
                permanentDamagePercent = (uint)Stats[PlayerFields.PermanentDamagePercent].Total,
                pushDamageFixedResist = (int)Stats[PlayerFields.PushDamageBonus].Total,
                pvpAirElementReduction = (int)Stats[PlayerFields.PvpAirElementReduction].Total,
                pvpAirElementResistPercent = (int)Stats[PlayerFields.PvpAirResistPercent].Total,
                pvpEarthElementReduction = (int)Stats[PlayerFields.PvpEarthElementReduction].Total,
                pvpEarthElementResistPercent = (int)Stats[PlayerFields.PvpEarthResistPercent].Total,
                pvpFireElementReduction = (int)Stats[PlayerFields.PvpFireElementReduction].Total,
                pvpFireElementResistPercent = (int)Stats[PlayerFields.PvpFireResistPercent].Total,
                pvpNeutralElementReduction = (int)Stats[PlayerFields.PvpNeutralElementReduction].Total,
                pvpNeutralElementResistPercent = (int)Stats[PlayerFields.PvpNeutralResistPercent].Total,
                pvpWaterElementReduction = (int)Stats[PlayerFields.PvpWaterElementReduction].Total,
                pvpWaterElementResistPercent = (int)Stats[PlayerFields.PvpWaterResistPercent].Total,
                shieldPoints = (uint)Stats[PlayerFields.Shield].Total,
                waterElementReduction = (int)Stats[PlayerFields.PvpWaterElementReduction].Total,
                waterElementResistPercent = (int)Stats.WaterResistPercent.Total,
                summoned = false,
                summoner = 0,
                tackleBlock = Stats[PlayerFields.TackleBlock].Total,
                tackleEvade = Stats[PlayerFields.TackleEvade].Total,
                initiative = (uint)stats[PlayerFields.Initiative].Total,
                fixedDamageReflection = Stats[PlayerFields.DamageReflection].Total,
            };
            #endregion
            return new GameFightCharacterInformations(Id, GetCustomLook(), new EntityDispositionInformations((short)Point.Point.CellId, (byte)Point.Dir), (sbyte)Team.Team, 0, true, 
                playerStats, new uint[0], this.Character.Infos.Name, new PlayerStatus(0), (byte)this.Character.Infos.Level, new ActorAlignmentInformations(0, 0, 0, 0), 
                (sbyte)this.Character.Infos.Breed, Convert.ToBoolean(this.Character.Infos.Sex));
        }
        public override GameFightFighterLightInformations GetGameFightFighterLightInformations()
        {
            return new GameFightFighterNamedLightInformations(false, IsAlive, Id, 0, (uint)Level, (sbyte)Character.Infos.Breed, Character.Infos.Name);
        }
        public override FightTeamMemberInformations GetTeamInformations()
        {
            return new FightTeamMemberCharacterInformations(Id, Character.Infos.Name, (byte)Level);
        }
        public override FightResultListEntry GetResult()
        {
            List<uint> list = new List<uint>();
            List<FightResultAdditionalData> additional = new List<FightResultAdditionalData>();
            foreach (var item in Result.Items)
            {
                list.Add((uint)item.Key);
                list.Add((uint)item.Value);
            }
            foreach (var item in Result.PlayerItems)
            {
                list.Add((uint)item.Template.id);
                list.Add((uint)item.Quantity);
            }
            
            if (this.Fight is AgressionFight)
            {
                byte grade = ExperienceManager.Instance.GetAlignementGrade((ushort)0);
                additional.Add(new FightResultPvpData(grade,
                    ExperienceManager.Instance.GetAlignementGradeHonor(grade), ExperienceManager.Instance.GetAlignementNextGradeHonor(grade), 0, Result.Honor));
            }
            else
            {
                additional.Add(new FightResultExperienceData(true, true, true, true, false /*(Character.Guild != null ? true : false)*/,
                                                    /*Character.Mounts.Any(x => x.IsRiding)*/false , false,
                                                    Character.Infos.Experience,
                                                    ExperienceManager.Instance.GetCharacterLevelExperience((byte)Character.Infos.Level),
                                                    ExperienceManager.Instance.GetCharacterNextLevelExperience((byte)Character.Infos.Level),
                                                    (int)Result.Experience, (int)Result.GuildeExp, (int)Result.MountExp, 0));
            }

            return new FightResultPlayerListEntry((uint)Team.Outcome, 0, new FightLoot(list.ToArray(), (uint)Result.Kamas),
                                                    Id, IsAlive, (byte)Character.Infos.Level, additional.ToArray());
        }
        #endregion

    }
}
