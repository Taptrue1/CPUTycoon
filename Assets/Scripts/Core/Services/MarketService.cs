using System;
using System.Linq;
using Core.CPU;
using Settings;
using UnityEngine;
using Zenject;

namespace Core.Services
{
    /// <summary>
    /// Market manages all the companies and their incomes, fans and so on.
    /// </summary>
    public class MarketService
    {
        //TODO remake all class to service
        private Processor _currentPlayerProcessor;
        
        private readonly DateTime _startDate;
        private readonly MarketSettings _marketSettings;
        
        [Inject]
        public MarketService(CoreSettings coreSettings)
        {
            _startDate = DateTime.Now; //TODO change it to start date
            _marketSettings = coreSettings.MarketSettings;
            
            //_playerCompany.ProcessorDeveloped += OnPlayerProcessorDeveloped;
        }

        public void Tick(DateTime currentDate)
        {
            /*
            if(_currentPlayerProcessor == null) return;

            var daysPassed = (currentDate - _startDate).Days;
            var clientsPerDay = _marketSettings.ClientsCount / 365;
            var playerProductCoeffs = _currentPlayerProcessor.Power / _currentPlayerProcessor.SellPrice;
            var activeProductsCoeffs =
                (from company in _marketSettings.Competitors
                    from product in company.ProductReleases
                    where product.EndSellDay >= daysPassed
                    select product.Power / product.Price).Select(dummy => (float) dummy).ToList();
            var totalProductsCoeffs = activeProductsCoeffs.Sum() + playerProductCoeffs;
            var playerIncome = Math.Round((playerProductCoeffs / totalProductsCoeffs) * clientsPerDay);

            //_playerCompany.Money.Value += playerIncome;
            Debug.Log(playerProductCoeffs / totalProductsCoeffs);
            Debug.Log($"Player income today is {playerIncome}");
            */
        }
        
        private void OnPlayerProcessorDeveloped(Processor processor)
        {
            _currentPlayerProcessor = processor;
        }
    }
}