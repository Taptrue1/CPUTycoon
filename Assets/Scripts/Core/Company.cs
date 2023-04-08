using System.Collections.Generic;
using Core.Technologies;

namespace Core
{
    public class Company
    {
        public string Name { get; }
        public double Money { get; private set; }
        public string ActiveProduct { get; private set; }
        
        private List<Technology> _technologies;
        private int _researchPointsPerTick;
        private int _developmentPointsPerTick;
        private int _researchPointsPrice;
        private int _developmentPointsPrice;
        
        public Company(string name, double money, string activeProduct)
        {
            Name = name;
            Money = money;
            ActiveProduct = activeProduct;
        }
    }
}