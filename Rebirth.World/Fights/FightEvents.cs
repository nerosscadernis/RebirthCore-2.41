using Rebirth.Common.Extensions;
using Rebirth.Common.Network;
using Rebirth.Common.Protocol.Enums;
using Rebirth.Common.Protocol.Messages;
using Rebirth.Common.Protocol.Types;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Game.Fights.Buffs;
using Rebirth.World.Game.Fights.Challenges;
using Rebirth.World.Game.Fights.Custom;
using Rebirth.World.Game.Fights.IA;
using Rebirth.World.Game.Fights.Marks;
using Rebirth.World.Game.Fights.Other;
using Rebirth.World.Game.Fights.Team;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Game.Spells;
using Rebirth.World.Game.Spells.Cast;
using Rebirth.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rebirth.World.Datas.Spells;

namespace Rebirth.World.Game.Fights
{
    public class FightEvents
    {
        #region Private Var
        private Fight Fight;
        #endregion

        #region Constructor
        public FightEvents(Fight fight)
        {
            Fight = fight;

            Fight.ActorAdded += OnFighterAdded;
            Fight.ActorRemoved += OnFighterRemoved;
            Fight.ActorChangedPosition += OnFighterChangePosition;
            Fight.SetReady += OnSetReady;
            Fight.StartedFight += OnStartedFight;
            Fight.PointsVariation += OnFighterPointsVariation;
            Fight.SequenceStart += OnSequenceSend;
            Fight.SequenceEnd += OnSequenceEndSend;
            Fight.CastedSpell += OnCastedSpell;
            Fight.LifePointChange += OnLifePointChanged;
            Fight.Heal += OnHealed;
            Fight.NoCastSpell += OnNoCastSpell;
            Fight.TurnStarted += OnTurnStarted;
            Fight.TurnPassed += OnTurnPassed;
            Fight.Move += OnMove;
            Fight.Slide += OnSlide;
            Fight.Teleport += OnTeleport;
            Fight.Die += OnDie;
            Fight.FightEnd += OnFightEnded;
            Fight.BuffAdd += OnBuffAdded;
            Fight.BuffDispell += OnBuffDispelled;
            Fight.BuffRemove += OnBuffRemoved;
            Fight.Tackle += OnTackled;
            Fight.ExchangePosition += OnExchangePosition;
            Fight.GlyphAdd += OnMarkAdded;
            Fight.GlyphRemoved += OnMarkDeleted;
            Fight.SummonAdded += OnSummonAdded;
            Fight.ModifyEffectDuration += OnEffectDuratonModify;
            Fight.ModifyLook += OnModifySkin;
            Fight.TrapAdd += OnTrapAdded;
            Fight.TrapRemoved += OnTrapDeleted;
            Fight.SetVisibility += OnVisibilitySet;
            Fight.SwitchController += OnSwitchController;
            Fight.ResetController += OnResetController;
            Fight.Reconnexion += OnFightReconnect;
            Fight.DodgePointLost += OnDodgePointLoss;
            Fight.RequestPassTurn += OnRequestPassTurn;
            Fight.CastedWeapon += OnCastedWeapon;
            Fight.ShieldPointChange += OnShieldChanged;
            Fight.PortailRemoved += OnPortailDeleted;
            Fight.ActivePortail += OnPortailChange;
            Fight.ChangeCooldown += OnCooldownChanged;
            Fight.DeclanchedPortail += OnDeclanchePortail;
            Fight.WallRemoved += OnWallDeleted;
            Fight.UnmarkCell += OnUnmarkCellFighter;
            Fight.ChangeOption += OnOptionChanged;
            Fight.ChallUpdate += OnChallUpDate;
            Fight.ChallTargetUpdate += OnChallChangeTarget;
        }
        #endregion

