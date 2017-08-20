using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Effects.Handlers;
using Rebirth.World.Effects.Handlers.Spells;
using Rebirth.World.Game.Effect.Instances;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Game.Fights.Buffs;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Datas.Monsters;
using Rebirth.World.Game.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rebirth.World.Datas.Spells;
using Rebirth.World.Managers;
using Rebirth.Common.GameData.D2O;
using Rebirth.Common.Protocol.Data;

namespace Rebirth.World.Game.Heal
{
    [EffectHandler(EffectsEnum.Effect_Summon)]
    public class InvokSummon : SpellEffectHandler
    {
        public InvokSummon(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }
        public override bool Apply(object token, bool declanched = false)
        {
            if (Fight.GetFighter(TargetedCell) == null)
            {
                TrySetAffectedActor(Caster);
                if (GetAffectedActors().Count() == 1)
                {
                    var monster = ObjectDataManager.Get<Monster>(Dice.DiceNum);
                    if (monster != null)
                    {
                        MonsterTemplate monsterTemplate = new MonsterTemplate((byte)(Spell.CurrentSpellLevel.grade - 1), monster);
                        if (monsterTemplate.Template.id == 282)
                            Caster.AddStaticSummon(new SummonMonster(monsterTemplate, Caster.Team, Fight, Fight.GetNextContextualId(), Caster, new Fights.Other.FightDisposition(TargetedCell.Id, Caster.Point.Point.OrientationTo(TargetedPoint, false))));
                        else if(monsterTemplate.Template.id == 3958)
                            Caster.AddSummon(new SummonSynchro(monsterTemplate, Caster.Team, Fight, Fight.GetNextContextualId(), Caster, new Fights.Other.FightDisposition(TargetedCell.Id, Caster.Point.Point.OrientationTo(TargetedPoint, false))));
                        else
                            Caster.AddSummon(new SummonMonster(monsterTemplate, Caster.Team, Fight, Fight.GetNextContextualId(), Caster, new Fights.Other.FightDisposition(TargetedCell.Id, Caster.Point.Point.OrientationTo(TargetedPoint, false))));
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
