using System;
using System.Linq;
using Core.CPU;
using Settings;
using UnityEngine;

namespace Core.Markets
{
    /// <summary>
    /// Market manages all the companies and their incomes, fans and so on.
    /// </summary>
    public class Market
    {
        private Processor _currentPlayerProcessor;
        
        private readonly DateTime _startDate;
        private readonly Company _playerCompany;
        private readonly MarketSettings _marketSettings;
        
        public Market(DateTime startDate, Company playerCompany, MarketSettings marketSettings)
        {
            _startDate = startDate;
            _playerCompany = playerCompany;
            _marketSettings = marketSettings;
            
            _playerCompany.ProcessorDeveloped += OnPlayerProcessorDeveloped;
        }

        public void Tick(DateTime currentDate)
        {
            if(_currentPlayerProcessor == null) return;

            var daysPassed = (currentDate - _startDate).Days;
            var clientsPerDay = _marketSettings.ClientsCount / 365;
            var playerProductCoeffs = _currentPlayerProcessor.Power / _currentPlayerProcessor.Price;
            var activeProductsCoeffs =
                (from company in _marketSettings.Competitors
                    from product in company.ProductReleases
                    where product.EndSellDay >= daysPassed
                    select product.Power / product.Price).Select(dummy => (float) dummy).ToList();
            var totalProductsCoeffs = activeProductsCoeffs.Sum() + playerProductCoeffs;
            var playerIncome = Math.Round((playerProductCoeffs / totalProductsCoeffs) * clientsPerDay);

            _playerCompany.Money.Value += playerIncome;
            Debug.Log(playerProductCoeffs / totalProductsCoeffs);
            Debug.Log($"Player income today is {playerIncome}");
        }
        
        private void OnPlayerProcessorDeveloped(Processor processor)
        {
            _currentPlayerProcessor = processor;
        }
    }
}