using Rebirth.Common.Pool;
using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Game.Fights.Challenges;
using Rebirth.World.Game.Fights.Marks;
using Rebirth.World.Game.Fights.Other;
using Rebirth.World.Game.Fights.Team;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rebirth.World.Game.Fights
{
    public class FightContent
    {
        #region Constructors
        protected FightContent(int fightId, MapTemplate map)
        {
            Challenges = new List<ChallengeHandler>();
            FightId = fightId;
            Fighters = new List<Fighter>();
            Spectators = new List<SpectatorFighter>();
            Leavers = new List<Fighter>();
            Teams = (new FightTeam[] { new FightTeam(TeamEnum.TEAM_CHALLENGER), new FightTeam(TeamEnum.TEAM_DEFENDER), new FightTeam(TeamEnum.TEAM_SPECTATOR) }).ToList();
            Map = map;
            IsStarting = false;
            FighterTeleport = new List<Fighter>(); 
        }
        #endregion

        #region Constantes
        protected const int PlacementTime = 30;
        #endregion

        #region Private and Protected Properties
        protected FightEvents Events
        {
            get;
            set;
        }
        public DateTime StartedTime;
        public FightTeam Winner;
        public FightTeam Loser;
        public SequenceTypeEnum m_lastSequenceAction;
        public int m_sequenceLevel;
        public readonly Stack<SequenceTypeEnum> m_sequences = new Stack<SequenceTypeEnum>();
        #endregion

        #region Public Properties
        public int FightId
        {
            get;
            protected set;
        }
        public virtual FightTypeEnum Type
        {
            get;
        }
        public List<Fighter> Fighters
        {
            get;
            protected set;
        }
        public List<SpectatorFighter> Spectators
        {
            get;
            protected set;
        }
        public List<Fighter> Leavers
        {
            get;
            protected set;
        }
        public List<FightTeam> Teams
        {
            get;
            protected set;
        }
        public MapTemplate Map
        {
            get;
            set;
        }
        public TimeLine TimeLine
        {
            get;
            protected set;
        }
        public CellMap[] BlueCells;
        public CellMap[] RedCells;
        public bool IsStarting
        {
            get;
            set;
        }
        public bool IsEnded
        { get; set; }
        public KeyValuePair<Fighter, KeyValuePair<int, SequenceTypeEnum>> ActualSequence;
        public bool StopSequenced
        {
            get { return IsSequencing || WaitAcknowledgment; }
        }      
        public FightMarkManager MarkManager
        { get; set; }
        public UniqueIdProvider ContextualIdProvider
        { get; set; }
        public SequenceTypeEnum Sequence
        {
            get;
            set;
        }
        public bool IsSequencing
        {
            get;
            set;
        }
        public bool WaitAcknowledgment
        {
            get;
            set;
        }
        public bool IsStoped
        {
            get;set;
        }
        public bool IsSecret
        {
            get;
            set;
        }

        public List<ChallengeHandler> Challenges
        { get; set; }

        #region Target Effect Help
        public List<Fighter> FighterTeleport
        { get; set; }
        #endregion

        #endregion

    }
}
