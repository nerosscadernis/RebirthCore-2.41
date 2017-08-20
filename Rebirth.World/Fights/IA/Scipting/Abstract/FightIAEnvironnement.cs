//using Rebirth.World.Game.Fights.Actors;
//using Rebirth.World.Datas.Maps;
//using Rebirth.World.Game.Maps.Pathfindings;
//using Rebirth.World.Game.Spells;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Rebirth.World.Game.Fights.IA.Scipting.Abstract
//{
//    public class FightIAEnvironnement
//    {
//        private Fight m_fight;
//        private AbstractIA m_fighter;
//        private EnvironmentAnalyser m_environnement;
//        public FightIAEnvironnement(Fight fight, AbstractIA fighter)
//        {
//            m_fight = fight;
//            this.m_fighter = fighter;
//            m_environnement = new EnvironmentAnalyser(fighter);
//        }

//        /// <summary>
//        /// Rapproche votre joueur d'une cellule.
//        /// </summary>
//        /// <param name="cellID"></param>
//        public bool moveTowardCell(int cellID)
//        {
//            if (m_fighter.IsDead)
//                return false;

//            if (cellID < 0 || cellID > 560)
//                return false;

//            if (cellID == -60)
//                return false;

//            if (m_fighter.Stats.MP.Total <= 0)
//                return false;

//            CellMap cellEnd = m_fight.Map.Cells[cellID];
//            CellMap currentCell = m_fight.Map.Cells[m_fighter.Point.Point.CellId];
//            AmaknaCore.Debug.Utilities.Pathfinder.GamePathfinder.Pathfinder path = new AmaknaCore.Debug.Utilities.Pathfinder.GamePathfinder.Pathfinder();
//            path.SetMapInFight(m_fight.Map, m_fight);
//            path.GetPath(currentCell.Point.CellId, cellEnd.Point.CellId);

//            var newPathGood = new AmaknaCore.Debug.Utilities.Pathfinder.GamePathfinder.CellWithOrientation[path.Cells.Count - 1];
//            Array.Copy(path.Cells.ToArray(), 1, newPathGood, 0, path.Cells.Count - 1);
//            path.Cells = newPathGood.ToList();
//            if (path.Cells.Count > m_fighter.Stats.MP.Total)
//                path.CutPath(m_fighter.Stats.MP.Total);

//            m_fighter.MovementRequest(path, true);

//            if (m_fighter.Stats.MP.Total > 0)
//                return true;
//            else
//                return false;

//            Thread.Sleep(800);
//        }

//        /// <summary>
//        /// Rapproche votre joueur vers CellID en utilise maximun maxPM
//        /// </summary>
//        /// <param name="cellID"></param>
//        /// <param name="maxPM"></param>
//        public bool moveTowardCell(int cellID, int maxPM)
//        {
//            if (m_fighter.IsDead)
//                return false;

//            if (cellID < 0 || cellID > 560)
//                return false;

//            if (m_fighter.Stats.MP.Total < maxPM)
//                return false;

//            if (cellID == m_fighter.Point.Point.CellId)
//                return false;

//            CellMap cellEnd = m_fight.Map.Cells[cellID];
//            CellMap currentCell = m_fight.Map.Cells[m_fighter.Point.Point.CellId];

//            var list = cellEnd.Point.GetAdjacentCells(new Func<short, bool>(this.m_environnement.CellInformationProvider.IsCellWalkable)).ToArray();
//            Pathfinder path = new Pathfinder(m_environnement.CellInformationProvider);

//            var currentPath = path.FindPath(currentCell.Point.CellId, cellEnd.Point.CellId, false, 300);
//            if (currentPath.MPCost > maxPM)
//                currentPath.CutPath(maxPM + 1);

//            m_fighter.MovementRequest(currentPath);

//            if (m_fighter.Stats.MP.Total > 0)
//                return true;
//            else
//                return false;

//            Thread.Sleep(800);
//        }

//        public bool canAttack(int id, int spellID)
//        {
//            if (m_fighter.IsDead)
//                return false;

//            var enemy = m_fight.GetFighter(id);

//            if (enemy == null)
//                return false;

//            var spell = m_fighter.Spells.FirstOrDefault(x => x.spellId == spellID);

//            if (spell == null)
//                return false;

//           return  m_fighter.CanCastSpell(new SpellTemplate(null, ObjectDataManager.Get<Spell>(spellID)), enemy.Point.Point.CellId, null);
//        }

//        public void attack(int id, int spellID)
//        {
//            if (m_fighter.IsDead)
//                return;

//            var enemy = m_fight.GetFighter(id);

//            if (enemy == null)
//                return;

//            var spell = m_fighter.Spells.FirstOrDefault(x => x.spellId == spellID);

//            if (spell == null)
//                return;

//            m_fighter.TryCastSpell(new SpellTemplate(null, ObjectDataManager.Get<Spell>(spellID)), enemy.Point.Point.CellId,true);

//            Thread.Sleep(800);
//        }

//        public void launchSpellInCell(int id, int spellId)
//        {
//            if (m_fighter.IsDead)
//                return;

//            var spell = m_fighter.Spells.FirstOrDefault(x => x.spellId == spellId);

//            if (spell == null)
//                return;

//            m_fighter.TryCastSpell(new SpellTemplate(null, ObjectDataManager.Get<Spell>(spellId)), (short)id, true);

//            Thread.Sleep(800);
//        }

