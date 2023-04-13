using System;
using System.Collections.Generic;
using Core.CPU;
using Core.Services;
using Core.Technologies;

namespace Core
{
    public class Company
    {
        public event Action<Processor> ActiveProductChanged;
        public event Action<Technology> ActiveTechnologyChanged;
        
        public string Name { get; }
        public double Money { get; private set; }
        public double ResearchPoints { get; private set; }
        public double DevelopmentPoints { get; private set; }
        public List<Technology> Technologies { get; private set; }

        private Processor _activeDevelopingProduct;
        private Technology _activeResearchingTechnology;

        //TODO now immutable, but should be mutable
        private const int ResearchPointsPrice = 1;
        private const int DevelopmentPointsPrice = 1;
        
        public Company(string name, double money, TickService tickService)
        {
            Name = name;
            Money = money;
            
            tickService.Tick += OnTick;
        }
        public void SetResearchPoints(double researchPoints)
        {
            if(researchPoints < 0) 
                throw new ArgumentException("Research points cannot be negative");
            
            ResearchPoints = researchPoints;
        }
        public void SetDevelopmentPoints(double developmentPoints)
        {
            if(developmentPoints < 0) 
                throw new ArgumentException("Development points cannot be negative");
            
            DevelopmentPoints = developmentPoints;
        }
        public void ResearchTechnology(Technology technology)
        {
            _activeResearchingTechnology = technology;
            ActiveTechnologyChanged?.Invoke(technology);
        }
        public void DevelopProcessor(Processor processor)
        {
            _activeDevelopingProduct = processor;
            ActiveProductChanged?.Invoke(processor);
        }

        private void OnTick()
        {
            if (_activeDevelopingProduct != null)
            {
                //Do progress on developing processor
            }
            if(_activeResearchingTechnology != null)
            {
                Money -= ResearchPoints * ResearchPointsPrice;
                _activeResearchingTechnology.Level.AddExperience(ResearchPoints);
            }
        }
    }
}