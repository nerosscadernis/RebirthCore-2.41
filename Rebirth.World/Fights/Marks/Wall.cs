using Rebirth.Common.Protocol.Data;
using Rebirth.Common.Protocol.Enums;
using Rebirth.Common.Protocol.Messages;
using Rebirth.Common.Protocol.Types;
using Rebirth.Common.Thread;
using Rebirth.World.Game.Effect;
using Rebirth.World.Effects.Handlers;
using Rebirth.World.Effects.Handlers.Spells;
using Rebirth.World.Game.Effect.Instances;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Game.Fights.IA;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Game.Spells;
using Rebirth.World.Game.Spells.Cast;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rebirth.World.Datas.Spells;
using Rebirth.Common.GameData.D2O;

namespace Rebirth.World.Game.Fights.Marks
{
    public class Wall
    {
        #region Constructor
        public Wall(short Id, SpellLevel spell, Fighter caster, DirectionsEnum dir, Fight fight, int color, SummonBomb bombFirst, SummonBomb bombSecond)
        {
            FirstBomb = bombFirst;
            SecondBomb = bombSecond;
            Dir = dir;
            //Color = 16711680;

            Color = (FirstBomb.SpellBomb.wallId == 2 ? 218 : FirstBomb.SpellBomb.wallId == 3 ? 46 : 55);
            Spell = spell;
            Caster = caster;
            Fight = fight;
            TargetedCell = Fight.Map.Cells[FirstBomb.Point.Point.GetCellInDirection(dir, 1).CellId];
            Size = (byte)(FirstBomb.Point.Point.DistanceToCell(SecondBomb.Point.Point) - 2);
            this.Id = Id;

            var zone = new Zones.Zone(SpellShapeEnum.L, Size, 0, dir);
            Cells = zone.GetCells(TargetedCell, fight.Map);
        }
        #endregion

        #region Var
        public short Id;
        public Fight Fight { get; set; }
        public SpellLevel Spell { get; set; }
        public Fighter Caster { get; set; }
        public CellMap[] Cells { get; set; }
        public CellMap TargetedCell { get; set; }
        public MapPoint DeclanchedCell { get; set; }
        public SpellEffectHandler[] Handlers { get; set; }
        public SpellCastHandler SpellHandler { get; set; }
        public bool IsVisible { get; set; }
        public DirectionsEnum Dir { get; set; }

        public SummonBomb FirstBomb { get; set; }
        public SummonBomb SecondBomb { get; set; }

        public byte Size { get; set; }
        public SpellShapeEnum Shape { get; set; }
        public int Color { get; set; }
        public double Boost
        {
            get
            {
                double myBoost = 0;
                foreach (var item in Fight.MarkManager.GetBombLier(this))
                {
                    myBoost += item.Stats[PlayerFields.BombBonus].Total;
                }
                return myBoost;
            }
        }
        #endregion    

        #region Public Func
        public bool IsValid()
        {
            List<MapPoint> cells = (from x in FirstBomb.Point.Point.GetCellsInLine(SecondBomb.Point.Point)
                                    where (Fight.GetFighter(x) != null && Fight.GetFighter(x).IsAlive && Fight.GetFighter(x) is SummonBomb 
                                    && (Fight.GetFighter(x) as SummonBomb).Summoner == FirstBomb.Summoner
                                    && (Fight.GetFighter(x) as SummonBomb).SpellBomb.wallId == FirstBomb.SpellBomb.wallId)
                                    select x).ToList();
            if (cells.Count - 2 > 0)
            {
                return false;
            }
            var dist = FirstBomb.Point.Point.DistanceToCell(SecondBomb.Point.Point);
            if (dist - 2 != Size)
                RefreshZone();
            return FirstBomb.IsAlive && SecondBomb.IsAlive 
                && dist < 8 
                && dist > 1
                && FirstBomb.Point.Point.IsLine(SecondBomb.Point.Point);
        }

        public void RefreshZone()
        {
            TargetedCell = Fight.Map.Cells[FirstBomb.Point.Point.GetCellInDirection(Dir, 1).CellId];
            var newSize = (byte)(FirstBomb.Point.Point.DistanceToCell(SecondBomb.Point.Point) - 2);
            if(newSize >= 0 && newSize < 7)
            {
                var zone = new Zones.Zone(SpellShapeEnum.L, newSize, 0, Dir);
                var newCells = zone.GetCells(TargetedCell, Fight.Map);
                if(newSize < Size)
                    foreach (var item in Cells.Except(newCells))
                    {
                        Fight.UnmarkCell(Caster, new GameActionMarkedCell((uint)(item.Id), 0, Color, (sbyte)GameActionMarkCellsTypeEnum.CELLS_CIRCLE));
                    }
                else
                    foreach (var item in newCells)
                    {
                        Fight.GlyphAdd(Caster,
                                    new GameActionMark(Caster.Id, (sbyte)Caster.Team.Team, 2825, 1, (short)(item.Id + Id), (sbyte)GameActionMarkTypeEnum.WALL, (short)item.Id,
                                    new GameActionMarkedCell[]
                                    { new GameActionMarkedCell((uint)item.Id, 0, Color, (sbyte)GameActionMarkCellsTypeEnum.CELLS_CIRCLE) }, true));
                    }

                Cells = newCells;
                Size = newSize;
            }
            else
                foreach (var item in Cells)
                {
                    Fight.UnmarkCell(Caster, new GameActionMarkedCell((uint)(item.Id), 0, Color, (sbyte)GameActionMarkCellsTypeEnum.CELLS_CIRCLE));
                }
        }