//        public void forceLaunchSpellInCell(int id, int spellId)
//        {
//            if (m_fighter.IsDead)
//                return;

//            m_fighter.TryCastSpell(new SpellTemplate(null, ObjectDataManager.Get<Spell>(spellId)), (short)id, true);

//            Thread.Sleep(800);
//        }

//        public int getNearestEnemy()
//        {
//            var enemy = m_environnement.GetNearestEnnemy();
//            if (enemy == null)
//                return -60;

//            return enemy.Id;           
//        }

//        public int getNearestEnemy(int playerId)
//        {
//            var fighter = this.m_fight.Fighters.FirstOrDefault(x => x.Id == playerId);
//            if (fighter == null)
//                return -60;

//            var enemy=  (from entry in this.m_fight.Fighters.FindAll((Fighter entry) => entry.Team.Team != fighter.Team.Team && entry.IsAlive && !entry.IsInvisible)
//               orderby entry.Point.Point.DistanceToCell(fighter.Point.Point)
//               select entry).FirstOrDefault<Fighter>();

//            if (enemy == null)
//                return -60;

//            return enemy.Id;
//        }

//        public IEnumerable<int> getEnemys()
//        {
//            var enemy = (from entry in this.m_fight.Fighters.FindAll((Fighter entry) => entry.Team.Team != m_fighter.Team.Team && entry.IsAlive && !entry.IsInvisible)
//                         orderby entry.Point.Point.DistanceToCell(m_fighter.Point.Point)
//                         select entry);

//            if (enemy == null)
//                return new int[0];

//            return enemy.Select(x => x.Id);
//        }

//        public int getFighterCell(int playerID)
//        {
//            var fighter = this.m_fight.Fighters.FirstOrDefault(x => x.Id == playerID);

//            if (fighter == null)
//                return -60;

//            return fighter.Point.Point.CellId;
//        }

//        public int getAdjacentCellNearestEnemy(int playerID)
//        {
//            var fighter = this.m_fight.Fighters.FirstOrDefault(x => x.Id == playerID);

//            if (fighter == null)
//                return -60;

//            if (fighter.Point.Point.DistanceTo(m_fighter.Point.Point) <= 1)
//                return -60;

//            var dist = m_fighter.Point.Point.DistanceToCell(fighter.Point.Point);
//            var list = fighter.Point.Point.GetAdjacentCells(new Func<short, bool>(walkable)).ToArray();
           
//            uint size = 6000;
//            MapPoint cell = null;
//            foreach (var current in list)
//            {    
//                var value = m_fighter.Point.Point.DistanceToCell(current);
//                if (value < size)
//                {
//                    cell = current;
//                    size = value;
//                }
//            }

//            if (cell == null)
//                return -60;

//            return cell.CellId;
//        }

//        public int getAdjacentCellNearestEnemyNoMove(int playerID)
//        {
//            var fighter = this.m_fight.Fighters.FirstOrDefault(x => x.Id == playerID);

//            if (fighter == null)
//                return -60;

//            var dist = m_fighter.Point.Point.DistanceToCell(fighter.Point.Point);
//            var list = fighter.Point.Point.GetAdjacentCells(new Func<short, bool>(walkable)).ToArray();

//            uint size = 6000;
//            MapPoint cell = null;
//            foreach (var current in list)
//            {
//                var value = m_fighter.Point.Point.DistanceToCell(current);
//                if (value < size)
//                {
//                    cell = current;
//                    size = value;
//                }
//            }

//            if (cell == null)
//                return -60;

//            return cell.CellId;
//        }

//        #region Ally
//        public int getNearestAlly()
//        {
//            var ally = m_environnement.GetNearestAlly();
//            if (ally == null)
//                return -60;

//            return ally.Id;
//        }

//        public int getNearestAllyNeedHeal()
//        {
//            var ally = m_environnement.GetNearestAllyNeddHeal();
//            if (ally == null)
//                return -60;

//            return ally.Id;
//        }


//        #endregion

//        #region Cells
//        public int getLaunchCellInvoc(int spellId, int playerId)
//        {
//            var fighter = m_fight.GetFighter(playerId);
//            var spell = m_fighter.Spells.FirstOrDefault(x => x.spellId == spellId);
//            if (fighter != null && spell != null)
//            {
//                var template = new SpellTemplate(null, ObjectDataManager.Get<Spell>((int)spell.spellId));
//                var cell = m_environnement.GetCellToCastSpell(fighter, template);
//                if (m_fighter.CanCastSpell(template, fighter.Point.Point.CellId, null))
//                {
//                    return cell.Id;
//                }
//            }
//            return -60;
//        }

//        public int getCellForLaunchSpell(int spellId, int playerId)
//        {
//            var fighter = m_fight.GetFighter(playerId);
//            var spell = m_fighter.Spells.FirstOrDefault(x => x.spellId == spellId);
//            if (fighter != null && spell != null)
//            {
//                var template = new SpellTemplate(null, ObjectDataManager.Get<Spell>((int)spell.spellId));
//                var cell = m_environnement.GetCellToCastSpellMaxRange(fighter, template);
//                if (cell != null)
//                {
//                    return cell.Id;
//                }
//            }
//            return -60;
//        }

//        private bool walkable(short cell)
//        {
//            return this.m_fight.IsCellFree(this.m_fight.Map.Cells[(int)cell]);
//        }
//        #endregion
//    }
//}
