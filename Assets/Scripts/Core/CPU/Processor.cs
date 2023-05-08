namespace Core.CPU
{
    public class Processor
    {
        public string Name { get; }
        public int Power { get; }
        public int SellPrice { get; }
        public int ImplementPrice { get; }
        public int DevelopmentPointsPrice { get; }

        public Processor(string name, int sellPrice)
        {
            Name = name;
            SellPrice = sellPrice;
        }
    }
}