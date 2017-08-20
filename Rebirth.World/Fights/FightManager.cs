using Rebirth.Common.Extensions;
using Rebirth.Common.Pool;
using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Datas.Characters;
using Rebirth.World.Game.Fights.Custom;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Datas.Monsters;

namespace Rebirth.World.Game.Fights
{
    public class FightManager : DataManager<FightManager>
    {
        private UniqueIdProvider m_provider = new UniqueIdProvider(0);

        public DuelFight CreatePvpFriend(MapTemplate map)
        {
            var fight = new DuelFight(m_provider.Pop(), map);
            return fight;
        }

        public AgressionFight CreateAgressionFight(Character source, Character target, MapTemplate map)
        {
            var fight = new AgressionFight(m_provider.Pop(), map);
            source.Map.Quit(source.Client);
            target.Map.Quit(target.Client);

            fight.AddCharacter(source, TeamEnum.TEAM_DEFENDER);
            fight.AddCharacter(target, TeamEnum.TEAM_CHALLENGER);
            return fight;
        }

        public AvAFight CreateAvAFight(Character source, Character target, MapTemplate map)
        {
            var fight = new AvAFight(m_provider.Pop(), map);
            source.Map.Quit(source.Client);
            target.Map.Quit(target.Client);

            fight.AddCharacter(source, TeamEnum.TEAM_DEFENDER);
            fight.AddCharacter(target, TeamEnum.TEAM_CHALLENGER);
            return fight;
        }

        public PvmFight CreatePvmFight(MapTemplate map)
        {
            return new PvmFight(m_provider.Pop(), map);
        }

        public KoliFight CreateKoliFight(MapTemplate map)
        {
            return new KoliFight(m_provider.Pop(), map);
        }

        //public DungeonFight CreateDungeonFight(MapTemplate map, MonsterGroup group)
        //{
        //    return new DungeonFight(m_provider.Pop(), map);
        //}

    }
}
