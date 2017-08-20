using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Effects.Handlers;
using Rebirth.World.Effects.Handlers.Spells;
using Rebirth.World.Game.Effect.Instances;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Game.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rebirth.World.Datas.Spells;
namespace Rebirth.World.Game.Effect.Spells.Move
{
    [EffectHandler(EffectsEnum.Effect_SwitchPosition)]
    public class SwitchPosition : SpellEffectHandler
    {
        public SwitchPosition(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }
        public override bool Apply(object token, bool declanched = false)
        {
            if (Caster.HasState(97))
                return false;

            Fighter fightActor = base.GetAffectedActors().FirstOrDefault();
            if (fightActor != null)
            {
                if (fightActor.HasState(97))
                    return false;
                base.Caster.ExchangePositions(fightActor);
                Fight.MarkManager.ExecuteGlyph(Caster, TriggerBuffType.AURA);
                Fight.MarkManager.ExecuteGlyph(fightActor, TriggerBuffType.AURA);
            }
            return true;
        }
    }
}
