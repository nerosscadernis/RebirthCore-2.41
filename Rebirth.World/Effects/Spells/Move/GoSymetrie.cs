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
    [EffectHandler(EffectsEnum.Effect_My_Sysmetrie)]
    public class GoSymetrie : SpellEffectHandler
    {
        public GoSymetrie(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply(object token, bool declanched = false)
        {
            foreach (var item in GetAffectedActors())
            {
                if (Caster.HasState(97))
                    continue;

                var cellEnd = Caster.Point.Point.GetCellSymetrie(TargetedPoint);

                if (cellEnd != null && cellEnd.CellId == TargetedPoint.CellId)
                    continue;

                var fighterBlock = Fight.GetFighter(cellEnd) != null && Fight.GetFighter(cellEnd).HasState(97) ? null : Fight.GetFighter(cellEnd);
                if (Fight.Map.IsCellFree(cellEnd.CellId))
                {
                    if (fighterBlock == null)
                    {
                        Caster.TeleportTo(Caster, cellEnd.CellId, ActionsEnum.ACTION_CHARACTER_TELEPORT_ON_SAME_MAP);
                        if (Fight.HasTrap(cellEnd.CellId))
                        {
                            Fight.DeclancheTrap(cellEnd.CellId);
                        }
                    }
                    else
                    {
                        Fight.FighterTeleport.Add(fighterBlock);
                        Fight.FighterTeleport.Add(Caster);
                        fighterBlock.TeleportTo(Caster, Caster.Point.Point.CellId, ActionsEnum.ACTION_CHARACTER_TELEPORT_ON_SAME_MAP);
                        Caster.TeleportTo(Caster, cellEnd.CellId, ActionsEnum.ACTION_CHARACTER_TELEPORT_ON_SAME_MAP);
                        if (Fight.HasTrap(cellEnd.CellId))
                        {
                            Fight.DeclancheTrap(cellEnd.CellId);
                        }
                        if (Fight.HasTrap(Caster.Point.Point.CellId))
                        {
                            Fight.DeclancheTrap(Caster.Point.Point.CellId);
                        }
                        Fight.MarkManager.ExecuteGlyph(Caster, TriggerBuffType.AURA);
                        Fight.MarkManager.ExecuteGlyph(fighterBlock, TriggerBuffType.AURA);
                    }
                }
            }           
            return false;
        }
    }
}
