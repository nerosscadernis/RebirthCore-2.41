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
using Rebirth.Common.GameData.D2O;
using Rebirth.World.Datas.Spells;

namespace Rebirth.World.Game.Fights.Marks
{
    public class Glyph
    {
        #region Constructor
        public Glyph(short Id, SpellLevel spell, Fighter caster, DirectionsEnum dir, CellMap TargetedCell, Fight fight, SpellShapeEnum shape, byte size, int color, int spellGlyph, TriggerBuffType type)
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
            Type = type;
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
        public SpellEffectHandler[] Handlers { get; set; }
        public TriggerBuffType Type { get; set; }
        public int SpellGlyph { get; set; }
        public byte Size { get; set; }
        public SpellShapeEnum Shape { get; set; }
        public int Color { get; set; }
        public int Duration { get; set; }
        public bool IsAura { get; set; }
        #endregion    

        #region Public Func
        public void Execute(Fighter fighter)
        { 
            SpellCastHandler spellCastHandler = SpellManager.Instance.GetSpellCastHandler(Caster, new SpellTemplate(ObjectDataManager.Get<Spell>((int)Spell.spellId), (byte)(Spell.grade - 1)) { IsGlyphSpell = true }, TargetedCell, false);
            spellCastHandler.Initialize();
            foreach (var item in spellCastHandler.GetEffectHandlers())
            {
                item.TrySetAffectedActor(fighter);
            }
            spellCastHandler.Execute(this);
        }

        public void Declanche(bool force = false)
        {
            if (IsAura && !force)
                return;
            SpellCastHandler spellCastHandler = SpellManager.Instance.GetSpellCastHandler(Caster, new SpellTemplate(ObjectDataManager.Get<Spell>((int)Spell.spellId), (byte)(Spell.grade - 1)) { IsGlyphSpell = true }, TargetedCell, false);
            spellCastHandler.Initialize();

            spellCastHandler.Execute(this);
        }

        public GameActionMark GetGameActionMark()
        {
            return new GameActionMark(Caster.Id, (sbyte)Caster.Team.Team, SpellGlyph, (sbyte)Spell.grade, Id, (sbyte)GameActionMarkTypeEnum.GLYPH, (short)TargetedCell.Id,
                (from x in Cells
                 select new GameActionMarkedCell((uint)x.Id, 0, Color, (sbyte)GameActionMarkCellsTypeEnum.CELLS_CIRCLE)).ToArray(), true);
        }

        public void Decrement()
        {
            Duration--;
            if (Duration == 0)
                Fight.MarkManager.RemoveGlyph(this);      
        }
        #endregion
    }
}
