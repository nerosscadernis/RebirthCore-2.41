using Rebirth.Common.Extensions;
using Rebirth.Common.GameData.D2O;
using Rebirth.Common.Protocol.Data;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Datas.Spells;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Game.Spells.Cast;
using Rebirth.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rebirth.World.Game.Spells
{
    public class SpellManager : DataManager<SpellManager>
    {
        private delegate SpellCastHandler SpellCastConstructor(Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical);
        private readonly System.Collections.Generic.Dictionary<int, SpellManager.SpellCastConstructor> m_spellsCastHandler = new System.Collections.Generic.Dictionary<int, SpellManager.SpellCastConstructor>();

        private void InitializeHandlers()
        {
            foreach (var current in
                from entry in System.Reflection.Assembly.GetEntryAssembly().DefinedTypes
                where entry.IsSubclassOf(typeof(SpellCastHandler)) && !entry.IsAbstract
                select entry)
            {
                if (current.GetCustomAttribute<DefaultSpellCastHandlerAttribute>(false) == null)
                {
                    SpellCastHandlerAttribute spellCastHandlerAttribute = current.GetCustomAttributes<SpellCastHandlerAttribute>().SingleOrDefault<SpellCastHandlerAttribute>();
                    if (spellCastHandlerAttribute == null)
                    {
                        Starter.Logger.Error(string.Join("SpellCastHandler '{0}' has no SpellCastHandlerAttribute, or more than 1", current.Name));
                    }
                    else
                    {
                        Spell spellTemplate = ObjectDataManager.Get<Spell>(spellCastHandlerAttribute.Spell);
                        if (spellTemplate == null)
                        {
                            Starter.Logger.Error(string.Join("SpellCastHandler '{0}' -> Spell {1} not found", current.Name, spellCastHandlerAttribute.Spell));
                        }
                        else
                        {
                            this.AddSpellCastHandler(current.BaseType, spellTemplate);
                        }
                    }
                }
            }
        }

        public SpellLevel GetSpellLevel(int templateid, int level)
        {
            Spell spellTemplate = ObjectDataManager.Get<Spell>(templateid);
            SpellLevel result;
            if (spellTemplate == null)
            {
                result = null;
            }
            else
            {
                result = ((spellTemplate.spellLevels.Count <= level - 1) ? null : ObjectDataManager.Get<SpellLevel>((int)spellTemplate.spellLevels[(level == 0 ? 0 : level - 1)]));
            }
            return result;
        }
        public void AddSpellCastHandler(System.Type handler, Spell spell)
        {
            System.Reflection.ConstructorInfo constructor = handler.GetConstructor(new System.Type[]
            {
                typeof(Fighter),
                typeof(SpellTemplate),
                typeof(CellMap),
                typeof(bool)
            });
            if (constructor == null)
            {
                throw new System.Exception(string.Format("Handler {0} : No valid constructor found !", handler.Name));
            }
            ReflectionExtensions.CreateDelegate<SpellManager.SpellCastConstructor>(constructor);
            this.m_spellsCastHandler.Add(spell.id, ReflectionExtensions.CreateDelegate<SpellManager.SpellCastConstructor>(constructor));
        }


        public SpellCastHandler GetSpellCastHandler(Fighter caster, SpellTemplate spell, CellMap targetedCell, bool critical)
        {
            SpellCastConstructor spellCastConstructor;
            return this.m_spellsCastHandler.TryGetValue(spell.Spell.id, out spellCastConstructor) ? spellCastConstructor(caster, spell, targetedCell, critical) : new DefaultSpellCastHandler(caster, spell, targetedCell, critical);
        }
    }
}
