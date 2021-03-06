﻿using Rebirth.Common.Protocol.Enums;
using Rebirth.Common.Thread;
using Rebirth.World.Effects.Handlers;
using Rebirth.World.Effects.Handlers.Spells;
using Rebirth.World.Game.Effect.Instances;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Game.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rebirth.World.Datas.Spells;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Effect.Spells.Steal
{

[EffectHandler(EffectsEnum.Effect_StealMP_77)]
public class StealMP : SpellEffectHandler
{
    public StealMP(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
    {
    }
    public override bool Apply(object token, bool declanched = false)
    {     
        foreach (Fighter current in base.GetAffectedActors())
        {
                EffectInteger effectInteger = base.GenerateEffect();
                short cost = 0;
                for (int i = 0; i < effectInteger.Value; i++)
                {
                    int currentMP = current.Stats.MP.Total - cost;
                    double pourcent = 0;
                    pourcent = ((((double)currentMP / current.Stats.MP.TotalMax) * ((double)Caster.Stats.MPAttack.Total / current.Stats.DodgeMPProbability.Total)) * 0.5) * 100;
                    pourcent = pourcent < 0 ? pourcent * -1 : pourcent;
                    pourcent = double.IsNaN(pourcent) ? 0 : pourcent;
                    pourcent = pourcent <= 100 ? pourcent : 100;
                    AsyncRandom asyncRandom = new AsyncRandom();
                    double num = asyncRandom.NextDouble(0, 100);
                    if (num < pourcent)
                        cost++;
                }
                if (effectInteger == null)
                {
                    return false;
                }
                if (cost > 0)
                {
                    if (this.Effect.Duration > 0)
                    {
                        base.AddStatBuff(current, (short)(cost * -1), PlayerFields.MP, true, 169, declanched);
                        base.AddStatBuff(Caster, cost, PlayerFields.MP, true, 169, declanched);
                    }
                    else
                    {
                        current.UseMP(cost, true);
                        Caster.RegainMP(cost);
                    }
                }
                if (cost < Dice.DiceNum)
                    Fight.DodgePointLost(Caster, current, Dice.DiceNum - cost, 309);
            }
        return true;
    }
}
}