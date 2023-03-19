namespace Core.Technologies
{
    public class Technology
    {
        public string Name { get; }
        public string Description { get; }
        public double Level => _level.Value;
        public double Power => _powerProgression.GetProgressionValue(Level);

        private readonly Level _level;
        private readonly Progression _powerProgression;

        public Technology(TechnologyData data)
        {
            Name = data.Name;
            Description = data.Description;

            _level = new(data.LevelProgression);
            _powerProgression = data.PowerProgression;
        }

        public void AddExperience(double experience)
        {
            _level.AddExperience(experience);
        }
    }
}