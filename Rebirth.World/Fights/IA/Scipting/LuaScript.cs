using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Fights.IA.Scipting
{
    /// <summary>
    /// Fuck je sais pas se que je fait!
    ///// </summary>
    //public class LuaScript
    //{
    //    public List<int> Monsters;
    //    public string Author;
    //    public string Description;
    //    private string m_filePath;

    //    private Fight m_fight;
    //    private Lua m_script;
    //    private Logger m_logger = LogManager.GetCurrentClassLogger();
    //    public LuaScript(string author, string description, List<int> monsterIds, string filePath)
    //    {
    //        this.Author = author;
    //        this.Monsters = monsterIds;
    //        this.Description = description;
    //        this.m_filePath = filePath;
    //    }

    //    public void RunScript()
    //    {
    //        try
    //        {
    //            var functionMain = m_script.GetFunction("Main");
    //            functionMain.Call();
    //        }catch(Exception e)
    //        {
    //            WorldServer.Instance.Pool.CallDelayed(1000, m_fight.PassTurn);
    //            m_logger.Error("IA Error : " + e.Message);
    //        }
    //    }

    //    public void PreLoading(Fight fight, AbstractIA fighter)
    //    {
    //        m_script = new Lua();
    //        m_fight = fight;
    //        m_script["fight"] = new Abstract.FightIAEnvironnement(fight, fighter);
    //        m_script["fighter"] = new Abstract.FighterIAEnvironnement(fight, fighter);
    //        m_script["util"] = new Abstract.UtilsIAEnvironnement(fight, fighter);
    //        m_script.LoadCLRPackage();
    //        m_script.DoFile(m_filePath);
    //    }

    //    public LuaScript Clone()
    //    {
    //        return new LuaScript(this.Author, this.Description, this.Monsters, m_filePath);
    //    }
    //}
}
