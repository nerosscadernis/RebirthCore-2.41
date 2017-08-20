using Rebirth.Common.Dispatcher;
using Rebirth.Common.GameData.D2O;
using Rebirth.Common.Protocol.Data;
using Rebirth.Common.Protocol.Messages;
using Rebirth.World.Datas.Spells;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rebirth.World.Frames
{
    public class FightContextFrame
    {
        [MessageHandler(GameFightTurnFinishMessage.Id)]
        private void HandlerGameFightTurnFinishMessage(Client client, GameFightTurnFinishMessage message)
        {
            if (client.Character.Fight == null)
                return;

            if (client.Character.Fight.IsStoped)
                return;

            if (client.Character.Fight.IsPlayerTurn((int)client.Character.Infos.Id))
            {
                client.Character.Fighter.ActionPass();
            }
            else if (client.Character.Fight.TimeLine.FighterPlaying is SummonMonster)
            {
                SummonMonster summon = (client.Character.Fight.TimeLine.FighterPlaying as SummonMonster);
                if (!summon.IsMyController((int)client.Character.Infos.Id))
                    return;

                summon.ActionPass();
            }
        }

        // in fight movement
        [MessageHandler(GameMapMovementRequestMessage.Id)]
        private void HandlerGameMapMovementRequestMessage(Client client, GameMapMovementRequestMessage message)
        {
            if (client.Character.Fight == null)
                return;

            if (client.Character.Fight.IsStoped)
            {
                client.Send(new GameMapNoMovementMessage());
                return;
            }

            if (client.Character.Fight.IsPlayerTurn((int)client.Character.Infos.Id))
            {
                client.Character.Fighter.MovementRequest(message.keyMovements.ToArray());
            }
            else if (client.Character.Fight.TimeLine.FighterPlaying is SummonMonster)
            {
                SummonMonster summon = (client.Character.Fight.TimeLine.FighterPlaying as SummonMonster);
                if (!summon.IsMyController((int)client.Character.Infos.Id))
                    return;

                summon.MovementRequest(message.keyMovements.ToArray());
            }
        }

        [MessageHandler(GameActionFightCastRequestMessage.Id)]
        private void HandleGameActionFightCastRequestMessage(Client client, GameActionFightCastRequestMessage message)
        {
            if (client.Character.Fight == null)
                return;

            if (client.Character.Fight.IsStoped)
            {
                client.Send(new GameActionFightNoSpellCastMessage());
                return;
            }

            if (client.Character.Fight.IsPlayerTurn((int)client.Character.Infos.Id))
            {
                var spell = client.Character.Spells.Spells.FirstOrDefault(x => x.Spell.id == message.spellId);
                if (spell == null)
                    return;

                client.Character.Fighter.TryCastSpell(spell, message.cellId);
            }
            else if (client.Character.Fight.TimeLine.FighterPlaying is SummonMonster)
            {
                SummonMonster summon = (client.Character.Fight.TimeLine.FighterPlaying as SummonMonster);
                if (!summon.IsMyController((int)client.Character.Infos.Id))
                    return;

                var spell = summon.Spells.FirstOrDefault(x => x.spellId == message.spellId);
                if (spell == null)
                    return;

                summon.TryCastSpell(new SpellTemplate(ObjectDataManager.Get<Spell>((int)spell.spellId), 0), message.cellId);
            }
        }

        [MessageHandler(GameActionFightCastOnTargetRequestMessage.Id)]
        private void HandleGameActionFightCastOnTargetRequestMessage(Client client, GameActionFightCastOnTargetRequestMessage message)
        {
            if (client.Character.Fight == null)
                return;

            if (client.Character.Fight.IsStoped)
            {
                client.Send(new GameActionFightNoSpellCastMessage());
                return;
            }

            if (client.Character.Fight.IsPlayerTurn((int)client.Character.Infos.Id))
            {
                var spell = client.Character.Spells.Spells.FirstOrDefault(x => x.Spell.id == message.spellId);
                if (spell == null)
                    return;

                Fighter target = client.Character.Fight.GetFighter((int)message.targetId);
                if (target != null && !target.IsInvisible)
                    client.Character.Fighter.TryCastSpell(spell, target.Point.Point.CellId);
            }
            else if (client.Character.Fight.TimeLine.FighterPlaying is SummonMonster)
            {
                SummonMonster summon = (client.Character.Fight.TimeLine.FighterPlaying as SummonMonster);
                if (!summon.IsMyController((int)client.Character.Infos.Id))
                    return;

                var spell = summon.Spells.FirstOrDefault(x => x.spellId == message.spellId);
                if (spell == null)
                    return;

                Fighter target = client.Character.Fight.GetFighter((int)message.targetId);
                if (target != null && !target.IsInvisible)
                    summon.TryCastSpell(new SpellTemplate(ObjectDataManager.Get<Spell>((int)spell.spellId), 0), target.Point.Point.CellId);
            }
        }

        [MessageHandler(GameActionAcknowledgementMessage.Id)]
        private void HandleGameActionAcknowledgementMessage(Client client, GameActionAcknowledgementMessage message)
        {
            if (client.Character.Fight == null && !client.Character.Fight.IsPlayerTurn(client.Character.Fighter.Id))
                return;

            if (client.Character.Fight.WaitAcknowledgment)
                client.Character.Fight.AcknowledgeAction();
            //??? plus besoin
        }

        [MessageHandler(GameFightTurnReadyMessage.Id)]
        private void HandleGameFightTurnReadyMessage(Client client, GameFightTurnReadyMessage message)
        {
            if (client.Character.Fight == null)
                return;

            client.Character.Fighter.SetReady(message.isReady);

            //??? plus besoin
        }

        [MessageHandler(GameFightOptionToggleMessage.Id)]
        private void HandleGameFightOptionToggleMessage(Client client, GameFightOptionToggleMessage message)
        {
            if (client.Character.Fight == null && client.Character.Fighter == null)
                return;

            client.Character.Fight.UpdateOption(client.Character.Fighter, message.option);
        }

        [MessageHandler(GameContextKickMessage.Id)]
        private void HandleGameContextKickMessage(Client client, GameContextKickMessage message)
        {
            if (client.Character.Fight == null && client.Character.Fighter == null)
                return;

            client.Character.Fight.KickCharacter(client.Character.Fighter, message.targetId);
        }

        [MessageHandler(ChallengeTargetsListRequestMessage.Id)]
        private void HandleChallengeTargetsListRequestMessage(Client client, ChallengeTargetsListRequestMessage message)
        {
            if (client.Character.Fight == null && client.Character.Fighter == null)
                return;

            var chall = client.Character.Fight.GetChall((int)message.challengeId);

            if (chall == null)
                return;

            client.Send(new ChallengeTargetsListMessage((chall.Target != null) ? new double[] { chall.Target.Id } : new double[0], chall.Target != null ? new short[] {
                (chall.Target.IsInvisible ? (short)0 : chall.Target.Point.Point.CellId) } : new short[0]));
        }
    }
}
