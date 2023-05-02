using System;
using System.Collections.Generic;

namespace Core.Datas
{
    [Serializable]
    public class TechnologyNodeData
    {
        public string Name;
        public string Description;
        public string SaveKey;
        public int SaveValue;
        public List<TechnologyNodeData> Children;
        
        public TechnologyNodeData(string name)
        {
            Name = name;
            Children = new List<TechnologyNodeData>();
        }
    }
}