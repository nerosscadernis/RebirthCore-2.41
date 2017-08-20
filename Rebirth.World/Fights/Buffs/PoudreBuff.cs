using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rebirth.Common.Protocol.Types;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Game.Effect.Instances;
using Rebirth.World.Game.Spells;
using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Datas.Spells;

namespace Rebirth.World.Game.Fights.Buffs
{
    public class PoudreBuff : Buff
    {
        public PoudreBuff(int id, Fighter target, Fighter caster, EffectBase effect, SpellTemplate spell, bool value, bool critical, bool dispelable, bool declanched) : base(id, target, caster, effect, spell, critical, dispelable, declanched)
        {
            Value = value;
        }
        public PoudreBuff(int id, Fighter target, Fighter caster, EffectBase effect, SpellTemplate spell, bool value, bool critical, bool dispelable, short customActionId, bool declanched) : base(id, target, caster, effect, spell, critical, dispelable, customActionId, declanched)
        {
            Value = value;
        }

        public bool Value
        {
            get;
            set;
        }

        public override void Apply(object token = null)
        {
        }

        public override void Dispell()
        {
        }

        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            object[] values = Effect.GetValues();
            return new FightTriggeredEffect((uint)Id, Target.Id, (short)Duration, 0, (uint)Spell.Spell.id, (uint)Effect.EffectId, 0, short.Parse(values[0].ToString()), short.Parse(values[1].ToString()), short.Parse(values[2].ToString()), (short)Effect.Delay);
        }
    }
}
