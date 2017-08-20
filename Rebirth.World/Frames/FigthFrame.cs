using Rebirth.Common.Dispatcher;
using Rebirth.Common.Protocol.Messages;
using Rebirth.World.Game.Fights;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Game.Fights.Custom;
using Rebirth.World.Game.Fights.Duel;
using Rebirth.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rebirth.World.Frames
{
    public class FigthFrame
    {
        //GameRolePlayAttackMonsterRequestMessage
        [MessageHandler(GameRolePlayAttackMonsterRequestMessage.Id)]
        public void HandlerGameRolePlayAttackMonsterRequestMessage(Client client, GameRolePlayAttackMonsterRequestMessage message)
        {
            //if (client.Character.Infos.IsDead)
            //    return;
            if (client.Character == null)
                return;

            var monster = client.Character.Map.GetMonsterGroup((int)message.monsterGroupId);
            if (monster == null)
                return;

            if (monster.CellId != client.Character.Infos.CellId)
                return;

            //if (monster.WithAlternative)
            //    client.Character.Map.StartFightDungeon(client.Character, monster);
            //else
                client.Character.Map.StartFightPvm(client.Character, monster);
            //client.Send(new SubscriptionLimitationMessage(3));
        }

        //[MessageHandler(GameRolePlayPlayerFightRequestMessage.Id)]
        //private void HandlerGameRolePlayPlayerFightRequestMessage(Network.Client client, GameRolePlayPlayerFightRequestMessage message)
        //{
        //    var character = WorldServer.GetCharacterById((int)message.targetId);
        //    if (character == null)
        //        return;

        //    if (client.Character.Map == null || character.Map == null)
        //        return;

        //    if (client.Character.Map.Id != character.Map.Id)
        //        return;


        //    if (message.friendly)
        //    {
        //        var fight = FightManager.Instance.CreatePvpFriend(character.Map);

        //        RequestFight request = new RequestFight(client.Character, character, fight);
        //        character.Request = request;
        //        client.Character.Request = request;

        //        request.Open();
        //    }
        //    else
        //    {
        //        if (!client.Character.Map.Coordinates.allowAggression)
        //            return;
        //        if (client.Character.Alignement.AlignSide != 0 && client.Character.Alignement.PvpEnable
        //            && character.Record.AlignSide != 0 && character.Record.PvpEnable
        //            && character.Record.AlignSide != client.Character.Alignement.AlignSide
        //            && character.Record.PvpEnable)
        //        {
        //            if (character.Map == client.Character.Map
        //                && !character.IsInFight && client.Character.Fight == null)
        //            {
        //                var fight = FightManager.Instance.CreateAgressionFight(client.Character, character, character.Map);
        //            }
        //        }
        //        else
        //        {
        //            if (client.Character.Level < 50)
        //            {
        //                client.Character.SendServerMessage("Vous devez etre level 50 minimum pour aggresser.");
        //                return;
        //            }
        //            if (character.Record.IsDead)
        //            {
        //                client.Character.SendServerMessage("Votre cible doit etre vivante pour l'aggresser.");
        //                return;
        //            }
        //            if (client.Character.Record.IsDead)
        //            {
        //                client.Character.SendServerMessage("Vous devez etre vivante pour aggresser.");
        //                return;
        //            }
        //            if (client.Character.Alliance != null && client.Character.Alliance != character.Alliance)
        //            {
        //                var fight = FightManager.Instance.CreateAvAFight(client.Character, character, character.Map);
        //            }
        //            else
        //            {
        //                client.Character.SendServerMessage("Vous devez posseder une alliance et etre dans une alliance differente de votre cible.");
        //            }
        //        }
        //    }
        //}

        [MessageHandler(GameRolePlayPlayerFightFriendlyAnswerMessage.Id)]
        private void HandlerGameRolePlayPlayerFightFriendlyAnswerMessage(Network.Client client, GameRolePlayPlayerFightFriendlyAnswerMessage message)
        {
            var request = client.Character.Request;
            if (request == null)
                return;

            if (message.accept)
                request.Accept();
            else
                request.Refuse();
        }

        [MessageHandler(GameFightPlacementPositionRequestMessage.Id)]
        private void HandlerGameFightPlacementPositionRequestMessage(Network.Client client, GameFightPlacementPositionRequestMessage message)
        {

            if (client.Character.Fight == null || client.Character.Fighter.IsReady || client.Character.Fight.IsStarting)
                return;

            client.Character.Fight.ChangeActorPosition(client.Character.Fighter, (int)message.cellId);
        }

        [MessageHandler(GameFightReadyMessage.Id)]
        private void HandlerGameFightReadyMessage(Network.Client client, GameFightReadyMessage message)
        {
            if (client.Character.Fight == null || client.Character.Fight.IsStarting)
            {
                //client.Character.SendServerMessage("Vous n'etes pas en combat ou le combat est deja lancer.");
                return;
            }

            client.Character.Fighter.SetReady(message.isReady);
        }

        [MessageHandler(GameFightJoinRequestMessage.Id)]
        private void HandlerGameFightJoinRequestMessage(Network.Client client, GameFightJoinRequestMessage message)
        {
            if (client.Character.Fight != null)
                return;

            Game.Fights.Fight fight = client.Character.Map.Fights.FirstOrDefault(x => x.FightId == message.fightId);
            if (fight != null)
            {
                if (message.fighterId == 0 && !fight.IsSecret)
                    fight.AddSpectator(client.Character);
                else if (!fight.IsStarting && !fight.IsEnded)
                {
                    var fighter = fight.GetFighter((int)message.fighterId);
                    if (!(fighter is MonsterFighter) && !fighter.Team.Close)
                    {
                        if (fight is AgressionFight)
                        {
                            var characterFighter = (CharacterFighter)fighter;
                            //if (client.Character.Align.AlignSide != characterFighter.Character.Align.AlignSide)
                            //{
                            //    //client.Character.SendServerMessage("Impossible de rejoindre ce combat car vous n'êtes pas du même alignement !");
                            //    return;
                            //}
                        }
                        if (fighter.Team.PartyOnly && client.Character.Party != null /*&& client.Character.Party.ContainsMember(fighter.Id)*/)
                        {
                            client.Character.Map.Quit(client);
                            fight.AddCharacter(client.Character, fight.GetFighter((int)message.fighterId).Team.Team);
                        }
                        else if (!fighter.Team.PartyOnly)
                        {
                            client.Character.Map.Quit(client);
                            fight.AddCharacter(client.Character, fight.GetFighter((int)message.fighterId).Team.Team);
                        }
                    }
                }
            }
        }

        [MessageHandler(GameContextQuitMessage.Id)]
        private void HandleGameContextQuitMessage(Network.Client client, GameContextQuitMessage message)
        {
            if (client.Character.Fight == null)
                return;

            client.Character.Fight.RemoveCharacter(client.Character.Fighter);
        }
    }
}
