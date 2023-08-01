using System;
using System.Collections.Generic;
using System.Linq;
using Core.CPU;
using Core.Datas;
using Core.Games;
using Settings;
using UnityEngine;

namespace Core.Services
{
    public class MarketService
    {
        public event Action<ProductData> NewProductAppeared;
        public Processor CurrentPlayerProcessor => _currentPlayerProcessor;
        
        private Processor _currentPlayerProcessor;
        private List<Processor> _processorsArchive;
        private ProductData[] _activeProducts;
        private int _daysPassed;

        private readonly Game _game;
        private readonly TimeService _timeService;
        private readonly MarketSettings _marketSettings;
        private readonly CurrencyService _currencyService;

        public MarketService(Game game, TimeService timeService, CurrencyService currencyService, CoreSettings coreSettings)
        {
            _game = game;
            _timeService = timeService;
            _currencyService = currencyService;
            _marketSettings = coreSettings.MarketSettings;

            timeService.Tick += OnTick;
            
            _game.ProcessorDeveloped += OnPlayerProcessorDeveloped;
        }

        private void OnTick()
        {
            _daysPassed = (_timeService.CurrentDate - _timeService.StartDate).Days;
            _activeProducts = GetActiveProducts();

            ReportNewProductAppeared();
            UpdatePlayerIncome();
        }

        #region Callbacks
        private void OnPlayerProcessorDeveloped(Processor processor)
        {
            _currentPlayerProcessor = processor;
        }
        #endregion

        #region Other
        private void ReportNewProductAppeared()
        {
            foreach(var product in _activeProducts)
                if(product.AppearDay == _daysPassed)
                    NewProductAppeared?.Invoke(product);
        }
        private void UpdatePlayerIncome()
        {
            if(_currentPlayerProcessor == null) return;
            //TODO change magic string to CurrencyData.Name
            _currencyService.GetCurrency("Money").Value += CalculatePlayerIncome();
        }
        private int CalculatePlayerIncome()
        {
            //TODO maybe remake progression to update every day (now it updates every year)
            var clientsCount = _marketSettings.ClientsCount.GetProgressionValue(1);
            var clientsPerDay = clientsCount / 365;
            var playerProductCoeffs = _currentPlayerProcessor.Power / _currentPlayerProcessor.SellPrice;
            var activeProfuctsCoeffs = _activeProducts.Select(product => product.Power / product.Price).ToList();
            var totalProductsCoeffs = activeProfuctsCoeffs.Sum() + playerProductCoeffs;
            var playerWaste = _currentPlayerProcessor.ProducePrice * clientsPerDay;
            var playerIncome = _currentPlayerProcessor.SellPrice *
                               (playerProductCoeffs / totalProductsCoeffs * clientsPerDay);
            Debug.Log($"Player waste {_currentPlayerProcessor.ProducePrice} * {clientsPerDay} = {playerWaste}\n" +
                      $"Player income {_currentPlayerProcessor.SellPrice} * {playerProductCoeffs} / {totalProductsCoeffs} * {clientsPerDay} = {playerIncome}");
            return (int)(playerIncome - playerWaste);
        }
        private ProductData[] GetActiveProducts()
        {
            var activeProducts = (from company in _marketSettings.Competitors
                from product in company.ProductReleases
                where product.AppearDay <= _daysPassed && product.AppearDay + product.Duration >= _daysPassed
                select product).ToArray();
            return activeProducts;
        }
        #endregion
    }
}