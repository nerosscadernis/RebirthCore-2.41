using Rebirth.Common.Protocol.Enums;
using Rebirth.Common.Protocol.Types;
using Rebirth.World.Game.Characters.Stats;
using Rebirth.World.Game.Fights.IA;
using Rebirth.World.Game.Fights.Other;
using Rebirth.World.Game.Fights.Team;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Datas.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Fights.Actors
{
    public class SummonDouble : SummonMonster, IStatsOwner
    {
        #region Constructeur
        public SummonDouble(FightTeam team, Fight fight, int id, Fighter caster, FightDisposition point) : base(null, team, fight, id, caster, point)
        {
            Stats = caster.Stats.CloneAndChangeOwner(this);
            Id = id;
            Level = caster.Level;
            Summoner = caster;
            Point = point;
        }
        #endregion

        #region Public Function
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
                summoned = true,
                summoner = Summoner.Id,
                tackleBlock = Stats[PlayerFields.TackleBlock].Total,
                tackleEvade = Stats[PlayerFields.TackleEvade].Total
            };
            #endregion
            GameFightCharacterInformations gameFightFighterInformations = this.Summoner.GetFighterInformations() as GameFightCharacterInformations;
            gameFightFighterInformations.stats = playerStats;
            gameFightFighterInformations.disposition = new EntityDispositionInformations(Point.Point.CellId, (byte)Point.Dir);
            gameFightFighterInformations.contextualId = Id;
            gameFightFighterInformations.breed = -1;

            return gameFightFighterInformations;
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
                summoned = true,
                summoner = Summoner.Id,
                tackleBlock = Stats[PlayerFields.TackleBlock].Total,
                tackleEvade = Stats[PlayerFields.TackleEvade].Total,
                initiative = (uint)Stats[PlayerFields.Initiative].Total
            };
            #endregion
            GameFightCharacterInformations gameFightFighterInformations = this.Summoner.GetFighterPreparation() as GameFightCharacterInformations;
            gameFightFighterInformations.stats = playerStats;
            gameFightFighterInformations.disposition = new EntityDispositionInformations(Point.Point.CellId, (byte)Point.Dir);
            gameFightFighterInformations.contextualId = Id;
            return gameFightFighterInformations;
        }
        public override FightTeamMemberInformations GetTeamInformations()
        {
            FightTeamMemberInformations fightTeamMemberInformations = this.Summoner.GetTeamInformations();
            fightTeamMemberInformations.id = this.Id;
            return fightTeamMemberInformations;
        }
        public override FightResultListEntry GetResult()
        {
            return new FightResultFighterListEntry((uint)Team.Outcome, 0, new FightLoot(new uint[0], 0), Id, IsAlive);
        }
        #endregion
    }
}
