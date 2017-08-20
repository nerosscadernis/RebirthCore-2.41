using Rebirth.Common.Extensions;
using Rebirth.Common.GameData.D2O;
using Rebirth.Common.Protocol.Data;
using Rebirth.Common.Protocol.Enums;
using Rebirth.Common.Thread;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Fights.Challenges
{
    public class ChallengeManager : DataManager<ChallengeManager>
    {
        private delegate ChallengeHandler ChallengeConstructor(Fight fight, ChallengeEnums chall);
        private readonly Dictionary<ChallengeEnums, ChallengeConstructor> m_challengesHandler = new Dictionary<ChallengeEnums, ChallengeConstructor>();
        private List<Challenge> m_challenges;

        public void Load()
        {
            m_challenges = ObjectDataManager.GetAll<Challenge>();
            InitializeHandlers();
        }

        private void InitializeHandlers()
        {
            foreach (var current in
                from entry in typeof(ChallengeHandler).GetTypeInfo().Assembly.GetTypes()
                where entry.GetTypeInfo().IsSubclassOf(typeof(ChallengeHandler)) && !entry.GetTypeInfo().IsAbstract
                select entry)
            {
                ChallengeAttribute[] array = current.GetTypeInfo().GetCustomAttributes<ChallengeAttribute>().ToArray<ChallengeAttribute>();
                foreach (ChallengeEnums current2 in array.Select((ChallengeAttribute entry) => entry.Effect))
                {
                    if (current.GetTypeInfo().IsSubclassOf(typeof(ChallengeHandler)))
                    {
                        System.Reflection.ConstructorInfo constructor = current.GetConstructor(new System.Type[]
                        {
                                            typeof(Fight),
                                            typeof(ChallengeEnums)
                       });
                        this.m_challengesHandler.Add(current2, constructor.CreateDelegate<ChallengeConstructor>());
                    }
                }
            }
        }

        public ChallengeHandler GetChallengeHandler(Fight fight, ChallengeEnums chall)
        {
            ChallengeManager.ChallengeConstructor challengeConstructor;
            ChallengeHandler result;
            if (this.m_challengesHandler.TryGetValue(chall, out challengeConstructor))
            {
                result = challengeConstructor(fight, chall);
            }
            else
            {
                result = new ChallengeHandler(fight, chall);
            }
            return result;
        }

        public ChallengeHandler GetChall(Fight fight)
        {
            var test = m_challengesHandler.Keys.ToList();

            return GetChallengeHandler(fight, test[new AsyncRandom().Next(test.Count)]);
        }
    }
}
