using Rebirth.Common.Protocol.Types;
using Rebirth.World.Datas.Spells;
using Rebirth.World.Game.Effect.Instances;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Game.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Fights.Buffs
{
    public class ControlBuff : Buff
    {
        public int TemplateId
        {
            get;
            set;
        }
        public ControlBuff(int id, Fighter target, Fighter caster, EffectBase effect, SpellTemplate spell, bool critical, bool dispelable, int templateId, bool declanched) : base(id, target, caster, effect, spell, critical, dispelable, declanched)
        {
            TemplateId = templateId;
        }
        public ControlBuff(int id, Fighter target, Fighter caster, EffectBase effect, SpellTemplate spell, bool critical, bool dispelable, short customActionId, int templateId, bool declanched) : base(id, target, caster, effect, spell, critical, dispelable, customActionId, declanched)
        {
            TemplateId = templateId;
        }
        public override void Apply(object token = null)
        {
        }
        public override void Dispell()
        {
        }
        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            return new FightTemporaryBoostEffect((uint)Id, Target.Id, Duration, Convert.ToSByte(Dispellable ? 0 : 1), (uint)Spell.Spell.id, (uint)Effect.Id, 0, 0);
        }
    }
}
