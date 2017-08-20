using Rebirth.Common.Protocol.Data;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Datas.Spells;
using Rebirth.World.Effects.Handlers.Spells;
using Rebirth.World.Game.Fights;
using Rebirth.World.Game.Fights.Actors;

namespace Rebirth.World.Game.Spells.Cast
{
    public abstract class SpellCastHandler
    {
       // private MapPoint m_castPoint;
        public Fighter Caster
        {
            get;
            protected set;
        }
        public SpellTemplate Spell
        {
            get;
            private set;
        }

        public SpellLevel CurrentSpellLevel
        {
            get
            {
                return this.Spell.CurrentSpellLevel;
            }
        }

        public SpellLevel CustomSpellSkin
        { get; set; }
      
        public CellMap TargetedCell
        {
            get;
            private set;
        }

        public short[] Portails
        {
            get;
            set;
        }
        public double BoostCase
        {
            get;
            set;
        }
        //public MapPoint TargetedPoint
        //{
        //    get;
        //    private set;
        //}
        public bool Critical
        {
            get;
            private set;
        }
        private bool m_silentcast;
        public virtual bool SilentCast
        {
            get
            {
                return m_silentcast;
            }
            set
            {
                m_silentcast = value;
            }
        }
        //public MarkTrigger MarkTrigger
        //{
        //    get;
        //    set;
        //}
        //public Cell CastCell
        //{
        //    get
        //    {
        //        return (this.MarkTrigger == null || this.MarkTrigger.Shapes.Length <= 0) ? this.Caster.Cell : this.MarkTrigger.Shapes[0].Cell;
        //    }
        //}
        //public MapPoint CastPoint
        //{
        //    get
        //    {
        //        MapPoint arg_1E_0;
        //        if ((arg_1E_0 = this.m_castPoint) == null)
        //        {
        //            arg_1E_0 = (this.m_castPoint = new MapPoint(this.CastCell));
        //        }
        //        return arg_1E_0;
        //    }
        //    set
        //    {
        //        this.m_castPoint = value;
        //    }
        //}
        public Fight Fight
        {
            get
            {
                return this.Caster.Fight;
            }
        }
        public MapTemplate Map
        {
            get
            {
                return this.Fight.Map;
            }
        }
        protected SpellCastHandler(Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical)
        {
            this.Caster = caster;
            this.Spell = spell;
            this.TargetedCell = targetedCell;
            this.Critical = critical;
            Portails = new short[0];
        }
        public abstract void Initialize(bool affectTarget = true);
        public abstract void Execute(object token);
        public virtual System.Collections.Generic.IEnumerable<SpellEffectHandler> GetEffectHandlers()
        {
            return new SpellEffectHandler[0];
        }
    }
}
