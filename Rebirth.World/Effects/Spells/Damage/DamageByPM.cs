using Rebirth.Common.Protocol.Enums;
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

namespace Rebirth.World.Game.Effect.Spells.Damage
{
    [EffectHandler(EffectsEnum.Effect_DamageByPMFire)]
    public class DamageByPM : SpellEffectHandler
    {
        public DamageByPM(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }
        public override bool Apply(object token, bool declanched = false)
        {
            foreach (Fighter current in base.GetAffectedActors())
            {
                if (this.Effect.Duration > 0)
                {
                    //int id = current.PopNextBuffId();

                    //var args = new short[] { 1, (short)(Dice.DiceFace - (Dice.DiceNum - 1)), (short)(Dice.DiceNum - 1) };

                    //var trigger = new TriggerBuff(id, current, base.Caster, Dice, base.Spell, null, Dice.DiceNum, false, true, TriggerBuffType.TURN_END, args, declanched);
                    //trigger.Applyed += ApplyDamage;
                    //base.AddTriggerBuff(current, trigger);
                }
                else
                {
                    var div = (double)Caster.Stats.MP.Total / (double)Caster.Stats.MP.TotalMax;
                    var min = (double)Dice.DiceNum * div;
                    var max = (double)Dice.DiceFace * div;
                    current.InflictDamage(new Fights.Other.Damage((int)max, (int)min, base.Caster, current, GetEffectSchool(base.Dice.EffectId), EffectGenerationType.Normal, base.Spell, Boost)
                    {
                        critical = base.Critical
                    });
                }
            }
            return true;
        }

        private EffectSchoolEnum GetEffectSchool(EffectsEnum effect)
        {
            EffectSchoolEnum result;
            switch (effect)
            {
                case EffectsEnum.Effect_DamageAirPerAP:
                    result = EffectSchoolEnum.Air;
                    break;
                case EffectsEnum.Effect_DamageWaterPerAP:
                    result = EffectSchoolEnum.Water;
                    break;
                case EffectsEnum.Effect_DamageByPMFire:
                    result = EffectSchoolEnum.Fire;
                    break;
                case EffectsEnum.Effect_DamageNeutralPerAP:
                    result = EffectSchoolEnum.Neutral;
                    break;
                case EffectsEnum.Effect_DamageEarthPerAP:
                    result = EffectSchoolEnum.Earth;
                    break;
                default:
                    throw new System.Exception(string.Format("Effect {0} has not associated School Type", effect));
            }
            return result;
        }
    }
}
