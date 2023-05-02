using System.Collections.Generic;
using System.Linq;
using Core.Technologies;

namespace Core.CPU
{
    public class Processor
    {
        public string Name { get; }
        public int Power { get; }
        public int SellPrice { get; }
        public int ImplementPrice { get; }
        public int DevelopmentPointsPrice { get; }

        public Processor(string name, int sellPrice, List<Technology> technologies)
        {
            Name = name;
            SellPrice = sellPrice;
            Power = technologies.Sum(technology => technology.Power);
            ImplementPrice = technologies.Sum(technology => technology.ImplementPrice);
            DevelopmentPointsPrice = technologies.Sum(technology => technology.ImplementPrice);
        }
    }
}