        #region Events
        public void OnFighterAdded(Fighter fighter)
        {
            var entityesDisposition = from entry in Fight.Fighters
                                      select new IdentifiedEntityDispositionInformations(entry.Point.Point.CellId, (byte)entry.Point.Dir, entry.Id);
            if (fighter is CharacterFighter)
            {
                #region BaseFight
                SendToFighter(fighter, new GameContextDestroyMessage());
                SendToFighter(fighter, new GameContextCreateMessage(2));
                (fighter as CharacterFighter).Character.RefreshStats();
                SendToFighter(fighter, new GameFightStartingMessage((sbyte)Fight.Type, Fight.Teams[(byte)TeamEnum.TEAM_CHALLENGER] == null ? 0 : Fight.Teams[(byte)TeamEnum.TEAM_CHALLENGER].LeaderId,
                                                                       Fight.Teams[(byte)TeamEnum.TEAM_DEFENDER] == null ? 0 : Fight.Teams[(byte)TeamEnum.TEAM_DEFENDER].LeaderId));
                SendToFighter(fighter, new GameFightJoinMessage(!Fight.IsStarting, false, fighter.IsReady, Fight.IsStarting,(short)(Fight.StartTimer.UntilTime / 100), (sbyte)Fight.Type));
                SendToFighter(fighter, new GameFightPlacementPossiblePositionsMessage((from x in Fight.RedCells
                                                                                       select (uint)x.Id).ToArray(), (from x in Fight.BlueCells
                                                                                                                      select (uint)x.Id).ToArray(), (sbyte)fighter.Team.Team));
                foreach (var item in Fight.Teams)
                {
                    SendToFighter(fighter, new GameFightOptionStateUpdateMessage((short)Fight.FightId, (sbyte)item.Team, (sbyte)FightOptionsEnum.FIGHT_OPTION_SET_CLOSED, item.Close));
                    SendToFighter(fighter, new GameFightOptionStateUpdateMessage((short)Fight.FightId, (sbyte)item.Team, (sbyte)FightOptionsEnum.FIGHT_OPTION_ASK_FOR_HELP, item.NeedHelp));
                    SendToFighter(fighter, new GameFightOptionStateUpdateMessage((short)Fight.FightId, (sbyte)item.Team, (sbyte)FightOptionsEnum.FIGHT_OPTION_SET_SECRET, item.IsSecret));
                    SendToFighter(fighter, new GameFightOptionStateUpdateMessage((short)Fight.FightId, (sbyte)item.Team, (sbyte)FightOptionsEnum.FIGHT_OPTION_SET_TO_PARTY_ONLY, item.PartyOnly));
                }

                SendToFighter(fighter, new IdolFightPreparationUpdateMessage(0, new Idol[0]));
                #endregion

                SendToAll(new GameEntitiesDispositionMessage(entityesDisposition.ToArray()));
                SendToFighter(fighter, new GameFightShowFighterMessage(fighter.GetFighterPreparation()));
                SendToAll(new GameFightUpdateTeamMessage((short)Fight.FightId, fighter.Team.GetTeamInformation()));
                SendToAll(new GameFightShowFighterMessage(fighter.GetFighterPreparation()));

                SendToFighter(fighter, new GameFightUpdateTeamMessage((short)Fight.FightId, Fight.Teams[(byte)TeamEnum.TEAM_CHALLENGER].GetTeamInformation()));
                SendToFighter(fighter, new GameFightUpdateTeamMessage((short)Fight.FightId, Fight.Teams[(byte)TeamEnum.TEAM_DEFENDER].GetTeamInformation()));

                foreach (var item in Fight.Fighters.FindAll(x => x.Id != fighter.Id))
                {
                    SendToFighter(fighter, new GameFightShowFighterMessage(item.GetFighterPreparation()));
                }

                Fight.Map.Send(new GameFightUpdateTeamMessage((short)Fight.FightId, fighter.Team.GetTeamInformation()));
            }
            else if(fighter is SpectatorFighter)
            {
                #region BaseFight           
                SendToFighter(fighter, new GameContextDestroyMessage());
                SendToFighter(fighter, new GameContextCreateMessage(2));
                SendToFighter(fighter, new GameFightStartingMessage((sbyte)Fight.Type, Fight.Teams[(byte)TeamEnum.TEAM_CHALLENGER] == null ? 0 : Fight.Teams[(byte)TeamEnum.TEAM_CHALLENGER].LeaderId,
                                                                                       Fight.Teams[(byte)TeamEnum.TEAM_DEFENDER] == null ? 0 : Fight.Teams[(byte)TeamEnum.TEAM_DEFENDER].LeaderId));
                SendToFighter(fighter, new GameFightSpectatorJoinMessage(false, false, false, Fight.IsStarting, (short)(Fight.IsStarting ? (Fight.StartTimer.UntilTime / 100) : 0), (sbyte)Fight.Type, new NamedPartyTeam[0]));
                if (!Fight.IsStarting)
                {
                    SendToFighter(fighter, new GameFightPlacementPossiblePositionsMessage((from x in Fight.RedCells
                                                                                           select (uint)x.Id).ToArray(), 
                                                                                                (from x in Fight.BlueCells
                                                                                                select (uint)x.Id).ToArray(), (sbyte)fighter.Team.Team));
                }
                #endregion

                foreach (var item in Fight.Fighters)
                {
                    SendToFighter(fighter, new GameFightShowFighterMessage(item.GetFighterPreparation()));
                }

                if (!Fight.IsStarting)
                {
                    SendToFighter(fighter, new IdolFightPreparationUpdateMessage(0, new Idol[0]));
                }
                else
                {
                    SendToFighter(fighter, new GameFightTurnListMessage(Fight.TimeLine.GetOrderIds(), Fight.GetDieIds()));
                    SendToFighter(fighter, new GameFightSpectateMessage((from x in Fight.Fighters
                                                                         from y in x.BuffList
                                                                         select y.GetFightDispellableEffectExtendedInformations()).ToArray(),
                                                                        new GameActionMark[0], Fight.TimeLine.ActualRound, Fight.StartedTime.DateTimeToUnixTimestampSeconds() - 3600, new Idol[0]));
                    SendToFighter(fighter, new GameFightNewRoundMessage(Fight.TimeLine.ActualRound));
                }
            }
            else
            {
                SendToAll(new GameEntitiesDispositionMessage(entityesDisposition.ToArray()));
                SendToAll(new GameFightUpdateTeamMessage((short)Fight.FightId, fighter.Team.GetTeamInformation()));
                SendToAll(new GameFightShowFighterMessage(fighter.GetFighterPreparation()));

                Fight.Map.Send(new GameFightUpdateTeamMessage((short)Fight.FightId, fighter.Team.GetTeamInformation()));
            }
        }

