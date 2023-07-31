using System.Collections.Generic;
using Core.Technologies;

namespace Core.CPU
{
    public class Processor
    {
        public string Name { get; }
        public double SellPrice { get; }
        public double Power { get; private set; }
        public double ProducePrice { get; private set;  }
        public double DevelopmentPrice { get; private set;  }
        public double DevelopmentPoints { get; private set;  }
        public List<Technology> Technologies { get; }

        public Processor(string name, int sellPrice, List<Technology> technologies)
        {
            Name = name;
            SellPrice = sellPrice;
            Technologies = technologies;
            ApplyTechnologiesBonus();
        }

        private void ApplyTechnologiesBonus()
        {
            foreach (var technology in Technologies)
            {
                Power += technology.Power;
                ProducePrice += technology.ProducePrice;
                DevelopmentPrice += technology.DevelopPrice;
                DevelopmentPoints += technology.DevelopPoints;
            }
        }
    }
}