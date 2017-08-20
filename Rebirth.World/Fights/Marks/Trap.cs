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
    public class Trap
    {
        #region Constructor
        public Trap(short Id, SpellLevel spell, Fighter caster, DirectionsEnum dir, CellMap TargetedCell, Fight fight, SpellShapeEnum shape, byte size, int color, int spellGlyph)
        {
            SpellGlyph = spellGlyph;
            Color = color;
            Spell = spell;
            Caster = caster;
            this.TargetedCell = TargetedCell;
            Fight = fight;
            Shape = shape;
            Size = size;
            this.Id = Id;

            var zone = new Zones.Zone(shape, size, 0, dir);
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

        public int SpellGlyph { get; set; }
        public byte Size { get; set; }
        public SpellShapeEnum Shape { get; set; }
        public int Color { get; set; }
        #endregion    

        #region Public Func
        public void Execute()
        {
            SpellHandler.Execute(this);
        }

        public void Init(Fighter fighter)
        {
            DeclanchedCell = fighter.Point.Point;
            SpellCastHandler spellCastHandler = SpellManager.Instance.GetSpellCastHandler(Caster, new SpellTemplate(ObjectDataManager.Get<Spell>(Spell.spellId), (byte)Spell.grade, ObjectDataManager.Get<SpellLevel>(ObjectDataManager.Get<Spell>((int)Spell.spellId).spellLevels[(int)Spell.grade])) { IsTrapSpell = true }, TargetedCell, false);
            spellCastHandler.Initialize();
            List<Fighter> fighters = Fight.GetAllFighter(Cells).ToList().FindAll(x => x.IsAlive);
            if (fighters.Contains(fighter))
            {
                fighters.Remove(fighter);
                fighters.Insert(0, fighter);
            }
            foreach (var item in spellCastHandler.GetEffectHandlers())
            {
                item.SetAffectedActors(fighters);
            }
            SpellHandler = spellCastHandler;
        }

        public GameActionMark GetGameActionMark()
        {
            return new GameActionMark(Caster.Id, (sbyte)Caster.Team.Team, SpellGlyph, (sbyte)Spell.grade, Id, (sbyte)GameActionMarkTypeEnum.TRAP, (short)TargetedCell.Id,
                (from x in Cells
                 select new GameActionMarkedCell((uint)x.Id, 0, Color, (sbyte)GameActionMarkCellsTypeEnum.CELLS_CIRCLE)).ToArray(), true);
        }
        #endregion
    }
}
