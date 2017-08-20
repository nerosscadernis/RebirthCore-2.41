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
    [EffectHandler(EffectsEnum.Effect_Teleport)]
    public class Teleport : SpellEffectHandler
    {
        public Teleport(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply(object token, bool declanched = false)
        {
            //foreach (var item in GetAffectedActors())
            //{
            if (Caster.HasState(97))
                return false;
            if (Fight.GetFighter(TargetedCell.Id) == null || Fight.Map.IsCellFree((short)TargetedCell.Id))
            {
                Caster.TeleportTo(Caster, (short)TargetedCell.Id, ActionsEnum.ACTION_CHARACTER_TELEPORT_ON_SAME_MAP);
                if (Fight.HasTrap(TargetedCell.Id))
                {
                    Fight.DeclancheTrap(TargetedCell.Id);
                }
                Fight.MarkManager.ExecuteGlyph(Caster, TriggerBuffType.AURA);
            }
            //}
            return false;
        }
    }
}
