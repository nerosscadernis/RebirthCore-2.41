
//using Rebirth.Common.Thread;
//using Rebirth.World.Game.Fights.Actors;
//using Rebirth.World.Datas.Maps;
//using Rebirth.World.Game.Maps.Pathfindings;
//using Rebirth.World.Game.Spells;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Rebirth.World.Game.Fights.IA.Scipting.Abstract
//{
//    public class UtilsIAEnvironnement
//    {
//        private Fight m_fight;
//        private AbstractIA m_fighter;
//        private EnvironmentAnalyser m_environnement;
//        public UtilsIAEnvironnement(Fight fight, AbstractIA fighter)
//        {
//            m_fight = fight;
//            this.m_fighter = fighter;
//            m_environnement = new EnvironmentAnalyser(fighter);
//        }
//        public int rand(int mini, int max)
//        {
//            var random = new AsyncRandom();
//            return random.Next(mini, max);
//        }

//        public int size(int[] table)
//        {
//            return table.Length;
//        }
//    }
//}
