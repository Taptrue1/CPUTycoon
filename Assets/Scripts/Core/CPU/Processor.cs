using System.Collections.Generic;
using Settings;

namespace Core.CPU
{
    public class Processor
    {
        public string Name { get; }
        public double SellPrice { get; }
        public double Power { get; private set; }
        public double DevelopmentPrice { get; private set;  }
        public double DevelopmentPointsPrice { get; private set;  }
        public List<BasePair> TechnologyPairs { get; }

        public Processor(string name, int sellPrice, List<BasePair> pairs)
        {
            Name = name;
            SellPrice = sellPrice;
            TechnologyPairs = pairs;

            SetupPairs(pairs);
        }

        private void SetupPairs(List<BasePair> pairs)
        {
            foreach (var pair in pairs)
                SetupPair(pair);
        }
        private void SetupPair(BasePair pair)
        {
            switch (pair)
            {
                case TechProcessPair techProcess:
                    Power += techProcess.Power;
                    DevelopmentPrice += techProcess.DevelopmentPrice;
                    break;
                case FrequencyPair frequency:
                    Power += frequency.Power;
                    DevelopmentPrice += frequency.DevelopmentPrice;
                    break;
                case FormFactorPair formFactor:
                    Power += formFactor.Power;
                    DevelopmentPrice += formFactor.DevelopmentPrice;
                    DevelopmentPointsPrice += formFactor.DevelopmentPointsPrice;
                    break;
                case RamPair ram:
                    Power += ram.Power;
                    DevelopmentPrice += ram.DevelopmentPrice;
                    break;
                case BitsPair bits:
                    Power += bits.Power;
                    DevelopmentPrice += bits.DevelopmentPrice;
                    break;
            }
        }
    }
}