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
using System.Threading.Tasks;
using Rebirth.World.Datas.Spells;
using Rebirth.World.Datas.Spells;

namespace Rebirth.World.Game.Effect.Spells.Other
{
    [EffectHandler(EffectsEnum.Effect_Illus)]
    public class Illus : SpellEffectHandler
    {
        public Illus(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply(object token, bool declanched = false)
        {
            InvisibleBuff buff = new InvisibleBuff(Caster.PopNextBuffId(), Caster, Caster, Dice, Spell, Critical, false, declanched)
            {
                Duration = 1
            };
            Caster.AddAndApplyBuff(buff);

            var inital = Caster.Point.Point;
            Caster.TeleportTo(Caster, (short)TargetedCell.Id, ActionsEnum.ACTION_CHARACTER_TELEPORT_ON_SAME_MAP);

            var listDir = new List<DirectionsEnum>() { DirectionsEnum.DIRECTION_NORTH_EAST, DirectionsEnum.DIRECTION_NORTH_WEST, DirectionsEnum.DIRECTION_SOUTH_EAST, DirectionsEnum.DIRECTION_SOUTH_WEST };
            listDir.Remove(inital.OrientationTo(TargetedPoint));

            foreach (var item in listDir)
            {
                var cellFine = inital.GetCellInDirection(item, (short)inital.DistanceToCell(TargetedPoint));
                if(Fight.IsCellFree(cellFine))
                {
                    var invoc = new SummonIllu(Caster.Team, Fight, Fight.GetNextContextualId(), Caster, new Fights.Other.FightDisposition(cellFine.CellId, item));
                    Caster.AddStaticSummon(invoc);
                }
            } 

            return true;
        }
    }
}
