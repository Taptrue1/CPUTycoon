using System.Collections.Generic;
using Core.Technologies;
using UnityEngine;
using XNode;

namespace Graphs
{
    public class TechNode : Node
    {
        public Sprite Icon;
        public string Name;
        public int Power;
        public int ResearchPoints;
        public int DevelopPoints;
        public int DevelopPrice;
        public int ProducePrice;
        public int Index;
        public TechnologyType Type;
        [SerializeField] public List<TechNode> Children = new();
        
        public override object GetValue(NodePort port)
        {
            return port.fieldName == "CurrentNode" ? this : null;
        }
    }
}