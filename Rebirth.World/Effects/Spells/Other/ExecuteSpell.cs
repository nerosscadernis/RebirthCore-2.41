using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Effects.Handlers;
using Rebirth.World.Effects.Handlers.Spells;
using Rebirth.World.Game.Effect.Instances;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Game.Fights.Buffs;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Game.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rebirth.World.Datas.Spells;
using Rebirth.Common.Protocol.Data;
using Rebirth.Common.GameData.D2O;

namespace Rebirth.World.Game.Effect.Spells.Other
{
    [EffectHandler(EffectsEnum.Effect_ExecuteSpell)]
    public class ExecuteSpell : SpellEffectHandler
    {
        public ExecuteSpell(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply(object token, bool declanched = false)
        {
            var spell = ObjectDataManager.Get<Spell>(Dice.DiceNum);
            if (spell != null)
            {
                //var test = Dice.Value == 1 ? new List<Fighter> { Caster } : GetAffectedActors();
                foreach (Fighter fighter in GetAffectedActors())
                {
                    if (Dice.Duration > 0 && !declanched)
                    {
                        if (Spell.Spell.id == 5403)
                        {
                            var template = new SpellTemplate(spell, (byte)(Dice.DiceFace - 1));
                            var trigger = new TriggerBuff(fighter.PopNextBuffId(), fighter, Caster, Effect, Spell, template, 0, Critical, true, TriggerBuffType.PORTAL, declanched) { InTarget = false };
                            AddTriggerBuff(fighter, trigger);
                        }
                        else
                        {
                            var buff = new DelayedBuff(fighter.PopNextBuffId(), fighter, Caster, Effect, Spell, null, (short)Dice.Value, Critical, false, this, false)
                            {
                                IsTurnEnd = true,
                            };
                            fighter.AddBuff(buff);
                        }
                    }
                    else
                    {
                        var template = new SpellTemplate(spell, (byte)(Dice.DiceFace - 1));
                        var spellCast = SpellManager.Instance.GetSpellCastHandler(Caster, template, Fight.Map.Cells[fighter.Point.Point.CellId], false);
                        spellCast.Initialize();
                        spellCast.Execute(null);
                        Fight.CastedSpell(Caster, spellCast, Fight.Map.Cells[fighter.Point.Point.CellId], FightSpellCastCriticalEnum.NORMAL, true);
                    }
                }
                return true;
            }
            return false;
        }
    }
}
