﻿using AmaknaCore.Debug.Utilities.Pathfinder.GamePathfinder;
using Rebirth.Common.Dispatcher;
using Rebirth.Common.Extensions;
using Rebirth.Common.GameData.Maps;
using Rebirth.Common.Protocol.Enums;
using Rebirth.Common.Protocol.Messages;
using Rebirth.Common.Protocol.Types;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Managers;
using Rebirth.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rebirth.World.Frames
{
    public class WorldFrame
    {
        [MessageHandler(MapInformationsRequestMessage.Id)]
        public void HandleMapInformationsRequestMessage(Client client, MapInformationsRequestMessage message)
        {
            var datas = new MapComplementaryInformationsDataMessage((uint)client.Character.Map.SubArea, client.Character.Map.Id,
                new HouseInformations[0], client.Character.Map.GetActors(client.Character), client.Character.Map.GetInteractives(client.Character), client.Character.Map.GetStatedElements(),
                new MapObstacle[0], new FightCommonInformations[0], false,
                client.Character.Map.GetFightStartingPositions());
            client.Send(datas);
            short timezoneOffset = (short)TimeZoneInfo.Local.GetUtcOffset(System.DateTime.Now).TotalMinutes;
            client.Send(new BasicTimeMessage(System.DateTime.Now.DateTimeToUnixTimestamp(), timezoneOffset));
            client.Send(new SetCharacterRestrictionsMessage(client.Character.Infos.Id, new ActorRestrictionsInformations()
            {
                cantAggress = false,
                cantAttack = false,
                cantAttackMonster = false,
                cantBeAggressed = false,
                cantBeAttackedByMutant = false,
                cantBeChallenged = false,
                cantBeMerchant = false,
                cantChallenge = false,
                cantChangeZone = false,
                cantChat = false,
                cantExchange = false,
                cantMinimize = false,
                cantMove = false,
                cantRun = false,
                cantSpeakToNPC = false,
                cantTrade = false,
                cantUseInteractive = false,
                cantUseObject = false,
                cantUseTaxCollector = false,
                cantWalk8Directions = false,
                forceSlowWalk = false
            }));
        }

        [MessageHandler(GameMapMovementConfirmMessage.Id)]
        private void HandleGameMapMovementConfirmMessage(Client client, GameMapMovementConfirmMessage message)
        {
            // Il manque encore des vérification on les feras au fur est à mesure <3
            client.Character.Map.ExecuteTrigger(CellTriggerType.END_MOVE_ON, client.Character.Infos.CellId, client.Character);
        }

        [MessageHandler(GameMapMovementRequestMessage.Id)]
        private void HandleGameMapMovementRequestMessage(Client client, GameMapMovementRequestMessage message)
        {
            // Il manque encore des vérification on les feras au fur est à mesure <3
            var map = client.Character.Map;
            if (map != null)
            {
                var path = new Pathfinder();
                path.SetMap(map, true);
                path.GetPath(client.Character.Infos.CellId, (short)(message.keyMovements.Last() & 4095));
                
                if(path.Cells != null)
                {
                    client.Character.Infos.CellId = (short)path.Cells.Last().Id;
                    client.Character.Infos.Direction = (DirectionsEnum)path.Cells.Last().Orientation;
                    map.MoveEntity(client, message.keyMovements.Select(x => (short)(x & 4095)).ToArray());
                }
                else
                {
                    var point = new MapPoint(client.Character.Infos.CellId);
                    client.Send(new GameMapNoMovementMessage((short)point.X, (short)point.Y));
                }
            }
        }

        [MessageHandler(ChangeMapMessage.Id)]
        private void HandleChangeMapMessage(Client client, ChangeMapMessage msg)
        {
            var map = client.Character.Map;
            MapTemplate newMap = null;
            CellMap cellMap = map.Cells[client.Character.Infos.CellId];
            if (cellMap != null)
            {
                if (cellMap.IsBotChange)
                {
                    if (map.MapAlternative.ContainsKey(MapNeighbour.Bottom))
                        newMap = MapManager.Instance.GetMap(map.MapAlternative[MapNeighbour.Bottom]);
                    else
                        newMap = MapManager.Instance.GetMap(map.Coordinates.posX, map.Coordinates.posY + 1, map.Coordinates.hasPriorityOnWorldmap, map.Coordinates.worldMap, map.Id, map.Coordinates.outdoor);
                    client.Character.Infos.CellId = cellMap.GetCellAfterChangeMap(MapNeighbour.Bottom);
                }
                else if (cellMap.IsTopChange)
                {
                    if (map.MapAlternative.ContainsKey(MapNeighbour.Top))
                        newMap = MapManager.Instance.GetMap(map.MapAlternative[MapNeighbour.Top]);
                    else
                        newMap = MapManager.Instance.GetMap(map.Coordinates.posX, map.Coordinates.posY - 1, map.Coordinates.hasPriorityOnWorldmap, map.Coordinates.worldMap, map.Id, map.Coordinates.outdoor);
                    client.Character.Infos.CellId = cellMap.GetCellAfterChangeMap(MapNeighbour.Top);
                }
                else if (cellMap.IsLeftChange)
                {
                    if (map.MapAlternative.ContainsKey(MapNeighbour.Left))
                        newMap = MapManager.Instance.GetMap(map.MapAlternative[MapNeighbour.Left]);
                    else
                        newMap = MapManager.Instance.GetMap(map.Coordinates.posX - 1, map.Coordinates.posY, map.Coordinates.hasPriorityOnWorldmap, map.Coordinates.worldMap, map.Id, map.Coordinates.outdoor);
                    client.Character.Infos.CellId = cellMap.GetCellAfterChangeMap(MapNeighbour.Left);
                }
                else if (cellMap.IsRightChange)
                {
                    if (map.MapAlternative.ContainsKey(MapNeighbour.Right))
                        newMap = MapManager.Instance.GetMap(map.MapAlternative[MapNeighbour.Right]);
                    else
                        newMap = MapManager.Instance.GetMap(map.Coordinates.posX + 1, map.Coordinates.posY, map.Coordinates.hasPriorityOnWorldmap, map.Coordinates.worldMap, map.Id, map.Coordinates.outdoor);
                    client.Character.Infos.CellId = cellMap.GetCellAfterChangeMap(MapNeighbour.Right);
                }
                else
                {
                    MapPoint[] test = new MapPoint(cellMap).GetAdjacentCells(new Func<short, bool>(x => map.IsCellFree(x))).ToArray();
                    CellMap[] cells = test.Select(x => map.Cells[x.CellId]).ToArray();

                    if (cells.Any(x => x.IsBotChange))
                    {
                        if (map.MapAlternative.ContainsKey(MapNeighbour.Bottom))
                            newMap = MapManager.Instance.GetMap(map.MapAlternative[MapNeighbour.Bottom]);
                        else
                            newMap = MapManager.Instance.GetMap(map.Coordinates.posX, map.Coordinates.posY + 1, map.Coordinates.hasPriorityOnWorldmap, map.Coordinates.worldMap, map.Id, map.Coordinates.outdoor);
                        client.Character.Infos.CellId = cellMap.GetCellAfterChangeMap(MapNeighbour.Bottom);
                    }
                    else if (cells.Any(x => x.IsTopChange))
                    {
                        if (map.MapAlternative.ContainsKey(MapNeighbour.Top))
                            newMap = MapManager.Instance.GetMap(map.MapAlternative[MapNeighbour.Top]);
                        else
                            newMap = MapManager.Instance.GetMap(map.Coordinates.posX, map.Coordinates.posY - 1, map.Coordinates.hasPriorityOnWorldmap, map.Coordinates.worldMap, map.Id, map.Coordinates.outdoor);
                        client.Character.Infos.CellId = cellMap.GetCellAfterChangeMap(MapNeighbour.Top);
                    }
                    else if (cells.Any(x => x.IsLeftChange))
                    {
                        if (map.MapAlternative.ContainsKey(MapNeighbour.Left))
                            newMap = MapManager.Instance.GetMap(map.MapAlternative[MapNeighbour.Left]);
                        else
                            newMap = MapManager.Instance.GetMap(map.Coordinates.posX - 1, map.Coordinates.posY, map.Coordinates.hasPriorityOnWorldmap, map.Coordinates.worldMap, map.Id, map.Coordinates.outdoor);
                        client.Character.Infos.CellId = cellMap.GetCellAfterChangeMap(MapNeighbour.Left);
                    }
                    else if (cells.Any(x => x.IsRightChange))
                    {
                        if (map.MapAlternative.ContainsKey(MapNeighbour.Right))
                            newMap = MapManager.Instance.GetMap(map.MapAlternative[MapNeighbour.Right]);
                        else
                            newMap = MapManager.Instance.GetMap(map.Coordinates.posX + 1, map.Coordinates.posY, map.Coordinates.hasPriorityOnWorldmap, map.Coordinates.worldMap, map.Id, map.Coordinates.outdoor);
                        client.Character.Infos.CellId = cellMap.GetCellAfterChangeMap(MapNeighbour.Right);
                    }
                }


                if (newMap == null)
                {
                    return;
                }

                var cellAfter = newMap.Cells[client.Character.Infos.CellId];
                if (!cellAfter.Mov)
                    client.Character.Infos.CellId = (short)newMap.Cells.FindAll(x => x.Mov).OrderBy(x => new MapPoint(x).DistanceTo(new MapPoint(cellAfter))).First().Id;

                map.Quit(client);
                newMap.Enter(client);
                //if (client.Character.PartyId > 0)
                //{
                //    Party party = PartyManager.Instance.GetPartyById(client.Character.PartyId);
                //    if (party != null)
                //    {
                //        party.OnMove();
                //    }
                //}
            }
        }
    }
}
