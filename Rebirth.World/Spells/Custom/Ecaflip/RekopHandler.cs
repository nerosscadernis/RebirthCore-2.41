using Rebirth.Common.Thread;
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

namespace Rebirth.World.Game.Spells.Custom.Ecaflip
{
    [SpellCastHandler(114)]
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
            effects.RemoveRange(0, 4);
            var tmp = effects[0];
            effects.Remove(tmp);

            var rdn = new AsyncRandom().Next(1, 3);

            switch (rdn)
            {
                case 1:
                    effects.RemoveRange(4, 8);
                    break;
                case 2:
                    effects.RemoveRange(0, 4);
                    effects.RemoveRange(4, 4);
                    break;
                case 3:
                    effects.RemoveRange(0, 8);
                    break;
            }

            effects.Insert(0, tmp);
            Handlers = effects.ToArray();
            base.Execute(token);
        }
    }
}
