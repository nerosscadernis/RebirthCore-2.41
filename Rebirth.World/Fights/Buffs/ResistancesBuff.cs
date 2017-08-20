using Rebirth.Common.Protocol.Enums;
using Rebirth.Common.Protocol.Types;
using Rebirth.World.Game.Effect.Instances;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Game.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rebirth.World.Datas.Spells;

namespace Rebirth.World.Game.Fights.Buffs
{
    public class ResistancesBuff : Buff
    {
        public short Value
        {
            get;
            private set;
        }
        public ResistancesBuff(int id, Fighter target, Fighter caster, EffectBase effect, SpellTemplate spell, short value, bool critical, bool dispelable, bool declanched) : base(id, target, caster, effect, spell, critical, dispelable, declanched)
        {
            this.Value = value;
        }
        public override void Apply(object token)
        {
            base.Target.Stats[PlayerFields.AirResistPercent].Context += (int)this.Value;
            base.Target.Stats[PlayerFields.FireResistPercent].Context += (int)this.Value;
            base.Target.Stats[PlayerFields.EarthResistPercent].Context += (int)this.Value;
            base.Target.Stats[PlayerFields.NeutralResistPercent].Context += (int)this.Value;
            base.Target.Stats[PlayerFields.WaterResistPercent].Context += (int)this.Value;
        }
        public override void Dispell()
        {
            base.Target.Stats[PlayerFields.AirResistPercent].Context -= (int)this.Value;
            base.Target.Stats[PlayerFields.FireResistPercent].Context -= (int)this.Value;
            base.Target.Stats[PlayerFields.EarthResistPercent].Context -= (int)this.Value;
            base.Target.Stats[PlayerFields.NeutralResistPercent].Context -= (int)this.Value;
            base.Target.Stats[PlayerFields.WaterResistPercent].Context -= (int)this.Value;
        }
        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            return new FightTemporaryBoostEffect((uint)base.Id, base.Target.Id, base.Duration, Convert.ToSByte(base.Dispellable ? 0 : 1), (uint)base.Spell.Spell.id, (uint)Effect.Uid, 0, (short)(Value < 0 ? Value * -1 : Value));
        }
    }
}
