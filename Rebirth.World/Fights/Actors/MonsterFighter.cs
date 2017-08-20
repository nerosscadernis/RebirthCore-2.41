using Rebirth.Common.Protocol.Enums;
using Rebirth.Common.Protocol.Types;
using Rebirth.World.Datas.Characters;
using Rebirth.World.Datas.Monsters;
using Rebirth.World.Game.Fights.IA;
using Rebirth.World.Game.Fights.Result;
using Rebirth.World.Game.Fights.Team;
using System.Collections.Generic;

namespace Rebirth.World.Game.Fights.Actors
{
    public class MonsterFighter : AbstractIA
    {
        #region Constructor
        public MonsterFighter(MonsterTemplate template, FightTeam team, Fight fight, int id) : base(template.Template.spells, template.GradeID, team, fight, template.Template.id, Look.Parse(template.Template.look))
        {
            Template = template;
            //if (template.Template.look == "{556|||110}")
            //    Console.WriteLine("Test");
            Stats = Template.Stats;
            Id = id;
            Level = (int)Template.Grade.level;
            Stats[PlayerFields.DamageBonus].Base = (int)((double)Level * 1d);
            Stats[PlayerFields.Initiative].Base = Level * 50;
            Stats[PlayerFields.TackleBlock].Base = Level / 3;
            Stats[PlayerFields.TackleEvade].Base = Level / 3;
        }
        #endregion

        #region Properties
        public MonsterTemplate Template { get; set; }
        public Look Look { get; set; }
        #endregion

        #region Function Get
        public override GameFightFighterInformations GetFighterInformations()
        {
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
            return new GameFightMonsterInformations(Id, GetCustomLook(), new FightEntityDispositionInformations(Point.Point.CellId, (byte)Point.Dir, 0), (sbyte)Team.Team, 0, true, playerStats, new uint[0], (uint)Template.Template.id, (sbyte)Template.GradeID);
        }
        public override GameFightFighterInformations GetFighterPreparation()
        {
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
                initiative = (uint)Stats[PlayerFields.Initiative].Total
            };
            #endregion
            return new GameFightMonsterInformations(Id, GetCustomLook(), new EntityDispositionInformations(Point.Point.CellId, (byte)Point.Dir), (sbyte)Team.Team, 0, true, playerStats, new uint[0], (uint)Template.Grade.monsterId, (sbyte)Template.GradeID);
        }
        public override GameFightFighterLightInformations GetGameFightFighterLightInformations()
        {
            return new GameFightFighterMonsterLightInformations(false, IsAlive, Id, 0, (uint)Level, (sbyte)Template.Template.creatureBoneId, (uint)Template.Template.id);
        }
        public override FightTeamMemberInformations GetTeamInformations()
        {
            return new FightTeamMemberMonsterInformations(Id, Template.Grade.monsterId, (sbyte)Template.GradeID);
        }
        public override FightResultListEntry GetResult()
        {
            if (Result is MonsterResult)
            {
                List<uint> list = new List<uint>();
                foreach (var item in (Result as MonsterResult).PlayerItems)
                {
                    list.Add((uint)item.Template.id);
                    list.Add((uint)item.Quantity);
                }
                return new FightResultFighterListEntry((uint)Team.Outcome, 0, new FightLoot(list.ToArray(), (uint)Result.Kamas), Id, IsAlive);
            }
            else
            {
                return new FightResultFighterListEntry((uint)Team.Outcome, 0, new FightLoot(new uint[0], 0), Id, IsAlive);
            }
        }
        #endregion
    }
}
