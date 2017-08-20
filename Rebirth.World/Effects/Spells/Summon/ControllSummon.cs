using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Effects.Handlers;
using Rebirth.World.Effects.Handlers.Spells;
using Rebirth.World.Game.Effect.Instances;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Game.Fights.Buffs;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Datas.Monsters;
using Rebirth.World.Game.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rebirth.World.Datas.Spells;

namespace Rebirth.World.Game.Heal
{
    [EffectHandler(EffectsEnum.Effect_ControlSummon)]
    public class ControllSummon : SpellEffectHandler
    {
        public ControllSummon(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }
        public override bool Apply(object token, bool declanched = false)
        {
            bool result;
            SummonMonster summon = Caster.GetSummonByCellId(TargetedCell.Id);
            if (summon != null)
            {
                foreach (var item in Caster.GetSummonByTemplate(summon.Template.Template.id))
                {
                    var buff = new ControlBuff(summon.PopNextBuffId(), summon, summon, Effect, Spell, Critical, false, summon.Template.Template.id, true);
                    summon.AddAndApplyBuff(buff);
                }
            }
            result = true;
            return result;  
        }
    }
}