        public void OnFighterRemoved(Fighter fighter)
        {
            SendToAll(new GameFightRemoveTeamMemberMessage((short)Fight.FightId, (sbyte)fighter.Team.Team, fighter.Id));
            SendToAll(new GameFightLeaveMessage(fighter.Id));
            Fight.Map.Send(new GameFightRemoveTeamMemberMessage((short)Fight.FightId, (sbyte)fighter.Team.Team, fighter.Id));
        }

        public void OnOptionChanged(Fighter fighter, sbyte option, bool state)
        {
            if (option > 0)
                SendToTeam(fighter, new GameFightOptionStateUpdateMessage((short)Fight.FightId, (sbyte)fighter.Team.Team, option, state));
            else
                SendToAll(new GameFightOptionStateUpdateMessage((short)Fight.FightId, (sbyte)fighter.Team.Team, option, state));
            Fight.Map.Send(new GameFightOptionStateUpdateMessage((short)Fight.FightId, (sbyte)fighter.Team.Team, option, state));
        }

        public void OnFighterChangePosition()
        {
            var entityesDisposition = from entry in Fight.Fighters
                                      select new IdentifiedEntityDispositionInformations(entry.Point.Point.CellId, (byte)entry.Point.Dir, entry.Id);

            SendToAll(new GameEntitiesDispositionMessage(entityesDisposition.ToArray()));
        }

        public void OnSetReady(Fighter fighter)
        {
            SendToAll(new GameFightHumanReadyStateMessage((uint)fighter.Id, fighter.IsReady));
        }

        public void OnStartedFight()
        {
            var entityesDisposition = from entry in Fight.Fighters
                                      select new IdentifiedEntityDispositionInformations((short)entry.Point.Point.CellId, (byte)entry.Point.Dir, (int)entry.Id);

            SendToAll(new GameEntitiesDispositionMessage(entityesDisposition.ToArray()));
            SendToAll(new GameFightStartMessage(new Idol[0]));
            SendToAll(new GameFightTurnListMessage(Fight.Fighters.Select(x => (double)x.Id).ToArray(), Fight.GetDieIds()));
            foreach (var item in Fight.Challenges)
            {
                SendToAll(new ChallengeInfoMessage((uint)item.Chall, item.Target != null ? item.Target.Id : 0, (uint)item.BoostSolo, (uint)item.BoostGroup));
            }
        }

        public void OnSequenceSend(Fighter fighter, SequenceTypeEnum sequence)
        {
            if(!Fight.IsEnded)
                SendToAll(new SequenceStartMessage((sbyte)sequence, fighter.Id));
        }

        public void OnSequenceEndSend(Fighter fighter, SequenceTypeEnum sequence, SequenceTypeEnum lastSequence)
        {
            if (Fight.finisherSpell != null)
            {
                foreach (var item in m_msg)
                {
                    dynamic d = item.Item1;
                    dynamic c = item.Item2;
                    if(c is GameActionFightSpellCastMessage)
                    {
                        c.spellId = Fight.finisherSpell.spellId;
                        c.spellLevel = (short)Fight.finisherSpell.grade;
                    }
                    dynamic e = item.Item3;
                    d.Invoke(c, e);
                }
            }
            else
            {
                foreach (var item in m_msg)
                {
                    dynamic d = item.Item1;
                    dynamic c = item.Item2;
                    dynamic e = item.Item3;
                    d.Invoke(c, e);
                }
            }
            m_msg.Clear();
            SendToAll(new SequenceEndMessage((uint)lastSequence, fighter.Id, (sbyte)sequence));
        }

        public void OnFighterPointsVariation(Fighter source, Fighter target, ActionsEnum action, int delta)
        {
            SendToAll(new GameActionFightPointsVariationMessage((uint)action,source.Id, target.Id, (short)delta));
        }

