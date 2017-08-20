using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Effects.Handlers;
using Rebirth.World.Effects.Handlers.Spells;
using Rebirth.World.Game.Effect.Instances;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Game.Fights.Marks;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Datas.Monsters;
using Rebirth.World.Game.Spells;
using Rebirth.World.Game.Spells.Cast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rebirth.World.Datas.Spells;
using System.Threading.Tasks;
using Rebirth.World.Managers;
using Rebirth.Common.GameData.D2O;
using Rebirth.Common.Protocol.Data;

namespace Rebirth.World.Game.Effect.Spells.Trigger
{
    [EffectHandler(EffectsEnum.Effect_Bomb)]
    public class BombEffect : SpellEffectHandler
    {
        public BombEffect(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }
        public override bool Apply(object token, bool declanched = false)
        {
            var monster = ObjectDataManager.Get<Monster>(Dice.DiceNum);
            if (monster != null && Caster.CountBomb() < 3)
            {
                var bombSpell = ObjectDataManager.Get<SpellBomb>(Dice.DiceNum);
                var fighter = Fight.GetFighter(TargetedCell);
                if (fighter != null)
                {
                    var spell = ObjectDataManager.Get<Spell>(bombSpell.instantSpellId);
                    if (spell != null)
                    {
                        var spellLevel = ObjectDataManager.Get<SpellLevel>((int)spell.spellLevels[(int)Spell.CurrentSpellLevel.grade - 1]);
                        if (spellLevel != null)
                        {
                            var spellTemplate = new SpellTemplate(spell, 0, spellLevel);
                            SpellCastHandler spellCastHandler = SpellManager.Instance.GetSpellCastHandler(Caster, spellTemplate, TargetedCell, false);
                            spellCastHandler.Initialize();
                            spellCastHandler.Execute(null);
                            Fight.CastedSpell(Caster, spellCastHandler, TargetedCell,  FightSpellCastCriticalEnum.NORMAL, false);
                        }
                    }
                }
                else
                {
                    MonsterTemplate monsterTemplate = new MonsterTemplate((byte)(Spell.CurrentSpellLevel.grade - 1), monster);
                    Caster.AddStaticSummon(new SummonBomb(monsterTemplate, Caster.Team, Fight, Fight.GetNextContextualId(), Caster, new Fights.Other.FightDisposition(TargetedCell.Id, Caster.Point.Point.OrientationTo(TargetedPoint, false)), Spell.CurrentSpellLevel, bombSpell));
                    Fight.MarkManager.UpdateWall();
                }
                return true;
            }
            return true;
        }
    }
}
