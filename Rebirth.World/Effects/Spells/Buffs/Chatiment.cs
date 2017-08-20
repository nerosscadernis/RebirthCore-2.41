using Rebirth.Common.Protocol.Enums;
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
using System.Threading.Tasks;
using Rebirth.World.Datas.Spells;

namespace Rebirth.World.Game.Effect.Spells.Buffs
{
    [EffectHandler(EffectsEnum.Effect_Punishment)]
    public class Chatiment : SpellEffectHandler
    {
        public Chatiment(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }
        public override bool Apply(object token, bool declanched = false)
        {
            foreach (Fighter current in base.GetAffectedActors())
            {
                var trigger = new TriggerBuff(current.PopNextBuffId(), current, base.Caster, (Effect as EffectDice), base.Spell, null, (short)base.Dice.Value, false, false, TriggerBuffType.AFTER_ATTACKED, declanched);
                trigger.Applyed += OnActorAttacked;
                current.AddAndApplyBuff(trigger, true);

                maxStack = base.Dice.DiceFace;
            }
            return true;
        }

        public short maxStack;
        public uint actualDuration;

        private void OnActorAttacked(TriggerBuff buff, object token)
        {
            if (Fight.TimeLine.ActualRound != actualDuration)
            {
                maxStack = base.Dice.DiceFace;
                actualDuration = Fight.TimeLine.ActualRound;
            }
            if (maxStack > 0)
            {
                Fights.Other.Damage damage = (Fights.Other.Damage)token;
                int num2 = damage.AfterReduction;
                if (num2 > maxStack)
                {
                    num2 = maxStack;
                }
                PlayerFields punishmentBoostType = GetPunishmentBoostType(base.Dice.DiceNum);
                if (punishmentBoostType == PlayerFields.Vitality)
                {
                    Caster.HealDirect(num2, Caster);
                    maxStack -= (short)num2;
                }
                else
                {
                    var newEffect = new EffectBase()
                    {
                        Id = 0,
                        Duration = 0,
                        Delay = 0,
                        EffectId = 0,
                        Uid = 0
                    };
                    StatBuff buff2 = new StatBuff(buff.Target.PopNextBuffId(), buff.Target, buff.Target, newEffect, base.Spell, (short)num2, punishmentBoostType, false, true, (short)(punishmentBoostType == PlayerFields.DamageBonusPercent ? 138 : 119), true)
                    {                       
                        ParentId = (uint)buff.Id
                    };
                    buff2.Duration = (short)base.Dice.Value;
                    buff.Target.AddAndApplyBuff(buff2, true);
                    maxStack -= (short)num2;
                }
            }
        }
        private static PlayerFields GetPunishmentBoostType(short punishementAction)
        {
            PlayerFields result;
            switch (punishementAction)
            {
                case 118:
                    result = PlayerFields.Strength;
                    return result;
                case 119:
                    result = PlayerFields.Agility;
                    return result;
                case 120:
                case 121:
                case 122:
                    break;
                case 123:
                    result = PlayerFields.Chance;
                    return result;
                case 124:
                    result = PlayerFields.Wisdom;
                    return result;
                case 125:
                    goto IL_61;
                case 126:
                    result = PlayerFields.Intelligence;
                    return result;
                case 138:
                    result = PlayerFields.DamageBonusPercent;
                    return result;

                default:
                    if (punishementAction == 407)
                    {
                        goto IL_61;
                    }
                    break;
            }
            throw new System.Exception(string.Format("PunishmentBoostType not found for action {0}", punishementAction));
        IL_61:
            result = PlayerFields.Vitality;
            return result;
        }
        private static EffectsEnum GetBuffEffectId(PlayerFields caracteristic)
        {
            EffectsEnum result;
            switch (caracteristic)
            {
                case PlayerFields.Strength:
                    result = EffectsEnum.Effect_AddStrength;
                    break;
                case PlayerFields.Vitality:
                    result = EffectsEnum.Effect_AddVitality;
                    break;
                case PlayerFields.Wisdom:
                    result = EffectsEnum.Effect_AddWisdom;
                    break;
                case PlayerFields.Chance:
                    result = EffectsEnum.Effect_AddChance;
                    break;
                case PlayerFields.Agility:
                    result = EffectsEnum.Effect_AddAgility;
                    break;
                case PlayerFields.Intelligence:
                    result = EffectsEnum.Effect_AddIntelligence;
                    break;
                case PlayerFields.DamageBonusPercent:
                    result = EffectsEnum.Effect_AddDamageBonusPercent;
                    break;
                default:
                    throw new System.Exception(string.Format("Buff Effect not found for caracteristic {0}", caracteristic));
            }
            return result;
        }
    }
}