        public void OnCastedSpell(Fighter source, SpellCastHandler spell, CellMap targetCell, FightSpellCastCriticalEnum critical, bool silentCast)
        {
            Fighter target = Fight.GetFighter(targetCell);

            if (spell.Spell.Spell.id == 0)
                SendToAll(new GameActionFightCloseCombatMessage(2, source.Id, true, true, target != null ? target.Id : 0, (short)targetCell.Id, (sbyte)critical, 0));
            else
                SendToAll(new GameActionFightSpellCastMessage((uint)ActionsEnum.ACTION_FIGHT_CAST_SPELL, source.Id, silentCast, spell.Spell.Spell.verbose_cast, target != null ? target.Id : 0, (short)targetCell.Id, (sbyte)critical,
                    (uint)spell.Spell.Spell.id, (sbyte)(spell.CurrentSpellLevel.grade), spell.Portails));
        }

        public void OnCastedWeapon(Fighter source, SpellCastHandler spell, CellMap targetCell, FightSpellCastCriticalEnum critical, bool silentCast)
        {
            Fighter target = Fight.GetFighter(targetCell);

            SendToAll(new GameActionFightCloseCombatMessage((uint)ActionsEnum.ACTION_FIGHT_CAST_SPELL, source.Id, silentCast, spell.Spell.Spell.verbose_cast,
                (target != null ? target.Id : 0), (short)targetCell.Id, (sbyte)critical, (uint)spell.CurrentSpellLevel.spellId));
        }

        public void OnLifePointChanged(Fighter target, Damage damage)
        {
            SendToAll(new GameActionFightLifePointsLostMessage((uint)ActionsEnum.ACTION_CHARACTER_LIFE_POINTS_LOST, damage.Source == null ? 0 : damage.Source.Id, target.Id, (uint)damage.AfterReduction, (uint)damage.Permanant));
        }

        public void OnShieldChanged(Fighter target, Fighter source, int life, int perma, int shield)
        {
            SendToAll(new GameActionFightLifeAndShieldPointsLostMessage((uint)ActionsEnum.ACTION_CHARACTER_LIFE_POINTS_LOST, source == null ? 0 : source.Id, target.Id, 
                (uint)life, (uint)perma, (uint)shield));
        }

        public void OnHealed(Fighter target, Fighter source, int healPoints)
        {
            SendToAll(new GameActionFightLifePointsGainMessage((uint)ActionsEnum.ACTION_CHARACTER_LIFE_POINTS_WIN, source.Id, target.Id, (uint)healPoints));
        }

        public void OnNoCastSpell(Fighter source, uint id)
        {
            SendToFighter(source, new GameActionFightNoSpellCastMessage(id));
        }

        public void OnTurnStarted()
        {
            if (Fight.TimeLine.IsNewRound)
                SendToAll(new GameFightNewRoundMessage(Fight.TimeLine.ActualRound));

            SendToAll(new GameFightTurnStartMessage(Fight.TimeLine.FighterPlaying.Id, 240));
           // Fight.MarkManager.ExecuteGlyph(Fight.TimeLine.FighterPlaying);
           //  Fight.MarkManager.DrecrementGlyph(Fight.TimeLine.FighterPlaying);

            if (Fight.TimeLine.FighterPlaying is CharacterFighter)
            {
                SendToOpposTeam(Fight.TimeLine.FighterPlaying, new GameFightSynchronizeMessage((from x in Fight.Fighters
                                                                                                where x.IsAlive && (x.Team != Fight.TimeLine.FighterPlaying.Team || !x.IsInvisible)
                                                                                                select x.GetFighterInformations()).ToArray()));
                SendToTeam(Fight.TimeLine.FighterPlaying, new GameFightSynchronizeMessage((from x in Fight.Fighters
                                                                                           where x.IsAlive && (x.Team == Fight.TimeLine.FighterPlaying.Team || !x.IsInvisible)
                                                                                           select x.GetFighterInformations()).ToArray()));
                SendToFighter(Fight.TimeLine.FighterPlaying, new GameFightTurnStartPlayingMessage());
            }
        }

        public void OnRequestPassTurn()
        {
            SendToAll(new GameFightTurnReadyRequestMessage(Fight.TimeLine.GetNextFighter().Id));
        }

        public void OnTurnPassed()
        {
            SendToAll(new GameFightTurnEndMessage(Fight.TimeLine.FighterPlaying.Id));
        }

        public void OnMove(Fighter fighter, short[] cellsId)
        {
            if (fighter.IsInvisible)
                SendToTeam(fighter, new GameMapMovementMessage(cellsId, fighter.Id));
            else
                SendToAll(new GameMapMovementMessage(cellsId, fighter.Id));
        }

        public void OnSlide(Fighter source, Fighter target, short cellId, ActionsEnum action)
        {
            if (target.IsInvisible)
                SendToTeam(target, new GameActionFightSlideMessage((uint)action, source.Id, target.Id, target.Point.Point.CellId, cellId));
            else
                SendToAll(new GameActionFightSlideMessage((uint)action, source.Id, target.Id, target.Point.Point.CellId, cellId));
        }

