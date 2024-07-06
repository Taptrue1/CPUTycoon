using System;

namespace Core.NEW
{
    public class Project
    {
        public event Action Completed;
        
        public string Name { get; }
        public double Difficulty { get; }
        public double Reward { get; }
        public double DevelopmentTime { get; }
        public double Costs { get; private set; }
        public double TimeLeft { get; private set; }

        private double _progress;
        private bool _isCompleted;
        
        public Project(string name, double difficulty, double costs, double reward, double developmentTime)
        {
            Name = name;
            Difficulty = difficulty;
            Costs = costs;
            Reward = reward;
            DevelopmentTime = developmentTime;
            TimeLeft = developmentTime;
        }

        public void AddTime(double time)
        {
            if (time < 0) throw new Exception("Time can't be negative");
            TimeLeft += time;
        }
        public void AddCosts(double costs)
        {
            if (costs < 0) throw new Exception("Costs can't be negative");
            Costs += costs;
        }
        public void AddProgress(double progress)
        {
            if (progress < 0) throw new Exception("Progress can't be negative");
            if(_isCompleted) return;
            _progress += progress;
            if (_progress < Difficulty) return;
            _isCompleted = true;
            Completed?.Invoke();
        }
    }
}