using Rebirth.Common.Protocol.Enums;
using Rebirth.Common.Protocol.Types;
using Rebirth.World.Effects.Handlers.Spells;
using Rebirth.World.Game.Effect.Instances;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Game.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rebirth.World.Datas.Spells;
using Rebirth.World.Datas.Characters;

namespace Rebirth.World.Game.Fights.Buffs
{
    public class SkinBuff : Buff
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
        public EntityLook CustomLook
        {
            get;
            set;
        }
        #endregion       

        public SkinBuff(int id, Fighter target, Fighter caster, EffectBase effect, SpellTemplate spell, SpellTemplate customSpell, short value, bool critical, bool dispelable, Look customLook, bool declanched) : base(id, target, caster, effect, spell, critical, dispelable, declanched)
        {
            this.Value = value;
            this.CustomSpell = customSpell;
            CustomLook = customLook.GetEntityLook();
        }

        public override void Apply(object token = null)
        {
            Target.Fight.ModifyLook(Caster, Target, CustomLook);
        }

        public override void Dispell()
        {
            Target.Fight.ModifyLook(Caster, Target, Target.GetCustomLook());
        }

        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            object[] values = Effect.GetValues();
            return new FightTemporaryBoostEffect((uint)Id, Target.Id, (short)Duration, (sbyte)(Dispelable ? 1 : 0), (uint)Spell.Spell.id, (uint)Effect.EffectId, 0, 0);
        }
    }
}
