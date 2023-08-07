using System;
using System.Collections.Generic;
using System.Linq;
using Graphs;

namespace Core.Technologies
{
    [Serializable]
    public class Technology
    {
        public string Name { get; }
        public int Power { get; }
        public int ResearchPoints { get; }
        public int DevelopPoints { get; }
        public int DevelopPrice { get; }
        public int ProducePrice { get; }
        public int Index { get; }
        public TechnologyType Type { get; }
        public List<Technology> Children { get; }
        
        private bool _isResearched;

        public Technology(TechNode techNode)
        {
            Name = techNode.Name;
            Power = techNode.Power;
            ResearchPoints = techNode.ResearchPoints;
            DevelopPoints = techNode.DevelopPoints;
            DevelopPrice = techNode.DevelopPrice;
            ProducePrice = techNode.ProducePrice;
            Index = techNode.Index;
            Type = techNode.Type;
            Children = techNode.Children.Select(child => new Technology(child)).ToList();
        }
        public void Research()
        {
            _isResearched = true;
        }
        public bool IsResearched()
        {
            return Name == "ROOT" || _isResearched;
        }
    }
}