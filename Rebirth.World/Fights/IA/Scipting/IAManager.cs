//using NLog;
//using NLua;
//using Rebirth.Common.Extensions;
//using Rebirth.Common.GameData.D2O;
//using Rebirth.Common.Initialization;
//using Rebirth.Common.Protocol.Data;
//using Rebirth.Common.Reflection;
//using Rebirth.World.Database;
//using Rebirth.World.Database.Spell;
//using Rebirth.World.Game.Effect;
//using Rebirth.World.Game.Fights.Actors;
//using Rebirth.World.Datas.Maps;
//using Rebirth.World.Game.Spells.Cast;
//using Rebirth.World.Network;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;
//namespace Rebirth.World.Game.Fights.IA.Scipting
//{
//    public class IAManager : DataManager<IAManager>
//    {
//        public List<LuaScript> m_scripts = new List<LuaScript>();

//        public void Load()
//        {
//            foreach (string file in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + @"\scripts\", "*.lua", SearchOption.AllDirectories))
//            {
//                var content = File.ReadAllText(file);
//                Lua newScript = new Lua();
//                newScript.DoFile(file);
//                string authorName = newScript.GetString("author");
//                string description = newScript.GetString("description");
//                LuaTable monsters = newScript.GetTable("monsters");
//                #region monsters trool
//                var output = new List<int>(monsters.Values.Count);
//                foreach (var test in monsters.Values)
//                {
//                    output.Add(int.Parse(test.ToString()));
//                }
//                #endregion
//                m_scripts.Add(new LuaScript(authorName, description, output, file));
//            }
//        }
//        /// <summary>
//        /// Test
//        /// </summary>
//        /// <param name="monsterId">id du monstre qu'on doit cherche l'IA</param>
//        /// <returns></returns>
//        public LuaScript GetScriptByMonsterAffect(int monsterId)
//        {
//            LuaScript result = null;
//            foreach(LuaScript script in m_scripts)
//            {
//                foreach (uint monster in script.Monsters)
//                {
//                    if (monster == monsterId)
//                    {
//                        result = script.Clone();
//                        break;
//                    }
//                }
//            }

//            if (result == null)
//                result = m_scripts.FirstOrDefault(e => e.Monsters.Any(x => x == 9999));

//            return result;
//        }
//    }
//}
