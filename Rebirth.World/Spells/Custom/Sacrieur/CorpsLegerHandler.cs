using Rebirth.World.Datas.Maps;
using Rebirth.World.Datas.Spells;
using Rebirth.World.Effects.Handlers.Spells;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Game.Spells.Cast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Spells.Custom.Sacrieur
{
    [SpellCastHandler(434)]
    public class CorpsLegerHandler : DefaultSpellCastHandler
    {
        public CorpsLegerHandler(Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(caster, spell, targetedCell, critical)
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
            effects.RemoveAt(1);
            Handlers = effects.ToArray();
            base.Execute(token);
        }
    }
}
