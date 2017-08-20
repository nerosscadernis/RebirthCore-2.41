using Rebirth.Common.Protocol.Data;
using Rebirth.Common.Protocol.Enums;
using Rebirth.Common.Protocol.Types;
using Rebirth.World.Effects.Handlers;
using Rebirth.World.Effects.Handlers.Spells;
using Rebirth.World.Game.Effect.Instances;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Game.Fights.Buffs;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Datas.Monsters;
using Rebirth.World.Game.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rebirth.World.Datas.Spells;
using Rebirth.World.Datas.Characters;

namespace Rebirth.World.Game.Effect.Spells.Other
{
    [EffectHandler(EffectsEnum.Effect_ChangeAppearance), EffectHandler(EffectsEnum.Effect_ChangeAppearance_335)]
    public class ChangeSkin : SpellEffectHandler
    {
        public ChangeSkin(EffectDice effect, Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply(object token, bool declanched = false)
        {
            if (Effect is EffectDice)
            {
                var test = GetAffectedActors();
                foreach (Fighter fighter in test)
                {
                    SkinBuff buff = null;
                    Look actualLook = fighter.BaseLook.Clone();
                    switch (Spell.Spell.id)
                    {
                        case 2880:
                            actualLook.BonesID = 1575;
                            actualLook.AddSkin(1443);
                            buff = new SkinBuff(fighter.PopNextBuffId(), fighter, Caster, Effect, Spell, null, 0, Critical, false, actualLook, declanched);
                            break;
                        case 2879:
                            actualLook.BonesID = 1575;
                            actualLook.AddSkin(1449);
                            buff = new SkinBuff(fighter.PopNextBuffId(), fighter, Caster, Effect, Spell, null, 0, Critical, false, actualLook, declanched);
                            break;
                        case 7859:
                            actualLook.BonesID = 3820;
                            actualLook.Scales.Clear();
                            buff = new SkinBuff(fighter.PopNextBuffId(), fighter, Caster, Effect, Spell, null, 0, Critical, false, actualLook, declanched);
                            break;
                        case 7850:
                            actualLook.BonesID = 3821;
                            actualLook.Scales.Clear();
                            buff = new SkinBuff(fighter.PopNextBuffId(), fighter, Caster, Effect, Spell, null, 0, Critical, false, actualLook, declanched);
                            break;
                    }
                    if(buff != null)
                        fighter.AddAndApplyBuff(buff);
                }
                return true;
            }
            return false;
        }
    }
}
