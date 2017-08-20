using Rebirth.Common.Protocol.Data;
using Rebirth.World.Game.Characters.Stats;
using Rebirth.World.Game.Fights.Buffs;
using Rebirth.World.Game.Fights.Other;
using Rebirth.World.Game.Fights.Team;
using Rebirth.World.Datas.Monsters;
using Rebirth.World.Game.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Fights.Actors
{
    public class SummonTree : SummonMonster, IStatsOwner
    {
        #region Constructeur
        public SummonTree(MonsterTemplate template, FightTeam team, Fight fight, int id, Fighter caster, FightDisposition point) : base(template, team, fight, id, caster, point)
        {
            
        }
        #endregion

        public override void AdjustStats()
        {
            int baseVita = (short)(Summoner.Stats.Health.TotalMax / 5);
            Stats = Summoner.Stats.CloneAndChangeOwner(this);
            Stats.Health.DamageTaken = 0;
            Stats.Health.Context = 0;
            Stats.Health.PermanentDamages = 0;
            Stats.Health.Base = Template.Stats.Health.Base;
            Stats.Vitality.Base = baseVita;
            Stats.Vitality.Given = 0;
            Stats.Vitality.Context = 0;
            Stats.Vitality.Equiped = 0;
            Stats.Vitality.Additionnal = 0;
        }
    }
}
