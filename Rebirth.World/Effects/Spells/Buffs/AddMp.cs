using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Datas.Spells;
using Rebirth.World.Effects.Handlers;
using Rebirth.World.Effects.Handlers.Spells;
using Rebirth.World.Game.Effect.Instances;
using Rebirth.World.Game.Fights.Actors;

namespace Rebirth.World.Game.Effect.Spells.Buffs
{
    [EffectHandler(EffectsEnum.Effect_AddMP), EffectHandler(EffectsEnum.Effect_AddMP_128)]
    public class AddMP : SpellEffectHandler
    {
        public AddMP(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }
        public override bool Apply(object token, bool declanched = false)
        {
            bool result;
            foreach (Fighter current in base.GetAffectedActors())
            {
                EffectInteger effectInteger = base.GenerateEffect();
                if (effectInteger == null)
                {
                    result = false;
                    return result;
                }
                if (this.Effect.Duration > 0 || Effect.Duration < 0)
                {
                    base.AddStatBuff(current, (short)effectInteger.Value, PlayerFields.MP, true, declanched);
                }
                else
                {
                    current.RegainMP((short)effectInteger.Value);
                }
            }
            result = true;
            return result;
        }
    }
}
