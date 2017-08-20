using Rebirth.Common.Protocol.Data;
using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Game.Characters.Stats;
using Rebirth.World.Game.Fights.Other;
using Rebirth.World.Game.Fights.Team;
using Rebirth.World.Datas.Monsters;
using Rebirth.World.Game.Spells;
using Rebirth.World.Game.Spells.Cast;
using Rebirth.Common.GameData.D2O;
using Rebirth.World.Datas.Spells;

namespace Rebirth.World.Game.Fights.Actors
{
    public class SummonBomb : SummonMonster, IStatsOwner
    {
        #region Constructeur
        public SummonBomb(MonsterTemplate template, FightTeam team, Fight fight, int id, Fighter caster, FightDisposition point, SpellLevel spelld, SpellBomb spellBomb) : base(template, team, fight, id, caster, point)
        {
            SpellBomb = spellBomb;
            SpellLevel = spelld;
            var spell = ObjectDataManager.Get<Spell>(2928);
            if (spell != null)
            {
                var spellLevel = ObjectDataManager.Get<SpellLevel>((int)spell.spellLevels[0]);
                if (spellLevel != null)
                {
                    var spellTemplate = new SpellTemplate(spell, 0, spellLevel);
                    SpellCastHandler spellCastHandler = SpellManager.Instance.GetSpellCastHandler(this, spellTemplate, Fight.Map.Cells[this.Point.Point.CellId], false);
                    spellCastHandler.Initialize();
                    spellCastHandler.Execute(null);
                }
            }
            SpellChain = ObjectDataManager.Get<SpellLevel>((int)ObjectDataManager.Get<Spell>(SpellBomb.chainReactionSpellId + 1).spellLevels[(int)SpellLevel.grade - 1]);
        }
        #endregion

        public SpellLevel SpellLevel
        { get; set; }

        public SpellBomb SpellBomb
        { get; set; }

        public SpellLevel SpellChain
        { get; set; }

        public bool DieByExplo
        { get; set; }

        public bool RoundDie
        { get; set; }

        public bool IsActive
        { get; set; }

        public void Execute()
        {
            if (!IsActive)
            {
                IsActive = true;
                Summoner.Stats[PlayerFields.BombBonus].Context += Stats[PlayerFields.BombBonus].Total;
                var Bombes = Summoner.Summons.FindAll(x => x is SummonBomb && x.Point.Point.DistanceToCell(Point.Point) < 3 && !(x as SummonBomb).IsActive);
                var spell = ObjectDataManager.Get<Spell>(SpellBomb.chainReactionSpellId);
                if (spell != null)
                {
                    var spellLevel = ObjectDataManager.Get<SpellLevel>((int)spell.spellLevels[(int)SpellLevel.grade - 1]);
                    if (spellLevel != null)
                    {
                        var spellTemplate = new SpellTemplate( spell, 0, spellLevel);
                        SpellCastHandler spellCastHandler = SpellManager.Instance.GetSpellCastHandler(Summoner, spellTemplate, Fight.Map.Cells[this.Point.Point.CellId], false);
                        spellCastHandler.Initialize();
                        spellCastHandler.Execute(null);
                        Fight.CastedSpell(this, spellCastHandler, spellCastHandler.TargetedCell, FightSpellCastCriticalEnum.NORMAL, false);
                    }
                }
                Stats[PlayerFields.BombBonus].Context = Summoner.Stats[PlayerFields.BombBonus].Total;
                spell = ObjectDataManager.Get<Spell>(SpellBomb.explodSpellId);
                if (spell != null)
                {
                    var spellLevel = ObjectDataManager.Get<SpellLevel>((int)spell.spellLevels[(int)SpellLevel.grade - 1]);
                    if (spellLevel != null)
                    {
                        var spellTemplate = new SpellTemplate(spell, 0, spellLevel);
                        SpellCastHandler spellCastHandler = SpellManager.Instance.GetSpellCastHandler(this, spellTemplate, Fight.Map.Cells[this.Point.Point.CellId], false);
                        spellCastHandler.Initialize();
                        spellCastHandler.Execute(null);
                        Fight.CastedSpell(this, spellCastHandler, spellCastHandler.TargetedCell, FightSpellCastCriticalEnum.NORMAL, false);
                    }
                }
                Summoner.ResetBombBonus();
            }
        }

        public override void Die(Fighter by)
        {
            base.Die(by);
            Fight.MarkManager.UpdateWall();
        }

        public override void AdjustStats()
        {
            int baseVita = (short)(Summoner.Stats.Health.TotalMax / 5);
            Stats = Summoner.Stats.CloneAndChangeOwner(this);
            Stats.Health.DamageTaken = 0;
            Stats.Health.Context = 0;
            Stats.Health.PermanentDamages = 0;
            Stats.Health.Base = Template.Stats.Health.Base;
            Stats.Vitality.Base = baseVita;
            Stats.Vitality.Given = 0;
            Stats.Vitality.Context = 0;
            Stats.Vitality.Equiped = 0;
            Stats.Vitality.Additionnal = 0;
        }
    }
}
