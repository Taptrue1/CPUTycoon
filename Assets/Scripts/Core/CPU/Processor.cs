using System;
using System.Collections.Generic;
using System.Linq;
using Core.Technologies;

namespace Core.CPU
{
    public class Processor
    {
        public string Name { get; }
        public double BenefitRatio { get; }
        public double PerUnitProfit { get; }
        public double DevelopPrice { get; }
        public double DevelopPointsPrice { get; }
        
        public List<Technology> Technologies { get; }
        public double TotalCosts { get; private set; }
        public DateTime ReleaseDate { get; private set; }

        public Processor(string name, int sellPrice, List<Technology> technologies)
        {
            Name = name;
            Technologies = technologies;
            BenefitRatio = technologies.Sum(tech => tech.Power) / sellPrice;
            PerUnitProfit = technologies.Sum(tech => tech.ProducePrice) - sellPrice;
            foreach (var technology in Technologies)
            {
                DevelopPrice += technology.DevelopPrice;
                DevelopPointsPrice += technology.DevelopPointsPrice;
            }
        }
    }
}