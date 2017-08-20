using Rebirth.Common.Protocol.Types;
using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Game.Effect.Instances;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Game.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rebirth.World.Datas.Spells;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Fights.Buffs
{
    public class InvisibleBuff : Buff
    {
        public InvisibleBuff(int id, Fighter target, Fighter caster, EffectBase effect, SpellTemplate spell, bool critical, bool dispelable, bool declanched) : base(id, target, caster, effect, spell, critical, dispelable, declanched)
        {
        }
        public InvisibleBuff(int id, Fighter target, Fighter caster, EffectBase effect, SpellTemplate spell, bool critical, bool dispelable, short customActionId, bool declanched) : base(id, target, caster, effect, spell, critical, dispelable, customActionId, declanched)
        {
        }
        public override void Apply(object token = null)
        {
            Target.IsInvisible = true;
            Caster.Fight.SetVisibility(this, (sbyte)GameActionFightInvisibilityStateEnum.INVISIBLE);
        }
        public override void Dispell()
        {
            Target.IsInvisible = false;
            Caster.Fight.SetVisibility(this, (sbyte)GameActionFightInvisibilityStateEnum.VISIBLE);
        }
        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            return new FightTemporaryBoostEffect((uint)Id, Target.Id, Duration, Convert.ToSByte(Dispellable ? 0 : 1), (uint)Spell.Spell.id, (uint)Effect.Id, 0, 0);
        }
    }
}
