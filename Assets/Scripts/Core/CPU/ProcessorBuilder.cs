using System;
using Utils.CustomNumbers;

namespace Core.CPU
{
    public class ProcessorBuilder
    {
        public Action<Processor> ProcessorCreated;

        private CustomNumber<float> _currentCache;
        private CustomNumber<float> _currentFrequency;
        private CustomNumber<float> _currentTechProcess;

        public ProcessorBuilder(ProcessorBuilderView view)
        {
            _currentCache = new();
            _currentFrequency = new();
            _currentTechProcess = new();
            
            view.Init(_currentTechProcess, _currentFrequency, _currentCache);
            
            view.CreateButtonClicked += OnCreateProcessorButtonClick;
        }

        private void OnCreateProcessorButtonClick()
        {
            var processor = new Processor((int)_currentTechProcess.Value, (int)_currentFrequency.Value, (int)_currentCache.Value);
            
            ProcessorCreated?.Invoke(processor);
        }
    }
}