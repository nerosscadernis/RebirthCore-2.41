using Rebirth.Common.IStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Npcs.Dialogs
{
    public interface IShopDialog : IActivity
    {
        bool BuyItem(int id, uint quantity);
        bool SellItem(int id, uint quantity);
    }
}
