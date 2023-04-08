using Core.Datas;
using Utils;

namespace Core.Technologies
{
    public class Technology
    {
        public string Name { get; }
        public string Description { get; }
        public Level Level { get; }
        public double ResearchCost => _researchCostProgression.GetProgressionValue(Level.Value);
        public double ImplementationCost => _implementationCostProgression.GetProgressionValue(Level.Value);
        public double Power => _powerProgression.GetProgressionValue(Level.Value);

        private readonly Progression _researchCostProgression;
        private readonly Progression _implementationCostProgression;
        private readonly Progression _powerProgression;

        public Technology(TechnologyData data, Level level)
        {
            Name = data.Name;
            Description = data.Description;
            
            Level = level;
            _researchCostProgression = data.ResearchCost;
            _implementationCostProgression = data.ImplementationCost;
            _powerProgression = data.Power;
        }
    }
}