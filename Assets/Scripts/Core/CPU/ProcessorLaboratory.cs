using System.Collections.Generic;

namespace Core.CPU
{
    public class ProcessorLaboratory : ITickable
    {
        public Processor CurrentProcessor => _currentProcessor;
        public List<Processor> ArchivedProcessors => _archivedProcessors;
        
        private Processor _currentProcessor;
        private List<Processor> _archivedProcessors;
        
        public ProcessorLaboratory(Processor currentProcessor, List<Processor> archivedProcessors)
        {
            _currentProcessor = currentProcessor;
            _archivedProcessors = archivedProcessors;
        }
        
        public void Tick()
        {
            
        }
        public void DevelopProcessor(Processor processor)
        {
            _archivedProcessors.Add(_currentProcessor);
            
            _currentProcessor = processor;
        }
    }
}