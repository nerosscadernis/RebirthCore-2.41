

















// Generated on 01/12/2017 03:53:14
using System;
using System.Collections.Generic;
using Rebirth.Common.GameData.D2O;

namespace Rebirth.Common.Protocol.Data
{

    [D2oClass("QuestObjectiveParameters")]

    public class QuestObjectiveParameters : IDataObject
    {

        public uint numParams;
        public int parameter0;
        public int parameter1;
        public int parameter2;
        public int parameter3;
        public int parameter4;
        public Boolean dungeonOnly;

        public string[] GetParams()
        {
            return new string[] { parameter0.ToString(), parameter1.ToString(), parameter2.ToString(), parameter3.ToString(), parameter4.ToString() };
        }
    }

}