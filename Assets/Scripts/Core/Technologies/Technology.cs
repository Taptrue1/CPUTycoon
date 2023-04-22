using System;
using Core.Datas;
using Utils;

namespace Core.Technologies
{
    [Serializable]
    public class Technology
    {
        public string Name { get; }
        public string Description { get; }
        public Level Level { get; }
        public double DevelopmentCost => _developmentCostProgression.GetProgressionValue(Level.Value);
        public double Power => _powerProgression.GetProgressionValue(Level.Value);
        
        private readonly Progression _developmentCostProgression;
        private readonly Progression _powerProgression;

        public Technology(TechnologyData data)
        {
            Name = data.Name;
            Description = data.Description;
            Level = new(data.ResearchCost, 0);
            
            _developmentCostProgression = data.DevelopmentCost;
            _powerProgression = data.Power;
        }
    }
}