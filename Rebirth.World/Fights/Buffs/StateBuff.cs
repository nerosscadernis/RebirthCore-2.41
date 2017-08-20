using Rebirth.Common.Protocol.Data;
using Rebirth.Common.Protocol.Types;
using Rebirth.World.Datas.Spells;
using Rebirth.World.Game.Effect.Instances;
using Rebirth.World.Game.Fights.Actors;

namespace Rebirth.World.Game.Fights.Buffs
{
    public class StateBuff : Buff
    {
        public SpellState State
        {
            get;
            private set;
        }
        public StateBuff(int id, Fighter target, Fighter caster, EffectBase effect, SpellTemplate spell, bool dispelable, SpellState state, bool declanched) : base(id, target, caster, effect, spell, false, dispelable, declanched)
        {
            this.State = state;
        }
        public StateBuff(int id, Fighter target, Fighter caster, EffectBase effect, SpellTemplate spell, bool dispelable, short customActionId, SpellState state, bool declanched) : base(id, target, caster, effect, spell, false, dispelable, customActionId, declanched)
        {
            this.State = state;
        }
        public override void Apply(object token = null)
        {
        }
        public override void Dispell()
        {
        }
        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            return new FightTemporaryBoostStateEffect((uint)base.Id, base.Target.Id, base.Duration, 1, (uint)base.Spell.Spell.id, Effect.Uid, 0, 1, (short)State.id);
        }
    }
}
