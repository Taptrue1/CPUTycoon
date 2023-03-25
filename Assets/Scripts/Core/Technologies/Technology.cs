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

        public Technology(TechnologyConfig config)
        {
            Name = config.Name;
            Description = config.Description;

            _level = new(config.ExperienceProgression);
            _powerProgression = config.PowerProgression;
        }

        public void AddExperience(double experience)
        {
            _level.AddExperience(experience);
        }
    }
}