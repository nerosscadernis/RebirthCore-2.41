using Rebirth.Common.Protocol.Enums;
using Rebirth.Common.Protocol.Types;
using Rebirth.World.Game.Fights.IA;
using Rebirth.World.Game.Fights.Other;
using Rebirth.World.Game.Fights.Team;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Datas.Monsters;
using Rebirth.World.Game.Spells;
using Rebirth.World.Game.Spells.Cast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rebirth.World.Datas.Characters;

namespace Rebirth.World.Game.Fights.Actors
{
    public class SummonMonster : AbstractIA
    {
        #region Constructeur
        public SummonMonster(MonsterTemplate template, FightTeam team, Fight fight, int id, Fighter caster, FightDisposition point) : base(template != null ? template.Template.spells : new List<uint>(), template != null ? template.GradeID : caster.Level, team, fight, template != null ? template.Template.id : 0, template != null ? Look.Parse(template.Template.look) : caster.BaseLook.Clone())
        {
            if (template != null)
            {
                Template = template;
                Stats = Template.Stats;
                Id = id;
                Level = (int)Template.Grade.level;
                Summoner = caster;
                Point = point;
                AdjustStats();
            }         
        }
        #endregion

        #region Properties
        public MonsterTemplate Template { get; set; }
        public Fighter Summoner { get; set; }
        public bool IsStatic { get; set; }
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
                summoned = true,
                summoner = Summoner.Id,
                tackleBlock = Stats[PlayerFields.TackleBlock].Total,
                tackleEvade = Stats[PlayerFields.TackleEvade].Total,
                initiative = (uint)Stats[PlayerFields.Initiative].Total
            };
            #endregion
            return new GameFightMonsterInformations(Id, GetCustomLook(), new EntityDispositionInformations(Point.Point.CellId, (byte)Point.Dir), (sbyte)Team.Team, 0, true, playerStats, new uint[0], (uint)Template.Grade.monsterId, (sbyte)Template.GradeID);
        }
        public override FightTeamMemberInformations GetTeamInformations()
        {
            return new FightTeamMemberMonsterInformations(Id, Template.Grade.monsterId, (sbyte)Template.GradeID);
        }
        public override FightResultListEntry GetResult()
        {
            return new FightResultFighterListEntry((uint)Team.Outcome, 0, new FightLoot(new uint[0], 0), Id, IsAlive);
        }
        public CharacterCharacteristicsInformations GetCharacterCharacteristicsInformations()
        {
            return new CharacterCharacteristicsInformations()
            {
                actionPoints = Stats[PlayerFields.AP],
                vitality = Stats[PlayerFields.Vitality],
                agility = Stats[PlayerFields.Agility],
                strength = Stats[PlayerFields.Strength],
                intelligence = Stats[PlayerFields.Intelligence],
                wisdom = Stats[PlayerFields.Wisdom],
                chance = Stats[PlayerFields.Chance],
                earthDamageBonus = Stats[PlayerFields.EarthDamageBonus],
                earthElementReduction = Stats[PlayerFields.EarthElementReduction],
                earthElementResistPercent = Stats[PlayerFields.EarthResistPercent],
                pvpEarthElementReduction = Stats[PlayerFields.PvpEarthElementReduction],
                pvpEarthElementResistPercent = Stats[PlayerFields.PvpEarthElementReduction],
                fireDamageBonus = Stats[PlayerFields.FireDamageBonus],
                fireElementReduction = Stats[PlayerFields.FireElementReduction],
                fireElementResistPercent = Stats[PlayerFields.FireResistPercent],
                pvpFireElementReduction = Stats[PlayerFields.PvpFireElementReduction],
                pvpFireElementResistPercent = Stats[PlayerFields.PvpFireResistPercent],
                waterDamageBonus = Stats[PlayerFields.WaterDamageBonus],
                waterElementReduction = Stats[PlayerFields.WaterElementReduction],
                waterElementResistPercent = Stats[PlayerFields.WaterResistPercent],
                pvpWaterElementReduction = Stats[PlayerFields.PvpWaterElementReduction],
                pvpWaterElementResistPercent = Stats[PlayerFields.PvpWaterResistPercent],
                neutralDamageBonus = Stats[PlayerFields.NeutralDamageBonus],
                neutralElementReduction = Stats[PlayerFields.NeutralElementReduction],
                neutralElementResistPercent = Stats[PlayerFields.NeutralResistPercent],
                pvpNeutralElementReduction = Stats[PlayerFields.PvpNeutralElementReduction],
                pvpNeutralElementResistPercent = Stats[PlayerFields.PvpNeutralResistPercent],
                airDamageBonus = Stats[PlayerFields.AirDamageBonus],
                airElementReduction = Stats[PlayerFields.AirElementReduction],
                airElementResistPercent = Stats[PlayerFields.AirResistPercent],
                pvpAirElementReduction = Stats[PlayerFields.PvpAirElementReduction],
                pvpAirElementResistPercent = Stats[PlayerFields.PvpAirResistPercent],
                actionPointsCurrent = Stats.AP.Total,
                additionnalPoints = 0,
                alignmentInfos = new ActorExtendedAlignmentInformations(),
                allDamagesBonus = Stats[PlayerFields.DamageBonus],
                criticalDamageBonus = Stats[PlayerFields.CriticalDamageBonus],
                criticalDamageReduction = Stats[PlayerFields.CriticalDamageReduction],
                criticalHit = Stats[PlayerFields.CriticalHit],
                criticalHitWeapon = 0,
                criticalMiss = Stats[PlayerFields.CriticalMiss],
                damagesBonusPercent = Stats[PlayerFields.DamageBonusPercent],
                dodgePALostProbability = Stats[PlayerFields.DodgeAPProbability],
                dodgePMLostProbability = Stats[PlayerFields.DodgeMPProbability],
                energyPoints = 10000,
                experience = 0,
                experienceLevelFloor = 0,
                experienceNextLevelFloor = 0,
                glyphBonusPercent = new CharacterBaseCharacteristic(),
                healBonus = Stats[PlayerFields.HealBonus],
                initiative = Stats[PlayerFields.Initiative],
                kamas = 0,
                lifePoints = (uint)Stats.Health.Total,
                maxEnergyPoints = 10000,
                maxLifePoints = (uint)Stats.Health.TotalMax,
                movementPoints = Stats[PlayerFields.MP],
                movementPointsCurrent = Stats.MP.Total,
                PAAttack = Stats[PlayerFields.APAttack],
                permanentDamagePercent = new CharacterBaseCharacteristic(),
                PMAttack = Stats[PlayerFields.MPAttack],
                probationTime = 0,
                prospecting = Stats[PlayerFields.Prospecting],
                pushDamageBonus = Stats[PlayerFields.PushDamageBonus],
                pushDamageReduction = Stats[PlayerFields.PushDamageReduction],
                range = Stats[PlayerFields.Range],
                reflect = Stats[PlayerFields.DamageReflection],
                runeBonusPercent = new CharacterBaseCharacteristic(),
                spellModifications = new CharacterSpellModification[0],
                spellsPoints = 0,
                statsPoints = 0,
                summonableCreaturesBoost = Stats[PlayerFields.SummonLimit],
                tackleBlock = Stats[PlayerFields.TackleBlock],
                tackleEvade = Stats[PlayerFields.TackleEvade],
                trapBonus = Stats[PlayerFields.TrapBonus],
                trapBonusPercent = Stats[PlayerFields.TrapBonusPercent],
                weaponDamagesBonusPercent = new CharacterBaseCharacteristic()
            };
        }

        public override void Die(Fighter by)
        {
            base.Die(by);
            Summoner.Summons.Remove(this);
            Fight.Fighters.Remove(this);
            Team.Fighters.Remove(this);
        }
        #endregion

        #region Private Function
        public virtual void AdjustStats()
        {
            Stats[PlayerFields.DamageBonus].Base = Level * 5;
            this.Stats.Vitality.Base = (short)(Stats.Vitality.Base * (1.0 + Summoner.Level / 100.0));
            this.Stats.Intelligence.Base = (int)((short)((double)this.Stats.Intelligence.Base * (1.0 + (double)Summoner.Level / 100.0)));
            this.Stats.Chance.Base = (int)((short)((double)this.Stats.Chance.Base * (1.0 + (double)Summoner.Level / 100.0)));
            this.Stats.Strength.Base = (int)((short)((double)this.Stats.Strength.Base * (1.0 + (double)Summoner.Level / 100.0)));
            this.Stats.Agility.Base = (int)((short)((double)this.Stats.Agility.Base * (1.0 + (double)Summoner.Level / 100.0)));
            this.Stats.Wisdom.Base = (int)((short)((double)this.Stats.Wisdom.Base * (1.0 + (double)Summoner.Level / 100.0)));
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion
    }
}
