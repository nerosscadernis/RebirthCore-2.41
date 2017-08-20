using Rebirth.Common.Protocol.Enums;
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
    public class SpellBoostBuff : Buff
    {
        public short Value
        {
            get;
            set;
        }
        public short SpellId
        {
            get;
            set;
        }
        public CharacterSpellModificationTypeEnum TypeBoost
        {
            get;
            set;
        }
        public SpellBoostBuff(int id, Fighter target, Fighter caster, EffectBase effect, SpellTemplate spell, short value, short spellId, bool critical, bool dispelable, bool declanched, CharacterSpellModificationTypeEnum type) : base(id, target, caster, effect, spell, critical, dispelable, declanched)
        {
            this.Value = value;
            this.SpellId = spellId;
            TypeBoost = type;
            //base.Duration += (short)Spell.CurrentSpellLevel.minCastInterval;
        }
        public SpellBoostBuff(int id, Fighter target, Fighter caster, EffectBase effect, SpellTemplate spell, short value, short spellId, bool critical, bool dispelable, short customActionId, bool declanched, CharacterSpellModificationTypeEnum type) : base(id, target, caster, effect, spell, critical, dispelable, customActionId, declanched)
        {
            this.Value = value;
            this.SpellId = spellId;
            TypeBoost = type;
            //base.Duration += (short)Spell.CurrentSpellLevel.minCastInterval;
        }
        public override void Apply(object token = null)
        {
            Target.Stats.SpellBoosts.UpdateBoost(SpellId, TypeBoost, Value);
            if (Target is CharacterFighter)
                (Target as CharacterFighter).Character.RefreshStats();
        }
        public override void Dispell()
        {
            Target.Stats.SpellBoosts.RemoveBoost(SpellId, TypeBoost, Value);
            if (Target is CharacterFighter)
                (Target as CharacterFighter).Character.RefreshStats();
        }
        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            var datas = new FightTemporarySpellBoostEffect((uint)Id, Target.Id, Duration, Convert.ToSByte(Dispellable ? 0 : 1), (uint)Spell.Spell.id, Effect.Uid, 0, (short)Target.Stats.SpellBoosts.GetSpellBoost((int)SpellId, TypeBoost), (uint)SpellId);
            return datas;
        }
    }
}
