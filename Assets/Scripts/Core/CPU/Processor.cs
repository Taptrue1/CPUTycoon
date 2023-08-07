using System.Collections.Generic;
using Core.Technologies;

namespace Core.CPU
{
    public class Processor
    {
        public string Name { get; }
        public double Power { get; }
        public double SellPrice { get; }
        public double ProducePrice { get; }
        public double DevelopmentPrice { get; }
        public double DevelopmentPoints { get; }
        public List<Technology> Technologies { get; }

        public Processor(string name, int sellPrice, List<Technology> technologies)
        {
            Name = name;
            SellPrice = sellPrice;
            Technologies = technologies;
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