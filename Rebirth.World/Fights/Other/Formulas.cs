using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Game.Characters.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Fights.Other
{
    public class Formulas
    {
        public static int CalculateHeal(int heal, StatsFields stats)
        {
            return (int)((double)(heal * (100 + stats[PlayerFields.Intelligence].TotalSafe)) / 100.0 + (double)stats[PlayerFields.HealBonus].TotalSafe);
        }
    }
}
