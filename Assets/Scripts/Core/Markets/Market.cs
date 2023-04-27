using System;
using Core.CPU;

namespace Core.Markets
{
    /// <summary>
    /// Market manages all the companies and their incomes, fans and so on.
    /// </summary>
    public class Market
    {
        private readonly Company _playerCompany;
        
        public Market(Company playerCompany)
        {
            _playerCompany = playerCompany;
            
            _playerCompany.ProcessorDeveloped += OnProcessorDeveloped;
        }

        private void OnProcessorDeveloped(Processor processor)
        {
            //test
            _playerCompany.Money.Value += processor.Price;
        }
    }
}