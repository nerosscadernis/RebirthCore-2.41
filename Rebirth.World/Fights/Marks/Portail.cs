using Rebirth.Common.Protocol.Data;
using Rebirth.Common.Protocol.Enums;
using Rebirth.Common.Protocol.Types;
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
using Rebirth.Common.GameData.D2O;

namespace Rebirth.World.Game.Fights.Marks
{
    public class Portail
    {
        #region Constructor
        public Portail(short Id, SpellTemplate spell, Fighter caster, DirectionsEnum dir, CellMap TargetedCell, Fight fight, SpellShapeEnum shape, byte size, int color)
        {
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
            m_active = true;
        }
        #endregion

        #region Var
        public short Id;
        public Fight Fight { get; set; }
        public SpellTemplate Spell { get; set; }
        public Fighter Caster { get; set; }
        public CellMap[] Cells { get; set; }
        public CellMap TargetedCell { get; set; }
        public SpellEffectHandler[] Handlers { get; set; }
        public byte Size { get; set; }
        public SpellShapeEnum Shape { get; set; }
        public int Color { get; set; }
        private bool m_active;
        public bool Active
        {
            get
            {
                return m_active;
            }
            set
            {
                if (m_active != value)
                {
                    m_active = value;
                    Fight.ActivePortail(this);
                }
                else
                    m_active = value;
            }
        }
        public bool Neutral
        {
            get;
            set;
        }
        public int DeclanchedRound
        {
            get;
            set;
        }
        #endregion    

        #region Public Func
        public void Execute(Fighter fighter)
        {
            Active = false;
            DeclanchedRound = (int)Fight.TimeLine.ActualRound;
            Fight.DeclanchedPortail(Caster, fighter, this);
            SpellState spellState = ObjectDataManager.Get<SpellState>(248);
            Common.Protocol.Data.Spell spell = ObjectDataManager.Get<Spell>(5325);
            if (spellState != null && spell != null)
            {
                int id = fighter.PopNextBuffId();
                StateBuff stateBuff = new StateBuff(id, fighter, this.Caster, new EffectBase() { Uid = 129317 }, new SpellTemplate(spell, 0), false, 950, spellState, false) { Duration = 1 };
                fighter.AddAndApplyBuff(stateBuff, true);
            }
        }

        public void Declanche()
        {

        }

        public bool IsActivable()
        {
            return DeclanchedRound - (int)Fight.TimeLine.ActualRound < 0 && Fight.GetFighter(TargetedCell) == null && !Neutral;
        }

        public GameActionMark GetGameActionMark()
        {
            return new GameActionMark(Caster.Id, (sbyte)Caster.Team.Team, (int)Spell.CurrentSpellLevel.spellId, (sbyte)Spell.CurrentSpellLevel.grade, Id, (sbyte)GameActionMarkTypeEnum.PORTAL, (short)TargetedCell.Id,
                (from x in Cells
                 select new GameActionMarkedCell((uint)x.Id, 0, 13243184, (sbyte)GameActionMarkCellsTypeEnum.CELLS_CIRCLE)).ToArray(), true);
        }
        #endregion
    }
}
