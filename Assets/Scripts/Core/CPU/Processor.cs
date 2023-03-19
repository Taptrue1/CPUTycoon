using System.Collections.Generic;
using System.Linq;

namespace Core.CPU
{
    public class Processor
    {
        public string Name { get; }
        public double Price { get; }
        public double Power => GetTotalPower();

        private readonly List<Technologies.Technology> _technologies;
        
        public Processor(string name, double price, List<Technologies.Technology> technologies)
        {
            Name = name;
            Price = price;
            _technologies = technologies;
        }
        
        public string GetTechnologiesString()
        {
            return string.Join("\n", _technologies.Select(technology => technology.Name));
        }
        
        private double GetTotalPower()
        {
            return _technologies.Sum(technology => technology.Power);
        }
    }
}