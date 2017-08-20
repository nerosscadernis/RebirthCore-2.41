using Rebirth.World.Game.Characters.Stats;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Game.Fights.Buffs;
using Rebirth.World.Game.Fights.Other;
using Rebirth.World.Game.Fights.Team;
using Rebirth.World.Datas.Monsters;
using Rebirth.World.Game.Spells;
using Rebirth.World.Game.Spells.Cast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rebirth.Common.Protocol.Data;
using Rebirth.Common.GameData.D2O;
using Rebirth.World.Datas.Spells;

namespace Rebirth.World.Game.Fights.Actors
{
    public class SummonSynchro : SummonMonster, IStatsOwner
    {
        #region Constructeur
        public SummonSynchro(MonsterTemplate template, FightTeam team, Fight fight, int id, Fighter caster, FightDisposition point) : base(template, team, fight, id, caster, point)
        {
            switch (Level)
            {
                case 6:
                    Stats[Common.Protocol.Enums.PlayerFields.DamageBonus].Base = 332;
                    break;
                case 5:
                    Stats[Common.Protocol.Enums.PlayerFields.DamageBonus].Base = 287;
                    break;
                case 4:
                    Stats[Common.Protocol.Enums.PlayerFields.DamageBonus].Base = 256;
                    break;
                case 3:
                    Stats[Common.Protocol.Enums.PlayerFields.DamageBonus].Base = 218;
                    break;
                case 2:
                    Stats[Common.Protocol.Enums.PlayerFields.DamageBonus].Base = 174;
                    break;
                case 1:
                    Stats[Common.Protocol.Enums.PlayerFields.DamageBonus].Base = 141;
                    break;
                default:
                    break;
            }
        }
        #endregion

        public override bool AddAndApplyBuff(Buff buff, bool freeIdIfFail = true)
        {
            var response = base.AddAndApplyBuff(buff, freeIdIfFail);
            if (HasState(244))
            {
                var spell = ObjectDataManager.Get<Spell>(5435);
                if (spell != null)
                {
                    var spellLevel = ObjectDataManager.Get<SpellLevel>((int)spell.spellLevels[Level - 1]);
                    if (spellLevel != null)
                    {
                        var spellTemplate = new SpellTemplate(spell, 0, spellLevel);
                        SpellCastHandler spellCastHandler = SpellManager.Instance.GetSpellCastHandler(this, spellTemplate, Fight.Map.Cells[this.Point.Point.CellId], false);
                        spellCastHandler.Initialize();
                        spellCastHandler.Execute(null);
                    }
                }
            }
            return response;
        }

        public override void Play()
        {
            var spell = ObjectDataManager.Get<Spell>(5434);
            var spellLevel = ObjectDataManager.Get<SpellLevel>((int)spell.spellLevels[Level - 1]);
            Summoner.AddAndApplyBuff(new StatBuff(Summoner.PopNextBuffId(), Summoner, this, new Effect.Instances.EffectBase() { Duration = 1 },
                        new SpellTemplate(spell, 0, spellLevel), -1, Common.Protocol.Enums.PlayerFields.AP, false, false, false));
            ActionPass();
        }

    }
}
