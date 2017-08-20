using Rebirth.Common.Network;
using Rebirth.Common.Protocol.Data;
using Rebirth.Common.Protocol.Enums;
using Rebirth.Common.Protocol.Messages;
using Rebirth.Common.Protocol.Types;
using Rebirth.Common.Thread;
using Rebirth.Common.Timers;
using Rebirth.World.Datas.Characters;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Datas.Monsters;
using Rebirth.World.Datas.Spells;
using Rebirth.World.Frames;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Game.Fights.Buffs;
using Rebirth.World.Game.Fights.Challenges;
using Rebirth.World.Game.Fights.Custom;
using Rebirth.World.Game.Fights.Marks;
using Rebirth.World.Game.Fights.Other;
using Rebirth.World.Game.Fights.Result;
using Rebirth.World.Game.Fights.Team;
using Rebirth.World.Game.Spells.Cast;
using Rebirth.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rebirth.World.Game.Fights
{
    public class Fight : FightContent
    {

        #region Constructor
        public TimerCore StartTimer;

        public Fight(int id, MapTemplate map) : base(id , map)
        {
            Events = new FightEvents(this);
            BlueCells = Map.BlueCells;
            RedCells = Map.RedCells;         
            PassTurnOk += OnPassTurn;
            FightEndOk += EndFight;
            MarkManager = new FightMarkManager(this);
        }
        #endregion

        #region Actions
        public Action<Fighter> ActorAdded;
        public Action<Fighter> ActorRemoved;
        public Action ActorChangedPosition;
        public Action<Fighter> SetReady;
        public Action StartedFight;
        public Action<Fighter, SequenceTypeEnum> SequenceStart;
        public Action<Fighter, SequenceTypeEnum, SequenceTypeEnum> SequenceEnd;
        public Action<Fighter, Fighter, ActionsEnum, int> PointsVariation;
        public Action<Fighter, Damage> LifePointChange;
        public Action<Fighter, Fighter, int, int, int> ShieldPointChange;
        public Action<Fighter, Fighter, int> Heal;
        public Action<Fighter, SpellCastHandler, CellMap, FightSpellCastCriticalEnum, bool> CastedSpell;
        public Action<Fighter, SpellCastHandler, CellMap, FightSpellCastCriticalEnum, bool> CastedWeapon;
        public Action<Fighter, uint> NoCastSpell;
        public Action TurnStarted;
        public Action TurnPassed;
        public Action FightEnd;
        public Action<Fighter, short[]> Move;
        public Action<Fighter, Fighter, short, ActionsEnum> Slide;
        public Action<Fighter, Fighter, short, ActionsEnum> Teleport;
        public Action<Fighter, Fighter> Die;
        public Action<Buff> BuffAdd;
        public Action<Buff> BuffDispell;
        public Action<Buff> BuffRemove;
        public Action<Fighter, double[]> Tackle;
        public Action<Fighter, Fighter, ActionsEnum> ExchangePosition;
        public Action<Fighter, GameActionMark> GlyphAdd;
        public Action<Glyph> GlyphRemoved;
        public Action<Portail> PortailRemoved;
        public Action<Wall> WallRemoved;
        public Action<SummonMonster> SummonAdded;
        public Action<Fighter, Fighter, short> ModifyEffectDuration;
        public Action<Fighter, Fighter, EntityLook> ModifyLook;
        public Action<Fighter, GameActionMark> TrapAdd;
        public Action<Trap> TrapRemoved;
        public Action<InvisibleBuff, sbyte> SetVisibility;
        public Action<SummonMonster> SwitchController;
        public Action<SummonMonster> ResetController;
        public Action<CharacterFighter> Reconnexion;
        public Action<Fighter, Fighter, int, int> DodgePointLost;
        public Action<Portail> ActivePortail;
        public Action RequestPassTurn;
        public Action<Fighter, Fighter, SpellTemplate, int> ChangeCooldown;
        public Action<Fighter, Fighter, Portail> DeclanchedPortail;
        public Action<Fighter, GameActionMarkedCell> UnmarkCell;
        public Action<Fighter, sbyte, bool> ChangeOption;
        public Action<ChallengeHandler> ChallUpdate;
        public Action<ChallengeHandler> ChallTargetUpdate;

        private Action PassTurnOk;
        private Action FightEndOk;
        public bool Finish = false;
        #endregion

        #region Traps
        public bool HasGlyph(int cellId)
        {
            return MarkManager.HasGlyph(cellId);
        }
        public bool HasTrap(int cellId)
        {
            return MarkManager.HasTrap(cellId);
        }
        public bool HasCenterTrap(int cellId)
        {
            return MarkManager.HasCenterTrap(cellId);
        }
        public void DeclancheTrap(int cellId)
        {
            MarkManager.DeclancheTrap(cellId);
        }
        #endregion

        #region Private Functions
        private void CheckAdderMap()
        {
            if (Teams[0].Fighters.Count > 0 && Teams[1].Fighters.Count > 0)
            {
                Map.AddFight(this);
            }
        }
        #endregion

        #region Get Functions
        public Fighter GetFighter(int id)
        {
            return Fighters.FirstOrDefault(x => x.Id == id && x.IsAlive);
        }
        public Fighter GetFighter(CellMap cell)
        {
            return Fighters.FirstOrDefault(x => x.Point.Point.CellId == cell.Id && x.IsAlive);
        }
        public Fighter GetFighter(MapPoint cell)
        {
            if (cell == null)
                return null;
            return Fighters.FirstOrDefault(x => x.Point.Point.CellId == cell.CellId && x.IsAlive);
        }

        public Fighter[] GetAllFighter(CellMap[] cells)
        {
            List<Fighter> list = new List<Fighter>();
            foreach (var item in cells)
            {
                list.AddRange((from x in Fighters
                               where x.Point.Point.CellId == item.Id
                               select x));
            }
            return list.ToArray();
        }

        public Fighter[] GetAllFighter(MapPoint[] cells)
        {
            List<Fighter> list = new List<Fighter>();
            foreach (var item in cells)
            {
                list.AddRange((from x in Fighters
                               where x.Point.Point.CellId == item.CellId
                               select x));
            }
            return list.ToArray();
        }

        public double[] GetDieIds()
        {
            return (from x in Fighters
                    where x.IsDead
                    select (double)x.Id).ToArray();
        }

        public FightCommonInformations GetFightCommonInformations()
        {
            return new FightCommonInformations(FightId, (sbyte)Type, (from x in Teams
                                                                     where x != null && x.Team != TeamEnum.TEAM_SPECTATOR
                                                                     select x.GetTeamInformation()).ToArray(), (from x in Teams
                                                                                                                where x != null && x.Team != TeamEnum.TEAM_SPECTATOR
                                                                                                                select (uint)x.CellId).ToArray(), new FightOptionsInformations[] {
                                                                                                                                                  new FightOptionsInformations(false, false, false, false),
                                                                                                                                                  new FightOptionsInformations(false, false, false, false) });
        }
        public FightExternalInformations GetFightExternalInformations()
        {
            var data = new FightExternalInformations(FightId, (sbyte)Type, IsStarting ? (int)DateTime.Now.Subtract(StartedTime).TotalMilliseconds : 0, IsSecret, (from x in Teams
                                                                                                                                                             where x != null && x.Team != TeamEnum.TEAM_SPECTATOR
                                                                                                                                                             select x.GetFightTeamLightInformations()).ToArray(), (from x in Teams
                                                                                                                                                                                                                   where x.Team != TeamEnum.TEAM_SPECTATOR
                                                                                                                                                                                                                   select x.GetFightOptionsInformations()).ToArray());
            return data;
        }

        public bool IsCellFree(CellMap cell)
        {
            var fighter = GetFighter(cell);
            if (fighter != null && fighter.IsAlive)
                return false;
            return  cell.Mov && !cell.NonWalkableDuringFight && !cell.FarmCell;
        }

        public bool IsCellFree(MapPoint cell)
        {
            var data = Map.Cells[cell.CellId];
            var fighter = GetFighter(cell);
            if (fighter != null && fighter.IsAlive)
                return false;
            return data != null && data.Mov && !data.NonWalkableDuringFight && !data.FarmCell;
        }

        public sbyte GetNextContextualId()
        {
            return (sbyte)(ContextualIdProvider.Pop());
        }

        public GameActionMark[] GetMarks(Fighter fighter)
        {
            List<GameActionMark> marks = new List<GameActionMark>();
            marks.AddRange(MarkManager.Glyphs.Select(x => x.GetGameActionMark()));
            marks.AddRange(MarkManager.Traps.FindAll(x => x.Caster.Team.Type == fighter.Team.Type || x.IsVisible).Select(x => x.GetGameActionMark()));
            return marks.ToArray();
        }

        public void SetRevealTrap(Fighter fighter, int[] cells)
        {
            var traps = MarkManager.GetTraps(cells);
            foreach (var trap in traps)
            {
                trap.IsVisible = true;
                TrapAdd(fighter, trap.GetGameActionMark());
            }
        }
        #endregion

        #region Public Functions
        public virtual void AddCharacter(Character character, TeamEnum team)
        {
            character.Fight = this;

            var fighter = new CharacterFighter(character, Teams[(byte)team], this);
            SetStartPoint(fighter);

            character.Fighter = fighter;

            Teams[(byte)team].AddFighter(fighter);
            Fighters.Add(fighter);

            ActorAdded(fighter);
            CheckAdderMap();

            //character.StopRegen();
        }
        public void AddMonster(MonsterTemplate monster, TeamEnum team, int id)
        {
            var fighter = new MonsterFighter(monster, Teams[(byte)team], this, id);
            SetStartPoint(fighter);

            Teams[(byte)team].AddFighter(fighter);
            Fighters.Add(fighter);

            ActorAdded(fighter);
            CheckAdderMap();
        }
        public void AddSpectator(Character character)
        {
            character.Map.Quit(character.Client);
            var fighter = new SpectatorFighter(character, Teams[2], this);
            Teams[2].AddFighter(fighter);
            Spectators.Add(fighter);

            ActorAdded(fighter);
        }
        public virtual void RemoveCharacter(Fighter fighter)
        {
            if (IsStarting)
            {
                Character character;
                
                Leavers.Add(fighter);
                if (fighter is CharacterFighter)
                {
                    character = (fighter as CharacterFighter).Character;
                    character.Client.UnRegister(typeof(FightContextFrame));
                    character.Client.Register(typeof(InventoryFrame));
                    character.Client.Register(typeof(WorldFrame));
                }
                else
                {
                    character = (fighter as SpectatorFighter).Character;
                    Spectators.Remove(fighter as SpectatorFighter);
                }

                if (fighter is CharacterFighter)
                {
                    StartSequence(SequenceTypeEnum.SEQUENCE_CHARACTER_DEATH);
                    fighter.Die(fighter);
                    fighter.Stats.Health.PermanentDamages = 0;
                    fighter.RemoveAllBuff();
                    EndSequence(SequenceTypeEnum.SEQUENCE_CHARACTER_DEATH);
                }
                CheckFightEnd();
            }
            else
            {
                Character character;               
                if (fighter is CharacterFighter)
                {
                    character = (fighter as CharacterFighter).Character;
                    fighter.Die(fighter);
                    fighter.Stats.Health.PermanentDamages = 0;
                    fighter.RemoveAllBuff();
                    (fighter as CharacterFighter).Character.Stats.ForceResetStat();
                    //if (!fighter.IsDisconnected)
                    //{
                    //    character.StartRegen();
                    //}
                    character.Fighter = null;
                    character.Fight = null;
                    fighter.Stats.Health.PermanentDamages = 0;
                    if (fighter.IsDead)
                    {
                        fighter.Stats.Health.DamageTaken = fighter.Stats.Health.TotalMax - 1;
                    }
                    //character.Inventory.DecrementCandys();
                    character.Client.Send(new GameContextDestroyMessage());
                    character.Client.Send(new GameContextCreateMessage(1));
                    character.Client.UnRegister(typeof(FightContextFrame));
                    character.Client.Register(typeof(InventoryFrame));
                    character.Client.Register(typeof(WorldFrame));
                    character.RefreshStats();
                    //character.Respawn();
                    fighter.Team.Fighters.Remove(fighter);
                    Fighters.Remove(fighter);
                    ActorRemoved(fighter);
                    if (Fighters.Count(x => x is CharacterFighter) <= 0)
                        EndFight();
                }
                else
                {
                    character = (fighter as SpectatorFighter).Character;
                    Spectators.Remove(fighter as SpectatorFighter);
                }
            }
        }
        public virtual void KickCharacter(Fighter by, double target)
        {
            if (!IsStarting && by.Team.LeaderId == by.Id)
            {
                var fighter = Fighters.FirstOrDefault(x => x.Id == target);
                if (fighter == null)
                    return;
                Character character;
                if (fighter is CharacterFighter)
                {
                    character = (fighter as CharacterFighter).Character;
                    (fighter as CharacterFighter).Character.Stats.ForceResetStat();
                    //if (!fighter.IsDisconnected)
                    //{
                    //    character.StartRegen();
                    //}
                    character.Fighter = null;
                    character.Fight = null;
                    character.Client.Send(new GameContextDestroyMessage());
                    character.Client.Send(new GameContextCreateMessage(1));
                    character.Client.UnRegister(typeof(FightContextFrame));
                    character.Client.Register(typeof(InventoryFrame));
                    character.Client.Register(typeof(WorldFrame));
                    character.RefreshStats();
                    Map.Enter(character.Client);
                    fighter.Team.Fighters.Remove(fighter);
                    Fighters.Remove(fighter);
                    ActorRemoved(fighter);
                    if (Fighters.Count(x => x is CharacterFighter) <= 0)
                        EndFight();
                }
            }
        }
        public virtual void DeconnectCharacter(Fighter fighter)
        {
            fighter.IsDisconnected = true;
            //WorldManager.Instance.AddDisconnect((fighter as CharacterFighter).Character);
            fighter.TurnDisconnect = TimeLine != null ? (int)TimeLine.ActualRound : 0;
            SendToAllCharacter(new TextInformationMessage(1, 182, new string[] { (fighter as CharacterFighter).Character.Infos.Name, "15" }));
        }
        public virtual void ReconnectCharacter(CharacterFighter fighter)
        {
            fighter.Character.Client.UnRegister(typeof(InventoryFrame));
            fighter.Character.Client.UnRegister(typeof(WorldFrame));
            fighter.Character.Client.Register(typeof(FightContextFrame));
            Reconnexion(fighter);
            fighter.IsDisconnected = false;
            //WorldManager.Instance.RemoveDisconnect(fighter.Character);
            SendToAllCharacter(new TextInformationMessage(1, 184, new string[] { fighter.Character.Infos.Name }));
        }
        public void SendToAllCharacter(NetworkMessage msg)
        {
            foreach(var player in Fighters)
            {
                if (player is CharacterFighter)
                {
                    if((player as CharacterFighter).Character.Client != null)
                        (player as CharacterFighter).Character.Client.Send(msg);               
                }
                        
            }
            
            foreach (var player in Spectators)
            {
                if (player is SpectatorFighter)
                {
                    (player as SpectatorFighter).Character.Client.Send(msg);
                }

            }
        }
        public void SendToAllSpectator(NetworkMessage msg)
        {
            foreach (var player in Spectators)
            {
                if (player is SpectatorFighter)
                {
                    (player as SpectatorFighter).Character.Client.Send(msg);
                }

            }
        }
        public void CheckStart()
        {
            if ((from x in Fighters
                 where !x.IsReady
                 select x).Count() == 0)
                StartFight();
        }
        public virtual void StartFight()
        {
            foreach (CharacterFighter fighter in Fighters.FindAll(x => x is CharacterFighter))
            {
                fighter.Character.Client.UnRegister(typeof(InventoryFrame));
                fighter.Character.Client.UnRegister(typeof(WorldFrame));
                fighter.Character.Client.Register(typeof(FightContextFrame));
            }
            Fighters = SetFightersOrder();
            ContextualIdProvider = new Common.Pool.UniqueIdProvider(Fighters.Aggregate((i1, i2) => i1.Id > i2.Id ? i1 : i2).Id); //items1.Aggregate((i1, i2) => i1.ID > i2.ID ? i1 : i2);
            Map.HideBlades(this);
            StartTimer.Dispose();
            StartedTime = DateTime.Now;
            IsStarting = true;
            TimeLine = new Other.TimeLine(Fighters);
            StartedFight();

            if (TimeLine.NextPlayer())
                IsPassing = false;

            TimeLine.FighterPlaying.OnTurnStarted();
            TurnStarted();
            //TurnStarted();
            //TimeLine.FighterPlaying.OnTurnStarted();
        }
        public void VerifChall(ChallengeActionEnum action, object token)
        {
            foreach (var item in Challenges)
            {
                item.Apply(action, token);
            }
        }
        public List<Fighter> SetFightersOrder()
        {
            List<Fighter> fighters1 = Fighters.FindAll(x => x.Team.Team == TeamEnum.TEAM_CHALLENGER).OrderByDescending(x => x.Stats[PlayerFields.Initiative].Total).ToList();
            List<Fighter> fighters2 = Fighters.FindAll(x => x.Team.Team == TeamEnum.TEAM_DEFENDER).OrderByDescending(x => x.Stats[PlayerFields.Initiative].Total).ToList();

            if (fighters1.Count > 0 && fighters2.Count > 0)
            {
                var newfighters = new List<Fighter>();

                if (fighters1[0].Stats.Initiative.Total > fighters2[0].Stats.Initiative.Total)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        if (fighters1.Count > i && fighters2.Count <= i)
                        {
                            newfighters.Add(fighters1[i]);
                        }
                        else if (fighters1.Count <= i && fighters2.Count > i)
                        {
                            newfighters.Add(fighters2[i]);
                        }
                        else if (fighters1.Count > i && fighters2.Count > i)
                        {
                            newfighters.Add(fighters1[i]);
                            newfighters.Add(fighters2[i]);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 8; i++)
                    {
                        if (fighters1.Count > i && fighters2.Count <= i)
                        {
                            newfighters.Add(fighters1[i]);
                        }
                        else if (fighters1.Count <= i && fighters2.Count > i)
                        {
                            newfighters.Add(fighters2[i]);
                        }
                        else if(fighters1.Count > i && fighters2.Count > i)
                        {
                            newfighters.Add(fighters2[i]);
                            newfighters.Add(fighters1[i]);
                        }
                    }
                }

                return newfighters;
            }
            return Fighters;
        }
        public ChallengeHandler GetChall(int challId)
        {
            return Challenges.FirstOrDefault(x => (int)x.Chall == challId);
        }
        public void PassTurn()
        {
            if (!SpellEnd())
            {
                if (IsSequencing)
                {
                    EndSequence(Sequence, true);
                }
                if (WaitAcknowledgment)
                {
                    AcknowledgeAction();
                }
                OnPassTurn();
            }
            else
                CheckFightEnd();
        }

        public bool IsPlayerTurn(int id)
        {
            if(IsStarting)
                return TimeLine.FighterPlaying.Id == id;
            return false;
        }

        public bool StartSequence(SequenceTypeEnum sequenceType)
        {
            m_lastSequenceAction = sequenceType;
            m_sequenceLevel++;
            bool result;
            if (IsSequencing)
            {
                result = false;
            }
            else
            {
                IsSequencing = true;
                Sequence = sequenceType;
                m_sequences.Push(sequenceType);
                SequenceStart(TimeLine.FighterPlaying, sequenceType);
                result = true;
            }
            return result;
        }

        public bool EndSequence(SequenceTypeEnum sequenceType, bool force = false)
        {
            bool result;
            if (!IsSequencing)
            {
                result = false;
            }
            else
            {
                m_sequenceLevel--;
                if (m_sequenceLevel > 0 && !force)
                {
                    result = false;
                }
                else if (TimeLine.FighterPlaying is MonsterFighter)
                {
                    EndAllSequences();
                    result = true;
                }
                else
                {
                    IsSequencing = false;
                    WaitAcknowledgment = true;
                    SequenceTypeEnum sequenceTypeEnum = m_sequences.Pop();
                    if (sequenceTypeEnum != sequenceType)
                    {
                        Starter.Logger.Error("Popped Sequence different (" + sequenceTypeEnum +" != "+ sequenceType +")");
                    }
                    SequenceEnd(TimeLine.FighterPlaying, sequenceType, m_lastSequenceAction);
                    result = true;
                }
            }
            return result;
        }

        public void EndAllSequences()
        {
            m_sequenceLevel = 0;
            IsSequencing = false;
            WaitAcknowledgment = false;
            while (m_sequences.Count > 0)
            {
                SequenceTypeEnum sequenceType = m_sequences.Pop();
                SequenceEnd(TimeLine.FighterPlaying, sequenceType, m_lastSequenceAction);
            }
        }

        public virtual void AcknowledgeAction()
        {
            WaitAcknowledgment = false;
        }

        public void RemoveAllBuff(Fighter fighter)
        {
            foreach (var item in Fighters.FindAll(x => x.IsAlive))
            {
                item.RemoveAllBuff(fighter);
            }
        }

        public void UpdateOption(Fighter fighter, sbyte option)
        {
            if (fighter.Team.LeaderId == fighter.Id)
            {
                switch ((FightOptionsEnum)option)
                {
                    case FightOptionsEnum.FIGHT_OPTION_SET_SECRET:
                        Teams[0].IsSecret = !IsSecret;
                        Teams[1].IsSecret = !IsSecret;
                        IsSecret = !IsSecret;
                        ChangeOption(fighter, option, IsSecret);
                        break;
                    case FightOptionsEnum.FIGHT_OPTION_SET_TO_PARTY_ONLY:
                        fighter.Team.PartyOnly = !fighter.Team.PartyOnly;
                        ChangeOption(fighter, option, fighter.Team.PartyOnly);
                        break;
                    case FightOptionsEnum.FIGHT_OPTION_SET_CLOSED:
                        fighter.Team.Close = !fighter.Team.Close;
                        ChangeOption(fighter, option, fighter.Team.Close);
                        break;
                    case FightOptionsEnum.FIGHT_OPTION_ASK_FOR_HELP:
                        fighter.Team.NeedHelp = !fighter.Team.NeedHelp;
                        ChangeOption(fighter, option, fighter.Team.NeedHelp);
                        break;
                }
            }
        }
        #endregion

        #region Placement
        public bool CellIsInPlacement(int id, FightTeam team)
        {
            if (team.Team == TeamEnum.TEAM_CHALLENGER)
                return RedCells.Any(e => e.Id == id);

            return BlueCells.Any(e => e.Id == id);
        }
        public void SetStartPoint(Fighter fighter)
        {
            if (!this.CellIsInPlacement(fighter is CharacterFighter ? (fighter as CharacterFighter).Character.Infos.CellId : 0, fighter.Team))
            {
                var playersInTeam = fighter.Team.Fighters.FindAll(x => x.Id != fighter.Id);
                CellMap newCell;
                if (fighter.Team.Team == TeamEnum.TEAM_CHALLENGER)
                    newCell = RedCells.First(x => !playersInTeam.Any(e => e.Point.Point.CellId == x.Id));
                else
                    newCell = BlueCells.First(x => !playersInTeam.Any(e => e.Point.Point.CellId == x.Id));

                fighter.Point = new Other.FightDisposition(newCell.Id, DirectionsEnum.DIRECTION_SOUTH_EAST);
            }
            else
                fighter.Point = new Other.FightDisposition(fighter is CharacterFighter ? (fighter as CharacterFighter).Character.Infos.CellId : 0, DirectionsEnum.DIRECTION_SOUTH_EAST);
            UpdateFightersPlacementDirection();
        }
        public void ChangeActorPosition(Fighter fighter, int newPos)
        {
            bool has = false;
            if (fighter.Team.Team == TeamEnum.TEAM_CHALLENGER)
                has = RedCells.Any(w => w.Id == newPos);
            else
                has = BlueCells.Any(w => w.Id == newPos);

            var currentPosition = fighter.Point.Point.CellId;
            if (has)
            {
                fighter.Point = new Other.FightDisposition((short)newPos, fighter.Point.Dir);
                UpdateFightersPlacementDirection();
                ActorChangedPosition();
            }
        }
        protected void UpdateFightersPlacementDirection()
        {
            foreach (Fighter current in Fighters)
            {
                current.Point.Dir = FindPlacementDirection(current);
            }
        }
        public DirectionsEnum FindPlacementDirection(Fighter fighter)
        {
            FightTeam fightTeam = fighter.Team.Team == TeamEnum.TEAM_CHALLENGER ? Teams[1] : Teams[0];
            Tuple<CellMap, uint> tuple = null;
            foreach (Fighter current in fightTeam.Fighters)
            {
                MapPoint point = current.Point.Point;
                if (tuple == null)
                {
                    tuple = Tuple.Create(Map.Cells[current.Point.Point.CellId], fighter.Point.Point.DistanceToCell(point));
                }
                else
                {
                    if (fighter.Point.Point.DistanceToCell(point) < tuple.Item2)
                    {
                        tuple = Tuple.Create(Map.Cells[current.Point.Point.CellId], fighter.Point.Point.DistanceToCell(point));
                    }
                }
            }
            DirectionsEnum result;
            if (tuple == null)
            {
                result = fighter.Point.Dir;
            }
            else
            {
                //result = new MapPoint(tuple.Item1).OrientationTo(fighter.Point.Point, false);
                result = fighter.Point.Point.OrientationTo(new MapPoint(tuple.Item1), false);
            }
            return result;
        }
        #endregion

        #region End Fight
        public void CommandFightEnd(bool winFight, Fighter currentFighter = null)
        {
            if (winFight == true && currentFighter != null)
            {
                foreach (var tmp in this.Fighters)
                {
                    if (tmp != null && tmp.IsAlive && tmp.Team != currentFighter.Team)
                    {
                        tmp.Die(currentFighter);
                    }
                }
                this.CheckFightEnd();
                return;
            }
            else
            {
                EndAllSequences();

                TimeLine.FighterPlaying = null;

                var fightersNew = (from x in Fighters
                                   select x).ToList();
                foreach (var fighter in fightersNew)
                {
                    fighter.KillAllSummon();
                }
                FightEnd();
                ResetFightersProperties();
                ReturnInMap(true);
                Dispose();
            }
        }
        public void CheckFightEnd()
        {
            if (!IsEnded && (Teams[0].AreAllDead() || Teams[1].AreAllDead()))
            {
                IsEnded = true;
                if (IsStarting)
                {
                    //if (m_sequences.FirstOrDefault() == SequenceTypeEnum.SEQUENCE_SPELL && TimeLine.FighterPlaying is CharacterFighter)
                    //{
                    //    var rdn = new AsyncRandom().Next(0, 100);
                    //    if (rdn <= 30)
                    //    {
                    //        var move = (TimeLine.FighterPlaying as CharacterFighter).Character.GetOneFinisher();
                    //        var spell = SpellManager.Instance.GetSpellLevel(move.spellLevel);
                    //        FinishDuration = move.duration;
                    //        finisherSpell = spell;
                    //    }
                    //}
                    //if (FinishDuration > 0)
                    //    WorldServer.Instance.Pool.CallDelayed(FinishDuration + 500, EndFight);
                    //else
                        new TimerCore(EndFight, 2000);
                }
                else
                    CancelFight();
            }
        }

        public int FinishDuration
        {
            get;
            set;
        }
        public SpellLevel finisherSpell
        {
            get;
            set;
        }

        public bool SpellEnd()
        {
            if (IsEnded && (Teams[0].AreAllDead() || Teams[1].AreAllDead()))
                if (IsStarting)
                    return true;
                else
                    return true;
            return false;
        }

        public void EndFight()
        {
            //if (EndIsCalled)
            //    return;
            //EndIsCalled = true;

            EndAllSequences();

            foreach (var item in Challenges)
            {
                item.Finish();
            }

            //AcknowsManager.Reset();
            if(TimeLine != null)
            {
                TimeLine.FighterPlaying = null;
            }

            var fightersNew = (from x in Fighters
                               select x).ToList();
            foreach (var fighter in fightersNew)
            {
                fighter.KillAllSummon();
            }


            DeterminsWinners();

            // Verification de fin pur les Challenges ici    
            FightEnd();

            ResetFightersProperties();

            foreach (Fighter item in Fighters)
            {
                if (item.Result != null)
                    item.Result.AssignResult();
            }

            ReturnInMap();

            Dispose();
        }

        public void CancelFight()
        {
            Map.HideBlades(this);
            FightEnd();
            ResetFightersProperties();
            ReturnInMap();
            Dispose();
        }

        public void DeterminsWinners()
        {
            if (Teams[0].AreAllDead())
            {
                Winner = Teams[1];
                Loser = Teams[0];

                Teams[1].Outcome = FightOutcomeEnum.RESULT_VICTORY;
                Teams[0].Outcome = FightOutcomeEnum.RESULT_LOST;
            }
            else
            {
                Winner = Teams[0];
                Loser = Teams[1];

                Teams[0].Outcome = FightOutcomeEnum.RESULT_VICTORY;
                Teams[1].Outcome = FightOutcomeEnum.RESULT_LOST;
            }

            // Ajouter les recompenses.
            GenerateResult();

            if (this is PvmFight)
            {
                if (Winner.Type == TeamTypeEnum.TEAM_TYPE_PLAYER)
                {
                    AsyncRandom rdn = new AsyncRandom();
                    var list = Winner.Fighters.FindAll(x => x is CharacterFighter);
                    foreach (var item in (this as PvmFight).Group.m_items)
                    {
                        list[rdn.Next(list.Count - 1)].Result.PlayerItems.Add(item);
                    }
                }
            }
        }

        public virtual void ResetFightersProperties()
        {
            foreach (CharacterFighter fighter in Fighters.FindAll(x => x is CharacterFighter))
            {
                fighter.RemoveAllBuff();
                fighter.Character.Stats.ForceResetStat();
                if(!fighter.IsDisconnected)
                {
                    fighter.Character.Client.UnRegister(typeof(FightContextFrame));
                    fighter.Character.Client.Register(typeof(InventoryFrame));
                    fighter.Character.Client.Register(typeof(WorldFrame));
                }
                fighter.Character.Fighter = null;
                fighter.Character.Fight = null;
                fighter.PassTurnEnd();
                fighter.Stats.Health.PermanentDamages = 0;
                if(fighter.IsDead)
                {
                    fighter.Stats.Health.DamageTaken = fighter.Stats.Health.TotalMax - 1;
                }
                //fighter.Character.Inventory.DecrementCandys();
            }
            foreach (MonsterFighter item in Fighters.FindAll(x => x is MonsterFighter))
            {
                item.Stats.Health.PermanentDamages = 0;
                item.Stats.Health.DamageTaken = 0;
                item.PassTurnEnd();
            }
            foreach (SpectatorFighter item in Spectators)
            {
                item.Character.Fighter = null;
                item.Character.Fight = null;
            }
        }

        public virtual void ReturnInMap(bool command = false)
        {
            foreach (CharacterFighter item in Fighters.FindAll(x => x is CharacterFighter && !x.IsDisconnected))
            {
                item.Character.Client.Send(new GameContextDestroyMessage());
                item.Character.Client.Send(new GameContextCreateMessage(1));
                item.Character.RefreshStats();
                //if (Winner.Team != item.Team.Team && !command)
                //    item.Character.Respawn();
                //else
                if (Map.Id == 152306688)
                {
                    item.Character.Quests.ValidationObjective(489, 3510);
                    item.Character.Quests.ValidationObjective(489, 3511);
                    item.Character.Teleport(152306690, item.Character.Infos.CellId);
                }
                else if (Map.Id == 152307712)
                {
                    item.Character.Quests.ValidationObjective(489, 3531);
                    item.Character.Teleport(152307714, 85);
                }
                else
                    Map.Enter(item.Character.Client);
            }
            foreach (SpectatorFighter item in Spectators)
            {
                item.Character.Client.Send(new GameContextDestroyMessage());
                item.Character.Client.Send(new GameContextCreateMessage(1));
                item.Character.RefreshStats();
                Map.Enter(item.Character.Client);
                item.Character.Fight = null;
            }
            //if(!command)
            //{
            //    if (Winner.Type == TeamTypeEnum.TEAM_TYPE_MONSTER)
            //    {
            //        foreach (CharacterFighter player in Loser.Fighters.FindAll(x => x is CharacterFighter))
            //        {
            //            SubAreaManager.Instance.AssignLoot(player.Character.Inventory.Items, Map.SubAreaId);
            //            player.Character.Inventory.Die();
            //        }
            //    }
            //}
            Fighters.Clear();
            Map.HideBlades(this);
        }

        public virtual void GenerateResult()
        {
            GlobalResult result = new GlobalResult(this);
            result.GenerateResult();
        }

        public void Dispose()
        {
            IsEnded = true;
            #region  Remove monster
            if(this is PvmFight)
            {
                if (Winner.Fighters.Any(x => x is MonsterFighter))
                    Map.AddMonster((this as PvmFight).Group);
            }
            #endregion
            Map.HideBlades(this);
            Map.RemoveFight(this);
            Fighters = null;
            Teams = null;
            Map = null;
            TimeLine.Dispose();
            TimeLine = null;
            BlueCells = null;
            RedCells = null;
        }
        #endregion

        #region Events
        private bool IsPassing = false;
        public void OnPassTurn()
        {
            if (!IsPassing && !IsEnded)
            {
                if (TimeLine.FighterPlaying.IsAlive)
                    TimeLine.FighterPlaying.PassTurn();
                IsPassing = true;
                //EndAllSequence();
                TurnPassed();
                foreach (var player in Fighters.FindAll(x => x is CharacterFighter && !x.IsDisconnected && !Leavers.Contains(x)))
                {
                    player.SetReady(false);
                }
                RequestPassTurn();
                while (Fighters.All(x => !x.IsReady))
                { }
                if (TimeLine.NextPlayer())
                    IsPassing = false;

                TimeLine.FighterPlaying.OnTurnStarted();

                TurnStarted();
            }
        }
        #endregion

        #region Command

        #endregion
    }
}
