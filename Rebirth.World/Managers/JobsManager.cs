using Rebirth.Common.Extensions;
using Rebirth.Common.GameData.D2O;
using Rebirth.Common.Protocol.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rebirth.World.Managers
{
    public class JobsManager : DataManager<JobsManager>
    {
        private List<Skill> _skills;
        private List<Recipe> _recipes;

        public void Init()
        {
            _skills = ObjectDataManager.GetAll<Skill>();
            _recipes = ObjectDataManager.GetAll<Recipe>();

            Starter.Logger.Debug("Skills load = " + _skills.Count);
            Starter.Logger.Debug("Recipes load = " + _recipes.Count);
        }

        public Skill[] GetSkillByJob(int jobId, int level)
        {
            return (from x in _skills
                    where x.parentJobId == jobId && x.levelMin <= level
                    select x).ToArray();
        }
    }
}
