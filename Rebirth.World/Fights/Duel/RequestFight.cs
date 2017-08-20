using Rebirth.Common.Protocol.Enums;
using Rebirth.Common.Protocol.Messages;
using Rebirth.World.Datas.Characters;
using Rebirth.World.Game.Fights.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Fights.Duel
{
    public class RequestFight
    {
        private Fight m_fight;
        private Character m_source, m_target;
        public RequestFight(Character source, Character target, Fight fight)
        {
            m_fight = fight;
            m_source = source;
            m_target = target;
        }

        public void Open()
        {
            m_source.Client.Send(new GameRolePlayPlayerFightFriendlyRequestedMessage(m_fight.FightId, (uint)m_source.Infos.Id, (uint)m_target.Infos.Id));
            m_target.Client.Send(new GameRolePlayPlayerFightFriendlyRequestedMessage(m_fight.FightId, (uint)m_source.Infos.Id, (uint)m_target.Infos.Id));
        }

        // On accepte le combat! c'est partis
        public void Accept()
        {
            m_source.Map.Quit(m_source.Client);
            m_target.Map.Quit(m_target.Client);

            if (m_fight is DuelFight)
            {
                var instance = (DuelFight)m_fight;

                m_source.Client.Send(new GameRolePlayPlayerFightFriendlyAnsweredMessage(m_fight.FightId, (uint)m_source.Infos.Id, (uint)m_target.Infos.Id, true));
                m_target.Client.Send(new GameRolePlayPlayerFightFriendlyAnsweredMessage(m_fight.FightId, (uint)m_source.Infos.Id, (uint)m_target.Infos.Id, true));

                m_fight.AddCharacter(m_target, TeamEnum.TEAM_DEFENDER);
                m_fight.AddCharacter(m_source, TeamEnum.TEAM_CHALLENGER);

                m_source.Request = null;
                m_target.Request = null;
            }
        }

        public void Refuse()
        {
            m_source.Client.Send(new GameRolePlayPlayerFightFriendlyAnsweredMessage(m_fight.FightId, (uint)m_source.Infos.Id, (uint)m_target.Infos.Id, false));
            m_target.Client.Send(new GameRolePlayPlayerFightFriendlyAnsweredMessage(m_fight.FightId, (uint)m_source.Infos.Id, (uint)m_target.Infos.Id, false));
        }
    }
}
