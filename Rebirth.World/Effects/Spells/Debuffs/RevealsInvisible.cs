﻿using Rebirth.Common.Protocol.Enums;
using Rebirth.Common.Thread;
using Rebirth.World.Effects.Handlers;
using Rebirth.World.Effects.Handlers.Spells;
using Rebirth.World.Game.Effect.Instances;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Game.Fights.Buffs;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Game.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rebirth.World.Datas.Spells;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Effect.Spells.Debuffs
{
    [EffectHandler(EffectsEnum.Effect_RevealsInvisible)]
    public class RevealsInvisible : SpellEffectHandler
    {
        public RevealsInvisible(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }
        public override bool Apply(object token, bool declanched = false)
        {
            bool result;
            if (Effect is EffectInteger)
            {
                foreach (Fighter current in base.GetAffectedActors())
                {
                    var buff = current.BuffList.FirstOrDefault(x => x is InvisibleBuff);
                    if (buff != null)
                        current.RemoveAndDispellBuff(buff, true);
                }
                Fight.SetRevealTrap(Caster, AffectedCells.Select(x => x.Id).ToArray());
            }
            result = true;
            return result;
        }
    }
}
