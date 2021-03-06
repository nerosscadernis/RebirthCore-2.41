﻿
//using System;
//using Rebirth.Common.Protocol.Enums;
//using Rebirth.World.Datas;
//using Rebirth.World.Datas.Npcs;
//using Rebirth.World.Datas.Characters;
//using Rebirth.World.Game.Npcs.Actions;
//using System.Collections.Generic;
//using System.Linq;
//using Rebirth.Common.Protocol.Messages;
//using Rebirth.World.Game.Hdv;

//[Discriminator("HdvBuy", typeof(NpcActionDatabase), new System.Type[]
//  {
//        typeof(NpcActionRecord)
//  })]
//public class NpcHdvBuyAction : NpcActionDatabase
//{
//    public const string Discriminator = "HdvBuy";
//    public NpcHdvBuyAction(NpcActionRecord record) : base(record)
//    {
//    }
//    public List<uint> Quantities
//    {
//        get
//        {
//            var csv = base.Record.GetParameter<string>(0, true);
//            return csv.Split(';').Select(x=> uint.Parse(x)).ToList();
//        }
//    }

//    public List<uint> Types
//    {
//        get
//        {
//            var csv = base.Record.GetParameter<string>(1, true);
//            return csv.Split(';').Select(x => uint.Parse(x)).ToList();
//        }
//    }

//    public int TaxPercentageAddItem
//    {
//        get
//        {
//            return base.Record.GetParameter<int>(2, true);
//        }
//    }
//    public int TaxPercentageModifItem
//    {
//        get
//        {
//            return base.Record.GetParameter<int>(3, true);
//        }
//    }
//    public int LevelMaxItem
//    {
//        get
//        {
//            return base.Record.GetParameter<int>(4, true);
//        }
//    }

//    public override NpcActionTypeEnum ActionType
//    {
//        get
//        {
//            return NpcActionTypeEnum.ACTION_BUY;
//        }
//    }

//    public override void Execute(NpcSpawn npc, Character character)
//    {
//        HdvBuyDialog dialog = new HdvBuyDialog()
//        {
//            Action = this,
//            Character = character,
//            Npc = npc
//        };
//        dialog.Open();
//    }
//}
