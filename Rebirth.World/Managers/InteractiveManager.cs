using Rebirth.Common.Extensions;
using Rebirth.Common.GameData.D2O;
using Rebirth.Common.Protocol.Data;
using Rebirth.World.Datas.Interactives;
using Rebirth.World.Datas.Interactives.Creation;
using Rebirth.World.Datas.Interactives.Types;
using Rebirth.World.Datas.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rebirth.World.Managers
{
    public class InteractiveManager : DataManager<InteractiveManager>
    {
        public void Init()
        {
            CreatorInit();
            Starter.Logger.Infos("Interactive construtors load = " + methods.Count);
        }

        #region Creator
        private Dictionary<string, InteractiveHandler> methods = new Dictionary<string, InteractiveHandler>();

        public void CreatorInit()
        {
            Type type = typeof(InteractiveCreator);
            object obj = Activator.CreateInstance(type);
            foreach (var methodInfo in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
            {
                InteractiveAttribute[] attributes = methodInfo.GetCustomAttributes<InteractiveAttribute>().ToArray();
                if (attributes.Length != 0)
                {
                    ParameterInfo[] parameters = methodInfo.GetParameters();
                    if (parameters.Length != 5)
                    {
                        throw new Exception(string.Format("Only two parameters is allowed to use the InteractiveHandler attribute. (method {0})", methodInfo.Name));
                    }

                    methods.Add(attributes.First().Name, new InteractiveHandler(methodInfo, obj, attributes));
                }
            }
        }

        public void GetInteractiveMap(MapTemplate map, string name, int identifier, uint cellid, bool onMap, int subArea)
        {

            if (methods.ContainsKey(name))
            {
                methods[name].Invoke(map, identifier, cellid, onMap, subArea);
            }
            else
                map.AddInteractive(new InteractiveDefault(identifier, cellid, onMap));
        }
        #endregion
    }
}
