using Rebirth.Common.IStructures;
using Rebirth.Common.Protocol.Enums;
using Rebirth.Common.Protocol.Messages;
using Rebirth.World.Datas.Characters;
using Rebirth.World.Datas.Jobs;
using Rebirth.World.Exchanges;
using Rebirth.World.Frames;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rebirth.World.Datas.Interactives.Dialogs
{
    public class CraftDialog : IActivity
    {
        public DialogTypeEnum DialogType
        {
            get
            {
                return DialogTypeEnum.DIALOG_EXCHANGE;
            }
        }

        public Character Character
        {
            get;
            private set;
        }

        public virtual void Open()
        {
            this.Character.State = PlayerState.In_Zaap_Panel;
            this.Character.Activity = this;
            Atelier.Use();
        }

        public Atelier Atelier
        {
            get;
            private set;
        }

        public CraftDialog(Character character, int skillId)
        {
            Character = character;
            Trader trader = new Trader(character);
            Atelier = new Atelier(character.Jobs.GetJob(skillId), Character, (uint)skillId);
            character.Client?.Register(typeof(ExchangeFrame));
        }

        public void Close()
        {
            this.Character.State = PlayerState.Available;
            this.Character.Activity = null;
            Character.Client.Send(new ExchangeLeaveMessage((sbyte)DialogType, Atelier.Close()));
            Character.Client?.UnRegister(typeof(ExchangeFrame));
        }
    }
}
