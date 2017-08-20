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
using Rebirth.World.Game.Spells.Cast;
using Rebirth.World.Datas.Spells;

namespace Rebirth.World.Game.Fights.Buffs
{
    public class TriggerBuff : Buff
    {
        public Action<TriggerBuff, object> Applyed;

        #region Properties
        public short Value
        {
            get;
            set;
        }
        public TriggerBuffType TriggerType
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
        public uint RoundLaunched
        {
            get;
            set;
        }
        public  short[] Args
        {
            get;
            set;
        }
        public bool InTarget
        {
            get;
            set;
        }
        #endregion       

        public TriggerBuff(int id, Fighter target, Fighter caster, EffectBase effect, SpellTemplate spell, SpellTemplate customSpell, short value, bool critical, bool dispelable, TriggerBuffType triggerType, bool declanched) : base(id, target, caster, effect, spell, critical, dispelable, declanched)
        {
            this.Value = value;
            TriggerType = triggerType;
            this.CustomSpell = customSpell;
            RoundLaunched = Target.Fight.TimeLine.ActualRound;
            Args = new short[0];
        }
        public TriggerBuff(int id, Fighter target, Fighter caster, EffectBase effect, SpellTemplate spell, SpellTemplate customSpell, short value, bool critical, bool dispelable, TriggerBuffType triggerType, short[] args, bool declanched) : base(id, target, caster, effect, spell, critical, dispelable, declanched)
        {
            this.Value = value;
            TriggerType = triggerType;
            this.CustomSpell = customSpell;
            RoundLaunched = Target.Fight.TimeLine.ActualRound;
            Args = args;
        }


        public override void Apply(object token = null)
        {
            if (CustomSpell != null)
            {
                var cell = Caster.Fight.Map.Cells[InTarget ? Target.Point.Point.CellId : Caster.Point.Point.CellId];
                SpellCastHandler spell = SpellManager.Instance.GetSpellCastHandler(InTarget ? Target : Caster, CustomSpell, cell, false);
                spell.Initialize();
                if (spell != null)
                {
                    Target.Fight.CastedSpell(Target, spell, cell, FightSpellCastCriticalEnum.NORMAL, false);
                    spell.Execute(token);
                }                  
            }
            else
                Applyed(this, token);
        }

        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            if (Args.Count() > 0)
            {
                return new FightTriggeredEffect((uint)Id, Target.Id, (short)Duration, (sbyte)(Dispelable ? 1 : 0), (uint)Spell.Spell.id, (uint)Effect.Uid, 0, Args[0], Args[1], Args[2], (short)Effect.Delay);
            }
            else
            {
                object[] values = Effect.GetValues();
                return new FightTriggeredEffect((uint)Id, Target.Id, (short)Duration, (sbyte)(Dispelable ? 1 : 0), (uint)Spell.Spell.id, Effect.Uid, 0, short.Parse(values[0].ToString()), short.Parse(values[1].ToString()), short.Parse(values[2].ToString()), (short)Effect.Delay);
            }
        }

        public override void Dispell()
        {     
        }
    }
}
