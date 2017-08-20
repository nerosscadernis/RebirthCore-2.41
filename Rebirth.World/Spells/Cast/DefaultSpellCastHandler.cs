using Rebirth.Common.Protocol.Enums;
using Rebirth.Common.Thread;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Datas.Spells;
using Rebirth.World.Effects.Handlers.Spells;
using Rebirth.World.Game.Effect.Instances;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Game.Fights.Buffs;
using Rebirth.World.Managers;
using System.Collections.Generic;
using System.Linq;

namespace Rebirth.World.Game.Spells.Cast
{
    [DefaultSpellCastHandler]
    public class DefaultSpellCastHandler : SpellCastHandler
    {
        protected bool m_initialized;
        public SpellEffectHandler[] Handlers
        {
            get;
            set;
        }
        public override bool SilentCast
        {
            get
            {
                bool arg_33_0;
                if (this.m_initialized)
                {
                    arg_33_0 = this.Handlers.Any((SpellEffectHandler entry) => entry.RequireSilentCast());
                }
                else
                {
                    arg_33_0 = false;
                }
                return arg_33_0;
            }
        }
        public DefaultSpellCastHandler(Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(caster, spell, targetedCell, critical)
        {
        }
        public override void Initialize(bool affectTargets)
        {
            AsyncRandom asyncRandom = new AsyncRandom();
            List<EffectBase> effects = new List<EffectBase>();
            foreach (var effect in base.Critical ? base.CurrentSpellLevel.criticalEffect : base.CurrentSpellLevel.effects)
            {
                var newEffect = EffectManager.Instance.ConvertExportedEffect(effect);
                effects.Add(newEffect);
            }

            System.Collections.Generic.List<EffectDice> list = effects.Cast<EffectDice>().ToList<EffectDice>();
            System.Collections.Generic.List<SpellEffectHandler> list2 = new System.Collections.Generic.List<SpellEffectHandler>();
            double num = asyncRandom.NextDouble();
            double num2 = (double)list.Sum((EffectDice entry) => entry.Random);
            bool flag = false;
            foreach (EffectDice current in list)
            {
                if (current.Random > 0 && Spell.Spell.id != 114)
                {
                    if (flag)
                    {
                        continue;
                    }
                    if (num > (double)current.Random / num2)
                    {
                        num -= (double)current.Random / num2;
                        continue;
                    }
                    flag = true;
                }
                SpellEffectHandler spellEffectHandler = EffectManager.Instance.GetSpellEffectHandler(current, base.Caster, base.Spell, base.TargetedCell, base.Critical);
                spellEffectHandler.Boost = BoostCase;
                if (affectTargets && !spellEffectHandler.Effect.Targets.HasFlag(SpellTargetType.TELEPORT_FIGHTER))
                    spellEffectHandler.SetAffectedActors();
                list2.Add(spellEffectHandler);
            }
            this.Handlers = list2.ToArray();
            this.m_initialized = true;
        }
        public override void Execute(object token)
        {
            if (!this.m_initialized)
            {
                this.Initialize(true);
            }
            for (int i = 0; i < Handlers.Length; i++)
            {
                SpellEffectHandler spellEffectHandler = Handlers[i];
                if (spellEffectHandler.Dice.Delay > 0)
                {
                    foreach (var item in spellEffectHandler.GetAffectedActors())
                    {
                        var buff = new DelayedBuff(item.PopNextBuffId(), item, Caster, spellEffectHandler.Effect, Spell, null, (short)spellEffectHandler.Dice.Value, Critical, false, spellEffectHandler, false);
                        item.AddBuff(buff);
                    }
                }
                else if (spellEffectHandler.Dice.TargetMask.Contains("U"))
                {
                    var fighter = Fight.GetFighter(TargetedCell);
                    if (fighter != null)
                        fighter.EffectsDie.Add(spellEffectHandler);
                }
                else
                    spellEffectHandler.Apply(token);
            }
        }
        public override System.Collections.Generic.IEnumerable<SpellEffectHandler> GetEffectHandlers()
        {
            return this.Handlers;
        }
    }
}