        public void OnTeleport(Fighter source, Fighter target, short cellId, ActionsEnum action)
        {
            if (target.IsInvisible)
                SendToTeam(target, new GameActionFightTeleportOnSameMapMessage((uint)action, source.Id, target.Id, cellId));
            else
                SendToAll(new GameActionFightTeleportOnSameMapMessage((uint)action, source.Id, target.Id, cellId));
        }

        public void OnDie(Fighter source, Fighter target)
        {
            SendToAll(new GameActionFightDeathMessage((uint)ActionsEnum.ACTION_CHARACTER_DEATH, source == null ? 0 : source.Id, target.Id));
        }

        public void OnFightEnded()
        {
            var msg = new GameFightEndMessage((int)DateTime.Now.Subtract(Fight.StartedTime).TotalMilliseconds, 
                (Fight is PvmFight ? (Fight as PvmFight).Group.Age : (short)0)
                , 0,
                (from x in Fight.Fighters 
                select x.Result != null ? x.GetResult() : new FightResultListEntry(0, 0, new FightLoot(new uint[0], 0))).ToArray(),
                new NamedPartyTeamWithOutcome[0]);
            SendToAll(msg);
          
        }

        public void OnBuffAdded(Buff buff)
        { 
            SendToAll(new GameActionFightDispellableEffectMessage((uint)buff.GetActionId(), buff.Caster.Id, buff.GetAbstractFightDispellableEffect()));
        }

        public void OnBuffDispelled(Buff buff)
        {
            SendToAll(new GameActionFightDispellSpellMessage(406, buff.Caster.Id, buff.Target.Id, (uint)buff.Spell.Spell.id));
        }

        public void OnBuffRemoved(Buff buff)
        {
            SendToAll(new GameActionFightDispellEffectMessage((uint)buff.GetActionId(), buff.Caster.Id, buff.Target.Id, buff.Id));
        }

        public void OnTackled(Fighter fighter, double[] tacklersId)
        {
            SendToAll(new GameActionFightTackledMessage((uint)ActionsEnum.ACTION_CHARACTER_ACTION_TACKLED, fighter.Id, tacklersId));
        }

        public void OnExchangePosition(Fighter source, Fighter target, ActionsEnum action)
        {
            SendToAll(new GameActionFightExchangePositionsMessage((uint)action, source.Id, target.Id, source.Point.Point.CellId, target.Point.Point.CellId));
        }

        public void OnMarkAdded(Fighter source, GameActionMark marks)
        {
            SendToAll(new GameActionFightMarkCellsMessage((uint)ActionsEnum.ACTION_FIGHT_ADD_GLYPH_CASTING_SPELL, source.Id, marks));
        }

        public void OnMarkDeleted(Glyph trigger)
        {
            SendToAll(new GameActionFightUnmarkCellsMessage(310, trigger.Caster.Id, trigger.Id));
        }

        public void OnSummonAdded(SummonMonster summon)
        {
            var msg = new GameActionFightSummonMessage(summon is SummonDouble ? (uint)ActionsEnum.ACTION_CHARACTER_ADD_DOUBLE : 
                summon is SummonBomb ? 1008 : (uint)ActionsEnum.ACTION_SUMMON_CREATURE, summon.Summoner.Id, new GameFightFighterInformations[] { summon.GetFighterInformations() });
            SendToAll(msg);
            SendToAll(new GameFightTurnListMessage(Fight.TimeLine.GetOrderIds(), Fight.GetDieIds()));
        }

        public void OnEffectDuratonModify(Fighter source, Fighter target, short delta)
        {
            SendToAll(new GameActionFightModifyEffectsDurationMessage((uint)ActionsEnum.ACTION_CHARACTER_MODIFY_DURATION_EFFECT, source.Id, target.Id, (short)(delta * -1)));
        }

        public void OnModifySkin(Fighter source, Fighter target, EntityLook look)
        {
            SendToAll(new GameActionFightChangeLookMessage(149, source.Id, target.Id, look));
        }

        public void OnTrapAdded(Fighter source, GameActionMark marks)
        {
            SendToTeam(source, new GameActionFightMarkCellsMessage((uint)ActionsEnum.ACTION_FIGHT_ADD_GLYPH_CASTING_SPELL, source.Id, marks));
        }

        public void OnTrapDeleted(Trap trigger)
        {
            SendToTeam(trigger.Caster, new GameActionFightUnmarkCellsMessage(310, trigger.Caster.Id, trigger.Id));
        }

