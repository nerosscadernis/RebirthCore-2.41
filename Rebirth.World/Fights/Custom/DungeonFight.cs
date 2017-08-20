//using Rebirth.World.Datas.Monsters;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Rebirth.Common.Protocol.Enums;
//using Rebirth.World.Datas.Characters;
//using Rebirth.World.Datas.Maps;
//using Rebirth.World.Game.Fights.Actors;
//using Rebirth.Common.Protocol.Messages;
//using Rebirth.World.Network;
//using Rebirth.World.Game.Fights.Challenges;
//using Rebirth.World.Managers;

//namespace Rebirth.World.Game.Fights.Custom
//{
//    public class DungeonFight : PvmFight
//    {
//        #region Constructor
//        public DungeonFight(int id, MapTemplate map) : base(id, map)
//        {
//        }
//        #endregion

//        #region Properties
//        private int Count
//        { get; set; }
//        #endregion

//        #region Public Func
//        public override void AddCharacter(Character character, TeamEnum team)
//        {
//            base.AddCharacter(character, team);

//            int countMonster = Fighters.Count(x => x is MonsterFighter);
//            int countCharacter = Fighters.Count(x => x is CharacterFighter);
//            if (countCharacter > 4 && countCharacter > countMonster)
//            {
//                AddMonster(Group.Monster[Count + 4], TeamEnum.TEAM_DEFENDER, (countMonster + 1) * -1);
//                Count++;
//            }
//            else if (countCharacter > 4 && countCharacter < countMonster)
//            {
//                Fighters.Remove(Fighters.Last(x => x is MonsterFighter));
//                Count--;
//            }
//        }
//        public override void StartFight()
//        {
//            Challenges.Add(ChallengeManager.Instance.GetChall(this));
//            base.StartFight();
//        }
//        public override void ReturnInMap(bool command = false)
//        {
//            var dungeon = DungeonManager.Instance.GetDungeonByMap(Map.Id);
//            if (dungeon != null && !command)
//            {
//                if (dungeon.mapIds.IndexOf(Map.Id) < dungeon.mapIds.Count() - 1)
//                {
//                    var newMap = MapManager.Instance.GetMap(dungeon.mapIds[dungeon.mapIds.IndexOf(Map.Id) + 1]);
//                    if (newMap != null)
//                    {
//                        foreach (CharacterFighter item in Fighters.FindAll(x => x is CharacterFighter && !x.IsDisconnected))
//                        {
//                            item.Character.Client.Send(new GameContextDestroyMessage());
//                            item.Character.Client.Send(new GameContextCreateMessage(1));
//                            item.Character.RefreshStats();
//                            if (Winner.Team != item.Team.Team && !command)
//                                item.Character.Respawn();
//                            else
//                            {
//                                item.Character.Teleport(newMap.Id, newMap.GetCellFree().Id);
//                            }
//                        }
//                        foreach (SpectatorFighter item in Spectators)
//                        {
//                            item.Character.Client.Send(new GameContextDestroyMessage());
//                            item.Character.Client.Send(new GameContextCreateMessage(1));
//                            item.Character.RefreshStats();
//                            item.Character.Map.Enter(item.Character.Client);
//                        }
//                        Fighters.Clear();
//                        Map.HideBlades(this);
//                    }
//                    else
//                        base.ReturnInMap(command);
//                }
//                else
//                {
//                    var newMap = MapManager.Instance.GetMapById(dungeon.exitMapId);
//                    if (newMap != null)
//                    {
//                        foreach (CharacterFighter item in Fighters.FindAll(x => x is CharacterFighter && !x.IsDisconnected))
//                        {
//                            item.Character.Client.Send(new GameContextDestroyMessage());
//                            item.Character.Client.Send(new GameContextCreateMessage(1));
//                            item.Character.RefreshStats();
//                            if (Winner.Team != item.Team.Team && !command)
//                                item.Character.Respawn();
//                            else
//                            {
//                                item.Character.Teleport(newMap.Id, newMap.GetCellFree().Id);
//                            }
//                        }
//                        foreach (SpectatorFighter item in Spectators)
//                        {
//                            item.Character.Client.Send(new GameContextDestroyMessage());
//                            item.Character.Client.Send(new GameContextCreateMessage(1));
//                            item.Character.RefreshStats();
//                            item.Character.Map.Enter(item.Character.Client);
//                        }
//                        Fighters.Clear();
//                        Map.HideBlades(this);
//                    }
//                    else
//                        base.ReturnInMap(command);
//                }
//            }
//            else
//                base.ReturnInMap(command);
//        }
//        #endregion
//    }
//}
