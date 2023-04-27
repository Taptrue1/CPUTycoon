using System;
using Core.CPU;
using Core.Services;
using Core.Technologies;
using Utils.CustomNumbers;

namespace Core
{
    public class Company
    {
        public event Action<Processor> ProcessorDeveloped;
        
        public string Name { get; }
        public CustomNumber<double> Money { get; private set; }
        public CustomNumber<int> ResearchPoints { get; private set; }
        public CustomNumber<int> DevelopmentPoints { get; private set; }

        private const int ResearchPointsPrice = 1;
        private const int DevelopmentPointsPrice = 1;
        
        public Company(string name, double money, TickService tickService)
        {
            Name = name;
            Money = new() {Value = money};
            ResearchPoints = new() {Value = 0};
            DevelopmentPoints = new() {Value = 0};

            //TODO turn in on when will be implemented research and development logic
            //tickService.Tick += OnTick;
        }
        public void SetResearchPoints(int researchPoints)
        {
            if(researchPoints < 0) 
                throw new ArgumentException("Research points cannot be negative");
            
            ResearchPoints.Value = researchPoints;
        }
        public void SetDevelopmentPoints(int developmentPoints)
        {
            if(developmentPoints < 0) 
                throw new ArgumentException("Development points cannot be negative");
            
            DevelopmentPoints.Value = developmentPoints;
        }
        public void ResearchTechnology(Technology technology)
        {
            technology.Level.AddExperience(100);
        }
        public void DevelopProcessor(Processor processor)
        {
            ProcessorDeveloped?.Invoke(processor);
        }
        public int GetResearchPointsPrice()
        {
            return ResearchPointsPrice * ResearchPoints.Value;
        }
        public int GetDevelopmentPointsPrice()
        {
            return DevelopmentPointsPrice * DevelopmentPoints.Value;
        }

        private void OnTick()
        {
            
        }
    }
}