using Rebirth.Common.Protocol.Enums;
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

namespace Rebirth.World.Game.Effect.Spells.Debuffs
{
    [EffectHandler(EffectsEnum.Effect_Remove_PA)]
    public class LostAP : SpellEffectHandler
    {
        public LostAP(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }
        public override bool Apply(object token, bool declanched = false)
        {
            bool result;
            foreach (Fighter current in base.GetAffectedActors())
            {
                EffectInteger effectInteger = base.GenerateEffect();
                short cost = 0;
                for (int i = 0; i < effectInteger.Value; i++)
                {
                    int currentAP = current.Stats.AP.Total - cost;
                    double pourcent = 0;
                    pourcent = ((((double)currentAP / current.Stats.AP.TotalMax) * ((double)Caster.Stats.APAttack.Total / current.Stats.DodgeAPProbability.Total)) * 0.5) * 100;
                    pourcent = double.IsNaN(pourcent) ? 0 : pourcent;
                    pourcent = pourcent < 0 ? pourcent * -1 : pourcent;
                    pourcent = pourcent <= 100 ? pourcent : 100;
                    AsyncRandom asyncRandom = new AsyncRandom();
                    double num = asyncRandom.NextDouble(0, 100);
                    if (num < pourcent)
                        cost++;
                }
                if (effectInteger == null)
                {
                    result = false;
                    return result;
                }
                if (cost > 0)
                {
                    if (this.Effect.Duration > 0)
                    {
                        base.AddStatBuff(current, -cost, PlayerFields.AP, true, 168, declanched);
                    }
                    else
                    {
                        current.UseAP(cost, true);
                    }
                }
                if (cost < Dice.DiceNum)
                    Fight.DodgePointLost(Caster, current, Dice.DiceNum - cost, 308);
            }
            result = true;
            return result;
        }
    }
}
