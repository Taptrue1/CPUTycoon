using Core.Datas;
using Utils;

namespace Core.Technologies
{
    public class Technology
    {
        public string Name { get; }
        public string Description { get; }
        public int Power { get; }
        public int ImplementPrice { get; }
        public int ResearchPointsPrice { get; }
        public int DevelopmentPointsPrice { get; }
        public bool IsResearched { get; }
        
        //TODO delete this
        public Level Level { get; private set; }
        
        //private Progress _progress;

        public Technology(TechnologyData data)
        {
            Name = data.Name;
            Description = data.Description;
            Power = data.Power;
            ImplementPrice = data.ImplementPrice;
            ResearchPointsPrice = data.ResearchPointsPrice;
            DevelopmentPointsPrice = data.DevelopmentPointsPrice;
        }
    }
}