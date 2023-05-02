using System;
using Core.CPU;
using Core.Technologies;
using Utils.CustomNumbers;

namespace Core
{
    public class Company
    {
        public event Action<Processor> ProcessorDeveloped;
        public event Action<Technology> TechnologyResearched;
        public event Action<Processor> DevelopingProcessorChanged;
        public event Action<Technology> ResearchingTechnologyChanged;
        
        public string Name { get; }
        public CustomNumber<double> Money { get; private set; }
        public CustomNumber<int> ResearchPoints { get; private set; }
        public CustomNumber<int> DevelopmentPoints { get; private set; }
        
        private Processor _developingProcessor;
        private Technology _researchingTechnology;

        private const int ResearchPointsPrice = 1;
        private const int DevelopmentPointsPrice = 1;
        
        public Company(string name, double money)
        {
            Name = name;
            Money = new() {Value = money};
            ResearchPoints = new() {Value = 0};
            DevelopmentPoints = new() {Value = 0};
        }

        public void Tick()
        {
            if (_developingProcessor != null)
            {
                //TODO add processor development
                //Money.Value -= DevelopmentPointsPrice * DevelopmentPoints.Value;
            }
            if(_researchingTechnology != null)
            {
                _researchingTechnology.Level.AddExperience(ResearchPoints.Value);
                Money.Value -= ResearchPointsPrice * ResearchPoints.Value;
            }
        }
        public void ResearchTechnology(Technology technology)
        {
            if(_researchingTechnology != null)
                _researchingTechnology.Level.Changed -= OnResearchingTechnologyLevelChanged;
            
            _researchingTechnology = technology;
            _researchingTechnology.Level.Changed += OnResearchingTechnologyLevelChanged;
            ResearchingTechnologyChanged?.Invoke(_researchingTechnology);
        }
        public void DevelopProcessor(Processor processor)
        {
            _developingProcessor = processor;
            DevelopingProcessorChanged?.Invoke(_developingProcessor);
        }
        public int GetResearchPointsPrice()
        {
            return ResearchPointsPrice * ResearchPoints.Value;
        }
        public int GetDevelopmentPointsPrice()
        {
            return DevelopmentPointsPrice * DevelopmentPoints.Value;
        }
        
        #region Callbacks

        private void OnDevelopingProcessorDeveloped()
        {
            //TODO add on processor developed logic
        }
        private void OnResearchingTechnologyLevelChanged()
        {
            TechnologyResearched?.Invoke(_researchingTechnology);
            _researchingTechnology.Level.Changed -= OnResearchingTechnologyLevelChanged;
            _researchingTechnology = null;
        }

        #endregion
    }
}