using Rebirth.Common.Extensions;
using Rebirth.Common.GameData.D2O;
using Rebirth.Common.GameData.Elements.Test;
using Rebirth.Common.GameData.Maps;
using Rebirth.Common.GameData.Maps.Elements;
using Rebirth.Common.IStructures;
using Rebirth.Common.Network;
using Rebirth.Common.Protocol.Data;
using Rebirth.Common.Protocol.Enums;
using Rebirth.Common.Protocol.Messages;
using Rebirth.Common.Protocol.Types;
using Rebirth.Common.Thread;
using Rebirth.World.Datas.Characters;
using Rebirth.World.Datas.Interactives.Types;
using Rebirth.World.Datas.Interactives.Types.Classic;
using Rebirth.World.Datas.Monsters;
using Rebirth.World.Datas.Npcs;
using Rebirth.World.Game.Fights;
using Rebirth.World.Game.Fights.Custom;
using Rebirth.World.Managers;
using Rebirth.World.Network;
using Rebirth.World.Npcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rebirth.World.Datas.Maps
{
    public class MapTemplate
    {
        #region Vars
        private List<AbstractInteractive> _interactives = new List<AbstractInteractive>();
        private List<CellTrigger> _triggers = new List<CellTrigger>();
        private ISpawn _spawner;
        #endregion

        #region Properties
        public List<Client> Clients { get; set; }
        public MapPosition Coordinates { get; set; }
        public List<CellMap> Cells { get; set; }
        public List<NpcSpawn> NpcSpawn { get; set; }
        public List<MonsterGroup> MonsterSpawn { get; set; }
        public List<Fight> Fights { get; set; }
        public Dictionary<MapNeighbour, int> MapAlternative { get; set; }
        public int Id { get; set; }
        public int SubArea { get; set; }
        public CellMap[] BlueCells { get; set; }
        public CellMap[] RedCells { get; set; }
        #endregion

        #region Constructor
        public MapTemplate(Map map)
        {
            Id = map.Id;
            SubArea = map.SubAreaId;
            Clients = new List<Client>();
            Coordinates = ObjectDataManager.Get<MapPosition>(map.Id);
            NpcSpawn = NpcManager.Instance.GetNpcOnMap(Id);
            MapAlternative = MapManager.Instance.GetAlternative(Id);
            MonsterSpawn = new List<MonsterGroup>();
            Fights = new List<Fight>();

            Cells = map.Cells.Select(x => new CellMap(x, map.Cells.IndexOf(x))).ToList();
            InitInteractive(map);
            GenerateCells();

            //var mapDungeon = MonsterManager.Instance.GetMonsterDungeon(map.Id);
            //if (mapDungeon != null)
            //    _spawner = new SpawningPoolMonsterDungeon(this, mapDungeon);
            //else
            if (MonsterManager.Instance.HasMonsterSpawn(Id))
                _spawner = new SpawningPoolMonsterFix(this);
            else
                _spawner = new SpawningPoolMonster(this);
        }
        #endregion

        #region Enter | Quit
        public void Enter(Client client, bool isConnection = false)
        {
            Clients.Add(client);

            if (Id == 152305664)
                client.Character.Quests.AddQuest(489);
            else if (Id == 153093380)
                client.Character.Quests.ValidationObjective(1629, 9655);

            client.Character.Map = this;
            client.Send(new CurrentMapMessage(Id, "649ae451ca33ec53bbcbcc33becf15f4"));
            short timezoneOffset = (short)TimeZoneInfo.Local.GetUtcOffset(System.DateTime.Now).TotalMinutes;
            client.Send(new BasicTimeMessage(System.DateTime.Now.DateTimeToUnixTimestamp(), timezoneOffset));
            client.Character.RefreshStats();
            client.Character.Infos.MapId = Id;

            if (!isConnection)
                client.Character.Quests.ValidParam(Id, 0, 4);
        }
        public void Quit(Client client)
        {
            Clients.Remove(client);
            Send(new GameContextRemoveElementMessage(client.Character.Infos.Id));
        }
        #endregion

        #region Interactives
        public void InitInteractive(Map map)
        {
            EleReader reader = new EleReader(".\\maps\\elements.ele");
            var ele = reader.ReadElements();
            // recupere Identifier qui est egale au ElementId que le serveur doit envoyer
            List<int> Identifierlist = (from x in map.Layers
                                        from i in x.Cells
                                        from y in i.Elements
                                        where y is GraphicalElement && (y as GraphicalElement).Identifier != 0
                                        select (y as GraphicalElement).Identifier).ToList();
            if (map.Cells.Any(x => x.Red) && map.Cells.Any(x => x.Blue))
            {
                AbstractInteractive sleep = new InteractiveZaap(0, 0);
            }

            List<int> ElementIdlist = (from x in map.Layers
                                       from i in x.Cells
                                       from y in i.Elements
                                       where y is GraphicalElement && Identifierlist.Contains((y as GraphicalElement).Identifier)
                                       select (y as GraphicalElement).ElementId).ToList();

            List<int> ElementHorsMap = (from x in map.Layers
                                        from i in x.Cells
                                        from y in i.Elements
                                        where y is GraphicalElement && Identifierlist.Contains((y as GraphicalElement).Identifier) && ((y as GraphicalElement).OffsetX < -50 || (y as GraphicalElement).OffsetX > 50)
                                        select (y as GraphicalElement).Identifier).ToList();

            List<int> CellIds = (from x in map.Layers
                                 from i in x.Cells
                                 from y in i.Elements
                                 where y is GraphicalElement && Identifierlist.Contains((y as GraphicalElement).Identifier)
                                 select i.CellId).ToList();

            int r = 0;
            foreach (var item in ElementIdlist)
            {
                EleGraphicalData element = ele.GraphicalDatas.FirstOrDefault(x => x.Value.Id == item).Value;
                if (element != null)
                {
                    switch (element.Type)
                    {
                        case EleGraphicalElementTypes.BLENDED:
                        case EleGraphicalElementTypes.NORMAL:
                        case EleGraphicalElementTypes.BOUNDING_BOX:
                        case EleGraphicalElementTypes.ANIMATED:
                            InteractiveManager.Instance.GetInteractiveMap(this, (element as NormalGraphicalElementData).Gfx.ToString(), 
                                Identifierlist[r], (uint)CellIds[r], !ElementHorsMap.Contains(Identifierlist[r]), SubArea);
                            break;
                        case EleGraphicalElementTypes.ENTITY:
                            InteractiveManager.Instance.GetInteractiveMap(this, (element as EntityGraphicalElementData).EntityLook, 
                                Identifierlist[r], (uint)CellIds[r], !ElementHorsMap.Contains(Identifierlist[r]), SubArea);
                            break;
                        default:
                            break;
                    }
                }
                r++;
            }
        }

        public void UpdateInteractive(Ressource interactive)
        {
            foreach (var client in Clients)
            {
                var datass = new StatedElementUpdatedMessage(interactive.GetStatedElement());
                client.Send(datass);
                var datas = new InteractiveElementUpdatedMessage(interactive.GetInteractiveElement(client.Character));
                client.Send(datas);
            }
        }

        public AbstractInteractive GetInterctive(int id)
        {
            return _interactives.FirstOrDefault(x => x.Identifier == id);
        }

        public List<AbstractInteractive> GetInterctives()
        {
            return _interactives;
        }

        public InteractiveElement[] GetInteractives(Character character)
        {
            return (from x in _interactives
                    select x.GetInteractiveElement(character)).ToArray();
        }

        public StatedElement[] GetStatedElements()
        {
            return (from x in _interactives
                    where x is IStatedElement
                    select (x as IStatedElement).GetStatedElement()).ToArray();
        }

        public void AddInteractive(AbstractInteractive interactive)
        {
            _interactives.Add(interactive);
            if (interactive is Ressource)
            {

            }
        }

        public AbstractInteractive GetZaap()
        {
            return _interactives.FirstOrDefault(x => x is InteractiveZaap);
        }

        public AbstractInteractive GetZaapi()
        {
            return _interactives.FirstOrDefault(x => x is InteractiveZaapi);
        }

        public void AddTriggers(List<CellTrigger> trigger)
        {
            _triggers.AddRange(trigger);
        }

        public List<CellTrigger> GetTriggers(int cellId)
        {
            return _triggers.FindAll(x => x.Position.Cell.Id == cellId);
        }

        public List<CellTrigger> GetTriggers()
        {
            return _triggers;
        }

        public bool ExecuteTrigger(CellTriggerType triggerType, int cellId, Character character)
        {
            bool result = false;
            foreach (CellTrigger current in GetTriggers(cellId))
            {
                if (current.TriggerType == triggerType)
                {
                    current.Apply(character);
                    result = true;
                }
            }
            return result;
        }
        #endregion

        #region Monsters
        public void AddMonster(MonsterGroup group)
        {
            MonsterSpawn.Add(group);

            Send(new GameRolePlayShowActorMessage(group.GetGameRolePlayGroupMonsterInformations()));
        }

        public void RemoveMonster(MonsterGroup group)
        {
            MonsterSpawn.Remove(group);
        }
        public MonsterGroup GetMonsterGroup(int id)
        {
            return MonsterSpawn.FirstOrDefault(e => e.ContextualId == id);
        }

        public bool HasMonster(int genericId, byte grade)
        {
            return MonsterSpawn.Any(x => x.Leader.Template.id == genericId && x.Leader.GradeID == grade);
        }

        public void StartFightPvm(Character character, MonsterGroup group)
        {
            Send(new GameContextRemoveElementMessage(group.ContextualId));

            PvmFight fight = FightManager.Instance.CreatePvmFight(character.Map);
            fight.Group = group;
            //WorldServer.Instance.Pool.CancelSimpleTimer(group.ageTimer);
            character.Map.Quit(character.Client);
            fight.AddCharacter(character, TeamEnum.TEAM_CHALLENGER);
            int i = 1;

            foreach (var monster in group.Monster)
            {
                fight.AddMonster(monster, TeamEnum.TEAM_DEFENDER, i * -1);
                i++;
            }

            RemoveMonster(group);
        }

        //public void StartFightDungeon(Character character, MonsterGroup group)
        //{
        //    Send(new GameContextRemoveElementMessage(group.ContextualId));

        //    DungeonFight fight = FightManager.Instance.CreateDungeonFight(character.Map, group);
        //    if (fight != null)
        //    {
        //        fight.Group = group;
        //        //WorldServer.Instance.Pool.CancelSimpleTimer(group.ageTimer);
        //        character.Map.Quit(character.Client);
        //        fight.AddCharacter(character, TeamEnum.TEAM_CHALLENGER);

        //        for (int i = 1; i < 5; i++)
        //        {
        //            fight.AddMonster(group.Monster[i - 1], TeamEnum.TEAM_DEFENDER, i * -1);
        //        }

        //        RemoveMonster(group);
        //    }
        //    else
        //        character.SendServerMessage("Erreur de creation du fight, le donjon ou la map suivante est introuvable !\nMerci de prevenir le staff. :)");
        //}
        #endregion

        #region Fight
        public void AddFight(Fight fight)
        {
            if (!Fights.Contains(fight))
                Fights.Add(fight);

            Send(new GameRolePlayShowChallengeMessage(fight.GetFightCommonInformations()));
            Send(new MapFightCountMessage((uint)Fights.Count));
            Send(new GameFightUpdateTeamMessage((short)fight.FightId, fight.Teams[0].GetTeamInformation()));
            Send(new GameFightUpdateTeamMessage((short)fight.FightId, fight.Teams[1].GetTeamInformation()));
        }

        public void RemoveFight(Fight fight)
        {
            if (Fights.Contains(fight))
                Fights.Remove(fight);

            Send(new MapFightCountMessage((uint)Fights.Count));
        }

        public void HideBlades(Fight fight)
        {
            Send(new GameRolePlayRemoveChallengeMessage(fight.FightId));
        }

        public FightCommonInformations[] GetFightsInformation()
        {
            return (from x in Fights
                    where !x.IsStarting
                    select x.GetFightCommonInformations()).ToArray();
        }

        public FightExternalInformations[] GetFightsExternalInformations()
        {
            return (from x in Fights
                    select x.GetFightExternalInformations()).ToArray();
        }
        #endregion

        #region Get
        public GameRolePlayActorInformations[] GetActors(Character character)
        {
            return Clients.Select(x => x.Character.GetGameRolePlayCharacterInformations()).ToArray()
                .AddRange(NpcSpawn.Select(x => x.GetGameRolePlayNpcInformations(character)).ToArray()
                .AddRange<GameRolePlayActorInformations>(MonsterSpawn.Select(e => e.GetGameRolePlayGroupMonsterInformations()).ToArray()));
        }
        #endregion

        #region Sender
        public void Send(NetworkMessage msg)
        {
            foreach(var item in Clients)
                item.Send(msg);
        }
        #endregion

        #region Cells Functions
        public FightStartingPositions GetFightStartingPositions()
        {
            return new FightStartingPositions(RedCells.Select(x => (uint)x.Id).ToList(), BlueCells.Select(x => (uint)x.Id).ToList());
        }

        public CellMap GetCellFree()
        {
            var cells = Cells.Where(x => x.Mov && !x.FarmCell);
            AsyncRandom asyncRandom = new AsyncRandom();
            var index = asyncRandom.Next(0, cells.Count() - 1);
            return cells.ElementAt(index);
        }

        public bool IsCellFree(short cell)
        {
            CellMap newCell = Cells.FirstOrDefault(x => x.Id == cell);
            return newCell != null ? newCell.Mov && !newCell.FarmCell : false;
        }

        public void MoveEntity(Client client, short[] cells)
        {
            foreach (var actor in Clients)
            {
                actor.Send(new GameMapMovementMessage(cells, client.Character.Infos.Id));
            }
        }

        public void GenerateCells()
        {
            if (Cells.Any(x => x.Red) && Cells.Any(x => x.Blue))
            {
                BlueCells = (from x in Cells
                             where x.Blue
                             select x).ToArray();
                RedCells = (from x in Cells
                            where x.Red
                            select x).ToArray();
            }
            else if (Cells.Count(x => x.Mov && !x.FarmCell && !x.NonWalkableDuringFight) > 16)
            {
                AsyncRandom random = new AsyncRandom();

                List<CellMap> cells = new List<CellMap>();
                while (cells.Count != 16)
                {
                    var tata = random.Next(0, 559);
                    if (!cells.Any(x => x.Id == tata))
                        if (Cells[tata].Mov && !Cells[tata].FarmCell && !Cells[tata].NonWalkableDuringFight)
                            cells.Add(Cells[tata]);
                }
                BlueCells = new CellMap[8];
                RedCells = new CellMap[8];

                Array.Copy(cells.ToArray(), 0, this.BlueCells, 0, 8);
                Array.Copy(cells.ToArray(), 8, this.RedCells, 0, 8);
            }
        }
        #endregion

        #region Npc
        public MapNpcsQuestStatusUpdateMessage GetMapNpcsQuestStatusUpdateMessage(Character character)
        {
            var npcs = NpcSpawn.FindAll(x => character.Quests.HasNpcQuest(x.NpcId) || x.HasQuest(character));
            GameRolePlayNpcQuestFlag[] quests = (from x in npcs
                                                 select new GameRolePlayNpcQuestFlag(
                                                     character.Quests.GetQuestsWithNpc(x.NpcId).Select(y => y.Id).ToArray(), x.GetQuests(character))).ToArray();

            return new MapNpcsQuestStatusUpdateMessage(Id, npcs.Select(x => x.Id * -1).ToArray(), quests, NpcSpawn.Except(npcs).Select(x => x.Id * -1).ToArray());
        }
        #endregion
    }
}
