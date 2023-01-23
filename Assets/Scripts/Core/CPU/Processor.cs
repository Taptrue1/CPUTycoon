using UnityEngine;

namespace Core.CPU
{
    public class Processor
    {
        private readonly int _techProcess;
        private readonly int _frequency;
        private readonly int _cache;

        private const float TechProcessCoefficient = 10000;
        private const float CacheCoefficient = 100;
        
        public Processor(int techProcess, int frequency, int cache)
        {
            _techProcess = techProcess;
            _frequency = frequency;
            _cache = cache;
        }

        public int GetPowerScore()
        {
            var score = Mathf.CeilToInt(TechProcessCoefficient * (10000 -_techProcess) + CacheCoefficient * _cache + _frequency);

            return score;
        }
    }
}
