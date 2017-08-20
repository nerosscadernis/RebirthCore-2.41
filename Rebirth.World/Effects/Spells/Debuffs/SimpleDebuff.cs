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
using System.Threading.Tasks;
using Rebirth.World.Datas.Spells;

namespace Rebirth.World.Game.Effect.Spells.Debuffs
{
    [EffectHandler(EffectsEnum.Effect_Debuff)]
    public class SimpleDebuff : SpellEffectHandler
    {
        public SimpleDebuff(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }
        public override bool Apply(object token, bool declanched = false)
        {
            bool result;
            if (Effect is EffectInteger)
            {
                foreach (Fighter current in base.GetAffectedActors())
                {
                    var buffs = current.BuffList.FindAll(x => x.Spell.Spell.id == (Effect as EffectInteger).Value);
                    foreach (var item in buffs)
                    {
                        current.RemoveAndDispellBuff(item, true);
                    }
                }
            }
            result = true;
            return result;
        }
    }
}
