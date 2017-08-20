using Rebirth.Common.Protocol.Data;
using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Datas.Spells;
using Rebirth.World.Game.Effect.Instances;
using Rebirth.World.Game.Fights;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Game.Fights.Buffs;
using Rebirth.World.Game.Zones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rebirth.World.Effects.Handlers.Spells
{
    public abstract class SpellEffectHandler : EffectHandler
    {
        private List<Fighter> m_customAffectedActors;
        private CellMap[] m_affectedCells;
        private MapPoint m_castPoint;
        private Zone m_effectZone;

        public Zone EffectZone
        {
            get
            {
                Zone arg_41_0;
                if ((arg_41_0 = this.m_effectZone) == null)
                {
                    if (Effect.ZoneShape == SpellShapeEnum.l)
                        arg_41_0 = (this.m_effectZone =
                            new Zone(this.Effect.ZoneShape
                            , (byte)Caster.Point.Point.DistanceToCell(TargetedPoint)
                            , (byte)Effect.ZoneMinSize, this.Caster.Point.Point.OrientationTo(this.TargetedPoint, true), Caster, Effect.StopAtTarget));
                    else
                        arg_41_0 = (this.m_effectZone =
                            new Zone(this.Effect.ZoneShape
                            , (byte)this.Effect.ZoneSize
                            , (byte)Effect.ZoneMinSize, this.Caster.Point.Point.OrientationTo(this.TargetedPoint, true), Caster, Effect.StopAtTarget));
                }
                return arg_41_0;
            }
            set
            {
                this.m_effectZone = value;
                this.RefreshZone();
            }
        }
        public CellMap[] AffectedCells
        { get { return m_affectedCells; } }
        public EffectDice Dice
        {
            get;
            private set;
        }
        public Fighter Caster
        {
            get;
            set;
        }
        public SpellTemplate Spell
        {
            get;
            private set;
        }
        public CellMap TargetedCell
        {
            get;
            set;
        }
        public MapPoint TargetedPoint
        {
            get;
            set;
        }
        public Fight Fight
        {
            get
            {
                return Caster.Fight;
            }
        }
        public bool Critical
        {
            get;
            private set;
        }
        public SpellTargetType Targets
        {
            get;
            set;
        }
        public double Boost { get; set; }
        public bool Touch { get; set; }

        public SpellEffectHandler(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect)
        {
            this.Dice = effect;
            this.Caster = caster;
            this.Spell = spell;
            this.TargetedCell = targetedCell;
            this.TargetedPoint = new MapPoint(this.TargetedCell);
            this.Critical = critical;
            this.Targets = effect.Targets;
            m_customAffectedActors = new List<Fighter>();
        }

        public List<Fighter> GetAffectedActors()
        {
            List<Fighter> result = new List<Fighter>();
            if (Fight.Fighters == null)
                return result;

            if (this.m_customAffectedActors.Count > 0)
            {
                result.AddRange(this.m_customAffectedActors.FindAll(x => x.IsAlive));
            }
            else
            {
                RefreshZone();

                List<int> Ids = new List<int>();
                List<int> NoIds = new List<int>();
                if (Dice.TargetMask.Contains('F'))
                {
                    string[] all = Dice.TargetMask.Split(',');
                    foreach (var item in all)
                    {
                        if (item.Contains('F'))
                        {
                            var id = item.Trim('F');
                            Ids.Add(Convert.ToInt32(id));
                        }
                    }
                }
                if (Dice.TargetMask.Contains('f'))
                {
                    string[] all = Dice.TargetMask.Split(',');
                    foreach (var item in all)
                    {
                        if (item.Contains('f'))
                        {
                            var id = item.Trim('f');
                            NoIds.Add(Convert.ToInt32(id));
                        }
                    }
                }
                List<int> States = new List<int>();
                List<int> NoStates = new List<int>();
                List<int> StatesMe = new List<int>();
                List<int> NoStatesMe = new List<int>();
                if (Dice.TargetMask.Contains('E'))
                {
                    string[] all = Dice.TargetMask.Split(',');
                    foreach (var item in all)
                    {
                        if (item.Contains('E'))
                        {
                            if (item[0] == '*')
                            {
                                var id = item.Trim('*').Trim('E');
                                StatesMe.Add(Convert.ToInt32(id));
                            }
                            else
                            {
                                var id = item.Trim('E');
                                States.Add(Convert.ToInt32(id));
                            }
                        }
                    }
                }
                if (Dice.TargetMask.Contains('e'))
                {
                    string[] all = Dice.TargetMask.Split(',');
                    foreach (var item in all)
                    {
                        if (item.Contains('e'))
                        {
                            if (item[0] == '*')
                            {
                                var id = item.Trim('*').Trim('e');
                                NoStatesMe.Add(Convert.ToInt32(id));
                            }
                            else
                            {
                                var id = item.Trim('e');
                                NoStates.Add(Convert.ToInt32(id));
                            }
                        }
                    }
                }
                int pourcentLife = 0;
                int myPourcentLife = 0;
                int pourcentLifeSup = 0;
                int myPourcentLifeSup = 0;
                if (Dice.TargetMask.Contains('V'))
                {
                    string[] all = Dice.TargetMask.Split(',');
                    foreach (var item in all)
                    {
                        if (item.Contains('V'))
                        {
                            if (item[0] == '*')
                            {
                                var id = item.Trim('*').Trim('V');
                                myPourcentLife = Convert.ToInt32(id);
                            }
                            else
                            {
                                var id = item.Trim('V');
                                pourcentLife = Convert.ToInt32(id);
                            }
                        }
                    }
                }
                if (Dice.TargetMask.Contains('v'))
                {
                    string[] all = Dice.TargetMask.Split(',');
                    foreach (var item in all)
                    {
                        if (item.Contains('v'))
                        {
                            if (item[0] == '*')
                            {
                                var id = item.Trim('*').Trim('v');
                                myPourcentLifeSup = Convert.ToInt32(id);
                            }
                            else
                            {
                                var id = item.Trim('v');
                                pourcentLifeSup = Convert.ToInt32(id);
                            }
                        }
                    }
                }
                List<int> NoBreed = new List<int>();
                if (Dice.TargetMask.Contains('b'))
                {
                    string[] all = Dice.TargetMask.Split(',');
                    foreach (var item in all)
                    {
                        if (item.Contains('b'))
                        {
                            if (item[0] == '*')
                            {
                                var id = item.Trim('*').Trim('b');
                                NoStatesMe.Add(Convert.ToInt32(id));
                            }
                            else
                            {
                                var id = item.Trim('b');
                                NoStates.Add(Convert.ToInt32(id));
                            }
                        }
                    }
                }
                bool CasterIsAlly = Dice.TargetMask.Contains("*l");
                bool CasterIsAllySummon = Dice.TargetMask.Contains("*j");
                bool isSelf = Dice.TargetMask.Contains('P');
                bool isNotSelf = Dice.TargetMask.Contains('p');

                if (this.Effect.Targets.HasFlag(SpellTargetType.ONLY_SELF))
                {
                    if ((Ids.Count > 0 ? Caster is SummonMonster && Ids.Contains((Caster as SummonMonster).Template.Template.id) : true)
                        && (NoIds.Count > 0 ? Caster is SummonMonster && !NoIds.Contains((Caster as SummonMonster).Template.Template.id) : true)
                        && (States.Count > 0 ? States.All(x => Caster.HasState(x)) : true)
                        && (NoStates.Count > 0 ? NoStates.All(x => !Caster.HasState(x)) : true)
                        && (NoStatesMe.Count > 0 ? NoStatesMe.All(x => !Caster.HasState(x)) : true)
                        && (StatesMe.Count > 0 ? StatesMe.All(x => Caster.HasState(x)) : true)
                        && (Caster is CharacterFighter ? NoBreed.All(x => x != (Caster as CharacterFighter).Character.Infos.Breed) : true)
                        && (pourcentLife > 0 ? (Caster.Stats.Health.Total * 100 / Caster.Stats.Health.TotalMax) < pourcentLife : true)
                        && (myPourcentLife > 0 ? (Caster.Stats.Health.Total * 100 / Caster.Stats.Health.TotalMax) < myPourcentLife : true)
                        && (pourcentLifeSup > 0 ? (Caster.Stats.Health.Total * 100 / Caster.Stats.Health.TotalMax) > pourcentLifeSup : true)
                        && (myPourcentLifeSup > 0 ? (Caster.Stats.Health.Total * 100 / Caster.Stats.Health.TotalMax) > myPourcentLifeSup : true)
                        && (Effect.Targets.HasFlag(SpellTargetType.TELEPORT_FIGHTER) ? Fight.FighterTeleport.Contains(Caster) : true))
                    {
                        result.Add(Caster);
                        return result;
                    }
                }
                if (Effect.Targets.HasFlag(SpellTargetType.SELF))
                {
                    if (Caster.Point.Point.CellId == TargetedPoint.CellId
                        && (Ids.Count > 0 ? Caster is SummonMonster ? Ids.Contains((Caster as SummonMonster).Template.Template.id) : false : true)
                        && (NoIds.Count > 0 ? Caster is SummonMonster ? !NoIds.Contains((Caster as SummonMonster).Template.Template.id) : true : true)
                        && (States.Count > 0 ? States.All(x => Caster.HasState(x)) : true)
                        && (NoStates.Count > 0 ? NoStates.All(x => !Caster.HasState(x)) : true)
                        && (NoStatesMe.Count > 0 ? NoStatesMe.All(x => !Caster.HasState(x)) : true)
                        && (StatesMe.Count > 0 ? StatesMe.All(x => Caster.HasState(x)) : true)
                        && (Caster is CharacterFighter ? NoBreed.All(x => x != (Caster as CharacterFighter).Character.Infos.Breed) : true)
                        && (pourcentLife > 0 ? (Caster.Stats.Health.Total * 100 / Caster.Stats.Health.TotalMax) < pourcentLife : true)
                        && (myPourcentLife > 0 ? (Caster.Stats.Health.Total * 100 / Caster.Stats.Health.TotalMax) < myPourcentLife : true)
                        && (pourcentLifeSup > 0 ? (Caster.Stats.Health.Total * 100 / Caster.Stats.Health.TotalMax) > pourcentLifeSup : true)
                        && (myPourcentLifeSup > 0 ? (Caster.Stats.Health.Total * 100 / Caster.Stats.Health.TotalMax) > myPourcentLifeSup : true)
                        && (Effect.Targets.HasFlag(SpellTargetType.TELEPORT_FIGHTER) ? Fight.FighterTeleport.Contains(Caster) : true))
                    {
                        result.Add(Caster);
                    }
                }
                if (Effect.Targets.HasFlag(SpellTargetType.ENEMY_ALL))
                {
                    result.AddRange((from entry in this.Fight.GetAllFighter(this.m_affectedCells)
                                     where !result.Contains(entry) && !m_customAffectedActors.Contains(entry) && !entry.IsDead && entry.Team.Team != Caster.Team.Team
                                     && (Ids.Count > 0 ? entry is SummonMonster ? Ids.Contains((entry as SummonMonster).Template.Template.id) : false : true)
                                     && (NoIds.Count > 0 ? entry is SummonMonster ? !NoIds.Contains((entry as SummonMonster).Template.Template.id) : true : true)
                                     && (States.Count > 0 ? States.All(x => entry.HasState(x)) : true)
                                     && (NoStates.Count > 0 ? NoStates.All(x => !entry.HasState(x)) : true)
                                     && (NoStatesMe.Count > 0 ? NoStatesMe.All(x => !Caster.HasState(x)) : true)
                                     && (StatesMe.Count > 0 ? StatesMe.All(x => Caster.HasState(x)) : true)
                                     && (isSelf ? entry is SummonMonster && (entry as SummonMonster).Summoner.Id == Caster.Id : true)
                                     && (isNotSelf ? entry is SummonMonster && (entry as SummonMonster).Summoner.Id != Caster.Id : true)
                                     && (entry is CharacterFighter ? NoBreed.All(x => x != (entry as CharacterFighter).Character.Infos.Breed) : true)
                                     && (CasterIsAlly ? Caster.Team.Type == entry.Team.Type : true)
                                     && (CasterIsAllySummon ? Caster.Team.Type == entry.Team.Type && Caster is SummonMonster : true)
                                     && (pourcentLife > 0 ? (entry.Stats.Health.Total * 100 / entry.Stats.Health.TotalMax) < pourcentLife : true)
                                     && (myPourcentLife > 0 ? (Caster.Stats.Health.Total * 100 / Caster.Stats.Health.TotalMax) < myPourcentLife : true)
                                     && (pourcentLifeSup > 0 ? (entry.Stats.Health.Total * 100 / entry.Stats.Health.TotalMax) > pourcentLifeSup : true)
                                     && (myPourcentLifeSup > 0 ? (Caster.Stats.Health.Total * 100 / Caster.Stats.Health.TotalMax) > myPourcentLifeSup : true)
                                     && (Effect.Targets.HasFlag(SpellTargetType.TELEPORT_FIGHTER) ? Fight.FighterTeleport.Contains(entry) : true)
                                     select entry).ToArray());
                }
                if (Effect.Targets.HasFlag(SpellTargetType.ALLY_ALL))
                {
                    result.AddRange((from entry in this.Fight.GetAllFighter(this.m_affectedCells)
                                     where !result.Contains(entry) && !m_customAffectedActors.Contains(entry) && !entry.IsDead && entry.Team.Team == Caster.Team.Team
                                     && (Ids.Count > 0 ? entry is SummonMonster ? Ids.Contains((entry as SummonMonster).Template.Template.id) : false : true)
                                     && (NoIds.Count > 0 ? entry is SummonMonster ? !NoIds.Contains((entry as SummonMonster).Template.Template.id) : true : true)
                                     && (States.Count > 0 ? States.All(x => entry.HasState(x)) : true)
                                     && (NoStates.Count > 0 ? NoStates.All(x => !entry.HasState(x)) : true)
                                     && (NoStatesMe.Count > 0 ? NoStatesMe.All(x => !Caster.HasState(x)) : true)
                                     && (StatesMe.Count > 0 ? StatesMe.All(x => Caster.HasState(x)) : true)
                                     && (isSelf ? entry is SummonMonster && (entry as SummonMonster).Summoner.Id == Caster.Id : true)
                                     && (isNotSelf ? entry is SummonMonster && (entry as SummonMonster).Summoner.Id != Caster.Id : true)
                                     && (entry is CharacterFighter ? NoBreed.All(x => x != (entry as CharacterFighter).Character.Infos.Breed) : true)
                                     && (CasterIsAlly ? Caster.Team.Type == entry.Team.Type : true)
                                     && (CasterIsAllySummon ? Caster.Team.Type == entry.Team.Type && Caster is SummonMonster : true)
                                     && (pourcentLife > 0 ? (entry.Stats.Health.Total * 100 / entry.Stats.Health.TotalMax) < pourcentLife : true)
                                     && (myPourcentLife > 0 ? (Caster.Stats.Health.Total * 100 / Caster.Stats.Health.TotalMax) < myPourcentLife : true)
                                     && (pourcentLifeSup > 0 ? (entry.Stats.Health.Total * 100 / entry.Stats.Health.TotalMax) > pourcentLifeSup : true)
                                     && (myPourcentLifeSup > 0 ? (Caster.Stats.Health.Total * 100 / Caster.Stats.Health.TotalMax) > myPourcentLifeSup : true)
                                     && (Effect.Targets.HasFlag(SpellTargetType.TELEPORT_FIGHTER) ? Fight.FighterTeleport.Contains(entry) : true)
                                     select entry).ToArray());
                }
                if (Effect.Targets.HasFlag(SpellTargetType.ALLY_NoSELF))
                {
                    result.AddRange((from entry in this.Fight.GetAllFighter(this.m_affectedCells)
                                     where !result.Contains(entry) && !m_customAffectedActors.Contains(entry) && !entry.IsDead && entry.Team.Team == Caster.Team.Team && entry.Id != Caster.Id
                                     && (Ids.Count > 0 ? entry is SummonMonster ? Ids.Contains((entry as SummonMonster).Template.Template.id) : false : true)
                                     && (NoIds.Count > 0 ? entry is SummonMonster ? !NoIds.Contains((entry as SummonMonster).Template.Template.id) : true : true)
                                     && (States.Count > 0 ? States.All(x => entry.HasState(x)) : true)
                                     && (NoStates.Count > 0 ? NoStates.All(x => !entry.HasState(x)) : true)
                                     && (NoStatesMe.Count > 0 ? NoStatesMe.All(x => !Caster.HasState(x)) : true)
                                     && (StatesMe.Count > 0 ? StatesMe.All(x => Caster.HasState(x)) : true)
                                     && (isSelf ? entry is SummonMonster && (entry as SummonMonster).Summoner.Id == Caster.Id : true)
                                     && (isNotSelf ? entry is SummonMonster && (entry as SummonMonster).Summoner.Id != Caster.Id : true)
                                     && (entry is CharacterFighter ? NoBreed.All(x => x != (entry as CharacterFighter).Character.Infos.Breed) : true)
                                     && (CasterIsAlly ? Caster.Team.Type == entry.Team.Type : true)
                                     && (CasterIsAllySummon ? Caster.Team.Type == entry.Team.Type && Caster is SummonMonster : true)
                                     && (pourcentLife > 0 ? (entry.Stats.Health.Total * 100 / entry.Stats.Health.TotalMax) < pourcentLife : true)
                                     && (myPourcentLife > 0 ? (Caster.Stats.Health.Total * 100 / Caster.Stats.Health.TotalMax) < myPourcentLife : true)
                                     && (pourcentLifeSup > 0 ? (entry.Stats.Health.Total * 100 / entry.Stats.Health.TotalMax) > pourcentLifeSup : true)
                                     && (myPourcentLifeSup > 0 ? (Caster.Stats.Health.Total * 100 / Caster.Stats.Health.TotalMax) > myPourcentLifeSup : true)
                                     && (Effect.Targets.HasFlag(SpellTargetType.TELEPORT_FIGHTER) ? Fight.FighterTeleport.Contains(entry) : true)
                                     select entry).ToArray());
                }
                if (Effect.Targets.HasFlag(SpellTargetType.ALL_SUMMONS))
                {
                    result.AddRange((from entry in this.Fight.GetAllFighter(this.m_affectedCells)
                                     where !result.Contains(entry) && !m_customAffectedActors.Contains(entry) && !entry.IsDead && entry is SummonMonster
                                     && (Ids.Count > 0 ? Ids.Contains((entry as SummonMonster).Template.Template.id) : true)
                                     && (NoIds.Count > 0 ? !NoIds.Contains((entry as SummonMonster).Template.Template.id) : true)
                                     && (States.Count > 0 ? States.All(x => entry.HasState(x)) : true)
                                     && (NoStates.Count > 0 ? NoStates.All(x => !entry.HasState(x)) : true)
                                     && (isSelf ? (entry as SummonMonster).Summoner.Id == Caster.Id : true)
                                     && (isNotSelf ? (entry as SummonMonster).Summoner.Id != Caster.Id : true)
                                     && (NoStatesMe.Count > 0 ? NoStatesMe.All(x => !Caster.HasState(x)) : true)
                                     && (StatesMe.Count > 0 ? StatesMe.All(x => Caster.HasState(x)) : true)
                                     && (entry is CharacterFighter ? NoBreed.All(x => x != (entry as CharacterFighter).Character.Infos.Breed) : true)
                                     && (CasterIsAlly ? Caster.Team.Type == entry.Team.Type : true)
                                     && (CasterIsAllySummon ? Caster.Team.Type == entry.Team.Type && Caster is SummonMonster : true)
                                     && (pourcentLife > 0 ? (entry.Stats.Health.Total * 100 / entry.Stats.Health.TotalMax) < pourcentLife : true)
                                     && (myPourcentLife > 0 ? (Caster.Stats.Health.Total * 100 / Caster.Stats.Health.TotalMax) < myPourcentLife : true)
                                     && (pourcentLifeSup > 0 ? (entry.Stats.Health.Total * 100 / entry.Stats.Health.TotalMax) > pourcentLifeSup : true)
                                     && (myPourcentLifeSup > 0 ? (Caster.Stats.Health.Total * 100 / Caster.Stats.Health.TotalMax) > myPourcentLifeSup : true)
                                     && (Effect.Targets.HasFlag(SpellTargetType.TELEPORT_FIGHTER) ? Fight.FighterTeleport.Contains(entry) : true)
                                     select entry).ToArray());
                }
                if (Effect.Targets.HasFlag(SpellTargetType.ALL_HUMAN))
                {
                    result.AddRange((from entry in this.Fight.GetAllFighter(this.m_affectedCells)
                                     where !result.Contains(entry) && !m_customAffectedActors.Contains(entry) && !entry.IsDead && !(entry is SummonMonster)
                                     && (Ids.Count > 0 ? entry is SummonMonster ? Ids.Contains((entry as SummonMonster).Template.Template.id) : false : true)
                                     && (NoIds.Count > 0 ? entry is SummonMonster ? !NoIds.Contains((entry as SummonMonster).Template.Template.id) : true : true)
                                     && (States.Count > 0 ? States.All(x => entry.HasState(x)) : true)
                                     && (NoStates.Count > 0 ? NoStates.All(x => !entry.HasState(x)) : true)
                                     && (NoStatesMe.Count > 0 ? NoStatesMe.All(x => !Caster.HasState(x)) : true)
                                     && (StatesMe.Count > 0 ? StatesMe.All(x => Caster.HasState(x)) : true)
                                     && (isSelf ? entry is SummonMonster && (entry as SummonMonster).Summoner.Id == Caster.Id : true)
                                     && (isNotSelf ? entry is SummonMonster && (entry as SummonMonster).Summoner.Id != Caster.Id : true)
                                     && (entry is CharacterFighter ? NoBreed.All(x => x != (entry as CharacterFighter).Character.Infos.Breed) : true)
                                     && (CasterIsAlly ? Caster.Team.Type == entry.Team.Type : true)
                                     && (CasterIsAllySummon ? Caster.Team.Type == entry.Team.Type && Caster is SummonMonster : true)
                                     && (pourcentLife > 0 ? (entry.Stats.Health.Total * 100 / entry.Stats.Health.TotalMax) < pourcentLife : true)
                                     && (myPourcentLife > 0 ? (Caster.Stats.Health.Total * 100 / Caster.Stats.Health.TotalMax) < myPourcentLife : true)
                                     && (pourcentLifeSup > 0 ? (entry.Stats.Health.Total * 100 / entry.Stats.Health.TotalMax) > pourcentLifeSup : true)
                                     && (myPourcentLifeSup > 0 ? (Caster.Stats.Health.Total * 100 / Caster.Stats.Health.TotalMax) > myPourcentLifeSup : true)
                                     && (Effect.Targets.HasFlag(SpellTargetType.TELEPORT_FIGHTER) ? Fight.FighterTeleport.Contains(entry) : true)
                                     select entry).ToArray());
                }
                if (Effect.Targets.HasFlag(SpellTargetType.ALLY_SUMMONS))
                {
                    result.AddRange((from entry in this.Fight.GetAllFighter(this.m_affectedCells)
                                     where !result.Contains(entry) && !m_customAffectedActors.Contains(entry) && !entry.IsDead && entry.Team.Team == Caster.Team.Team && (entry is SummonMonster)
                                     && (Ids.Count > 0 ? Ids.Contains((entry as SummonMonster).Template.Template.id) : true)
                                     && (NoIds.Count > 0 ? !NoIds.Contains((entry as SummonMonster).Template.Template.id) : true)
                                     && (States.Count > 0 ? States.All(x => entry.HasState(x)) : true)
                                     && (NoStates.Count > 0 ? NoStates.All(x => !entry.HasState(x)) : true)
                                     && (isSelf ? (entry as SummonMonster).Summoner.Id == Caster.Id : true)
                                     && (isNotSelf ? (entry as SummonMonster).Summoner.Id != Caster.Id : true)
                                     && (NoStatesMe.Count > 0 ? NoStatesMe.All(x => !Caster.HasState(x)) : true)
                                     && (StatesMe.Count > 0 ? StatesMe.All(x => Caster.HasState(x)) : true)
                                     && (entry is CharacterFighter ? NoBreed.All(x => x != (entry as CharacterFighter).Character.Infos.Breed) : true)
                                     && (CasterIsAlly ? Caster.Team.Type == entry.Team.Type : true)
                                     && (CasterIsAllySummon ? Caster.Team.Type == entry.Team.Type && Caster is SummonMonster : true)
                                     && (pourcentLife > 0 ? (entry.Stats.Health.Total * 100 / entry.Stats.Health.TotalMax) < pourcentLife : true)
                                     && (myPourcentLife > 0 ? (Caster.Stats.Health.Total * 100 / Caster.Stats.Health.TotalMax) < myPourcentLife : true)
                                     && (pourcentLifeSup > 0 ? (entry.Stats.Health.Total * 100 / entry.Stats.Health.TotalMax) > pourcentLifeSup : true)
                                     && (myPourcentLifeSup > 0 ? (Caster.Stats.Health.Total * 100 / Caster.Stats.Health.TotalMax) > myPourcentLifeSup : true)
                                     && (Effect.Targets.HasFlag(SpellTargetType.TELEPORT_FIGHTER) ? Fight.FighterTeleport.Contains(entry) : true)
                                     select entry).ToArray());
                }
                if (Effect.Targets.HasFlag(SpellTargetType.ENNEMY_SUMMONS))
                {
                    result.AddRange((from entry in this.Fight.GetAllFighter(this.m_affectedCells)
                                     where !result.Contains(entry) && !m_customAffectedActors.Contains(entry) && !entry.IsDead && entry.Team.Team != Caster.Team.Team && (entry is SummonMonster)
                                     && (Ids.Count > 0 ? Ids.Contains((entry as SummonMonster).Template.Template.id) : true)
                                     && (NoIds.Count > 0 ? !NoIds.Contains((entry as SummonMonster).Template.Template.id) : true)
                                     && (States.Count > 0 ? States.All(x => entry.HasState(x)) : true)
                                     && (NoStates.Count > 0 ? NoStates.All(x => !entry.HasState(x)) : true)
                                     && (isSelf ? (entry as SummonMonster).Summoner.Id == Caster.Id : true)
                                     && (isNotSelf ? (entry as SummonMonster).Summoner.Id != Caster.Id : true)
                                     && (NoStatesMe.Count > 0 ? NoStatesMe.All(x => !Caster.HasState(x)) : true)
                                     && (StatesMe.Count > 0 ? StatesMe.All(x => Caster.HasState(x)) : true)
                                     && (entry is CharacterFighter ? NoBreed.All(x => x != (entry as CharacterFighter).Character.Infos.Breed) : true)
                                     && (CasterIsAlly ? Caster.Team.Type == entry.Team.Type : true)
                                     && (CasterIsAllySummon ? Caster.Team.Type == entry.Team.Type && Caster is SummonMonster : true)
                                     && (pourcentLife > 0 ? (entry.Stats.Health.Total * 100 / entry.Stats.Health.TotalMax) < pourcentLife : true)
                                     && (myPourcentLife > 0 ? (Caster.Stats.Health.Total * 100 / Caster.Stats.Health.TotalMax) < myPourcentLife : true)
                                     && (pourcentLifeSup > 0 ? (entry.Stats.Health.Total * 100 / entry.Stats.Health.TotalMax) > pourcentLifeSup : true)
                                     && (myPourcentLifeSup > 0 ? (Caster.Stats.Health.Total * 100 / Caster.Stats.Health.TotalMax) > myPourcentLifeSup : true)
                                     && (Effect.Targets.HasFlag(SpellTargetType.TELEPORT_FIGHTER) ? Fight.FighterTeleport.Contains(entry) : true)
                                     select entry).ToArray());
                }
            }
            return result;
        }
        public IEnumerable<Fighter> GetEffectActor()
        {
            IEnumerable<Fighter> result;
            if (m_customAffectedActors != null)
            {
                result = m_customAffectedActors;
            }
            else
            {
                if (Effect.Targets.HasFlag(SpellTargetType.ONLY_SELF))
                {
                    result = new[]
                    {
                        Caster
                    };
                }
                else
                {
                    //result = (
                    //from entry in this.Fight.GetAllFighter(this.m_affectedCells)
                    //where !entry.IsDead /*&& this.IsValidTarget(entry) && entry.Porter == null*/
                    //select entry).ToArray<Fighter>();

                    result = new Fighter[] { Fight.GetFighter(TargetedCell) };
                }
                result = null;
            }
            return result;
        }

        public EffectInteger GenerateEffect()
        {
            EffectInteger effectInteger = this.Effect.GenerateEffect(EffectGenerationContext.Spell, EffectGenerationType.Normal) as EffectInteger;
            if (effectInteger != null)
            {
                effectInteger.Value = (short)((double)effectInteger.Value * base.Efficiency);
            }
            return effectInteger;
        }
        public void SetAffectedActors()
        {
            this.m_customAffectedActors = GetAffectedActors();
        }
        public void SetAffectedActors(System.Collections.Generic.IEnumerable<Fighter> actors)
        {
            this.m_customAffectedActors = actors.ToList();
        }
        public void SetAffectedActor(Fighter actor)
        {
            this.m_customAffectedActors = new List<Fighter> { actor };
        }
        public void TrySetAffectedActor(Fighter actor)
        {
            var result = new List<Fighter>();
            List<int> Ids = new List<int>();
            if (Dice.TargetMask.Contains('F'))
            {
                string[] all = Dice.TargetMask.Split(',');
                foreach (var item in all)
                {
                    if (item.Contains('F'))
                    {
                        var id = item.Trim('F');
                        Ids.Add(Convert.ToInt32(id));
                    }
                }
            }
            List<int> States = new List<int>();
            List<int> NoStates = new List<int>();
            List<int> StatesMe = new List<int>();
            List<int> NoStatesMe = new List<int>();
            if (Dice.TargetMask.Contains('E'))
            {
                string[] all = Dice.TargetMask.Split(',');
                foreach (var item in all)
                {
                    if (item.Contains('E'))
                    {
                        if (item[0] == '*')
                        {
                            var id = item.Trim('*').Trim('E');
                            StatesMe.Add(Convert.ToInt32(id));
                        }
                        else
                        {
                            var id = item.Trim('E');
                            States.Add(Convert.ToInt32(id));
                        }
                    }
                }
            }
            if (Dice.TargetMask.Contains('e'))
            {
                string[] all = Dice.TargetMask.Split(',');
                foreach (var item in all)
                {
                    if (item.Contains('e'))
                    {
                        if (item[0] == '*')
                        {
                            var id = item.Trim('*').Trim('e');
                            NoStatesMe.Add(Convert.ToInt32(id));
                        }
                        else
                        {
                            var id = item.Trim('e');
                            NoStates.Add(Convert.ToInt32(id));
                        }
                    }
                }
            }
            int pourcentLife = 0;
            int myPourcentLife = 0;
            if (Dice.TargetMask.Contains('V'))
            {
                string[] all = Dice.TargetMask.Split(',');
                foreach (var item in all)
                {
                    if (item.Contains('V'))
                    {
                        if (item[0] == '*')
                        {
                            var id = item.Trim('*').Trim('V');
                            myPourcentLife = Convert.ToInt32(id);
                        }
                        else
                        {
                            var id = item.Trim('V');
                            pourcentLife = Convert.ToInt32(id);
                        }
                    }
                }
            }
            List<int> NoBreed = new List<int>();
            if (Dice.TargetMask.Contains('b'))
            {
                string[] all = Dice.TargetMask.Split(',');
                foreach (var item in all)
                {
                    if (item.Contains('b'))
                    {
                        if (item[0] == '*')
                        {
                            var id = item.Trim('*').Trim('b');
                            NoStatesMe.Add(Convert.ToInt32(id));
                        }
                        else
                        {
                            var id = item.Trim('b');
                            NoStates.Add(Convert.ToInt32(id));
                        }
                    }
                }
            }
            bool CasterIsAlly = Dice.TargetMask.Contains("*l");
            bool CasterIsAllySummon = Dice.TargetMask.Contains("*j");
            bool isSelf = Dice.TargetMask.Contains('P');
            if (this.Effect.Targets.HasFlag(SpellTargetType.ONLY_SELF))
            {
                if ((Ids.Count > 0 ? Caster is SummonMonster && Ids.Contains((Caster as SummonMonster).Template.Template.id) : true)
                    && (States.Count > 0 ? States.All(x => Caster.HasState(x)) : true)
                    && (NoStates.Count > 0 ? NoStates.All(x => !Caster.HasState(x)) : true)
                    && (NoStatesMe.Count > 0 ? NoStatesMe.All(x => !Caster.HasState(x)) : true)
                    && (StatesMe.Count > 0 ? StatesMe.All(x => Caster.HasState(x)) : true)
                    && (Caster is CharacterFighter ? NoBreed.All(x => x != (Caster as CharacterFighter).Character.Infos.Breed) : true)
                    && (pourcentLife > 0 ? (Caster.Stats.Health.Total * 100 / Caster.Stats.Health.TotalMax) > pourcentLife : true)
                    && (myPourcentLife > 0 ? (Caster.Stats.Health.Total * 100 / Caster.Stats.Health.TotalMax) > myPourcentLife : true)
                    && m_customAffectedActors.Contains(actor))
                {
                    result.Add(Caster);
                    return;
                }
            }
            if (Effect.Targets.HasFlag(SpellTargetType.SELF))
            {
                if (Caster.Point.Point.CellId == TargetedPoint.CellId
                    && (Ids.Count > 0 ? Caster is SummonMonster && Ids.Contains((Caster as SummonMonster).Template.Template.id) : true)
                    && (States.Count > 0 ? States.All(x => Caster.HasState(x)) : true)
                    && (NoStates.Count > 0 ? NoStates.All(x => !Caster.HasState(x)) : true)
                    && (NoStatesMe.Count > 0 ? NoStatesMe.All(x => !Caster.HasState(x)) : true)
                    && (StatesMe.Count > 0 ? StatesMe.All(x => Caster.HasState(x)) : true)
                    && (Caster is CharacterFighter ? NoBreed.All(x => x != (Caster as CharacterFighter).Character.Infos.Breed) : true)
                    && (pourcentLife > 0 ? (Caster.Stats.Health.Total * 100 / Caster.Stats.Health.TotalMax) > pourcentLife : true)
                    && (myPourcentLife > 0 ? (Caster.Stats.Health.Total * 100 / Caster.Stats.Health.TotalMax) > myPourcentLife : true))
                {
                    result.Add(Caster);
                }
            }
            if (Effect.Targets.HasFlag(SpellTargetType.ENEMY_ALL))
            {
                if (!result.Contains(actor) && !m_customAffectedActors.Contains(actor) && !actor.IsDead && actor.Team.Team != Caster.Team.Team
                                 && (Ids.Count > 0 ? actor is SummonMonster && Ids.Contains((actor as SummonMonster).Template.Template.id) : true)
                                 && (States.Count > 0 ? States.All(x => actor.HasState(x)) : true)
                                 && (NoStates.Count > 0 ? NoStates.All(x => !actor.HasState(x)) : true)
                                 && (NoStatesMe.Count > 0 ? NoStatesMe.All(x => !Caster.HasState(x)) : true)
                                 && (StatesMe.Count > 0 ? StatesMe.All(x => Caster.HasState(x)) : true)
                                 && (actor is CharacterFighter ? NoBreed.All(x => x != (actor as CharacterFighter).Character.Infos.Breed) : true)
                                 && (CasterIsAlly ? Caster.Team.Type == actor.Team.Type : true)
                                 && (CasterIsAllySummon ? Caster.Team.Type == actor.Team.Type && Caster is SummonMonster : true)
                                 && (pourcentLife > 0 ? (actor.Stats.Health.Total * 100 / actor.Stats.Health.TotalMax) > pourcentLife : true)
                                 && (myPourcentLife > 0 ? (Caster.Stats.Health.Total * 100 / Caster.Stats.Health.TotalMax) > myPourcentLife : true))
                {
                    result.Add(actor);
                }
            }
            if (Effect.Targets.HasFlag(SpellTargetType.ALLY_ALL))
            {
                if (!result.Contains(actor) && !m_customAffectedActors.Contains(actor) && !actor.IsDead && actor.Team.Team == Caster.Team.Team
                                 && (Ids.Count > 0 ? actor is SummonMonster && Ids.Contains((actor as SummonMonster).Template.Template.id) : true)
                                 && (States.Count > 0 ? States.All(x => actor.HasState(x)) : true)
                                 && (NoStates.Count > 0 ? NoStates.All(x => !actor.HasState(x)) : true)
                                 && (NoStatesMe.Count > 0 ? NoStatesMe.All(x => !Caster.HasState(x)) : true)
                                 && (StatesMe.Count > 0 ? StatesMe.All(x => Caster.HasState(x)) : true)
                                 && (actor is CharacterFighter ? NoBreed.All(x => x != (actor as CharacterFighter).Character.Infos.Breed) : true)
                                 && (CasterIsAlly ? Caster.Team.Type == actor.Team.Type : true)
                                 && (CasterIsAllySummon ? Caster.Team.Type == actor.Team.Type && Caster is SummonMonster : true)
                                 && (pourcentLife > 0 ? (actor.Stats.Health.Total * 100 / actor.Stats.Health.TotalMax) > pourcentLife : true)
                                 && (myPourcentLife > 0 ? (Caster.Stats.Health.Total * 100 / Caster.Stats.Health.TotalMax) > myPourcentLife : true))
                    result.Add(actor);
            }
            if (Effect.Targets.HasFlag(SpellTargetType.ALL_SUMMONS))
            {
                if (!result.Contains(actor) && !m_customAffectedActors.Contains(actor) && !actor.IsDead && actor is SummonMonster
                                 && (Ids.Count > 0 ? Ids.Contains((actor as SummonMonster).Template.Template.id) : true)
                                 && (States.Count > 0 ? States.All(x => actor.HasState(x)) : true)
                                 && (NoStates.Count > 0 ? NoStates.All(x => !actor.HasState(x)) : true)
                                 && (isSelf ? (actor as SummonMonster).Summoner.Id == Caster.Id : true)
                                 && (NoStatesMe.Count > 0 ? NoStatesMe.All(x => !Caster.HasState(x)) : true)
                                 && (StatesMe.Count > 0 ? StatesMe.All(x => Caster.HasState(x)) : true)
                                 && (actor is CharacterFighter ? NoBreed.All(x => x != (actor as CharacterFighter).Character.Infos.Breed) : true)
                                 && (CasterIsAlly ? Caster.Team.Type == actor.Team.Type : true)
                                 && (CasterIsAllySummon ? Caster.Team.Type == actor.Team.Type && Caster is SummonMonster : true)
                                 && (pourcentLife > 0 ? (actor.Stats.Health.Total * 100 / actor.Stats.Health.TotalMax) > pourcentLife : true)
                                 && (myPourcentLife > 0 ? (Caster.Stats.Health.Total * 100 / Caster.Stats.Health.TotalMax) > myPourcentLife : true))
                    result.Add(actor);
            }
            if (Effect.Targets.HasFlag(SpellTargetType.ALL_HUMAN))
            {
                if (!result.Contains(actor) && !m_customAffectedActors.Contains(actor) && !actor.IsDead && !(actor is SummonMonster)
                                 && (Ids.Count > 0 ? actor is SummonMonster && Ids.Contains((actor as SummonMonster).Template.Template.id) : true)
                                 && (States.Count > 0 ? States.All(x => actor.HasState(x)) : true)
                                 && (NoStates.Count > 0 ? NoStates.All(x => !actor.HasState(x)) : true)
                                 && (NoStatesMe.Count > 0 ? NoStatesMe.All(x => !Caster.HasState(x)) : true)
                                 && (StatesMe.Count > 0 ? StatesMe.All(x => Caster.HasState(x)) : true)
                                 && (actor is CharacterFighter ? NoBreed.All(x => x != (actor as CharacterFighter).Character.Infos.Breed) : true)
                                 && (CasterIsAlly ? Caster.Team.Type == actor.Team.Type : true)
                                 && (CasterIsAllySummon ? Caster.Team.Type == actor.Team.Type && Caster is SummonMonster : true)
                                 && (pourcentLife > 0 ? (actor.Stats.Health.Total * 100 / actor.Stats.Health.TotalMax) > pourcentLife : true)
                                 && (myPourcentLife > 0 ? (Caster.Stats.Health.Total * 100 / Caster.Stats.Health.TotalMax) > myPourcentLife : true))
                    result.Add(actor);
            }
            if (Effect.Targets.HasFlag(SpellTargetType.ALLY_SUMMONS))
            {
                if (!result.Contains(actor) && !m_customAffectedActors.Contains(actor) && !actor.IsDead && actor.Team.Team == Caster.Team.Team && (actor is SummonMonster)
                                 && (Ids.Count > 0 ? Ids.Contains((actor as SummonMonster).Template.Template.id) : true)
                                 && (States.Count > 0 ? States.All(x => actor.HasState(x)) : true)
                                 && (NoStates.Count > 0 ? NoStates.All(x => !actor.HasState(x)) : true)
                                 && (isSelf ? (actor as SummonMonster).Summoner.Id == Caster.Id : true)
                                 && (NoStatesMe.Count > 0 ? NoStatesMe.All(x => !Caster.HasState(x)) : true)
                                 && (StatesMe.Count > 0 ? StatesMe.All(x => Caster.HasState(x)) : true)
                                 && (actor is CharacterFighter ? NoBreed.All(x => x != (actor as CharacterFighter).Character.Infos.Breed) : true)
                                 && (CasterIsAlly ? Caster.Team.Type == actor.Team.Type : true)
                                 && (CasterIsAllySummon ? Caster.Team.Type == actor.Team.Type && Caster is SummonMonster : true)
                                 && (pourcentLife > 0 ? (actor.Stats.Health.Total * 100 / actor.Stats.Health.TotalMax) > pourcentLife : true)
                                 && (myPourcentLife > 0 ? (Caster.Stats.Health.Total * 100 / Caster.Stats.Health.TotalMax) > myPourcentLife : true))
                    result.Add(actor);
            }
            if (Effect.Targets.HasFlag(SpellTargetType.ENNEMY_SUMMONS))
            {
                if (!result.Contains(actor) && !m_customAffectedActors.Contains(actor) && !actor.IsDead && actor.Team.Team != Caster.Team.Team && (actor is SummonMonster)
                                 && (Ids.Count > 0 ? Ids.Contains((actor as SummonMonster).Template.Template.id) : true)
                                 && (States.Count > 0 ? States.All(x => actor.HasState(x)) : true)
                                 && (NoStates.Count > 0 ? NoStates.All(x => !actor.HasState(x)) : true)
                                 && (isSelf ? (actor as SummonMonster).Summoner.Id == Caster.Id : true)
                                 && (NoStatesMe.Count > 0 ? NoStatesMe.All(x => !Caster.HasState(x)) : true)
                                 && (StatesMe.Count > 0 ? StatesMe.All(x => Caster.HasState(x)) : true)
                                 && (actor is CharacterFighter ? NoBreed.All(x => x != (actor as CharacterFighter).Character.Infos.Breed) : true)
                                 && (CasterIsAlly ? Caster.Team.Type == actor.Team.Type : true)
                                 && (CasterIsAllySummon ? Caster.Team.Type == actor.Team.Type && Caster is SummonMonster : true)
                                 && (pourcentLife > 0 ? (actor.Stats.Health.Total * 100 / actor.Stats.Health.TotalMax) > pourcentLife : true)
                                 && (myPourcentLife > 0 ? (Caster.Stats.Health.Total * 100 / Caster.Stats.Health.TotalMax) > myPourcentLife : true))
                    result.Add(actor);
            }
            if (Effect.Targets.HasFlag(SpellTargetType.TELEPORT_FIGHTER))
            {
                result.AddRange(Fight.FighterTeleport);
            }
            m_customAffectedActors.AddRange(result);
        }
        public void RefreshZone()
        {
            this.m_affectedCells = this.EffectZone.GetCells(this.TargetedCell, this.Fight.Map);
        }

        public StatBuff AddStatBuff(Fighter target, int value, PlayerFields caracteritic, bool dispelable, bool declanched)
        {
            int id = target.PopNextBuffId();
            StatBuff statBuff = new StatBuff(id, target, this.Caster, this.Effect, this.Spell, value, caracteritic, this.Critical, dispelable, declanched);
            target.AddAndApplyBuff(statBuff, true);
            return statBuff;
        }
        public StatBuff AddStatBuff(Fighter target, int value, PlayerFields caracteritic, bool dispelable, short customActionId, bool declanched, short customValue = -1)
        {
            int id = target.PopNextBuffId();
            StatBuff statBuff = new StatBuff(id, target, this.Caster, this.Effect, this.Spell, value, caracteritic, this.Critical, dispelable, customActionId, declanched);
            statBuff.CustomValue = customValue;
            target.AddAndApplyBuff(statBuff, true);
            return statBuff;
        }

        public void AddTriggerBuff(Fighter target, TriggerBuff trigger)
        {
            int id = target.PopNextBuffId();
            target.AddAndApplyBuff(trigger, true);
        }

        //public TriggerBuff AddTriggerBuff(FightActor target, bool dispelable, BuffTriggerType trigger, TriggerBuffApplyHandler applyTrigger, TriggerBuffRemoveHandler removeTrigger)
        //{
        //    int id = target.PopNextBuffId();
        //    TriggerBuff triggerBuff = new TriggerBuff(id, target, this.Caster, this.Dice, this.Spell, this.Critical, dispelable, trigger, applyTrigger, removeTrigger);
        //    target.AddAndApplyBuff(triggerBuff, true);
        //    return triggerBuff;
        //}

        //public TriggerBuff AddTriggerBuff(FightActor target, FightActor caster, bool dispelable, BuffTriggerType trigger, TriggerBuffApplyHandler applyTrigger)
        //{
        //    int id = target.PopNextBuffId();
        //    TriggerBuff triggerBuff = new TriggerBuff(id, target, caster, this.Dice, this.Spell, this.Critical, dispelable, trigger, applyTrigger);
        //    target.AddAndApplyBuff(triggerBuff, true);
        //    return triggerBuff;
        //}

        public StateBuff AddStateBuff(Fighter target, bool dispelable, SpellState state, bool declanched)
        {
            int id = target.PopNextBuffId();
            StateBuff stateBuff = new StateBuff(id, target, this.Caster, this.Dice, this.Spell, dispelable, 950, state, declanched);
            target.AddAndApplyBuff(stateBuff, true);
            return stateBuff;
        }

        public virtual bool RequireSilentCast()
        {
            return false;
        }
    }
}
