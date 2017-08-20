using Rebirth.Common.IStructures;
using Rebirth.Common.Protocol.Messages;
using Rebirth.Common.Protocol.Types;
using Rebirth.Common.Timers;
using Rebirth.World.Datas.Characters;
using Rebirth.World.Datas.Items;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rebirth.World.Datas.Interactives.Types
{
    public class Ressource : AbstractInteractive, IStatedElement
    {
        public uint[] ActionId
        {
            get;
        }
        public uint CellId
        {
            get;
            set;
        }
        public int Identifier
        {
            get;
            set;
        }
        public int Type
        {
            get;
        }
        public int ItemId
        {
            get;
        }
        public uint State
        {
            get;
            set;
        }
        public int MapId
        {
            get;
            set;
        }

        public short Age
        {
            get;
            set;
        }
        public bool IsOnMap
        {
            get;
            set;
        }

        public InteractiveElement GetInteractiveElement(Character character)
        {
            if (State == 0)
                return new InteractiveElementWithAgeBonus(Identifier, Type, (from x in ActionId
                                                                             where character.Jobs.ContainsSkill(x) || x == 102
                                                                             select new InteractiveElementSkill(x, 0)).ToArray(), (from x in ActionId
                                                                                                                                   where !character.Jobs.ContainsSkill(x) && x != 102
                                                                                                                                   select new InteractiveElementSkill(x, 0)).ToArray(), IsOnMap, Age);
            else
                return new InteractiveElement(Identifier, Type, new InteractiveElementSkill[0], (from x in ActionId
                                                                                                 select new InteractiveElementSkill(x, 0)).ToArray(), IsOnMap);
        }

        public StatedElement GetStatedElement()
        {
            return new StatedElement(Identifier, CellId, State, true);
        }

        public bool Used;
        public Character CharacterUser;

        TimerCore _age;
        TimerCore _action;
        TimerCore _respawn;

        public Ressource(int Identifier, uint CellId, int Type, uint[] ActionId, int MapId, bool onMap)
        {
            _age = new TimerCore(new Action(TickRefresh), new TimeSpan(0, 3, 0), new TimeSpan(0, 3, 0));
            this.Age = 0;
            this.Identifier = Identifier;
            this.CellId = CellId;
            this.Type = Type;
            this.ActionId = ActionId;
            this.MapId = MapId;
            IsOnMap = onMap;
        }

        public void Use(Character character, uint skill)
        {
            if (!Used)
            {
                if (character.Jobs.ContainsSkill(ActionId[0]) || ActionId[0] == 102)
                {
                    MapTemplate map = MapManager.Instance.GetMap(MapId);
                    if (map != null)
                    {
                        Used = true;
                        map.Send(new InteractiveUsedMessage((uint)character.Infos.Id, (uint)Identifier, ActionId[0], 30, false));
                        _action = new TimerCore(new Action(TickAction), 3000);
                        CharacterUser = character;
                    }
                }
                else
                {
                    character.Client.Send(new InteractiveUseErrorMessage((uint)Identifier, ActionId[0]));
                }
            }
            else
            {
                character.Client.Send(new InteractiveUseErrorMessage((uint)Identifier, ActionId[0]));
            }
        }

        public void TickAction()
        {
            State = 1;
            MapTemplate map = MapManager.Instance.GetMap(MapId);
            if (map != null)
            {
                map.UpdateInteractive(this);
                map.Send(new InteractiveUseEndedMessage((uint)Identifier, ActionId[0]));
            }

            if (map.Id == 153093380)
            {
                switch (Type)
                {
                    case 1:
                        CharacterUser.Quests.ValidationObjective(1629, 9659);
                        break;
                    case 38:
                        CharacterUser.Quests.ValidationObjective(1629, 9657);
                        break;
                    case 254:
                        CharacterUser.Quests.ValidationObjective(1629, 9658);
                        break;
                    case 17:
                        CharacterUser.Quests.ValidationObjective(1629, 9660);
                        break;
                    case 75:
                        CharacterUser.Quests.ValidationObjective(1629, 9661);
                        break;
                }
            }

            _respawn = new TimerCore(new Action(TickRespawn), 30000);
            _age.Dispose();

            if (ActionId[0] != 102)
            {
                CharacterUser.Jobs.Recolte(ActionId[0], Age);
            }
            else
            {
                int rdn = new Random().Next(1, 20);
                CharacterUser.Inventory.AddItem(new PlayerItem(CharacterUser, 311, rdn));
                CharacterUser.Client.Send(new ObtainedItemMessage((uint)311, (uint)rdn));
            }
            Age = 0;
        }

        public void TickRespawn()
        {
            Used = false;
            State = 0;
            MapTemplate map = MapManager.Instance.GetMap(MapId);
            if (map != null)
            {
                map.UpdateInteractive(this);
            }
            _age = new TimerCore(new Action(TickRefresh), new TimeSpan((long)1.8e+6), new TimeSpan((long)1.8e+6));
        }

        public void TickRefresh()
        {
            Age += 20;
            if (Age == 200)
                _age.Dispose();
        }
    }
}
