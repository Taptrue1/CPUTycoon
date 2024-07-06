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
        public double Power;
        public double ResearchPoints;
        public double DevelopPoints;
        public double DevelopPrice;
        public double ProducePrice;
        public int Index;
        public TechnologyType Type;
        [SerializeField] public List<TechNode> Children = new();
        
        public override object GetValue(NodePort port)
        {
            return port.fieldName == "CurrentNode" ? this : null;
        }
    }
}