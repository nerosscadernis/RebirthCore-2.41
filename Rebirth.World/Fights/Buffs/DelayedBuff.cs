using Rebirth.Common.Protocol.Types;
using Rebirth.World.Datas.Spells;
using Rebirth.World.Effects.Handlers.Spells;
using Rebirth.World.Game.Effect.Instances;
using Rebirth.World.Game.Fights.Actors;

namespace Rebirth.World.Game.Fights.Buffs
{
    public class DelayedBuff : Buff
    {
        #region Properties
        public short Value
        {
            get;
            set;
        }
        public SpellTemplate CustomSpell
        {
            get;
            private set;
        }
        public bool Dispelable
        {
            get;
            set;
        }
        public SpellEffectHandler EffectHandler
        {
            get;
            set;
        }
        public bool IsTurnEnd
        {
            get;
            set;
        }
        #endregion       

        public DelayedBuff(int id, Fighter target, Fighter caster, EffectBase effect, SpellTemplate spell, SpellTemplate customSpell, short value, bool critical, bool dispelable, SpellEffectHandler effectHandler, bool declanched) : base(id, target, caster, effect, spell, critical, dispelable, declanched)
        {
            this.Value = value;
            this.CustomSpell = customSpell;
            EffectHandler = effectHandler;
        }

        public override void Apply(object token = null)
        {
        }

        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            object[] values = Effect.GetValues();
            return new FightTriggeredEffect((uint)Id, Target.Id, (short)Duration, (sbyte)(Dispelable ? 1 : 0), (uint)Spell.Spell.id, (uint)Effect.EffectId, 0, short.Parse(values[0].ToString()), short.Parse(values[1].ToString()), short.Parse(values[2].ToString()), (short)Effect.Delay);
        }

        public override void Dispell()
        {
            if (!Caster.Fight.IsEnded)
            {
                EffectHandler.TargetedPoint = Target.Point.Point;
                EffectHandler.TargetedCell = Target.Fight.Map.Cells[Target.Point.Point.CellId];
                EffectHandler.Apply(this, true);
            }
        }
    }
}
