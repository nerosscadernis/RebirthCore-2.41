using Rebirth.Common.Protocol.Data;
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

namespace Rebirth.World.Game.Effect.Spells.Summon
{
    [EffectHandler(EffectsEnum.Effect_DeclanchBomb)]
    public class DeclanchBomb : SpellEffectHandler
    {
        public DeclanchBomb(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }
        public override bool Apply(object token, bool declanched = false)
        {
            bool result;
            foreach (Fighter current in base.GetAffectedActors())
            {
                if (current is SummonBomb)
                    if (Dice.Duration > 1)
                    {
                        var buff = new TriggerBuff(current.PopNextBuffId(), current, Caster, Dice, Spell, null, 0, Critical, false, TriggerBuffType.DIE, declanched);
                        buff.Applyed += Execute;
                        current.AddAndApplyBuff(buff);
                    }
                    else if (Dice.Duration > 0)
                    {
                        var buff = new TriggerBuff(current.PopNextBuffId(), current, Caster, Dice, Spell, null, 0, Critical, false, TriggerBuffType.BUFF_ENDED, declanched);
                        buff.Applyed += Execute;
                        current.AddAndApplyBuff(buff);
                    }
                    else if (!(current as SummonBomb).IsActive)
                        (current as SummonBomb).Execute();

            }
            result = true;
            return result;
        }
        public void Execute(TriggerBuff buff, object token)
        {
            if (buff.Target is SummonBomb)
                (buff.Target as SummonBomb).Execute();
        }
    }
}
