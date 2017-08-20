using Rebirth.Common.Protocol.Data;
using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Game.Effect;
using Rebirth.World.Effects.Handlers.Spells;
using Rebirth.World.Game.Effect.Instances;
using Rebirth.World.Game.Effect.Spells.Buffs;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Game.Fights.Buffs;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Game.Spells.Cast;
using Rebirth.World.Datas.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rebirth.World.Managers;
using Rebirth.World.Datas.Monsters;
using Rebirth.Common.GameData.D2O;

namespace Rebirth.World.Game.Spells.Custom.Enutrof
{
    [SpellCastHandler(41)]
    public class SacHandler : DefaultSpellCastHandler
    {
        public SacHandler(Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(caster, spell, targetedCell, critical)
        {
        }

        public object TemporaryBoostStateEffect { get; private set; }

        public override void Execute(object token)
        {
            if (!this.m_initialized)
            {
                this.Initialize(true);
            }

            SummonMonster summon;
            var monster = ObjectDataManager.Get<Monster>(base.Handlers[1].Dice.DiceNum);
            if (monster != null)
            {
                MonsterTemplate monsterTemplate = new MonsterTemplate((byte)(Spell.CurrentSpellLevel.grade - 1), monster);
                summon = new SummonMonster(monsterTemplate, Caster.Team, Fight, Fight.GetNextContextualId(), Caster, new Fights.Other.FightDisposition(TargetedCell.Id, Caster.Point.Point.OrientationTo(new MapPoint(TargetedCell), false)));
                Caster.AddSummon(summon);

                SpellTemplate spell = new SpellTemplate(ObjectDataManager.Get<Spell>(3252), 0);
                var sacrificeEffect = spell.CurrentSpellLevel.effects[0];
                var killEffect = spell.CurrentSpellLevel.effects[1];

                foreach (var target in base.Handlers[0].GetAffectedActors())
                {
                    if (target != summon)
                    {
                        SacrificeBuff buff = new SacrificeBuff(target.PopNextBuffId(), target, summon, new EffectDice(sacrificeEffect), spell, Critical, true, false);
                        target.AddBuff(buff);
                    }
                }

                SpellEffectHandler spellEffectHandler = EffectManager.Instance.GetSpellEffectHandler(new EffectDice(killEffect), summon, base.Spell, base.TargetedCell, base.Critical);
                var killbuff = new DelayedBuff(Caster.PopNextBuffId(), summon, Caster, spellEffectHandler.Effect, Spell, null, (short)killEffect.value, Critical, false, spellEffectHandler, false);
                Caster.AddBuff(killbuff);
            }
        }
    }
}