        public void OnVisibilitySet(InvisibleBuff buff, sbyte state)
        {
            if (state == 1)
            {
                SendToTeam(buff.Target, new GameActionFightInvisibilityMessage(150, buff.Caster.Id, buff.Target.Id, (sbyte)GameActionFightInvisibilityStateEnum.DETECTED));
                SendToOpposTeam(buff.Target, new GameActionFightInvisibilityMessage(150, buff.Caster.Id, buff.Target.Id, state));
            }
            else
            {
                SendToOpposTeam(buff.Target, new GameFightSynchronizeMessage((from x in Fight.Fighters
                                                                                                where x.IsAlive && (x.Team != Fight.TimeLine.FighterPlaying.Team || !x.IsInvisible)
                                                                                                select x.GetFighterInformations()).ToArray()));
                SendToAll(new GameActionFightInvisibilityMessage(150, buff.Caster.Id, buff.Target.Id, state));
            }
        }

        public void OnSwitchController(SummonMonster slave)
        {
            List<SpellItem> spells = new List<SpellItem>();
            foreach (var item in slave.Spells)
            {
                spells.Add(new SpellItem((int)item.spellId, (sbyte)item.grade));
            }
            List<ShortcutSpell> shortcuts = new List<ShortcutSpell>();
            for (int i = 0; i < spells.Count; i++)
            {
                shortcuts.Add(new ShortcutSpell((sbyte)i, (uint)spells[i].spellId));
            }
            SendToFighter(slave.Summoner, new SlaveSwitchContextMessage(slave.Summoner.Id, slave.Id, spells.ToArray(), slave.GetCharacterCharacteristicsInformations(), shortcuts.ToArray()));
        }

        public void OnResetController(SummonMonster slave)
        {            
            //SendToFighter(slave.Summoner, new GameFightResumeWithSlavesMessage(new GameFightResumeSlaveInfo(slave.Id, slave.Summoner.SpellHistory.GetGameFightSpellCooldowns(), (sbyte)slave.Summoner.Summons.Count, 0));
        }