        public List<Fighter> Fighters = new List<Fighter>();

        public void Declanche(Fighter fighter)
        {
            if (fighter is SummonBomb && (fighter as SummonBomb).Summoner.Id == FirstBomb.Summoner.Id
                && (fighter as SummonBomb).SpellBomb.wallId == FirstBomb.SpellBomb.wallId)
                return;
            else
            {
                SpellCastHandler spellCastHandler = SpellManager.Instance.GetSpellCastHandler(Caster,
                    new SpellTemplate(ObjectDataManager.Get<Spell>(Spell.spellId), (byte)Spell.grade, ObjectDataManager.Get<SpellLevel>(ObjectDataManager.Get<Spell>((int)Spell.spellId).spellLevels[(int)Spell.grade])) { IsGlyphSpell = true }
                    , TargetedCell, false);
                spellCastHandler.BoostCase = (Boost / 100d) + 1d;
                spellCastHandler.Initialize();
                foreach (var item in spellCastHandler.GetEffectHandlers())
                {
                    item.TrySetAffectedActor(fighter);
                }
                spellCastHandler.Execute(this);
            }
        }

        public void Declanche()
        {
            SpellCastHandler spellCastHandler = SpellManager.Instance.GetSpellCastHandler(Caster,
                    new SpellTemplate(ObjectDataManager.Get<Spell>(Spell.spellId), (byte)Spell.grade,
                    ObjectDataManager.Get<SpellLevel>(ObjectDataManager.Get<Spell>((int)Spell.spellId).spellLevels[(int)Spell.grade]))
                    { IsGlyphSpell = true }
                    , TargetedCell, false);
            spellCastHandler.BoostCase = (Boost / 100d) + 1d;
            spellCastHandler.Initialize();
            foreach (var fighter in (from x in Cells
                                     where Fight.GetFighter(x) != null
                                     select Fight.GetFighter(x)))
            {
                if (!(fighter is SummonBomb) || (fighter is SummonBomb && (fighter as SummonBomb).Summoner.Id != FirstBomb.Summoner.Id
                && (fighter as SummonBomb).SpellBomb.wallId != FirstBomb.SpellBomb.wallId))
                {
                    foreach (var item in spellCastHandler.GetEffectHandlers())
                    {
                        item.TrySetAffectedActor(fighter);
                    }
                }
            }
            spellCastHandler.Execute(this);
        }

        public void Execute(Fighter fighter)
        {
            if (!Fighters.Any(x => x.Id == fighter.Id))
            {
                if (fighter is SummonBomb && (fighter as SummonBomb).Summoner.Id == FirstBomb.Summoner.Id
                     && (fighter as SummonBomb).SpellBomb.wallId == FirstBomb.SpellBomb.wallId)
                    return;
                else
                {
                    SpellCastHandler spellCastHandler = SpellManager.Instance.GetSpellCastHandler(Caster, new SpellTemplate(ObjectDataManager.Get<Spell>(Spell.spellId), (byte)Spell.grade, ObjectDataManager.Get<SpellLevel>(ObjectDataManager.Get<Spell>((int)Spell.spellId).spellLevels[(int)Spell.grade])) { IsGlyphSpell = true }
                    , TargetedCell, false);
                    spellCastHandler.BoostCase = (Boost / 100d) + 1d;
                    spellCastHandler.Initialize();
                    foreach (var item in spellCastHandler.GetEffectHandlers())
                    {
                        item.TrySetAffectedActor(fighter);
                    }
                    spellCastHandler.Execute(this);
                    if (!Fighters.Any(x => x.Id == fighter.Id))
                        Fighters.Add(fighter);
                }
            }
        }

        public void Execute()
        {
            DefaultSpellCastHandler spellCastHandler = SpellManager.Instance.GetSpellCastHandler(Caster,
                                    new SpellTemplate(ObjectDataManager.Get<Spell>(Spell.spellId), (byte)Spell.grade, ObjectDataManager.Get<SpellLevel>(ObjectDataManager.Get<Spell>((int)Spell.spellId).spellLevels[(int)Spell.grade])) { IsGlyphSpell = true }
                , Fight.Map.Cells[0], false) as DefaultSpellCastHandler;
            spellCastHandler.BoostCase = (Boost / 100d) + 1d;
            spellCastHandler.Initialize(false);

            foreach (var fighter in Fight.GetAllFighter(Cells))
            {
                if (!Fighters.Any(x => x.Id == fighter.Id) && 
                    (!(fighter is SummonBomb) || (fighter is SummonBomb && (fighter as SummonBomb).Summoner.Id != FirstBomb.Summoner.Id
                && (fighter as SummonBomb).SpellBomb.wallId != FirstBomb.SpellBomb.wallId)))
                {
                    foreach (var item in spellCastHandler.GetEffectHandlers())
                    {
                        item.TrySetAffectedActor(fighter);
                        if(!Fighters.Any(x => x.Id == fighter.Id))
                            Fighters.Add(fighter);
                    }
                }
            }
            spellCastHandler.Execute(this);
        }

        public GameActionMark GetGameActionMark()
        {
            return new GameActionMark(Caster.Id, (sbyte)Caster.Team.Team, 2825, 1, Id, (sbyte)GameActionMarkTypeEnum.WALL, (short)TargetedCell.Id,
                (from x in Cells
                 select new GameActionMarkedCell((uint)x.Id, 0, Color, (sbyte)GameActionMarkCellsTypeEnum.CELLS_CIRCLE)).ToArray(), true);
        }
        #endregion

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
