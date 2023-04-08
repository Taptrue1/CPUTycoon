using System;

namespace Utils
{
    [Serializable]
    public class Level
    {
        public event Action Changed;
        
        public double Value { get; private set; }
        
        private double _currentExperience;
        private double _targetExperience;
        private Progression _experienceProgression;
        
        public Level(Progression experienceProgression, double value = 1, double currentExperience = 0)
        {
            Value = value;
            _currentExperience = currentExperience;
            _experienceProgression = experienceProgression;
            _targetExperience = experienceProgression.GetProgressionValue(value);
        }

        public void AddExperience(double experience)
        {
            _currentExperience += experience;
            
            if(_currentExperience < _targetExperience) return;
            
            Value++;
            _currentExperience -= _targetExperience;
            _targetExperience = _experienceProgression.GetProgressionValue(Value);
            
            Changed?.Invoke();
        }
    }
}