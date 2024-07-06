using System;
using System.Collections.Generic;
using System.Linq;
using Graphs;
using UnityEngine;

namespace Core.Technologies
{
    [Serializable]
    public class Technology
    {
        public Sprite Icon { get; }
        public string Name { get; }
        public double Power { get; }
        public double DevelopPrice { get; }
        public double ProducePrice { get; }
        public double DevelopPointsPrice { get; }
        public double ResearchPointsPrice { get; }
        public int Index { get; }
        public TechnologyType Type { get; }
        public List<Technology> Children { get; }
        
        private bool _isResearched;

        public Technology(TechNode techNode)
        {
            Icon = techNode.Icon;
            Name = techNode.Name;
            Power = techNode.Power;
            ResearchPointsPrice = techNode.ResearchPoints;
            DevelopPointsPrice = techNode.DevelopPoints;
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