namespace Core.CPU
{
    public class Processor
    {
        private readonly int _techProcess;
        private readonly int _frequency;
        private readonly int _cache;

        public Processor(int techProcess, int frequency, int cache)
        {
            _techProcess = techProcess;
            _frequency = frequency;
            _cache = cache;
        }

        public int GetPowerScore()
        {
            var score = _techProcess + _cache + _frequency;

            return score;
        }
    }
}
