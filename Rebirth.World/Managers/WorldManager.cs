using Rebirth.Common.Dispatcher;
using Rebirth.Common.Extensions;
using Rebirth.Common.Protocol.Enums;
using Rebirth.Common.Protocol.Messages;
using Rebirth.Common.Protocol.Types;
using Rebirth.World.Datas.Characters;
using Rebirth.World.Frames;
using Rebirth.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rebirth.World.Managers
{
    public class WorldManager : DataManager<WorldManager>
    {
        #region Properties
        public List<Character> Characters = new List<Character>();
        #endregion

        #region Public Methods
        public void Enter(Character character)
        {
            character.Client.Register(typeof(WorldManager));
            character.Client.Register(typeof(FriendFrame));
            character.Client.Register(typeof(QuestFrame));
            character.Client.Register(typeof(WorldFrame));
            character.Client.Register(typeof(InteractiveFrame));
            character.Client.Register(typeof(ActivityFrame));
            character.Client.Register(typeof(NpcFrame));
            character.Client.Register(typeof(InventoryFrame));
            character.Client.Register(typeof(FigthFrame));

            character.Client.Send(new InventoryContentAndPresetMessage(character.Inventory.GetObjectItems(), (uint)character.Inventory.Kamas, new Preset[0], new IdolsPreset[0]));
            character.Client.Send(new ShortcutBarContentMessage((sbyte)ShortcutBarEnum.GENERAL_SHORTCUT_BAR, (from x in character.Shortcuts.GetShortcuts(ShortcutBarEnum.GENERAL_SHORTCUT_BAR)
                                                                                                                select x.GetNetworkShortcut()).ToArray()));
            character.Client.Send(new ShortcutBarContentMessage((sbyte)ShortcutBarEnum.SPELL_SHORTCUT_BAR, (from x in character.Shortcuts.GetShortcuts(ShortcutBarEnum.SPELL_SHORTCUT_BAR)
                                                                                                                select x.GetNetworkShortcut()).ToArray()));
            character.Client.Send(new RoomAvailableUpdateMessage(1));
            character.Client.Send(new HavenBagPackListMessage(new sbyte[0]));
            character.Client.Send(new EmoteListMessage(new byte[0]));
            character.Client.Send(new JobDescriptionMessage(character.Jobs.GetJobDescription()));
            character.Client.Send(new JobExperienceMultiUpdateMessage(character.Jobs.GetJobExperience()));
            character.Client.Send(new JobCrafterDirectorySettingsMessage(character.Jobs.GetJobCrafterDirectorySettings()));
            character.Client.Send(new AlignmentRankUpdateMessage(0, false));
            character.Client.Send(new DareCreatedListMessage(new DareInformations[0], new DareVersatileInformations[0]));
            character.Client.Send(new DareSubscribedListMessage(new DareInformations[0], new DareVersatileInformations[0]));
            character.Client.Send(new DareWonListMessage(new double[0]));
            character.Client.Send(new DareRewardsListMessage(new DareReward[0]));
            character.Client.Send(new PrismsListMessage(new PrismSubareaEmptyInfo[0]));

            character.Client.Send(new EnabledChannelsMessage(new sbyte[0], new sbyte[] { 0, 1, 2, 3, 4, 5, 6, 7, 12, 13, 9, 10 }));
            character.Client.Send(new SpellListMessage(true, new SpellItem[0]));
            character.Client.Send(new ShortcutBarContentMessage((sbyte)ShortcutBarEnum.SPELL_SHORTCUT_BAR, (from x in character.Shortcuts.GetShortcuts(ShortcutBarEnum.SPELL_SHORTCUT_BAR)
                                                                                                            select x.GetNetworkShortcut()).ToArray()));
            character.Client.Send(new InventoryWeightMessage(0, 1000));
            character.Client.Send(new FriendWarnOnConnectionStateMessage(true));
            character.Client.Send(new FriendWarnOnLevelGainStateMessage(true));
            character.Client.Send(new FriendGuildWarnOnAchievementCompleteStateMessage(true));
            character.Client.Send(new WarnOnPermaDeathStateMessage(true));
            character.Client.Send(new GuildMemberWarnOnConnectionStateMessage(true));

            character.Client.Send(new SequenceNumberRequestMessage());
            character.Client.Send(new TextInformationMessage(1, 89, new string[0]));
            character.Client.Send(new TextInformationMessage(0, 152, character.Infos.LastConnexionForGame()));
            character.Client.Send(new TextInformationMessage(0, 153, new string[] { "127.0.0.1" }));
            // Les deux en dessous c'est si on a recup de la vie offline
            character.Client.Send(new OnConnectionEventMessage(2));
            character.Client.Send(new TextInformationMessage(0, 1, new string[] { "50" }));

            character.Client.Send(new ServerExperienceModificatorMessage(300));
            character.Client.Send(new SpouseStatusMessage(false));
            character.Client.Send(new AchievementListMessage(new uint[0], new AchievementRewardable[0]));
            //character.Client.Send(new GameRolePlayArenaUpdatePlayerInfosMessage(new ArenaRankInfos(7, 0, 0, 0)));

            if (character.Client.Account.Role > CharacterRoleEnum.Player)
                character.Client.Send(new CharacterCapabilitiesMessage(4096));
            else
                character.Client.Send(new CharacterCapabilitiesMessage(4095));

            character.Client.Send(new AlmanachCalendarDateMessage(297));
            character.Client.Send(new IdolListMessage(new uint[0], new uint[0], new PartyIdol[0]));
            character.Client.Send(new CharacterExperienceGainMessage(0, 0, 0, 0));
            character.Client.Send(new MailStatusMessage(0, 0));
            character.Client.Send(new StartupActionsListMessage(new StartupActionAddObject[0]));
            character.Client.Send(new AccountHouseMessage(new AccountHouseInformations[0]));

            character.Client.Send(new CharacterLoadingCompleteMessage());
        }
        #endregion

        #region Private Void
        [MessageHandler(GameContextCreateRequestMessage.Id)]
        public void HandleGameContextCreateRequestMessage(Client client, GameContextCreateRequestMessage message)
        {
            client.UnRegister(typeof(WorldManager));

            client.Send(new GameContextDestroyMessage());
            client.Send(new GameContextCreateMessage(1));
            client.Character.Map = MapManager.Instance.GetMap(client.Character.Infos.MapId);
            client.Character.Map.Enter(client, true);
        }
        #endregion
    }
}
