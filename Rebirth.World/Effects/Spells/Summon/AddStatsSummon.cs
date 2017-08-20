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

namespace Rebirth.World.Game.Effect.Spells.Summon
{
    [EffectHandler(EffectsEnum.Effect_StatsSummon)]
    public class AddStatsSummon : SpellEffectHandler
    {
        public AddStatsSummon(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }
        public override bool Apply(object token, bool declanched = false)
        {
            bool result;
            foreach (Fighter current in base.GetAffectedActors())
            {
                PlayerFields field = GetField(Dice.DiceNum);
                if (field > 0)
                {
                    base.AddStatBuff(current, (short)(Caster.Stats[field].Total * Dice.Value / 100), field, true, declanched);
                }
            }
            result = true;
            return result;
        }

        private PlayerFields GetField(int num)
        {
            switch (num)
            {
                case 14:
                    return PlayerFields.Agility;
                case 91:
                    return PlayerFields.AirDamageBonus;
            }
            throw new System.Exception(string.Format("'{0}' has no binded caracteristic [AddStatsSummon]", num));
        }
    }
}
