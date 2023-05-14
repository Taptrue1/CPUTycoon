using Settings;

namespace Core.CPU
{
    public class Processor
    {
        public string Name { get; }
        public int SellPrice { get; }
        public int DevelopmentPoints { get; }

        public readonly TechProcessPair TechProcess;
        public readonly FrequencyPair Frequency;
        public readonly FormFactorPair FormFactor;
        public readonly RamPair Ram;
        public readonly int Bits;

        public Processor(string name, int sellPrice, TechProcessPair techProcess, FrequencyPair frequency, FormFactorPair formFactor, RamPair ram, int bits)
        {
            Name = name;
            SellPrice = sellPrice;
            DevelopmentPoints = formFactor.DevelopmentPointsPrice;
            
            TechProcess = techProcess;
            Frequency = frequency;
            FormFactor = formFactor;
            Ram = ram;
            Bits = bits;
        }
    }
}