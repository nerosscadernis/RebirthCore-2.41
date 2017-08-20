using Rebirth.Common.GameData.D2O;
using Rebirth.Common.IStructures;
using Rebirth.Common.Protocol.Data;
using Rebirth.Common.Protocol.Enums;
using Rebirth.Common.Protocol.Messages;
using Rebirth.Common.Protocol.Types;
using Rebirth.World.Datas.Jobs;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Datas.Quests;
using Rebirth.World.Datas.Shortcuts;
using Rebirth.World.Datas.Spells;
using Rebirth.World.Exchanges;
using Rebirth.World.Game.Characters.Stats;
using Rebirth.World.Game.Fights;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Game.Fights.Duel;
using Rebirth.World.Managers;
using Rebirth.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rebirth.World.Datas.Characters
{
    public class Character : IStatsOwner
    {
        #region Vars

        #endregion

        #region Properties
        public Client Client { get; set; }
        public CharacterInfos Infos { get; set; }
        public Look Look { get; set; }
        public StatsFields Stats { get; set; }
        public JobInfos Jobs { get; set; }
        public QuestInfos Quests { get; set; }
        public Inventaire Inventory { get; set; }
        public SpellsInfo Spells { get; set; }
        public ShortcutBar Shortcuts { get; set; }

        // Active Infos
        public MapTemplate Map { get; set; }
        public Fight Fight { get; set; }
        public Fighter Fighter { get; set; }
        public PlayerState State { get; set; }
        public IActivity Activity { get; set; }
        public RequestFight Request { get; set; }
        public object Party { get; set; }
        public Trader Trader { get; set; }
        #endregion

        #region Constructors
        public Character()
        { }
        public Character(CharacterRecord record)
        {
            Infos = new CharacterInfos(record.Id, record.Infos);
            Look = new Look(record.Look);
            Stats = new StatsFields(this);
            Stats.Initialize(record.Stats);
            Jobs = new JobInfos(this, record.Jobs);
            Jobs.LevelUp += OnJobLevelUp;
            Jobs.ExperienceUp += OnJobExperienceChanged;
            Quests = new QuestInfos(this, record.Quests);
            Inventory = new Inventaire(record.Inventory, this);
            Spells = new SpellsInfo(record.Spells);
            Shortcuts = new ShortcutBar(this, record.Shortcuts);

            record = null;
        }
        #endregion

        #region Private Methods

        #endregion

        #region Stats
        public void RefreshStats()
        {
            if (Fight == null)
                Client.Send(new CharacterStatsListMessage(GetCharacterCharacteristicsInformations()));
            else
                Client.Send(new FighterStatsListMessage(GetCharacterCharacteristicsInformations()));
        }
        public CharacterCharacteristicsInformations GetCharacterCharacteristicsInformations()
        {
            //new ActorAlignmentInformations((sbyte)Record.AlignSide, (sbyte)Record.Honor, (sbyte)ExperienceManager.Instance.GetAlignementGrade((ushort)Record.Honor), (this.Id + this.Record.Level)));
            var align = new ActorExtendedAlignmentInformations(0, 0,
                (sbyte)ExperienceManager.Instance.GetAlignementGrade(0), (Infos.Id + Infos.Experience), 0,
                (uint)ExperienceManager.Instance.GetAlignementGradeHonor(1), (uint)ExperienceManager.Instance.GetAlignementNextGradeHonor(1),
                (sbyte)AggressableStatusEnum.NON_AGGRESSABLE);
            var data = new CharacterCharacteristicsInformations()
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
                alignmentInfos = align,
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
                experience = Infos.Experience,
                experienceLevelFloor = ExperienceManager.Instance.GetCharacterLevelExperience(Infos.Level),
                experienceNextLevelFloor = ExperienceManager.Instance.GetCharacterNextLevelExperience((byte)(Infos.Level)),
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
                spellModifications = Stats.SpellBoosts.GetCharacterSpellModifications(),
                spellsPoints = (uint)Infos.SpellsPoint,
                statsPoints = (uint)Infos.StatsPoint,
                summonableCreaturesBoost = Stats[PlayerFields.SummonLimit],
                tackleBlock = Stats[PlayerFields.TackleBlock],
                tackleEvade = Stats[PlayerFields.TackleEvade],
                trapBonus = Stats[PlayerFields.TrapBonus],
                trapBonusPercent = Stats[PlayerFields.TrapBonusPercent],
                weaponDamagesBonusPercent = new CharacterBaseCharacteristic(),
                meleeDamageDonePercent = new CharacterBaseCharacteristic(),
                meleeDamageReceivedPercent = new CharacterBaseCharacteristic(),
                rangedDamageDonePercent = new CharacterBaseCharacteristic(),
                rangedDamageReceivedPercent = new CharacterBaseCharacteristic(),
                spellDamageDonePercent = new CharacterBaseCharacteristic(),
                spellDamageReceivedPercent = new CharacterBaseCharacteristic(),
                weaponDamageDonePercent = new CharacterBaseCharacteristic(),
                weaponDamageReceivedPercent = new CharacterBaseCharacteristic(),
            };
            return data;
        }
        #endregion

        #region Teleportation
        public void Teleport(int mapId, short cellId)
        {
            if (Activity != null)
                Activity.Close();
            var newMap = (MapTemplate)MapManager.Instance.GetMap(mapId);
            Map.Quit(Client);
            var cellActor = newMap.Cells.First(entry => entry.Id == Infos.CellId);
            Infos.CellId = cellId;
            newMap.Enter(Client);
            //if (PartyId > 0)
            //{
            //    Party.Party party = PartyManager.Instance.GetPartyById(PartyId);
            //    if (party != null)
            //    {
            //        party.OnMove();
            //    }
            //}
            //if (IsOnJail && mapId != 105120002)
            //    Teleport(105120002, 245);
        }
        #endregion

        #region Jobs
        private void OnJobExperienceChanged(object sender, JobTemplate.UpExperienceEventArgs e)
        {
            Client.Send(new JobExperienceUpdateMessage(e.Data.GetJobExperience()));
        }
        private void OnJobLevelUp(object sender, JobTemplate.UpLevelEventArgs e)
        {
            Client.Send(new JobLevelUpMessage(e.Data.Level, e.Data.GetJobDescription()));
        }
        #endregion

        #region Experience
        public void AddExperience(double amount)
        {
            byte level = Infos.Level;

            if (Infos.Experience + amount >= ExperienceManager.Instance.GetCharacterNextLevelExperience(Infos.Level) && Infos.Level < ExperienceManager.Instance.HighestCharacterLevel)
            {
                Infos.Experience += (long)amount;
                int difference = Infos.Level - level;
                OnLevelChanged(Infos.Level, difference);
                Client.Send(new CharacterExperienceGainMessage(amount, 0, 0, 0));
            }
            else
            {
                Infos.Experience += (long)amount;
                Client.Send(new CharacterExperienceGainMessage(amount, 0, 0, 0));
                RefreshStats();
            }
        }
        private void OnLevelChanged(byte currentLevel, int difference)
        {
            if (difference > 0)
            {
                Infos.SpellsPoint += (ushort)difference;
                Infos.StatsPoint += (ushort)(difference * 5);
            }
            Stats.Health.Base += (int)((short)(difference * 5));
            Stats.Health.DamageTaken = 0;
            if (currentLevel >= 100 && (int)currentLevel - difference < 100)
            {
                Stats.AP.Base++;
            }
            else
            {
                if (currentLevel < 100 && (int)currentLevel - difference >= 100)
                {
                    Stats.AP.Base--;
                }
            }

            Breed breed = ObjectDataManager.Get<Breed>(Infos.Breed);
            if (breed != null)
            {
                foreach (var item in breed.breedSpellsId)
                {
                    var spell = ObjectDataManager.Get<Spell>((int)item);
                    if (spell != null)
                    {
                        if (spell.obtentionLevel > currentLevel - difference && spell.obtentionLevel <= currentLevel && !Spells.HasSpell(spell.id))
                        {
                            Spells.AddSpell(spell.id);
                            Shortcuts.AddShortcut(ShortcutBarEnum.SPELL_SHORTCUT_BAR, new ShortcutSpell((sbyte)Shortcuts.GetNextFreeSlot(ShortcutBarEnum.SPELL_SHORTCUT_BAR), (uint)spell.id));
                        }
                    }
                }
            }

            //if (currentLevel >= Record.LastLevel)
            //    Client.Send(new ServerExperienceModificatorMessage(600));

            RefreshStats();
            //RefreshActor();

            Client.Send(new CharacterLevelUpMessage(currentLevel));
            Map.Send(new CharacterLevelUpInformationMessage(currentLevel, Infos.Name, (uint)Infos.Id));

            //if (Party != null)
            //    Party.UpdateMember(this);
        }
        #endregion

        #region Public Methods
        public CharacterRecord GetRecord()
        {
            return new CharacterRecord()
            {
                Id = Infos.Id,
                Infos = Infos.GetDatas(),
                Look = Look.GetDatas(),
                Guild = new byte[0],
                Inventory = Inventory.GetDatas(),
                Stats = Stats.GetDatas(),
                Jobs = Jobs.GetDatas(),
                Quests = Quests.GetDatas(),
                Spells = Spells.GetDatas(),
                Shortcuts = Shortcuts.GetDatas(),
            };
        }

        public CharacterBaseInformations GetCharacterBaseInformations()
        {
            return new CharacterBaseInformations(Infos.Id, Infos.Name, Infos.Level, Look.GetEntityLook(), (byte)Infos.Breed, Convert.ToBoolean(Infos.Sex));
        }

        public GameRolePlayCharacterInformations GetGameRolePlayCharacterInformations()
        {
            ActorRestrictionsInformations rest = new ActorRestrictionsInformations();
            return
                new GameRolePlayCharacterInformations(Infos.Id, Look.GetEntityLook(),
                new EntityDispositionInformations((short)Infos.CellId, (byte)Infos.Direction), Infos.Name, GetHumanInformations(),
                Infos.AccountId, new ActorAlignmentInformations(0, 0, 0, (Infos.Id + Infos.Level)));
        }

        public HumanInformations GetHumanInformations()
        {
            HumanInformations humanInformations = new HumanInformations(new ActorRestrictionsInformations(), Convert.ToBoolean(Infos.Sex), new HumanOption[0]);

            List<HumanOption> list = new List<HumanOption>();
            //if (Guild != null)
            //{
            //    list.Add(new HumanOptionGuild(Guild.GetGuildInformations()));
            //}
            //if (Alliance != null)
            //{
            //    list.Add(new HumanOptionAlliance(Alliance.GetAllianceInformations(), (sbyte)AggressableStatusEnum.AvA_ENABLED_AGGRESSABLE));
            //}
            if (Infos.ActiveTitle > 0)
            {
                list.Add(new HumanOptionTitle((uint)Infos.ActiveTitle, ""));
            }
            if (Infos.ActiveOrnament > 0)
            {
                list.Add(new HumanOptionOrnament((uint)Infos.ActiveOrnament));
            }

            humanInformations.options = list.ToArray();
            return humanInformations;
        }        
        #endregion
    }
}
