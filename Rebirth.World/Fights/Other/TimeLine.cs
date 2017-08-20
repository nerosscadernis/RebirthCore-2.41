using Rebirth.Common.Timers;
using Rebirth.World.Game.Fights.Actors;
using System.Collections.Generic;
using System.Linq;

namespace Rebirth.World.Game.Fights.Other
{
    public class TimeLine
    {
        #region Constructor
        public TimeLine(List<Fighter> fighters)
        {
            m_fighters = fighters;
            FighterPlaying = m_fighters.Last();
            ActualRound = 0;
            Timer = new TimerCore(Timer_Elapsed, TimeTurn, TimeTurn, true);
        }
        #endregion

        #region Variables
        const int TimeTurn = 24000;
        #endregion

        #region Properties
        List<Fighter> m_fighters;
        public List<Fighter> Fighters
        {
            get { return m_fighters.FindAll(x => !(x is SummonMonster) || ((x is SummonMonster) && !(x as SummonMonster).IsStatic)); }
        }
        public Fighter FighterPlaying
        {
            get;
            set;
        }
        public uint ActualRound
        {
            get;
            set;
        }
        public bool IsNewRound
        {
            get;
            set;
        }
        #endregion

        public TimerCore Timer;

        private void Timer_Elapsed()
        {
            Starter.Logger.Debug("I' pass Turn !");
            FighterPlaying?.ActionPass();
        }

        public bool NextPlayer()
        {
            Timer.Stop();
            var start = true;
            while (FighterPlaying.IsDead || start)
            {
                int index = Fighters.IndexOf(FighterPlaying);
                if (index == Fighters.Count - 1)
                {
                    FighterPlaying = Fighters[0];
                    ActualRound++;
                    IsNewRound = true;
                }
                else
                {
                    FighterPlaying = Fighters[index + 1];
                }
                start = false;
            }
            Timer.Start();
            return true;
        }

        public Fighter GetNextFighter()
        {
            Fighter fighter = null;
            var start = true;
            while ((fighter == null ? true : fighter.IsDead) || start)
            {
                int index = Fighters.IndexOf(fighter == null ? FighterPlaying : fighter);
                if (index == Fighters.Count - 1)
                {
                    fighter = Fighters[0];
                    IsNewRound = true;
                }
                else
                {
                    fighter = Fighters[index + 1];
                }
                start = false;
            }
            return fighter;
        }

        public double[] GetOrderIds()
        {
            return (from x in Fighters
                    where x.IsAlive
                    select (double)x.Id).ToArray();
        }

        public void InsertFighter(Fighter fighter)
        {
            var index = m_fighters.IndexOf(FighterPlaying);
            m_fighters.Insert(index, fighter);
        }

        public void Dispose()
        {
            Timer.Dispose();

        }
    }
}