        public void OnFightReconnect(CharacterFighter fighter)
        {
            var entityesDisposition = from entry in Fight.Fighters
                                      select new IdentifiedEntityDispositionInformations((short)entry.Point.Point.CellId, (byte)entry.Point.Dir, (int)entry.Id);
            if (!Fight.IsStarting)
            {
                #region BaseFight
                (fighter as CharacterFighter).Character.RefreshStats();
                SendToFighter(fighter, new GameFightStartingMessage((sbyte)Fight.Type, Fight.Teams[(byte)TeamEnum.TEAM_CHALLENGER] == null ? 0 : Fight.Teams[(byte)TeamEnum.TEAM_CHALLENGER].LeaderId,
                                                                       Fight.Teams[(byte)TeamEnum.TEAM_DEFENDER] == null ? 0 : Fight.Teams[(byte)TeamEnum.TEAM_DEFENDER].LeaderId));
                SendToFighter(fighter, new GameFightJoinMessage(!Fight.IsStarting, false, fighter.IsReady, Fight.IsStarting, (short)(Fight.StartTimer.UntilTime / 100), (sbyte)Fight.Type));
                SendToFighter(fighter, new GameFightPlacementPossiblePositionsMessage((from x in Fight.BlueCells
                                                                                       select (uint)x.Id).ToArray(), (from x in Fight.RedCells
                                                                                                                      select (uint)x.Id).ToArray(), (sbyte)fighter.Team.Team));
                foreach (var item in Fight.Teams)
                {
                    SendToFighter(fighter, new GameFightOptionStateUpdateMessage((short)Fight.FightId, (sbyte)item.Team, (sbyte)FightOptionsEnum.FIGHT_OPTION_SET_CLOSED, item.Close));
                    SendToFighter(fighter, new GameFightOptionStateUpdateMessage((short)Fight.FightId, (sbyte)item.Team, (sbyte)FightOptionsEnum.FIGHT_OPTION_ASK_FOR_HELP, item.NeedHelp));
                    SendToFighter(fighter, new GameFightOptionStateUpdateMessage((short)Fight.FightId, (sbyte)item.Team, (sbyte)FightOptionsEnum.FIGHT_OPTION_SET_SECRET, item.IsSecret));
                    SendToFighter(fighter, new GameFightOptionStateUpdateMessage((short)Fight.FightId, (sbyte)item.Team, (sbyte)FightOptionsEnum.FIGHT_OPTION_SET_TO_PARTY_ONLY, item.PartyOnly));
                }

                SendToFighter(fighter, new IdolFightPreparationUpdateMessage(0, new Idol[0]));
                #endregion

                SendToFighter(fighter, new GameEntitiesDispositionMessage(entityesDisposition.ToArray()));
                SendToFighter(fighter, new GameFightShowFighterMessage(fighter.GetFighterPreparation()));

                SendToFighter(fighter, new GameFightUpdateTeamMessage((short)Fight.FightId, Fight.Teams[(byte)TeamEnum.TEAM_CHALLENGER].GetTeamInformation()));
                SendToFighter(fighter, new GameFightUpdateTeamMessage((short)Fight.FightId, Fight.Teams[(byte)TeamEnum.TEAM_DEFENDER].GetTeamInformation()));

                foreach (var item in Fight.Fighters.FindAll(x => x.Id != fighter.Id))
                {
                    SendToFighter(fighter, new GameFightShowFighterMessage(item.GetFighterPreparation()));
                }

                Fight.Map.Send(new GameFightUpdateTeamMessage((short)Fight.FightId, fighter.Team.GetTeamInformation()));
                SendToFighter(fighter,new MapComplementaryInformationsWithCoordsMessage(
                                        (uint)Fight.Map.SubArea,
                                        Fight.Map.Id,
                                        new Common.Protocol.Types.HouseInformations[0],
                                        Fight.Map.GetActors(fighter.Character),
                                        Fight.Map.GetInteractives(fighter.Character),
                                        Fight.Map.GetStatedElements(), new Common.Protocol.Types.MapObstacle[0],
                                        new FightCommonInformations[0],
                                        true,
                                        (short)Fight.Map.Coordinates.posX,
                                        (short)Fight.Map.Coordinates.posY,
                                        new FightStartingPositions(new List<uint>(), new List<uint>())));
            }
            else
            {
               SendToFighter(fighter, new GameFightStartingMessage((sbyte)Fight.Type, Fight.Teams[(byte)TeamEnum.TEAM_CHALLENGER] == null ? 0 : Fight.Teams[(byte)TeamEnum.TEAM_CHALLENGER].LeaderId,
                                                                                          Fight.Teams[(byte)TeamEnum.TEAM_DEFENDER] == null ? 0 : Fight.Teams[(byte)TeamEnum.TEAM_DEFENDER].LeaderId));
                SendToFighter(fighter, new GameFightJoinMessage(false, false, false, Fight.IsStarting, Fight.IsStarting ? (short)0 : (short)(Fight.StartTimer.UntilTime / 100), (sbyte)Fight.Type));

                foreach (var item in Fight.Fighters)
                {
                    SendToFighter(fighter, new GameFightShowFighterMessage(item.GetFighterInformations()));
                }
                SendToFighter(fighter, new GameEntitiesDispositionMessage(entityesDisposition.ToArray()));
                SendToFighter(fighter, new GameFightResumeMessage(fighter.GetEffectsExtended(), Fight.GetMarks(fighter), Fight.IsStarting ? Fight.TimeLine.ActualRound : 0,
                    Fight.IsStarting ? 1 : 0, new Idol[0], fighter.SpellHistory.GetGameFightSpellCooldowns(), (sbyte)fighter.Summons.Count, 0));
                SendToFighter(fighter, new GameFightTurnListMessage(Fight.TimeLine.GetOrderIds(), Fight.GetDieIds()));
                SendToFighter(fighter, new GameFightSynchronizeMessage((from x in Fight.Fighters
                                                                        where x.IsAlive && (x.Team == fighter.Team || !x.IsInvisible)
                                                                        select x.GetFighterInformations()).ToArray()));
                //SendToFighter(fighter, new ChallengeInfoMessage(0, 0, 0, 0));
                fighter.Character.RefreshStats();
                SendToFighter(fighter, new GameFightNewRoundMessage(Fight.TimeLine.ActualRound));
                SendToFighter(fighter, new GameFightOptionStateUpdateMessage((short)Fight.FightId, (sbyte)fighter.Team.Team, 2, false));
                SendToFighter(fighter, new GameFightOptionStateUpdateMessage((short)Fight.FightId, (sbyte)fighter.Team.Team, 1, false));
                SendToFighter(fighter, new GameFightOptionStateUpdateMessage((short)Fight.FightId, (sbyte)fighter.Team.Team, 3, false));
                SendToFighter(fighter, new GameFightOptionStateUpdateMessage((short)Fight.FightId, (sbyte)fighter.Team.Team, 0, false));
                SendToFighter(fighter, new GameFightTurnStartMessage(Fight.TimeLine.FighterPlaying.Id, 300));
                SendToFighter(fighter,
                    new MapComplementaryInformationsWithCoordsMessage(
                        (uint)Fight.Map.SubArea,
                        Fight.Map.Id,
                        new Common.Protocol.Types.HouseInformations[0],
                        Fight.Map.GetActors(fighter.Character),
                        Fight.Map.GetInteractives(fighter.Character),
                        Fight.Map.GetStatedElements(), new Common.Protocol.Types.MapObstacle[0],
                        new FightCommonInformations[0],
                        true,
                        (short)Fight.Map.Coordinates.posX,
                        (short)Fight.Map.Coordinates.posY,
                        new FightStartingPositions(new List<uint>(), new List<uint>())));
            }                          
        }

        public void OnDodgePointLoss(Fighter source, Fighter target, int delta, int customAction)
        {
            SendToAll(new GameActionFightDodgePointLossMessage((uint)customAction, source.Id, target.Id, (uint)delta));
        }

