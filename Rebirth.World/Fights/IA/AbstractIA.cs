using Rebirth.Common.Protocol.Data;
using Rebirth.Common.Protocol.Types;
using Rebirth.Common.Timers;
using Rebirth.World.Datas.Characters;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Game.Fights.IA.Scipting;
using Rebirth.World.Game.Fights.Team;
using System.Collections.Generic;

namespace Rebirth.World.Game.Fights.IA
{
    public class AbstractIA : Fighter
    {
        //public LuaScript scripting = null;
        #region Constructeur
        public AbstractIA(List<uint> spellIds, int grade, FightTeam team, Fight fight, int id, Look look) : base(team, fight, look, true)
        {
            //this.Environment = new EnvironmentAnalyser(this);
            //foreach (var item in spellIds)
            //{
            //    var spell = ObjectDataManager.Get<Spell>((int)item);
            //    if (spell != null)
            //        Spells.Add(SpellManager.Instance.GetSpellLevel(spell.spellLevels.Count <= grade ? (int)spell.spellLevels.Last() : (int)spell.spellLevels[grade]));
            //    else
            //        ConsoleLogger.Error("[AbstractIA] SpellId : " + item + " doesn't exist.");
            //}

            //scripting = IAManager.Instance.GetScriptByMonsterAffect(id).Clone();
            //if (scripting != null)
            //    scripting.PreLoading(fight, this);

        }
        #endregion

        #region Var
        //public EnvironmentAnalyser Environment
        //{
        //    get;
        //    private set;
        //}
        public List<SpellLevel> Spells = new List<SpellLevel>();
        #endregion

        #region Virtual functions
        public virtual void Play()
        {
            if (!Control)
                new TimerCore(ActionPass, 1000);
        }

        public virtual void StartActions()
        {
            //if (scripting != null)
            //    scripting.RunScript();
            //else
                new TimerCore(ActionPass, 1000);
        }
        #endregion

        public override FightResultListEntry GetResult()
        {
            return new FightResultFighterListEntry((uint)Team.Outcome, 0, new FightLoot(new uint[0], 0), Id, IsAlive);
        }
    }
}
