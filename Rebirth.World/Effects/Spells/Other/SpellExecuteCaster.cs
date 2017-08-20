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
using Rebirth.World.Datas.Spells;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rebirth.Common.Protocol.Data;
using Rebirth.Common.GameData.D2O;

namespace Rebirth.World.Game.Effect.Spells.Other
{
    [EffectHandler(EffectsEnum.Effect_SpellExecute_Caster), EffectHandler(EffectsEnum.Effect_SpellExecute1)]
    public class SpellExecuteCaster : SpellEffectHandler
    {
        public SpellExecuteCaster(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply(object token, bool declanched = false)
        {
            if (Effect is EffectDice)
            {
                var test = GetAffectedActors();
                foreach (Fighter fighter in test)
                {
                    var spell = ObjectDataManager.Get<Spell>(Dice.DiceNum);
                    if (spell != null)
                    {
                        var template = new SpellTemplate(spell, (byte)(Dice.DiceFace - 1));
                        if (Effect.Duration > 0 && Effect.Duration < 0)
                        {
                            var trigger = new TriggerBuff(fighter.PopNextBuffId(), fighter, Caster, Effect, Spell, template, 0, Critical, true, TriggerBuffType.BEFORE_ATTACKED, declanched) { InTarget = true };
                            AddTriggerBuff(fighter, trigger);
                        }
                        else
                        {
                            var spellCast = SpellManager.Instance.GetSpellCastHandler(fighter, template, Fight.Map.Cells[Caster.Point.Point.CellId], false);
                            spellCast.Initialize();
                            spellCast.Execute(null);
                            Fight.CastedSpell(fighter, spellCast, TargetedCell, FightSpellCastCriticalEnum.NORMAL, true);
                        }
                    }
                }
                return true;
            }
            return false;
        }
    }
}