        public void OnPortailDeleted(Portail trigger)
        {
            SendToAll(new GameActionFightUnmarkCellsMessage(310, trigger.Caster.Id, trigger.Id));
        }

        public void OnPortailChange(Portail trigger)
        {
            SendToAll(new GameActionFightActivateGlyphTrapMessage(310, trigger.Caster.Id, trigger.Id, trigger.Active));
        }

        public void OnCooldownChanged(Fighter source, Fighter target, SpellTemplate spell, int nbr)
        {
            var msg = new GameActionFightSpellCooldownVariationMessage(1045, source.Id, target.Id, spell.CurrentSpellLevel.spellId, (short)nbr);
            SendToAll(msg);
        }

        public void OnDeclanchePortail(Fighter source, Fighter target, Portail portail)
        {
            SendToAll(new GameActionFightTriggerGlyphTrapMessage((uint)ActionsEnum.ACTION_FIGHT_CAST_SPELL, source.Id, portail.Id, target.Id, 5325));
        }

        public void OnWallDeleted(Wall trigger)
        {
            foreach (var item in trigger.Cells)
            {
                SendToAll(new GameActionFightUnmarkCellsMessage(310, trigger.Caster.Id, (short)(item.Id + trigger.Id)));
            }
        }

        public void OnUnmarkCellFighter(Fighter source, GameActionMarkedCell cell)
        {
            SendToAll(new GameActionFightUnmarkCellsMessage(310, source.Id, (short)cell.cellId));
        }

        public void OnChallUpDate(ChallengeHandler Chall)
        {
            SendToAll(new ChallengeResultMessage((uint)Chall.Chall, Chall.Success));
        }

        public void OnChallChangeTarget(ChallengeHandler Chall)
        {
            SendToAll(new ChallengeTargetUpdateMessage((uint)Chall.Chall, Chall.Target.Id));
        }
        #endregion

        #region Sender
        List<Tuple<object, object, object>> m_msg = new List<Tuple<object, object, object>>();

        public void SendToAll(NetworkMessage msg, int not = 0)
        {
            if (Fight.IsSequencing)
            {
                m_msg.Add(new Tuple<object, object, object>(new Action<NetworkMessage, int>(SendToAll), msg, 80));
            }
            else
            {
                foreach (CharacterFighter character in Fight.Fighters.FindAll(x => x is CharacterFighter && (x as CharacterFighter).Character.Client != null))
                {
                    character.Character.Client.Send(msg);
                }
                foreach (SpectatorFighter item in Fight.Spectators)
                {
                    item.Character.Client.Send(msg);
                }
            }
        }

        public void SendExecpt(Fighter fighter, NetworkMessage msg)
        {
            if (Fight.IsSequencing)
            {
                m_msg.Add(new Tuple<object, object, object>(new Action<Fighter, NetworkMessage>(SendExecpt), fighter, msg));
            }
            else
            {
                foreach (CharacterFighter character in Fight.Fighters.FindAll(x => x is CharacterFighter && x.Id != fighter.Id && (x as CharacterFighter).Character.Client != null))
                {
                    character.Character.Client.Send(msg);
                }
            }
        }

        public void SendToFighter(Fighter fighter, NetworkMessage msg)
        {
            if (Fight.IsSequencing)
            {
                m_msg.Add(new Tuple<object, object, object>(new Action<Fighter, NetworkMessage>(SendToFighter), fighter, msg));
            }
            else
            {
                if (fighter is CharacterFighter && (fighter as CharacterFighter).Character.Client != null)
                    (fighter as CharacterFighter).Character.Client.Send(msg);
                else if (fighter is SpectatorFighter)
                    (fighter as SpectatorFighter).Character.Client.Send(msg);
            }
        }

        public void SendToTeam(Fighter fighter, NetworkMessage msg)
        {
            if (Fight.IsSequencing)
            {
                m_msg.Add(new Tuple<object, object, object>(new Action<Fighter, NetworkMessage>(SendToTeam), fighter, msg));
            }
            else
            {
                foreach (CharacterFighter character in Fight.Fighters.FindAll(x => x is CharacterFighter && x.Team == fighter.Team && (x as CharacterFighter).Character.Client != null))
                {
                    character.Character.Client.Send(msg);
                }
            }
        }

        public void SendToOpposTeam(Fighter fighter, NetworkMessage msg)
        {
            if (Fight.IsSequencing)
            {
                m_msg.Add(new Tuple<object, object, object>(new Action<Fighter, NetworkMessage>(SendToOpposTeam), fighter, msg));
            }
            else
            {
                foreach (CharacterFighter character in Fight.Fighters.FindAll(x => x is CharacterFighter && x.Team != fighter.Team && (x as CharacterFighter).Character.Client != null))
                {
                    character.Character.Client.Send(msg);
                }
                foreach (SpectatorFighter item in Fight.Spectators)
                {
                    item.Character.Client.Send(msg);
                }
            }
        }
        #endregion
    }
}
