using System;
using System.Collections.Generic;
using Core.CPU;
using Core.Technologies;
using Unity.VisualScripting;
using Utils.CustomNumbers;
using Zenject;

namespace Core
{
    public class Game
    {
        [Inject] private CoroutineRunner _coroutineRunner;
        
        private CustomNumber<double> _money;
        private DateTime _currentDate;
        private ProcessorLaboratory _processorLaboratory;
        private TechnologyLaboratory _technologyLaboratory;

        public Game()
        {
            _money = new CustomNumber<double>();
            _currentDate = new DateTime(1980, 1, 1);
            _processorLaboratory = new ProcessorLaboratory(null, null);
            _technologyLaboratory = new TechnologyLaboratory(220, 12, new List<Technology>());
        }
    }
}