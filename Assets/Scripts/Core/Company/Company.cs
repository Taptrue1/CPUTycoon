using Core.CPU;
using Core.Technologies;

namespace Core.Company
{
    public class Company
    {
        public string Name { get; private set; }
        public double Money { get; private set; }
        
        private ProcessorLaboratory _processorLaboratory;
        private TechnologyLaboratory _technologyLaboratory;
        
        public Company(string name, double money, ProcessorLaboratory processorLaboratory, TechnologyLaboratory technologyLaboratory)
        {
            Name = name;
            Money = money;
            _processorLaboratory = processorLaboratory;
            _technologyLaboratory = technologyLaboratory;
        }
    }
}