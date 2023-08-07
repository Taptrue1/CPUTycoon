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
        public event Action<Processor> PlayerProductChanged;
        public Processor PlayerProduct => _playerProduct;

        private int _daysPassed;
        private double _adDuration;
        private double _adMultiplier;
        private double _maxPlayerProductSales;
        private Processor _playerProduct;
        private ProductData[] _activeProducts;
        private List<Processor> _playerProductsArchive;

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
            _playerProductsArchive = new List<Processor>();
            _game.ProcessorDeveloped += OnPlayerProcessorDeveloped;
            timeService.Tick += OnTick;
        }
        public void ActivateAdBonus(double salesMultiplier, int duration)
        {
            _adMultiplier = salesMultiplier;
            _adDuration = duration;
        }
        
        private void OnTick()
        {
            _daysPassed = (_timeService.CurrentDate - _timeService.StartDate).Days;
            _activeProducts = GetActiveProducts();
            _adDuration = Math.Max(0, _adDuration - 1);

            ReportNewProductAppeared();
            UpdatePlayerIncome();
        }

        #region Callbacks
        private void OnPlayerProcessorDeveloped(Processor processor)
        {
            _playerProduct = processor;
            _maxPlayerProductSales = 0;
            PlayerProductChanged?.Invoke(processor);
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
            if(_playerProduct == null) return;
            //TODO change magic string to CurrencyData.Name
            _currencyService.GetCurrency("Money").Value += CalculatePlayerIncome();
        }
        private double CalculatePlayerIncome()
        {
            var playerProductCoeffs = _playerProduct.Power / _playerProduct.SellPrice;
            var activeProfuctsCoeffs = _activeProducts.Sum(product => product.Power / product.Price);
            var totalProductsCoeffs = activeProfuctsCoeffs + playerProductCoeffs;
            var clientsCount = _marketSettings.ClientsCount.GetProgressionValue(_daysPassed);
            var clientsPerDay = _adDuration > 0 ? clientsCount / 365 * _adMultiplier : clientsCount / 365;
            var totalPlayerClients = playerProductCoeffs / totalProductsCoeffs * clientsPerDay;
            var playerWaste = _playerProduct.ProducePrice * totalPlayerClients;
            var playerIncome = _playerProduct.SellPrice * totalPlayerClients;
            var playerProfit = Math.Ceiling(playerIncome - playerWaste);
            _maxPlayerProductSales = Math.Max(_maxPlayerProductSales, totalPlayerClients);
            if (totalPlayerClients <= _maxPlayerProductSales / 10)
            {
                _playerProductsArchive.Add(_playerProduct);
                _playerProduct = null;
                PlayerProductChanged?.Invoke(null);
            }
            Debug.Log($"Player waste {_playerProduct.ProducePrice} * {clientsPerDay} = {playerWaste}\n" +
                      $"Player income {_playerProduct.SellPrice} * {playerProductCoeffs} / {totalProductsCoeffs} * {clientsPerDay} = {playerIncome}");
            return playerProfit;
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