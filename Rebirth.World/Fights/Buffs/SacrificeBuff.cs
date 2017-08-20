using Rebirth.Common.Protocol.Types;
using Rebirth.World.Datas.Spells;
using Rebirth.World.Game.Effect.Instances;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Game.Fights.Other;
using Rebirth.World.Game.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Fights.Buffs
{
    public class SacrificeBuff : Buff
    {
        public SacrificeBuff(int id, Fighter target, Fighter caster, EffectBase effect, SpellTemplate spell, bool critical, bool dispelable, bool declanched) : base(id, target, caster, effect, spell, critical, dispelable, declanched)
        {
        }
        public SacrificeBuff(int id, Fighter target, Fighter caster, EffectBase effect, SpellTemplate spell, bool critical, bool dispelable, short customActionId, bool declanched) : base(id, target, caster, effect, spell, critical, dispelable, customActionId, declanched)
        {
        }
        public override void Apply(object token = null)
        {
            if (token is Damage)
            {
                Caster.InflictDirectDamage((Damage)token, true);
            }
        }
        public override void Dispell()
        {
        }
        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            return new FightTriggeredEffect((uint)Id, Target.Id, Duration, (sbyte)(Dispellable ? 1 : 0), (uint)Spell.Spell.id, (uint)Effect.Uid, 0, 0, 0, 0, 0);
        }
    }
}
