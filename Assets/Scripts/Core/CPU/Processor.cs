using System.Collections.Generic;
using System.Linq;
using Core.Technologies;

namespace Core.CPU
{
    public class Processor
    {
        public string Name { get; }
        public double Price { get; }
        public double Power { get; }

        private List<Technology> _technologies;

        public Processor(string name, double price, List<Technology> technologies)
        {
            Name = name;
            Price = price;
            Power = technologies.Sum(technology => technology.Power);
            
            _technologies = technologies;
        }
    }
}