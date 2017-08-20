using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rebirth.World.Game.Fights.Team;
using Rebirth.World.Game.Fights.Result;
using Rebirth.Common.Protocol.Messages;
using Rebirth.World.Datas.Characters;

namespace Rebirth.World.Game.Fights.Custom
{
    public class DuelFight : Fight
    {
        #region Constructor
        public DuelFight(int id, MapTemplate map) : base (id, map)
        {
            Teams[(byte)TeamEnum.TEAM_CHALLENGER].Type = TeamTypeEnum.TEAM_TYPE_PLAYER;
            Teams[(byte)TeamEnum.TEAM_DEFENDER].Type = TeamTypeEnum.TEAM_TYPE_PLAYER;
        }
        #endregion

        #region Properties
        public override FightTypeEnum Type
        {
            get
            {
                return FightTypeEnum.FIGHT_TYPE_CHALLENGE;
            }
        }
        #endregion

        #region Override Functions
        public override void RemoveCharacter(Fighter fighter)
        {
            Character character;
            Leavers.Add(fighter);
            if (fighter is CharacterFighter)
            {
                character = (fighter as CharacterFighter).Character;
                if (!IsStarting)
                {
                    Fighters.Remove(fighter);
                }
                else
                {
                    fighter.IsDisconnected = true;
                }
            }
            else
            {
                character = (fighter as SpectatorFighter).Character;
                Spectators.Remove(fighter as SpectatorFighter);
            }

            if (fighter is CharacterFighter)
            {
                fighter.Die(fighter);
                fighter.RemoveAllBuff();
                fighter.PassTurnEnd();
                fighter.Stats.Health.PermanentDamages = 0;
                fighter.Stats.Health.DamageTaken = (fighter as CharacterFighter).StartLife;
            }

            if (Fighters != null)
            {
                character.Fighter = null;
                character.Fight = null;
                character.Client.Send(new GameContextDestroyMessage());
                character.Client.Send(new GameContextCreateMessage(1));
                character.RefreshStats();
                character.Client.Send(new GameRolePlayRemoveChallengeMessage(FightId));
                Map.Enter(character.Client);
            }
            CheckFightEnd();
        }
        #endregion

        #region End
        public override void ResetFightersProperties()
        {
            base.ResetFightersProperties();
            foreach (CharacterFighter item in Fighters)
            {
                item.Stats.Health.DamageTaken = item.StartLife;
            }
        }

        public override void ReturnInMap(bool command = false)
        {
            foreach (CharacterFighter item in Fighters.FindAll(x => x is CharacterFighter && !x.IsDisconnected))
            {
                item.Character.Client.Send(new GameContextDestroyMessage());
                item.Character.Client.Send(new GameContextCreateMessage(1));
                item.Character.RefreshStats();
                Map.Enter(item.Character.Client);
            }
            foreach (SpectatorFighter item in Spectators)
            {
                item.Character.Client.Send(new GameContextDestroyMessage());
                item.Character.Client.Send(new GameContextCreateMessage(1));
                item.Character.RefreshStats();
                Map.Enter(item.Character.Client);
            }
            Fighters.Clear();
        }
        #endregion
    }
}
