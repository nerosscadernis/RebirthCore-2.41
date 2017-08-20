using Rebirth.Common.Protocol.Enums;
using Rebirth.Common.Protocol.Types;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Game.Fights.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Fights.Team
{
    public class FightTeam
    {
        public int CellId
        {
            get
            {
                return Fighters[0] is CharacterFighter ? (Fighters[0] as CharacterFighter).Character.Infos.CellId : 255;
            }
        }
        public TeamEnum Team { get; set; }
        public TeamTypeEnum Type { get; set; }
        public FightOutcomeEnum Outcome { get; set; }
        public int LeaderId
        {
            get
            {
                return Fighters.Count > 0 ? Fighters[0].Id : 0;
            }
        }

        public Fighter getLeader
        {
            get
            {
                return Fighters.Count > 0 ? Fighters[0] : null;
            }
        }
        public List<Fighter> Fighters { get; set; }

        public bool Close { get; set; }
        public bool NeedHelp { get; set; }
        public bool PartyOnly { get; set; }
        public bool IsSecret { get; set; }

        public FightTeam(TeamEnum team)
        {
            this.Team = team;
            Fighters = new List<Fighter>();
        }

        public void AddFighter(Fighter fighter)
        {
            Fighters.Add(fighter);
        }

        public FightTeamInformations GetTeamInformation()
        {
            CharacterFighter leader = (getLeader is CharacterFighter) ? (CharacterFighter)getLeader : null;
            if (leader != null && leader.Fight is AgressionFight)
            {
                return new FightTeamInformations((sbyte)(Team), LeaderId, (sbyte)0, (sbyte)Type, 0, Fighters.Select(x => x.GetTeamInformations()).ToArray());
            }
            else
            {
                return new FightTeamInformations((sbyte)(Team), LeaderId, -1, (sbyte)Type, 0, Fighters.Select(x => x.GetTeamInformations()).ToArray());
            }
        }

        public FightOptionsInformations GetFightOptionsInformations()
        {
            return new FightOptionsInformations(IsSecret, PartyOnly, !Close, NeedHelp);
        }

        public FightTeamLightInformations GetFightTeamLightInformations()
        {
            return new FightTeamLightInformations((sbyte)(Team), LeaderId, 0, (sbyte)Type, 0, false, false, false, false, false, (sbyte)Fighters.Count, (uint)Fighters.Average(x => x.Level));
        }

        public GameFightFighterLightInformations[] GetFightersInformations()
        {
            return (from x in Fighters
                    select x.GetGameFightFighterLightInformations()).ToArray();
        }

        public bool AreAllDead()
        {
            return Fighters.FirstOrDefault(x => x.IsAlive) == null;
        }

    }
}
