using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Effects.Handlers;
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
using Rebirth.World.Datas.Spells;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Effect.Spells.Damage
{
    [EffectHandler(EffectsEnum.Effect_Dodge)]
    public class Dodge : SpellEffectHandler
    {
        public Dodge(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }
        public override bool Apply(object token, bool declanched = false)
        {
            foreach (Fighter current in base.GetAffectedActors())
            {
                var buff = new TriggerBuff(current.PopNextBuffId(), current, Caster, Dice, Spell, null, 0, Critical, true, TriggerBuffType.BEFORE_ATTACKED, declanched);
                buff.Applyed += ApplyDodge;
                base.AddTriggerBuff(current, buff);
            }
            return true;
        }

        private void ApplyDodge(TriggerBuff buff, object token)
        {
            if (token is Fights.Other.Damage)
            {
                var damage = token as Fights.Other.Damage;
                if (damage.Source.Point.Point.DistanceTo(damage.Target.Point.Point) == 1)
                {
                    damage.Amount = 0;
                    ((Fights.Other.Damage)token).Amount = 0;
                    MapPoint startCell = damage.Target.Point.Point;
                    MapPoint lastCell = damage.Target.Point.Point;
                    MapPoint nextCell;
                    if (TargetedPoint.CellId == damage.Target.Point.Point.CellId)
                        nextCell = lastCell.GetNearestCellInDirection(damage.Source.Point.Point.OrientationTo(damage.Target.Point.Point, true));
                    else
                        nextCell = lastCell.GetNearestCellInDirection(TargetedPoint.OrientationTo(damage.Target.Point.Point, true));
                    Fighter fighter = Fight.GetFighter(nextCell);
                    if (nextCell == null || fighter != null || Fight.Map.Cells[nextCell.CellId].NonWalkableDuringFight || !Fight.Map.Cells[nextCell.CellId].Mov)
                    {
                        damage.Target.InflictPushDamage(damage.Source, Spell, 1);
                        if (fighter != null)
                        {
                            fighter.InflictPushDamage(damage.Source, Spell, 1, true);
                        }
                    }
                    else
                    {
                        lastCell = nextCell;
                    }
                    if (lastCell != startCell)
                        damage.Target.SlideTo(damage.Source, lastCell, ActionsEnum.ACTION_CHARACTER_PUSH);
                }
            }
        }
    }
}
