using Rebirth.World.Effects.Handlers.Spells;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Game.Spells.Cast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rebirth.World.Datas.Spells;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Spells.Custom.Elio
{
    [SpellCastHandler(5403)]
    public class RekopHandler : DefaultSpellCastHandler
    {
        public RekopHandler(Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(caster, spell, targetedCell, critical)
        {
        }

        public object TemporaryBoostStateEffect { get; private set; }

        public override void Execute(object token)
        {
            if (!this.m_initialized)
            {
                this.Initialize(true);
            }

            List<SpellEffectHandler> effects = base.Handlers.ToList();
            Handlers = effects.ToArray();
            base.Execute(token);
        }
    }
}
